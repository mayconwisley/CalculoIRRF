#nullable enable

using HtmlAgilityPack;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace CalculoIRRF.Infrastructure.Tributacao;

/// <summary>Leitura das páginas de tabelas, comum a todas as fontes.</summary>
internal static partial class LeituraDePagina
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");
    private static readonly string[] Meses = Cultura.DateTimeFormat.MonthNames.Take(12).Select(mes => Normalizar(mes).ToLowerInvariant()).ToArray();

    [GeneratedRegex(@"\d{1,3}(?:\.\d{3})*,\d{2}")]
    private static partial Regex ValorMonetario { get; }

    // Aceita vírgula ou ponto decimal: há fonte que publica "7.5%".
    [GeneratedRegex(@"\d{1,2}(?:[,.]\d+)?")]
    private static partial Regex Percentual { get; }

    public static async Task<HtmlDocument> ObterDocumentoAsync(HttpClient httpClient, Uri endereco, CancellationToken cancellationToken)
    {
        using var resposta = await httpClient.GetAsync(endereco, cancellationToken);
        resposta.EnsureSuccessStatusCode();

        var documento = new HtmlDocument();
        documento.LoadHtml(await resposta.Content.ReadAsStringAsync(cancellationToken));
        return documento;
    }

    /// <summary>Texto das células de cada linha com dados (as linhas de cabeçalho, só com &lt;th&gt;, ficam de fora).</summary>
    public static string[][] LerLinhas(HtmlNode tabela) =>
        (tabela.SelectNodes(".//tr[td]") ?? Enumerable.Empty<HtmlNode>())
            .Select(linha => LerTextos(linha.SelectNodes("./td")))
            .ToArray();

    public static string[] LerTextos(HtmlNodeCollection? nos) =>
        (nos ?? Enumerable.Empty<HtmlNode>()).Select(no => WebUtility.HtmlDecode(no.InnerText).Trim()).ToArray();

    /// <summary>Faixas de INSS a partir de linhas "salário de contribuição | alíquota".</summary>
    public static FaixaInssPublicada[] LerFaixasInss(IEnumerable<string[]> linhas) => linhas
        .Where(colunas => colunas.Length >= 2 && ValorMonetario.IsMatch(colunas[0]))
        .Select(colunas => new FaixaInssPublicada(ExtrairUltimoValor(colunas[0]), ExtrairPercentual(colunas[1])))
        .ToArray();

    /// <summary>Faixas do IRRF a partir de linhas "base de cálculo | alíquota | dedução"; a última faixa recebe o limite que representa "acima de".</summary>
    public static FaixaIrrfPublicada[] LerFaixasIrrf(IEnumerable<string[]> linhas)
    {
        var faixas = linhas
            .Where(colunas => colunas.Length >= 3 && ValorMonetario.IsMatch(colunas[0]))
            .Select(colunas => new FaixaIrrfPublicada(ExtrairUltimoValor(colunas[0]), ExtrairPercentual(colunas[1]), ExtrairUltimoValor(colunas[2])))
            .ToArray();
        if (faixas.Length > 0)
            faixas[^1] = faixas[^1] with { Limite = TabelaIrrfPublicada.LimiteUltimaFaixa };
        return faixas;
    }

    public static decimal ExtrairUltimoValor(string texto) =>
        ValorMonetario.Matches(texto).Select(match => decimal.Parse(match.Value, Cultura)).LastOrDefault();

    public static decimal ExtrairPrimeiroValor(string texto)
    {
        var match = ValorMonetario.Match(texto);
        return match.Success ? decimal.Parse(match.Value, Cultura) : 0m;
    }

    /// <summary>Alíquota da célula; "isento" ou "-" resultam em zero.</summary>
    public static decimal ExtrairPercentual(string texto)
    {
        var match = Percentual.Match(texto);
        return match.Success ? decimal.Parse(match.Value.Replace(',', '.'), CultureInfo.InvariantCulture) : 0m;
    }

    /// <summary>Número do mês por extenso, como "Março" ou "marco"; nulo se o texto não for um mês.</summary>
    public static int? ObterMes(string nome)
    {
        var indice = Array.IndexOf(Meses, Normalizar(nome).ToLowerInvariant());
        return indice >= 0 ? indice + 1 : null;
    }

    public static string Normalizar(string valor)
    {
        var semAcentos = string.Concat(WebUtility.HtmlDecode(valor)
            .Normalize(NormalizationForm.FormD)
            .Where(caractere => CharUnicodeInfo.GetUnicodeCategory(caractere) != UnicodeCategory.NonSpacingMark));
        return semAcentos.Replace(' ', ' ').Trim();
    }
}
