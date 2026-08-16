using CalculoIRRF.Application.DTOs;

namespace CalculoIRRF.Application.UseCases;

public interface ISimularPensaoUseCase
{
    Task<SimulacaoPensaoDto> ExecutarAsync(SimularPensaoRequest request, CancellationToken cancellationToken);
}
