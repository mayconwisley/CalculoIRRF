namespace CalculoIRRF.Application.Management;

/// <summary>Consulta e atualiza as faixas de INSS para empregado, doméstico e trabalhador avulso.</summary>
public interface IAtualizadorTabelaInss
{
    Uri FonteOficial { get; }

    Task<AtualizacaoTabelaInssResultado> AtualizarAsync(CancellationToken cancellationToken);
}

public sealed record AtualizacaoTabelaInssResultado(DateOnly Competencia, int QuantidadeFaixas, Uri Fonte);
