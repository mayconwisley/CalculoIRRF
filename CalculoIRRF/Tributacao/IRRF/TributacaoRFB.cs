using CalculoIRRF.Modelo.Validacao;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalculoIRRF.Tributacao.IRRF
{
    public class TributacaoRFB
    {


        public async Task AtualizarOnline()
        {
            var htmlDocument = await AcessarSite();


            var dataPlubicacao = BuscarDataPublicacaoOnline(htmlDocument);
            var dataAtualizacao = BuscarDataAtualizacaoOnline(htmlDocument);

            var valorDependente = BuscarValorDependenteOnline(htmlDocument);
            var valorDescontoSimplificado = BuscarDescontoSimplicadoOnline(htmlDocument);

            BuscarIRRFOnline(htmlDocument);
        }

        private async Task<HtmlDocument> AcessarSite()
        {
            string urlRfb = $@"https://www.gov.br/receitafederal/pt-br/assuntos/meu-imposto-de-renda/tabelas/{DateTime.Now:yyyy}";

            var htmlClient = new HttpClient();
            string html = await htmlClient.GetStringAsync(urlRfb);

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            return htmlDoc;
        }


        private List<TributacaoRFBObj> BuscarIRRFOnline(HtmlDocument htmlDocument)
        {
            Validar validar = new Validar();
            List<TributacaoRFBObj> listTributacaoRFBObj = new List<TributacaoRFBObj>();
            try
            {
                var table = htmlDocument.DocumentNode.SelectSingleNode("//table");

                if (table != null)
                {
                    var rows = table.SelectNodes(".//tr");

                    foreach (var row in rows)
                    {
                        var cells = row.SelectNodes(".//td");
                        if (cells != null)
                        {
                            listTributacaoRFBObj.Add(new TributacaoRFBObj
                            {
                                BaseCalculo = validar.ExtrairMaiorValor(cells[0].InnerText.Trim()),
                                Aliquota = validar.ExtrairValor(cells[1].InnerText.Trim()),
                                Deducao = validar.ExtrairValor(cells[2].InnerText.TrimEnd())
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
            catch (Exception)
            {
                throw;
            }
        }

        private decimal BuscarValorDependenteOnline(HtmlDocument htmlDocument)
        {
            Validar validar = new Validar();

            try
            {
                var spans = htmlDocument.DocumentNode.SelectNodes("//p/em/span");
                if (spans != null)
                {
                    string[] br = new string[1];
                    br[0] = "<br>";

                    var valores = spans[1].InnerHtml.Split(br, StringSplitOptions.None);
                    var deducaoDependente = validar.ExtrairValor(valores[0].Trim());
                    return deducaoDependente;
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }
        private decimal BuscarDescontoSimplicadoOnline(HtmlDocument htmlDocument)
        {

            Validar validar = new Validar();

            try
            {
                var spans = htmlDocument.DocumentNode.SelectNodes("//p/em/span");
                if (spans != null)
                {
                    string[] br = new string[1];
                    br[0] = "<br>";

                    var valores = spans[1].InnerHtml.Split(br, StringSplitOptions.None);
                    var deducaoSimplificado = validar.ExtrairValor(valores[1].Trim());

                    return deducaoSimplificado;
                }
                return 0;
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
