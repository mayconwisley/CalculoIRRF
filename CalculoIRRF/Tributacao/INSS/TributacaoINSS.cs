using CalculoIRRF.Modelo.Inss;
using CalculoIRRF.Modelo.Validacao;
using CalculoIRRF.Objetos.Tributacao;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalculoIRRF.Tributacao.INSS
{
    public class TributacaoINSS
    {
        public async Task AtualizarOnline()
        {
            Cadastro cadastro;
            InssGov inssGov;

            var htmlDocument = await AcessarSite();

            if (htmlDocument is null)
            {
                return;
            }

            var dataPublicacao = BuscarDataPublicacaoOnline(htmlDocument);
            var dataAtualizacao = BuscarDataAtualizacaoOnline(htmlDocument);
            var tributacaoINSS = BuscarINSSOnline(htmlDocument);

            foreach (var item in tributacaoINSS)
            {
                cadastro = new Cadastro();
                inssGov = new InssGov
                {
                    DataCriacao = dataPublicacao,
                    DataAtualizacao = dataAtualizacao,
                    Sequencia = item.Sequencia,
                    BaseCaculo = item.BaseCalculo,
                    Aliquota = item.Aliquota
                };

                cadastro.GravarInssOnline(inssGov);
            }
        }

        private async Task<HtmlDocument> AcessarSite()
        {
            try
            {
                string urlRfb = $@"https://www.gov.br/inss/pt-br/direitos-e-deveres/inscricao-e-contribuicao/tabela-de-contribuicao-mensal";
                var htmlClient = new HttpClient();
                var status = await htmlClient.GetAsync(urlRfb);

                if (status.IsSuccessStatusCode)
                {
                    string html = await htmlClient.GetStringAsync(urlRfb);
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(html);
                    return htmlDoc;
                }
                else
                {
                    return null;
                }
            }
            catch (HttpRequestException)
            {
                throw;
            }
        }

        private List<TributacaoINSSObj> BuscarINSSOnline(HtmlDocument htmlDocument)
        {
            Validar validar = new Validar();
            List<TributacaoINSSObj> listTributacaoRFBObj = new List<TributacaoINSSObj>();
            try
            {
                var table = htmlDocument.DocumentNode.SelectSingleNode("//table");

                if (table != null)
                {
                    var rows = table.SelectNodes(".//tr");
                    int sequencia = 0;
                    foreach (var row in rows)
                    {

                        if (validar.ExtrairValor(row.InnerText) == 0)
                        {
                            continue;
                        }

                        var cells = row.SelectNodes(".//td");

                        if (cells != null)
                        {
                            listTributacaoRFBObj.Add(new TributacaoINSSObj
                            {
                                Sequencia = ++sequencia,
                                BaseCalculo = validar.ExtrairMaiorValor(cells[0].InnerText.Trim()),
                                Aliquota = validar.ExtrairValor(cells[1].InnerText.Trim()),

                            });
                        }
                    }
                    return listTributacaoRFBObj;
                }
                else
                {
                    return new List<TributacaoINSSObj>();
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private DateTime BuscarDataPublicacaoOnline(HtmlDocument htmlDocument)
        {
            try
            {
                var dataPublicacao = htmlDocument.DocumentNode.SelectNodes("//span[contains(@class, 'documentPublished')]/span");
                if (dataPublicacao != null)
                {
                    var data = DateTime.Parse(dataPublicacao[1].InnerText.Trim().Replace("h", ":"));
                    return data;
                }
                return DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private DateTime BuscarDataAtualizacaoOnline(HtmlDocument htmlDocument)
        {
            try
            {
                var dataPublicacao = htmlDocument.DocumentNode.SelectNodes("//span[contains(@class, 'documentModified')]/span");
                if (dataPublicacao != null)
                {
                    var data = DateTime.Parse(dataPublicacao[1].InnerText.Trim().Replace("h", ":"));
                    return data;
                }
                return DateTime.Now;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}

