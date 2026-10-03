#nullable enable

using HtmlAgilityPack;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CalculoIRRF.Infrastructure.Tributacao.Fontes;

/// <summary>
/// Tabela anual da PLR na página de tabelas da Receita Federal. Os sites alternativos não a publicam, e a fonte oficial
/// vale sozinha; ela é atualizada junto com a tabela mensal do IRRF.
/// </summary>
public sealed partial class FontePlrReceitaFederal : IFonteTabela<TabelaPlrPublicada>
{
    // Título de cada tabela da seção: "A partir de maio de 2025" ou "De janeiro a abril de 2025".
    [GeneratedRegex(@"a partir de (?<mes>[a-z]+)(?: de)? (?<ano>20\d{2})", RegexOptions.IgnoreCase)]
    private static partial Regex VigenciaAPartirDe { get; }

    public string Nome => "Receita Federal";

    public bool Oficial => true;

    public Uri Endereco { get; } = new(PaginaReceitaFederal.CatalogoUrl);

    public async Task<TabelaPlrPublicada> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var (pagina, documento) = await PaginaReceitaFederal.ObterPaginaMaisRecenteAsync(httpClient, Endereco, cancellationToken);
        var secao = (documento.DocumentNode.SelectNodes("//h2") ?? Enumerable.Empty<HtmlNode>())
            .FirstOrDefault(titulo => LeituraDePagina.Normalizar(titulo.InnerText).Contains("participacao nos lucros", StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException("A tabela da PLR não foi encontrada na página da Receita Federal.");
        // A primeira tabela da seção é a vigente; as seguintes são as de períodos anteriores do mesmo ano.
        var tabela = secao.SelectSingleNode("following::table[1]")
            ?? throw new InvalidOperationException("A tabela da PLR não foi encontrada na página da Receita Federal.");
        var titulo = tabela.SelectSingleNode("preceding::h3[1]");

        return new TabelaPlrPublicada(ExtrairCompetencia(titulo?.InnerText, pagina), LeituraDePagina.LerFaixasIrrf(LeituraDePagina.LerLinhas(tabela)));
    }

    private static DateOnly ExtrairCompetencia(string? titulo, Uri pagina)
    {
        var match = VigenciaAPartirDe.Match(LeituraDePagina.Normalizar(titulo ?? string.Empty));
        if (match.Success && LeituraDePagina.ObterMes(match.Groups["mes"].Value) is { } mes)
            return new DateOnly(int.Parse(match.Groups["ano"].Value, CultureInfo.InvariantCulture), mes, 1);

        // Sem a data no título, a tabela vale desde o início do ano da página.
        return PaginaReceitaFederal.AnoDaPagina(pagina) is { } ano
            ? new DateOnly(ano, 1, 1)
            : throw new InvalidOperationException("Não foi possível identificar a vigência da tabela da PLR.");
    }
}
