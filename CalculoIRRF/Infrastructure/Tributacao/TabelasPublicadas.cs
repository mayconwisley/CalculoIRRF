#nullable enable

using System.Net.Http;

namespace CalculoIRRF.Infrastructure.Tributacao;

/// <summary>Página na internet que publica uma tabela tributária.</summary>
public interface IFonteTabela<TTabela>
{
    /// <summary>Nome exibido ao usuário, como "debit.com.br".</summary>
    string Nome { get; }

    bool Oficial { get; }

    Uri Endereco { get; }

    /// <summary>Lê a tabela de vigência mais recente publicada na página.</summary>
    Task<TTabela> ObterAsync(HttpClient httpClient, CancellationToken cancellationToken);
}

public interface ITabelaPublicada<in TTabela>
{
    DateOnly Competencia { get; }

    /// <summary>Lança <see cref="InvalidOperationException"/> se a página trouxe uma estrutura que não corresponde à tabela.</summary>
    void Validar();

    /// <summary>Compara os valores que todas as fontes publicam, para confirmar a tabela de uma fonte com outra.</summary>
    bool TemMesmosValores(TTabela outra);
}

public sealed record FaixaInssPublicada(decimal Limite, decimal Aliquota);

public sealed record TabelaInssPublicada(DateOnly Competencia, IReadOnlyList<FaixaInssPublicada> Faixas) : ITabelaPublicada<TabelaInssPublicada>
{
    public void Validar()
    {
        if (Faixas.Count != 4 ||
            Faixas[0].Limite <= 0m ||
            Faixas[0].Aliquota <= 0m ||
            Faixas[^1].Aliquota >= 100m ||
            !Faixas.Select(faixa => faixa.Limite).EstritamenteCrescente() ||
            !Faixas.Select(faixa => faixa.Aliquota).EstritamenteCrescente())
            throw new InvalidOperationException("A página retornou uma estrutura de faixas INSS inválida.");
    }

    public bool TemMesmosValores(TabelaInssPublicada outra) => Faixas.SequenceEqual(outra.Faixas);
}

public sealed record FaixaIrrfPublicada(decimal Limite, decimal Aliquota, decimal Deducao);

public sealed record ReducaoMensalPublicada(int Faixa, decimal LimiteRendimentos, decimal Multiplicador, decimal ValorBase);

/// <param name="ValorDependente">Nulo quando a fonte não publica o valor; o valor já cadastrado continua valendo.</param>
/// <param name="ValorSimplificado">Nulo quando a fonte não publica o valor.</param>
/// <param name="ReducoesMensais">Nulo quando a fonte não publica a redução; a regra já cadastrada continua valendo.</param>
public sealed record TabelaIrrfPublicada(
    DateOnly Competencia,
    IReadOnlyList<FaixaIrrfPublicada> Faixas,
    decimal? ValorDependente = null,
    decimal? ValorSimplificado = null,
    IReadOnlyList<ReducaoMensalPublicada>? ReducoesMensais = null) : ITabelaPublicada<TabelaIrrfPublicada>
{
    /// <summary>Limite gravado na última faixa para representar "acima de".</summary>
    public const decimal LimiteUltimaFaixa = 9_999_999_999_999.99m;

    public void Validar() => SequenciaDecimal.ValidarFaixasProgressivas(Faixas, "IRRF");

    // Só as faixas: é o que todas as fontes publicam.
    public bool TemMesmosValores(TabelaIrrfPublicada outra) => Faixas.SequenceEqual(outra.Faixas);
}

/// <summary>Tabela anual exclusiva da PLR, com o mesmo formato de faixas da tabela progressiva do IRRF.</summary>
public sealed record TabelaPlrPublicada(DateOnly Competencia, IReadOnlyList<FaixaIrrfPublicada> Faixas) : ITabelaPublicada<TabelaPlrPublicada>
{
    public void Validar() => SequenciaDecimal.ValidarFaixasProgressivas(Faixas, "da PLR");

    public bool TemMesmosValores(TabelaPlrPublicada outra) => Faixas.SequenceEqual(outra.Faixas);
}

public sealed record FaixaSalarioFamiliaPublicada(decimal LimiteRemuneracao, decimal Cota);

/// <summary>Até 10/2019 eram duas faixas de remuneração, com cotas diferentes; depois, uma só.</summary>
public sealed record TabelaSalarioFamiliaPublicada(DateOnly Competencia, IReadOnlyList<FaixaSalarioFamiliaPublicada> Faixas) : ITabelaPublicada<TabelaSalarioFamiliaPublicada>
{
    public void Validar()
    {
        if (Faixas.Count is < 1 or > 2 ||
            Faixas.Any(faixa => faixa.LimiteRemuneracao <= 0m || faixa.Cota <= 0m) ||
            !Faixas.Select(faixa => faixa.LimiteRemuneracao).EstritamenteCrescente())
            throw new InvalidOperationException("A página retornou uma estrutura de salário-família inválida.");
    }

    public bool TemMesmosValores(TabelaSalarioFamiliaPublicada outra) => Faixas.SequenceEqual(outra.Faixas);
}

public sealed record SalarioMinimoPublicado(DateOnly Competencia, decimal Valor) : ITabelaPublicada<SalarioMinimoPublicado>
{
    public void Validar()
    {
        // Um valor fora dessa faixa indica que a página mudou e outro número foi lido no lugar do salário mínimo.
        if (Valor is < 500m or > 100_000m)
            throw new InvalidOperationException($"A página retornou um salário mínimo fora do esperado ({Valor:N2}).");
    }

    public bool TemMesmosValores(SalarioMinimoPublicado outra) => Valor == outra.Valor;
}

internal static class SequenciaDecimal
{
    /// <summary>Cinco faixas, a primeira isenta, com limites, alíquotas e deduções crescentes e a última "acima de".</summary>
    public static void ValidarFaixasProgressivas(IReadOnlyList<FaixaIrrfPublicada> faixas, string nomeTabela)
    {
        if (faixas.Count != 5 ||
            faixas[0].Limite <= 0m ||
            faixas[0].Aliquota != 0m ||
            faixas[0].Deducao != 0m ||
            faixas[^1].Limite != TabelaIrrfPublicada.LimiteUltimaFaixa ||
            !faixas.Select(faixa => faixa.Limite).EstritamenteCrescente() ||
            !faixas.Select(faixa => faixa.Aliquota).EstritamenteCrescente() ||
            !faixas.Select(faixa => faixa.Deducao).EstritamenteCrescente())
            throw new InvalidOperationException($"A página retornou uma estrutura de faixas {nomeTabela} inválida.");
    }

    public static bool EstritamenteCrescente(this IEnumerable<decimal> valores)
    {
        decimal? anterior = null;
        foreach (var valor in valores)
        {
            if (valor <= anterior) return false;
            anterior = valor;
        }
        return true;
    }
}
