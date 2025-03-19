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
        string urlRfb = $@"https://www.gov.br/receitafederal/pt-br/assuntos/meu-imposto-de-renda/tabelas/{DateTime.Now:yyyy}";

        var htmlDocument = await AcessarUrl.AcessarSite(urlRfb) ?? throw new ArgumentException($"Site atual inválido\n{urlRfb}");
        var dataAtualizacao = BuscarDataAtualizacaoOnline(htmlDocument);
        var isIrrf = await _irrfServices.IsGov(dataAtualizacao);

        if (isIrrf)
        {
            return null;
        }

        var dataPublicacao = BuscarDataPublicacaoOnline(htmlDocument);

        var valorDependente = BuscarValorDependenteOnline(htmlDocument);
        var valorDescontoSimplificado = BuscarDescontoSimplicadoOnline(htmlDocument);

        var tributacaoRFB = BuscarIRRFOnline(htmlDocument);
        var countItem = tributacaoRFB.Count;

        foreach (var item in tributacaoRFB)
        {
            IrrfRfb irrfRfb = new()
            {
                DataCriacao = dataPublicacao,
                DataAtualizacao = dataAtualizacao,
                Dependente = valorDependente,
                Simplificado = valorDescontoSimplificado,
                Sequencia = item.Sequencia,
                BaseCaculo = item.BaseCalculo,
                Aliquota = item.Aliquota,
                Deducao = item.Deducao
            };

            if (countItem == item.Sequencia)
            {
                irrfRfb.BaseCaculo = double.Parse("9999999999999.99");
            }
            await _irrfServices.GravarRfb(irrfRfb);
        }

        var listIrrfRfb = await _irrfServices.ListarTodosDataAtualizacao(dataAtualizacao);
        return [.. listIrrfRfb];
    }
    private static List<TributacaoRFBObj> BuscarIRRFOnline(HtmlDocument htmlDocument)
    {
        List<TributacaoRFBObj> listTributacaoRFBObj = [];

        var table = htmlDocument.DocumentNode.SelectSingleNode("//table");

        if (table != null)
        {
            var rows = table.SelectNodes(".//tr");
            int sequencia = 0;
            foreach (var row in rows)
            {
                var cells = row.SelectNodes(".//td");
                if (cells != null)
                {
                    listTributacaoRFBObj.Add(new TributacaoRFBObj
                    {
                        Sequencia = ++sequencia,
                        BaseCalculo = Validar.ExtrairMaiorValor(cells[0].InnerText.Trim()),
                        Aliquota = Validar.ExtrairValor(cells[1].InnerText.Trim()),
                        Deducao = Validar.ExtrairValor(cells[2].InnerText.TrimEnd())
                    });
                }
            }
            return listTributacaoRFBObj;
        }
        else
        {
            return new List<TributacaoRFBObj>();
        }
    }

    private static double BuscarValorDependenteOnline(HtmlDocument htmlDocument)
    {
        var spans = htmlDocument.DocumentNode.SelectNodes("//p/em/span");
        if (spans != null)
        {
            string[] br = ["<br>"];
            var valores = spans[1].InnerHtml.Split(br, StringSplitOptions.None);
            var deducaoDependente = Validar.ExtrairValor(valores[0].Trim());
            return deducaoDependente;
        }
        return 0;
    }
    private static double BuscarDescontoSimplicadoOnline(HtmlDocument htmlDocument)
    {
        var spans = htmlDocument.DocumentNode.SelectNodes("//p/em/span");
        if (spans != null)
        {
            string[] br = ["<br>"];
            var valores = spans[1].InnerHtml.Split(br, StringSplitOptions.None);
            if (valores.Length == 1)
            {
                return 0;
            }
            var deducaoSimplificado = Validar.ExtrairValor(valores[1].Trim());

            return deducaoSimplificado;
        }
        return 0;
    }
    private static DateTime BuscarDataPublicacaoOnline(HtmlDocument htmlDocument)
    {
        var dataPublicacao = htmlDocument.DocumentNode.SelectNodes("//span[contains(@class, 'documentPublished')]/span");
        if (dataPublicacao != null)
        {
            var data = DateTime.Parse(dataPublicacao[1].InnerText.Trim().Replace("h", ":"));
            return data;
        }
        return DateTime.Now;
    }
    private static DateTime BuscarDataAtualizacaoOnline(HtmlDocument htmlDocument)
    {
        var dataPublicacao = htmlDocument.DocumentNode.SelectNodes("//span[contains(@class, 'documentModified')]/span");
        if (dataPublicacao != null)
        {
            var data = DateTime.Parse(dataPublicacao[1].InnerText.Trim().Replace("h", ":"));
            return data;
        }
        return DateTime.Now;
    }
}
