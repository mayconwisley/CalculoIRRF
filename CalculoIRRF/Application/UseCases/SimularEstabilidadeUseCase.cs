using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Estabilidade;

namespace CalculoIRRF.Application.UseCases;

public sealed class SimularEstabilidadeUseCase : ISimularEstabilidadeUseCase
{
    public SimulacaoEstabilidadeDto Executar(SimularEstabilidadeRequest request)
    {
        var resultado = CalculadoraEstabilidade.Calcular(
            request.MediaRemuneratoria,
            request.DiasBase,
            request.Demissao,
            request.FimEstabilidade,
            request.Complementos);

        return new SimulacaoEstabilidadeDto(
            resultado.DiasEstabilidade,
            resultado.Avos,
            resultado.Indenizacao,
            resultado.DecimoTerceiro,
            resultado.Ferias,
            resultado.TercoFerias,
            resultado.FgtsOitoPorCento,
            resultado.MultaFgtsQuarentaPorCento,
            resultado.Complementos,
            resultado.Total);
    }
}
