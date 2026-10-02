#nullable enable

using CalculoIRRF.Application.Management;
using CalculoIRRF.Infrastructure.Persistence;
using HtmlAgilityPack;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;

namespace CalculoIRRF.Infrastructure.Tributacao;

/// <summary>
/// Adaptador da fonte pública oficial. A gravação só ocorre após a página e todos os valores
/// obrigatórios serem validados, preservando a última tabela local em caso de falha da fonte.
/// </summary>
public sealed partial class AtualizadorTabelaIrrfDaReceitaFederal(
    Func<HttpClient> criarHttpClient,
    BancoTributario banco,
    ICacheTabelasTributarias cache) : IAtualizadorTabelaIrrf
{
    private const string CatalogoUrl = "https://www.gov.br/receitafederal/pt-br/assuntos/meu-imposto-de-renda/tabelas";
    private const decimal LimiteUltimaFaixa = 9_999_999_999_999.99m;
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");

    [GeneratedRegex(@"/tabelas/(?<ano>20\d{2})/?$", RegexOptions.IgnoreCase)]
    private static partial Regex AnoNaUrl { get; }

    [GeneratedRegex(@"\d{1,3}(?:\.\d{3})*,\d{2}")]
    private static partial Regex ValorMonetario { get; }

    [GeneratedRegex(@"\d{1,2}(?:,\d+)?")]
    private static partial Regex Percentual { get; }

    [GeneratedRegex(@"0,\d{3,6}")]
    private static partial Regex MultiplicadorReducao { get; }

    [GeneratedRegex(@"a partir de (?:janeiro de )?(?<ano>20\d{2})", RegexOptions.IgnoreCase)]
    private static partial Regex VigenciaAPartirDe { get; }

    public Uri FonteOficial => new(CatalogoUrl);

    public async Task<AtualizacaoTabelaIrrfResultado> AtualizarAsync(CancellationToken cancellationToken)
    {
        using var httpClient = criarHttpClient();
        var fonte = await ObterPaginaMaisRecenteAsync(httpClient, cancellationToken);
        var documento = await ObterDocumentoAsync(httpClient, fonte, cancellationToken);
        var tabela = ExtrairTabela(documento, fonte);

        await PersistirAsync(tabela, cancellationToken);

        return new AtualizacaoTabelaIrrfResultado(tabela.Competencia, tabela.Faixas.Count, fonte);
    }

    private async Task<Uri> ObterPaginaMaisRecenteAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var catalogo = await ObterDocumentoAsync(httpClient, FonteOficial, cancellationToken);
        var fontes = (catalogo.DocumentNode.SelectNodes("//a[@href]") ?? Enumerable.Empty<HtmlNode>())
            .Select(link => link.GetAttributeValue("href", string.Empty))
            .Select(CriarUriAbsoluta)
            .Where(uri => uri is not null)
            .Select(uri => uri!)
            .Select(uri => (Uri: uri, Match: AnoNaUrl.Match(uri.AbsolutePath)))
            .Where(item => item.Match.Success)
            .OrderByDescending(item => int.Parse(item.Match.Groups["ano"].Value, CultureInfo.InvariantCulture))
            .Select(item => item.Uri)
            .Distinct()
            .ToArray();

        if (fontes.Length == 0)
            throw new InvalidOperationException("A Receita Federal não disponibilizou uma página anual de tabelas para consulta.");

        return fontes[0];
    }

    private static async Task<HtmlDocument> ObterDocumentoAsync(HttpClient httpClient, Uri uri, CancellationToken cancellationToken)
    {
        using var resposta = await httpClient.GetAsync(uri, cancellationToken);
        resposta.EnsureSuccessStatusCode();

        var html = await resposta.Content.ReadAsStringAsync(cancellationToken);
        var documento = new HtmlDocument();
        documento.LoadHtml(html);
        return documento;
    }

    private static Uri? CriarUriAbsoluta(string href) =>
        Uri.TryCreate(new Uri(CatalogoUrl), href, out var uri) && uri.Scheme == Uri.UriSchemeHttps ? uri : null;

    private static TabelaIrrfOnline ExtrairTabela(HtmlDocument documento, Uri fonte)
    {
        var titulo = (documento.DocumentNode.SelectNodes("//h2|//h3") ?? Enumerable.Empty<HtmlNode>())
            .FirstOrDefault(node => Normalizar(node.InnerText).Contains("tabela de incidencia mensal", StringComparison.OrdinalIgnoreCase));
        var tabela = titulo?.SelectSingleNode("following::table[1]")
            ?? throw new InvalidOperationException("A tabela de incidência mensal não foi encontrada na página da Receita Federal.");

        var faixas = (tabela.SelectNodes(".//tr[td]") ?? Enumerable.Empty<HtmlNode>())
            .Select(linha => linha.SelectNodes("./td")?.Select(coluna => WebUtility.HtmlDecode(coluna.InnerText).Trim()).ToArray() ?? Array.Empty<string>())
            .Where(colunas => colunas.Length >= 3)
            .Select(colunas => new FaixaIrrfOnline(ExtrairUltimoValor(colunas[0]), ExtrairPercentual(colunas[1]), ExtrairUltimoValor(colunas[2])))
            .ToList();

        if (faixas.Count != 5 || faixas.Select(faixa => faixa.Aliquota).Distinct().Count() != 5)
            throw new InvalidOperationException("A página oficial retornou uma estrutura de faixas IRRF inválida. Nenhum dado foi alterado.");

        faixas[^1] = faixas[^1] with { Limite = LimiteUltimaFaixa };
        var conteudo = WebUtility.HtmlDecode(documento.DocumentNode.InnerText);
        var competencia = ExtrairCompetencia(titulo.ParentNode?.InnerText ?? documento.DocumentNode.InnerText, fonte);
        var dependente = ExtrairValorRotulo(conteudo, "Dedução mensal por dependente");
        var simplificado = ExtrairValorRotulo(conteudo, "Limite mensal de desconto simplificado");
        var reducoes = ExtrairReducoesMensais(documento);

        return new TabelaIrrfOnline(competencia, faixas, dependente, simplificado, reducoes);
    }

    private static IReadOnlyList<ReducaoMensalOnline> ExtrairReducoesMensais(HtmlDocument documento)
    {
        var titulo = (documento.DocumentNode.SelectNodes("//h2|//h3") ?? Enumerable.Empty<HtmlNode>())
            .FirstOrDefault(node => Normalizar(node.InnerText).Contains("tabela de reducao mensal", StringComparison.OrdinalIgnoreCase));
        var tabela = titulo?.SelectSingleNode("following::table[1]")
            ?? throw new InvalidOperationException("A tabela de redução mensal não foi encontrada na página da Receita Federal.");

        var reducoes = (tabela.SelectNodes(".//tr[td]") ?? Enumerable.Empty<HtmlNode>())
            .Select(linha => linha.SelectNodes("./td")?.Select(coluna => WebUtility.HtmlDecode(coluna.InnerText).Trim()).ToArray() ?? Array.Empty<string>())
            .Where(colunas => colunas.Length >= 2)
            .Select((colunas, indice) => new ReducaoMensalOnline(
                indice + 1,
                ExtrairUltimoValor(colunas[0]),
                indice == 0 ? 0m : ExtrairMultiplicadorReducao(colunas[1]),
                ExtrairPrimeiroValor(colunas[1])))
            .ToArray();

        if (reducoes.Length != 2 ||
            reducoes[0].LimiteRendimentos != 5_000m ||
            reducoes[0].ValorBase <= 0m ||
            reducoes[1].LimiteRendimentos != 7_350m ||
            reducoes[1].Multiplicador <= 0m ||
            reducoes[1].ValorBase <= 0m ||
            Math.Abs(reducoes[1].ValorBase - reducoes[1].Multiplicador * reducoes[1].LimiteRendimentos) > 0.01m)
            throw new InvalidOperationException("A página oficial retornou uma estrutura de redução mensal inválida. Nenhum dado foi alterado.");

        return reducoes;
    }

    private async Task PersistirAsync(TabelaIrrfOnline tabela, CancellationToken cancellationToken)
    {
        var competencia = tabela.Competencia.ToDateTime(TimeOnly.MinValue);
        await using var conexao = await banco.AbrirAsync(cancellationToken);
        await using var transacao = conexao.BeginTransaction();

        await conexao.ExecutarAsync(transacao, "DELETE FROM Irrf WHERE Competencia = $competencia", cancellationToken, ("$competencia", competencia));
        for (var indice = 0; indice < tabela.Faixas.Count; indice++)
        {
            var faixa = tabela.Faixas[indice];
            await conexao.ExecutarAsync(transacao, "INSERT INTO Irrf (Competencia, Faixa, Valor, Porcentagem, Deducao) VALUES ($competencia, $faixa, $valor, $porcentagem, $deducao)", cancellationToken,
                ("$competencia", competencia), ("$faixa", indice + 1), ("$valor", (double)faixa.Limite), ("$porcentagem", (double)faixa.Aliquota), ("$deducao", (double)faixa.Deducao));
        }

        await AtualizarParametroAsync(conexao, transacao, "Dependente", competencia, tabela.ValorDependente, cancellationToken);
        await AtualizarParametroAsync(conexao, transacao, "Simplificado", competencia, tabela.ValorSimplificado, cancellationToken);

        await conexao.ExecutarAsync(transacao, "DELETE FROM ReducaoMensalIrrf WHERE Competencia = $competencia", cancellationToken, ("$competencia", competencia));
        foreach (var reducao in tabela.ReducoesMensais)
            await conexao.ExecutarAsync(transacao, "INSERT INTO ReducaoMensalIrrf (Competencia, Faixa, LimiteRendimentos, Multiplicador, ValorBase) VALUES ($competencia, $faixa, $limite, $multiplicador, $valorBase)", cancellationToken,
                ("$competencia", competencia), ("$faixa", reducao.Faixa), ("$limite", (double)reducao.LimiteRendimentos), ("$multiplicador", (double)reducao.Multiplicador), ("$valorBase", (double)reducao.ValorBase));

        await transacao.CommitAsync(cancellationToken);
        cache.Invalidar();
    }

    private static async Task AtualizarParametroAsync(SqliteConnection conexao, SqliteTransaction transacao, string tabela, DateTime competencia, decimal valor, CancellationToken cancellationToken)
    {
        var ids = await conexao.ListarAsync(transacao, $"SELECT Id FROM {tabela} WHERE Competencia = $competencia LIMIT 2", leitor => leitor.GetInt32(0), cancellationToken, ("$competencia", competencia));
        if (ids.Length > 1)
            throw new InvalidOperationException($"Há mais de um registro em {tabela} para a competência {competencia:MM/yyyy}. Nenhum dado foi alterado.");

        if (ids.Length == 0)
            await conexao.ExecutarAsync(transacao, $"INSERT INTO {tabela} (Competencia, Valor) VALUES ($competencia, $valor)", cancellationToken, ("$competencia", competencia), ("$valor", (double)valor));
        else
            await conexao.ExecutarAsync(transacao, $"UPDATE {tabela} SET Valor = $valor WHERE Id = $id", cancellationToken, ("$valor", (double)valor), ("$id", ids[0]));
    }

    private static DateOnly ExtrairCompetencia(string conteudo, Uri fonte)
    {
        var match = VigenciaAPartirDe.Match(Normalizar(conteudo));
        if (!match.Success)
            match = AnoNaUrl.Match(fonte.AbsolutePath);

        if (!match.Success || !int.TryParse(match.Groups["ano"].Value, out var ano))
            throw new InvalidOperationException("Não foi possível identificar a vigência da tabela oficial.");

        return new DateOnly(ano, 1, 1);
    }

    private static decimal ExtrairUltimoValor(string texto)
    {
        var valores = ValorMonetario.Matches(texto).Select(match => decimal.Parse(match.Value, Cultura)).ToArray();
        return valores.LastOrDefault();
    }

    private static decimal ExtrairPrimeiroValor(string texto)
    {
        var match = ValorMonetario.Match(texto);
        return match.Success ? decimal.Parse(match.Value, Cultura) : 0m;
    }

    private static decimal ExtrairPercentual(string texto)
    {
        var match = Percentual.Match(texto);
        return match.Success ? decimal.Parse(match.Value, Cultura) : 0m;
    }

    private static decimal ExtrairMultiplicadorReducao(string texto)
    {
        var match = MultiplicadorReducao.Match(texto);
        return match.Success ? decimal.Parse(match.Value, Cultura) : 0m;
    }

    private static decimal ExtrairValorRotulo(string conteudo, string rotulo)
    {
        var match = Regex.Match(conteudo, $@"{Regex.Escape(rotulo)}\s*:\s*(?:R\$\s*)?(?<valor>\d{{1,3}}(?:\.\d{{3}})*,\d{{2}})", RegexOptions.IgnoreCase);
        if (!match.Success)
            throw new InvalidOperationException($"O valor de '{rotulo}' não foi encontrado na página oficial.");

        return decimal.Parse(match.Groups["valor"].Value, Cultura);
    }

    private static string Normalizar(string valor)
    {
        var semAcentos = string.Concat(WebUtility.HtmlDecode(valor)
            .Normalize(NormalizationForm.FormD)
            .Where(caractere => CharUnicodeInfo.GetUnicodeCategory(caractere) != UnicodeCategory.NonSpacingMark));
        return semAcentos.Replace('\u00A0', ' ').Trim();
    }

    private sealed record TabelaIrrfOnline(DateOnly Competencia, IReadOnlyList<FaixaIrrfOnline> Faixas, decimal ValorDependente, decimal ValorSimplificado, IReadOnlyList<ReducaoMensalOnline> ReducoesMensais);
    private sealed record FaixaIrrfOnline(decimal Limite, decimal Aliquota, decimal Deducao);
    private sealed record ReducaoMensalOnline(int Faixa, decimal LimiteRendimentos, decimal Multiplicador, decimal ValorBase);
}
