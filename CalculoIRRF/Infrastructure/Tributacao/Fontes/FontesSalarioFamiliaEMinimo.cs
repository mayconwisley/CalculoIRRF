#nullable enable

using HtmlAgilityPack;
using System.Globalization;
using System.Net.Http;
using System.Text.RegularExpressions;

namespace CalculoIRRF.Infrastructure.Tributacao.Fontes;

/// <summary>Página oficial do INSS com o limite de remuneração e a cota do salário-família de cada período.</summary>
public sealed partial class FonteSalarioFamiliaGovBr : IFonteTabela<TabelaSalarioFamiliaPublicada>
{
    // "A partir de 01/01/2026" ou "A partir de 1º/01/2022".
    [GeneratedRegex(@"a partir de (?<dia>\d{1,2})º?/(?<mes>\d{1,2})/(?<ano>\d{4})", RegexOptions.IgnoreCase)]
    private static partial Regex Vigencia { get; }

    // "Até 1.980,38 cota 67,54" ou "de 907,77 a 1.364,43 cota 32,80".
    [GeneratedRegex(@"(?<limite>\d{1,3}(?:\.\d{3})*,\d{2})\s*cota\s*(?<cota>\d{1,3}(?:\.\d{3})*,\d{2})", RegexOptions.IgnoreCase)]
    private static partial Regex FaixaComCota { get; }

    public string Nome => "gov.br (INSS)";

    public bool Oficial => true;

    public Uri Endereco { get; } = new("https://www.gov.br/inss/pt-br/direitos-e-deveres/salario-familia/valor-limite-para-direito-ao-salario-familia");

    public async Task<TabelaSalarioFamiliaPublicada> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var documento = await LeituraDePagina.ObterDocumentoAsync(httpClient, Endereco, cancellationToken);
        // Uma linha por período: a vigência, uma coluna por faixa e o normativo; vale a mais recente.
        var maisRecente = (documento.DocumentNode.SelectNodes("//table") ?? Enumerable.Empty<HtmlNode>())
            .SelectMany(LeituraDePagina.LerLinhas)
            .Select(colunas => (Colunas: colunas, Vigencia: colunas.Length > 0 ? Vigencia.Match(LeituraDePagina.Normalizar(colunas[0])) : Match.Empty))
            .Where(item => item.Vigencia.Success)
            .Select(item => (Competencia: new DateOnly(int.Parse(item.Vigencia.Groups["ano"].Value, CultureInfo.InvariantCulture), int.Parse(item.Vigencia.Groups["mes"].Value, CultureInfo.InvariantCulture), 1), item.Colunas))
            .OrderByDescending(item => item.Competencia)
            .FirstOrDefault();

        if (maisRecente.Colunas is null)
            throw new InvalidOperationException("A tabela do salário-família não foi encontrada na página do INSS.");

        var faixas = maisRecente.Colunas.Skip(1)
            .Select(coluna => FaixaComCota.Match(coluna))
            .Where(match => match.Success)
            .Select(match => new FaixaSalarioFamiliaPublicada(LeituraDePagina.ExtrairUltimoValor(match.Groups["limite"].Value), LeituraDePagina.ExtrairUltimoValor(match.Groups["cota"].Value)))
            .ToArray();
        return new TabelaSalarioFamiliaPublicada(maisRecente.Competencia, faixas);
    }
}

/// <summary>
/// Salário mínimo pela tabela oficial de contribuição do INSS: desde a EC 103/2019 (art. 28), a 1ª faixa vai até
/// exatamente um salário mínimo.
/// </summary>
public sealed class FonteSalarioMinimoGovBr : IFonteTabela<SalarioMinimoPublicado>
{
    private readonly FonteInssGovBr _inss = new();

    public string Nome => "gov.br (INSS)";

    public bool Oficial => true;

    public Uri Endereco => _inss.Endereco;

    public async Task<SalarioMinimoPublicado> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var inss = await _inss.ObterAsync(httpClient, cancellationToken);
        return new SalarioMinimoPublicado(inss.Competencia, inss.Faixas.Count > 0 ? inss.Faixas[0].Limite : 0m);
    }
}

/// <summary>Salário mínimo pela 1ª faixa da tabela do INSS publicada pelo Debit (EC 103/2019, art. 28).</summary>
public sealed class FonteSalarioMinimoDebit : IFonteTabela<SalarioMinimoPublicado>
{
    private readonly FonteInssDebit _inss = new();

