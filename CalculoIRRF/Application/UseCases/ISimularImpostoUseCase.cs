using CalculoIRRF.Application.DTOs;

namespace CalculoIRRF.Application.UseCases;

public interface ISimularImpostoUseCase
{
    Task<SimulacaoImpostoDto> ExecutarAsync(SimularImpostoRequest request, CancellationToken cancellationToken);
}
