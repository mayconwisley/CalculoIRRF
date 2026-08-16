namespace CalculoIRRF.Application.DTOs;

public sealed record SimulacaoEstabilidadeDto(
    int DiasEstabilidade,
    int Avos,
    decimal Indenizacao,
    decimal DecimoTerceiro,
    decimal Ferias,
    decimal TercoFerias,
    decimal FgtsOitoPorCento,
    decimal MultaFgtsQuarentaPorCento,
    decimal Complementos,
    decimal Total);
