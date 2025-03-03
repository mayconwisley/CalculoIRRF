namespace CalculoIRRF.Tributacao.IRRF;

public class TributacaoRFB
{
    //public async Task AtualizarOnline()
    //{
    //    IrrfServices cadastro;
    //    IrrfRfb irrfRfb;
    //    Irrf irrf;

    //    //string urlRfb = $@"https://www.gov.br/receitafederal/pt-br/assuntos/meu-imposto-de-renda/tabelas/{DateTime.Now:yyyy}";
    //    string urlRfb = $@"https://www.gov.br/receitafederal/pt-br/assuntos/meu-imposto-de-renda/tabelas/2014";
    //    var htmlDocument = await AcessarUrl.AcessarSite(urlRfb);

    //    if (htmlDocument is null)
    //    {
    //        return;
    //    }

    //    var dataPublicacao = BuscarDataPublicacaoOnline(htmlDocument);
    //    var dataAtualizacao = BuscarDataAtualizacaoOnline(htmlDocument);

    //    var valorDependente = BuscarValorDependenteOnline(htmlDocument);
    //    var valorDescontoSimplificado = BuscarDescontoSimplicadoOnline(htmlDocument);

    //    var tributacaoRFB = BuscarIRRFOnline(htmlDocument);
    //    var countItem = tributacaoRFB.Count;

    //    foreach (var item in tributacaoRFB)
    //    {
    //        cadastro = new IrrfServices();
    //        irrfRfb = new IrrfRfb
    //        {
    //            DataCriacao = dataPublicacao,
    //            DataAtualizacao = dataAtualizacao,
    //            Dependente = valorDependente,
    //            Simplificado = valorDescontoSimplificado,
    //            Sequencia = item.Sequencia,
    //            BaseCaculo = item.BaseCalculo,
    //            Aliquota = item.Aliquota,
    //            Deducao = item.Deducao
    //        };

    //        if (countItem == item.Sequencia)
    //        {
    //            irrfRfb.BaseCaculo = decimal.Parse("9999999999999.99");
    //        }
    //        //  cadastro.GravarRfbOnline(irrfRfb);

    //        irrf = new Irrf
    //        {
    //            Competencia = DateTime.Parse(dataAtualizacao.Date.ToString("MM/yyyy")),
    //            Faixa = item.Sequencia,
    //            Valor = item.BaseCalculo,
    //            Porcentagem = item.Aliquota,
    //            Deducao = item.Deducao
    //        };

    //        if (countItem == item.Sequencia)
    //        {
    //            irrf.Valor = decimal.Parse("9999999999999.99");
    //        }
    //        await cadastro.Gravar(irrf);

    //    }
    //}
    //private List<TributacaoRFBObj> BuscarIRRFOnline(HtmlDocument htmlDocument)
    //{
    //    Validar validar = new Validar();
    //    List<TributacaoRFBObj> listTributacaoRFBObj = new List<TributacaoRFBObj>();
    //    try
    //    {
    //        var table = htmlDocument.DocumentNode.SelectSingleNode("//table");

    //        if (table != null)
    //        {
    //            var rows = table.SelectNodes(".//tr");
    //            int sequencia = 0;
    //            foreach (var row in rows)
    //            {
    //                var cells = row.SelectNodes(".//td");
    //                if (cells != null)
    //                {
    //                    listTributacaoRFBObj.Add(new TributacaoRFBObj
    //                    {
    //                        Sequencia = ++sequencia,
    //                        BaseCalculo = Validar.ExtrairMaiorValor(cells[0].InnerText.Trim()),
    //                        Aliquota = Validar.ExtrairValor(cells[1].InnerText.Trim()),
    //                        Deducao = Validar.ExtrairValor(cells[2].InnerText.TrimEnd())
    //                    });
    //                }
    //            }
    //            return listTributacaoRFBObj;
    //        }
    //        else
    //        {
    //            return new List<TributacaoRFBObj>();
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    //private decimal BuscarValorDependenteOnline(HtmlDocument htmlDocument)
    //{
    //    Validar validar = new Validar();

    //    try
    //    {
    //        var spans = htmlDocument.DocumentNode.SelectNodes("//p/em/span");
    //        if (spans != null)
    //        {
    //            string[] br = new string[1];
    //            br[0] = "<br>";

    //            var valores = spans[1].InnerHtml.Split(br, StringSplitOptions.None);
    //            var deducaoDependente = Validar.ExtrairValor(valores[0].Trim());
    //            return deducaoDependente;
    //        }
    //        return 0;
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    //private decimal BuscarDescontoSimplicadoOnline(HtmlDocument htmlDocument)
    //{

    //    Validar validar = new Validar();

    //    try
    //    {
    //        var spans = htmlDocument.DocumentNode.SelectNodes("//p/em/span");
    //        if (spans != null)
    //        {
    //            string[] br = new string[1];
    //            br[0] = "<br>";

    //            var valores = spans[1].InnerHtml.Split(br, StringSplitOptions.None);
    //            if (valores.Length == 1)
    //            {
    //                return 0;
    //            }
    //            var deducaoSimplificado = Validar.ExtrairValor(valores[1].Trim());

    //            return deducaoSimplificado;
    //        }
    //        return 0;
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    //private DateTime BuscarDataPublicacaoOnline(HtmlDocument htmlDocument)
    //{
    //    try
    //    {
    //        var dataPublicacao = htmlDocument.DocumentNode.SelectNodes("//span[contains(@class, 'documentPublished')]/span");
    //        if (dataPublicacao != null)
    //        {
    //            var data = DateTime.Parse(dataPublicacao[1].InnerText.Trim().Replace("h", ":"));
    //            return data;
    //        }
    //        return DateTime.Now;
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    //private DateTime BuscarDataAtualizacaoOnline(HtmlDocument htmlDocument)
    //{
    //    try
    //    {
    //        var dataPublicacao = htmlDocument.DocumentNode.SelectNodes("//span[contains(@class, 'documentModified')]/span");
    //        if (dataPublicacao != null)
    //        {
    //            var data = DateTime.Parse(dataPublicacao[1].InnerText.Trim().Replace("h", ":"));
    //            return data;
    //        }
    //        return DateTime.Now;
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
}