    public string Nome => "debit.com.br";

    public bool Oficial => false;

    public Uri Endereco => _inss.Endereco;

    public async Task<SalarioMinimoPublicado> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var inss = await _inss.ObterAsync(httpClient, cancellationToken);
        return new SalarioMinimoPublicado(inss.Competencia, inss.Faixas.Count > 0 ? inss.Faixas[0].Limite : 0m);
    }
}

/// <summary>Salário-família do Debit: tabelas com a vigência no título e uma linha por faixa (limite e cota).</summary>
public sealed partial class FonteSalarioFamiliaDebit : IFonteTabela<TabelaSalarioFamiliaPublicada>
{
    // Título de cada tabela: "INSS a partir de 01/01/2026".
    [GeneratedRegex(@"a partir de \d{2}/(?<mes>\d{2})/(?<ano>\d{4})", RegexOptions.IgnoreCase)]
    private static partial Regex Vigencia { get; }

    public string Nome => "debit.com.br";

    public bool Oficial => false;

    public Uri Endereco { get; } = new("https://www.debit.com.br/tabelas/salario-familia");

    public async Task<TabelaSalarioFamiliaPublicada> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var (competencia, linhas) = await PaginaDebit.ObterTabelaMaisRecenteAsync(httpClient, Endereco, Vigencia, cancellationToken);
        return new TabelaSalarioFamiliaPublicada(competencia, LerFaixas(linhas));
    }

    /// <summary>Linhas "limite de remuneração | cota"; a linha "acima de", sem cota, fica de fora.</summary>
    internal static FaixaSalarioFamiliaPublicada[] LerFaixas(IEnumerable<string[]> linhas) => linhas
        .Where(colunas => colunas.Length >= 2 && LeituraDePagina.TemValorMonetario(colunas[0]) && LeituraDePagina.TemValorMonetario(colunas[1]))
        .Select(colunas => new FaixaSalarioFamiliaPublicada(LeituraDePagina.ExtrairUltimoValor(colunas[0]), LeituraDePagina.ExtrairUltimoValor(colunas[1])))
        .ToArray();
}

/// <summary>Salário-família do Portal Contábeis, no mesmo formato de seções das demais tabelas do site.</summary>
public sealed class FonteSalarioFamiliaContabeis : IFonteTabela<TabelaSalarioFamiliaPublicada>
{
    public string Nome => "contabeis.com.br";

    public bool Oficial => false;

    public Uri Endereco { get; } = new("https://www.contabeis.com.br/tabelas/salario-familia/");

    public async Task<TabelaSalarioFamiliaPublicada> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var (competencia, linhas) = await PaginaContabeis.ObterTabelaMaisRecenteAsync(httpClient, Endereco, cancellationToken);
        return new TabelaSalarioFamiliaPublicada(competencia, FonteSalarioFamiliaDebit.LerFaixas(linhas));
    }
}

/// <summary>Salário mínimo do Portal Contábeis: cada linha traz o ano, a data de vigência, o valor e o ato legal.</summary>
public sealed class FonteSalarioMinimoContabeis : IFonteTabela<SalarioMinimoPublicado>
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");

    public string Nome => "contabeis.com.br";

    public bool Oficial => false;

    public Uri Endereco { get; } = new("https://www.contabeis.com.br/tabelas/salario-minimo/");

    public async Task<SalarioMinimoPublicado> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken)
    {
        var (_, linhas) = await PaginaContabeis.ObterTabelaMaisRecenteAsync(httpClient, Endereco, cancellationToken);
        // A data de vigência da linha é mais precisa que o título da seção, por exemplo nos reajustes de maio.
        var maisRecente = linhas
            .Where(colunas => colunas.Length >= 3 && LeituraDePagina.TemValorMonetario(colunas[2]))
            .Select(colunas => (Valido: DateOnly.TryParseExact(colunas[1].Trim(), "dd/MM/yyyy", Cultura, DateTimeStyles.None, out var vigencia), Vigencia: vigencia, Valor: LeituraDePagina.ExtrairUltimoValor(colunas[2])))
            .Where(item => item.Valido)
            .OrderByDescending(item => item.Vigencia)
            .FirstOrDefault();

        if (!maisRecente.Valido)
            throw new InvalidOperationException("O salário mínimo não foi encontrado na página.");
        return new SalarioMinimoPublicado(new DateOnly(maisRecente.Vigencia.Year, maisRecente.Vigencia.Month, 1), maisRecente.Valor);
    }
}
