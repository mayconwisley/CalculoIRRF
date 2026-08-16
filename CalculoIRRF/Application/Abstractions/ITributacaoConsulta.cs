using CalculoIRRF.Application.DTOs;

namespace CalculoIRRF.Application.Abstractions;

/// <summary>Porta de leitura das tabelas tributárias. A aplicação não conhece EF Core.</summary>
public interface ITributacaoConsulta
{
    Task<PerfilTributarioDto> ObterPerfilAsync(DateOnly competencia, CancellationToken cancellationToken);
}
