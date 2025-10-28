using CalculoIRRF.Model;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Services.Validacao;
using CalculoIRRF.Tributacao.AcessarSite;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Tributacao.IRRF;

public class TributacaoRFB(IIrrfServices _irrfServices)
{
	public async Task<List<IrrfRfb>> AtualizarOnline()
	{
		string urlRfb = $@"https://www.contabeis.com.br/tabelas/imposto-renda/";

		var htmlDocument = await AcessarUrl.AcessarSite(urlRfb) ??
			throw new ArgumentException($"Site atual inválido\n{urlRfb}");


		var vigencia = Vigencia(htmlDocument);
		var isIrrf = await _irrfServices.IsGov(vigencia);
		if (isIrrf)
			return null;

		var faixa = await _irrfServices.UltimaFaixaIrrf(vigencia);
		if (faixa > 0)
			return null;

		var impostoRFB = GerarTabela(htmlDocument);
		var dependente = ValorDependente(htmlDocument);
		var simplificado = 0.0;

		var countItem = impostoRFB.Count;

		foreach (var item in impostoRFB)
		{
			if (item.Sequencia == 1)
				simplificado = item.BaseCalculo * 0.25;

			IrrfRfb irrfRfb = new()
			{
				Dependente = dependente,
				Simplificado = simplificado,
				Sequencia = item.Sequencia,
				BaseCaculo = item.BaseCalculo,
				Aliquota = item.Aliquota,
				Deducao = item.Deducao,
				Vigencia = vigencia,
			};

			if (countItem == item.Sequencia)
				irrfRfb.BaseCaculo = double.Parse("9999999999999.99");

			await _irrfServices.GravarRfb(irrfRfb);
		}

		var listIrrfRfb = await _irrfServices.ListarTodosDataVigencia(vigencia);
		return [.. listIrrfRfb];
	}
	private static List<TributacaoRFBObj> GerarTabela(HtmlDocument htmlDocument)
	{
		List<TributacaoRFBObj> listTributacaoRFBObj = [];
		var section = htmlDocument.DocumentNode.SelectSingleNode("//section[contains(@class, 'impostoRenda')]") ??
			throw new Exception("Tabela IRRF não encontrada na página");

		var linhas = section.SelectNodes(".//ul[contains(@class, 'itemList') and contains(@class, 'qtd3')]") ??
			throw new Exception("Nenhum faixa de IRRF encontrada");

		int sequencia = 1;
		foreach (var linha in linhas)
		{
			var coluna = linha.SelectNodes(".//li");
			if (coluna is null || coluna.Count < 3)
				continue;

			if (coluna[1].InnerText.Contains("Dedução por dependente:"))
				continue;

			var rendimento = Validar.ExtrairMaiorValor(coluna[0].InnerText.Trim());
			var aliquota = Validar.ExtrairValor(coluna[1].InnerText.Trim());
			var deducao = Validar.ExtrairValor(coluna[2].InnerText.TrimEnd());

			double simplificado = 0.0;
			if (sequencia == 1)
				simplificado = rendimento * 0.25;

			listTributacaoRFBObj.Add(new TributacaoRFBObj
			{
				Sequencia = sequencia++,
				BaseCalculo = rendimento,
				Aliquota = aliquota,
				Deducao = deducao,
			});
		}
		return listTributacaoRFBObj;
	}
	private static double ValorDependente(HtmlDocument htmlDocument)
	{
		var section = htmlDocument.DocumentNode.SelectSingleNode("//section[contains(@class, 'impostoRenda')]") ??
			throw new Exception("Tabela IRRF não encontrada na página");

		var linhas = section.SelectNodes(".//ul[contains(@class, 'itemList') and contains(@class, 'qtd3')]") ??
			throw new Exception("Nenhum faixa de IRRF encontrada");

		double dependente = 0.0;
		foreach (var linha in linhas)
		{
			var coluna = linha.SelectNodes(".//li");
			if (coluna is null || coluna.Count < 3)
				continue;

			if (coluna[1].InnerText.Contains("Dedução por dependente:"))
				dependente = Validar.ExtrairValor(coluna[2].InnerText.TrimEnd());
		}
		return dependente;
	}
	private static DateTime Vigencia(HtmlDocument htmlDocument)
	{
		var section = htmlDocument.DocumentNode.SelectSingleNode("//section[contains(@class, 'impostoRenda')]") ??
			throw new Exception("Tabela IRRF não encontrada na página");

		var vigencia = section.SelectSingleNode(".//span") ??
			throw new Exception("Vigência não encontrada");

		DateTime dtVigencia = DateTime.Parse(vigencia.InnerText.Replace(" ", "").Trim());

		return dtVigencia;
	}
}
