using CalculoIRRF.Application.DTOs;

namespace CalculoIRRF.Application.UseCases;

/// <summary>Calculadora que devolve o resultado como demonstrativo (13º, férias, rescisão e as demais).</summary>
public interface ISimularDemonstrativoUseCase<in TRequest>
{
    Task<DemonstrativoDto> ExecutarAsync(TRequest request, CancellationToken cancellationToken);
}
