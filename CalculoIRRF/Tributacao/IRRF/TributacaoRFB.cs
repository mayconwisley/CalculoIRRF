using CalculoIRRF.Objetos.Tributacao;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace CalculoIRRF.Tributacao.IRRF
{
    public class TributacaoRFB
    {
        public async Task BuscarIRRFOnline()
        {
            IrrfRfb irrfRfb = new IrrfRfb();

            string urlRfb = @"https://www.gov.br/receitafederal/pt-br/assuntos/meu-imposto-de-renda/tabelas/";
            string ano = DateTime.Now.ToString("yyyy");

            urlRfb = $"{urlRfb}{ano}";

            try
            {
                var htmlClient = new HttpClient();
                string html = await htmlClient.GetStringAsync(urlRfb);

                var htmlDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.LoadHtml(html);

                var table = htmlDoc.DocumentNode.SelectSingleNode("//table");

                if (table != null)
                {
                    var rows = table.SelectNodes(".//tr");

                    foreach (var row in rows)
                    {
                        var cells = row.SelectNodes(".//td");
                        if (cells != null)
                        {
                            foreach (var cell in cells)
                            {
                                var teste = $"{cell.InnerText.Trim()}\t";
                            }
                        }
                    }
                }
                else
                {
                    string erro = "Não encontrado";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
