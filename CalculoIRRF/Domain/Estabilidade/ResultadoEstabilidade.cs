namespace CalculoIRRF.Domain.Estabilidade;

public sealed record ResultadoEstabilidade(
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
