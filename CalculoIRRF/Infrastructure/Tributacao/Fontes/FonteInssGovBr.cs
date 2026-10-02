#nullable enable

using HtmlAgilityPack;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CalculoIRRF.Infrastructure.Tributacao.Fontes;

/// <summary>Página oficial do INSS com a tabela dos segurados empregado, doméstico e trabalhador avulso.</summary>
public sealed partial class FonteInssGovBr : IFonteTabela<TabelaInssPublicada>
{
    [GeneratedRegex(@"(?:competencia\s+)?janeiro de (?<ano>20\d{2})", RegexOptions.IgnoreCase)]
    private static partial Regex CompetenciaJaneiro { get; }

    public string Nome => "gov.br (INSS)";

    public bool Oficial => true;

    public Uri Endereco { get; } = new("https://www.gov.br/inss/pt-br/direitos-e-deveres/inscricao-e-contribuicao/tabela-de-contribuicao-mensal");

    public async Task<TabelaInssPublicada> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var documento = await LeituraDePagina.ObterDocumentoAsync(httpClient, Endereco, cancellationToken);
        var tabela = (documento.DocumentNode.SelectNodes("//table") ?? Enumerable.Empty<HtmlNode>())
            .FirstOrDefault(item => LeituraDePagina.Normalizar(item.InnerText).Contains("aliquota progressiva para fins de recolhimento ao inss", StringComparison.OrdinalIgnoreCase))
            ?? throw new InvalidOperationException("A tabela de INSS para empregados não foi encontrada na página.");

        return new TabelaInssPublicada(ExtrairCompetencia(documento.DocumentNode.InnerText), LeituraDePagina.LerFaixasInss(LeituraDePagina.LerLinhas(tabela)));
    }

    private static DateOnly ExtrairCompetencia(string conteudo)
    {
        var match = CompetenciaJaneiro.Match(LeituraDePagina.Normalizar(conteudo));
        if (!match.Success || !int.TryParse(match.Groups["ano"].Value, out var ano))
            throw new InvalidOperationException("Não foi possível identificar a competência da tabela de INSS.");

        return new DateOnly(ano, 1, 1);
    }
}
