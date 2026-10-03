#nullable enable

using HtmlAgilityPack;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CalculoIRRF.Infrastructure.Tributacao.Fontes;

/// <summary>
/// O catálogo de tabelas da Receita Federal aponta uma página por ano; a mais recente traz a tabela mensal do IRRF,
/// a redução mensal e a tabela anual da PLR.
/// </summary>
internal static partial class PaginaReceitaFederal
{
    public const string CatalogoUrl = "https://www.gov.br/receitafederal/pt-br/assuntos/meu-imposto-de-renda/tabelas";

    [GeneratedRegex(@"/tabelas/(?<ano>20\d{2})/?$", RegexOptions.IgnoreCase)]
    private static partial Regex AnoNaUrl { get; }

    public static async Task<(Uri Endereco, HtmlDocument Documento)> ObterPaginaMaisRecenteAsync(HttpClient httpClient, Uri catalogo, CancellationToken cancellationToken)
    {
        var indice = await LeituraDePagina.ObterDocumentoAsync(httpClient, catalogo, cancellationToken);
        var pagina = (indice.DocumentNode.SelectNodes("//a[@href]") ?? Enumerable.Empty<HtmlNode>())
            .Select(link => Uri.TryCreate(catalogo, link.GetAttributeValue("href", string.Empty), out var uri) && uri.Scheme == Uri.UriSchemeHttps ? uri : null)
            .Where(uri => uri is not null)
            .Select(uri => (Uri: uri!, Ano: AnoDaPagina(uri!)))
            .Where(item => item.Ano is not null)
            .OrderByDescending(item => item.Ano)
            .Select(item => item.Uri)
            .FirstOrDefault()
            ?? throw new InvalidOperationException("A Receita Federal não disponibilizou uma página anual de tabelas para consulta.");

        return (pagina, await LeituraDePagina.ObterDocumentoAsync(httpClient, pagina, cancellationToken));
    }

    public static int? AnoDaPagina(Uri pagina)
    {
        var match = AnoNaUrl.Match(pagina.AbsolutePath);
        return match.Success ? int.Parse(match.Groups["ano"].Value, CultureInfo.InvariantCulture) : null;
    }
}
