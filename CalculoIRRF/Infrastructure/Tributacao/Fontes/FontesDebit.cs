#nullable enable

using HtmlAgilityPack;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CalculoIRRF.Infrastructure.Tributacao.Fontes;

/// <summary>Tabelas do INSS do Debit, que costuma publicar a tabela do ano antes da página oficial.</summary>
public sealed partial class FonteInssDebit : IFonteTabela<TabelaInssPublicada>
{
    // Título de cada tabela: "INSS a partir de 01/01/2026".
    [GeneratedRegex(@"a partir de \d{2}/(?<mes>\d{2})/(?<ano>\d{4})", RegexOptions.IgnoreCase)]
    private static partial Regex Vigencia { get; }

    public string Nome => "debit.com.br";

    public bool Oficial => false;

    public Uri Endereco { get; } = new("https://www.debit.com.br/tabelas/tabelas-inss");

    public async Task<TabelaInssPublicada> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var (competencia, linhas) = await PaginaDebit.ObterTabelaMaisRecenteAsync(httpClient, Endereco, Vigencia, cancellationToken);
        return new TabelaInssPublicada(competencia, LeituraDePagina.LerFaixasInss(linhas));
    }
}

/// <summary>Tabelas do IRRF do Debit. Trazem só as faixas, sem o desconto simplificado e a redução mensal.</summary>
public sealed partial class FonteIrrfDebit : IFonteTabela<TabelaIrrfPublicada>
{
    // Título de cada tabela: "Tabela de IRRF de 05/2025 a 10/2026".
    [GeneratedRegex(@"de (?<mes>\d{2})/(?<ano>\d{4})", RegexOptions.IgnoreCase)]
    private static partial Regex Vigencia { get; }

    public string Nome => "debit.com.br";

    public bool Oficial => false;

    public Uri Endereco { get; } = new("https://www.debit.com.br/tabelas/tabelas-irrf");

    public async Task<TabelaIrrfPublicada> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var (competencia, linhas) = await PaginaDebit.ObterTabelaMaisRecenteAsync(httpClient, Endereco, Vigencia, cancellationToken);
        return new TabelaIrrfPublicada(competencia, LeituraDePagina.LerFaixasIrrf(linhas));
    }
}

internal static class PaginaDebit
{
    /// <summary>A página traz o histórico completo, cada tabela com a vigência no título acima dela; devolve a mais recente.</summary>
    public static async Task<(DateOnly Competencia, string[][] Linhas)> ObterTabelaMaisRecenteAsync(HttpClient httpClient, Uri endereco, Regex vigencia, CancellationToken cancellationToken)
    {
        var documento = await LeituraDePagina.ObterDocumentoAsync(httpClient, endereco, cancellationToken);
        var maisRecente = (documento.DocumentNode.SelectNodes("//table") ?? Enumerable.Empty<HtmlNode>())
            .Select(tabela => (Tabela: tabela, Titulo: vigencia.Match(LeituraDePagina.Normalizar(tabela.SelectSingleNode("preceding::h2[1]")?.InnerText ?? string.Empty))))
            .Where(item => item.Titulo.Success)
            .Select(item => (Competencia: Competencia(item.Titulo), item.Tabela))
            .Where(item => item.Competencia is not null)
            .OrderByDescending(item => item.Competencia)
            .FirstOrDefault();

        if (maisRecente.Tabela is null)
            throw new InvalidOperationException("Nenhuma tabela com vigência foi encontrada na página.");

        return (maisRecente.Competencia!.Value, LeituraDePagina.LerLinhas(maisRecente.Tabela));
    }

    private static DateOnly? Competencia(Match titulo)
    {
        var mes = int.Parse(titulo.Groups["mes"].Value, CultureInfo.InvariantCulture);
        return mes is >= 1 and <= 12 ? new DateOnly(int.Parse(titulo.Groups["ano"].Value, CultureInfo.InvariantCulture), mes, 1) : null;
    }
}
