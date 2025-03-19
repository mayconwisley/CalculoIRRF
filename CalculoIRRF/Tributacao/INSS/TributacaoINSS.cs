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

        string urlInss = $@"https://www.gov.br/inss/pt-br/direitos-e-deveres/inscricao-e-contribuicao/tabela-de-contribuicao-mensal";

        var htmlDocument = await AcessarUrl.AcessarSite(urlInss);

        if (htmlDocument is null)
        {
            return null;
        }

        var dataPublicacao = BuscarDataPublicacaoOnline(htmlDocument);
        var dataAtualizacao = BuscarDataAtualizacaoOnline(htmlDocument);
        var tributacaoINSS = BuscarINSSOnline(htmlDocument);
        var isInss = await _inssServices.IsGov(dataAtualizacao);

        if (isInss)
        {
            return null;
        }

        foreach (var item in tributacaoINSS)
        {
            inssGov = new InssGov
            {
                DataCriacao = dataPublicacao,
                DataAtualizacao = dataAtualizacao,
                Sequencia = item.Sequencia,
                BaseCaculo = item.BaseCalculo,
                Aliquota = item.Aliquota
            };
            await _inssServices.Gravar(inssGov);
        }

        var listInssGov = await _inssServices.ListarTodosPorDataAtualizacao(dataAtualizacao);
        return [.. listInssGov];
    }

    private static List<TributacaoINSSObj> BuscarINSSOnline(HtmlDocument htmlDocument)
    {
        List<TributacaoINSSObj> listTributacaoRFBObj = [];

        var table = htmlDocument.DocumentNode.SelectSingleNode("//table");

        if (table != null)
        {
            var rows = table.SelectNodes(".//tr");
            int sequencia = 0;
            foreach (var row in rows)
            {

                if (Validar.ExtrairValor(row.InnerText) == 0)
                {
                    continue;
                }

                var cells = row.SelectNodes(".//td");

                if (cells != null)
                {
                    listTributacaoRFBObj.Add(new TributacaoINSSObj
                    {
                        Sequencia = ++sequencia,
                        BaseCalculo = Validar.ExtrairMaiorValor(cells[0].InnerText.Trim()),
                        Aliquota = Validar.ExtrairValor(cells[1].InnerText.Trim()),

                    });
                }
            }
            return listTributacaoRFBObj;
        }
        else
        {
            return [];
        }
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

