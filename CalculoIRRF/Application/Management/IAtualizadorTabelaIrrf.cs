namespace CalculoIRRF.Application.Management;

/// <summary>
/// Atualiza os parâmetros mensais do IRRF a partir da publicação da Receita Federal ou, enquanto ela não publica
/// a tabela mais recente, de fontes alternativas que a confirmem.
/// </summary>
public interface IAtualizadorTabelaIrrf
{
    Uri FonteOficial { get; }

    Task<AtualizacaoTabelaResultado> AtualizarAsync(CancellationToken cancellationToken);
}
