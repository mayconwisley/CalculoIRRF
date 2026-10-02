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

    public void Validar()
    {
        if (Faixas.Count != 5 ||
            Faixas[0].Limite <= 0m ||
            Faixas[0].Aliquota != 0m ||
            Faixas[0].Deducao != 0m ||
            Faixas[^1].Limite != LimiteUltimaFaixa ||
            !Faixas.Select(faixa => faixa.Limite).EstritamenteCrescente() ||
            !Faixas.Select(faixa => faixa.Aliquota).EstritamenteCrescente() ||
            !Faixas.Select(faixa => faixa.Deducao).EstritamenteCrescente())
            throw new InvalidOperationException("A página retornou uma estrutura de faixas IRRF inválida.");
    }

    // Só as faixas: é o que todas as fontes publicam.
    public bool TemMesmosValores(TabelaIrrfPublicada outra) => Faixas.SequenceEqual(outra.Faixas);
}

internal static class SequenciaDecimal
{
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
