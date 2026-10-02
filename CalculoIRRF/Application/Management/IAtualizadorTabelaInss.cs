namespace CalculoIRRF.Application.Management;

/// <summary>Consulta e atualiza as faixas de INSS para empregado, doméstico e trabalhador avulso.</summary>
public interface IAtualizadorTabelaInss
{
    Uri FonteOficial { get; }

    Task<AtualizacaoTabelaResultado> AtualizarAsync(CancellationToken cancellationToken);
}
