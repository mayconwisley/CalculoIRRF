using CalculoIRRF.Model;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Services.Validacao;
using CalculoIRRF.Tributacao.AcessarSite;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Tributacao.INSS;

public class TributacaoINSS(IInssServices _inssServices)
{
	public async Task<List<InssGov>> AtualizarOnline()
	{
		InssGov inssGov;

		string urlInss = $@"https://www.contabeis.com.br/tabelas/inss/";

		var htmlDocument = await AcessarUrl.AcessarSite(urlInss) ?? throw new ArgumentException($"Site inválido\n{urlInss}");
		var vigencia = Vigencia(htmlDocument);
		var isInss = await _inssServices.IsGov(vigencia);
		if (isInss)
			return null;

		var faixa = await _inssServices.UltimaFaixaInss(vigencia);
		if (faixa > 0)
			return null;

		var tributacaoINSS = GerarTabela(htmlDocument);

		foreach (var item in tributacaoINSS)
		{
			inssGov = new InssGov
			{
				Vigencia = vigencia,
				Sequencia = item.Sequencia,
				BaseCaculo = item.BaseCalculo,
				Aliquota = item.Aliquota
			};
			await _inssServices.Gravar(inssGov);
		}

		var listInssGov = await _inssServices.ListarTodosPorVigencia(vigencia);
		return [.. listInssGov];
	}

	private static List<TributacaoINSSObj> GerarTabela(HtmlDocument htmlDocument)
	{
		List<TributacaoINSSObj> tributacaoINSSObjs = [];

		var section = htmlDocument.DocumentNode.SelectSingleNode("//section[contains(@class, 'inss')]") ??
			throw new Exception("Tabela INSS não encontrada na página");

		var linhas = section.SelectNodes(".//ul[contains(@class, 'itemList') and contains(@class, 'qtd3')]") ??
			throw new Exception("Nenhum faixa de INSS encontrada");

		int sequencia = 1;
		foreach (var linha in linhas)
		{
			var coluna = linha.SelectNodes(".//li");
			if (coluna is null || coluna.Count < 3)
				continue;

			if (coluna[1].InnerText.Contains("O valor máximo do INSS do segurado empregado é"))
				continue;

			var baseCalculo = Validar.ExtrairMaiorValor(coluna[0].InnerText.Trim());
			var aliquota = Validar.ExtrairValor(coluna[1].InnerText.Trim());

			tributacaoINSSObjs.Add(new TributacaoINSSObj
			{
				Sequencia = sequencia++,
				BaseCalculo = Validar.ExtrairMaiorValor(coluna[0].InnerText.Trim()),
				Aliquota = Validar.ExtrairValor(coluna[1].InnerText.Trim()),
			});

		}
		return tributacaoINSSObjs;

	}
	private static DateTime Vigencia(HtmlDocument htmlDocument)
	{
		var section = htmlDocument.DocumentNode.SelectSingleNode("//section[contains(@class, 'inss')]") ??
			throw new Exception("Tabela INSS não encontrada na página");

		var vigencia = section.SelectSingleNode(".//span") ??
			throw new Exception("Vigência não encontrada");

		DateTime dtVigencia = DateTime.Parse(vigencia.InnerText.Replace(" ", "").Trim());

		return dtVigencia;
	}
}

