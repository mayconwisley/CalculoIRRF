using CalculoIRRF.Application.DTOs;

namespace CalculoIRRF.Application.UseCases;

public interface ISimularEstabilidadeUseCase
{
    SimulacaoEstabilidadeDto Executar(SimularEstabilidadeRequest request);
}
