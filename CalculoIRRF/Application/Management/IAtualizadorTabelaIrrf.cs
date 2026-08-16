namespace CalculoIRRF.Application.Management;

/// <summary>
/// Atualiza os parâmetros mensais do IRRF a partir da publicação oficial da Receita Federal.
/// </summary>
public interface IAtualizadorTabelaIrrf
{
    Uri FonteOficial { get; }

    Task<AtualizacaoTabelaIrrfResultado> AtualizarAsync(CancellationToken cancellationToken);
}

public sealed record AtualizacaoTabelaIrrfResultado(
    DateOnly Competencia,
    int QuantidadeFaixas,
    Uri Fonte);
