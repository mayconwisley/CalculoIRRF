namespace CalculoIRRF.Application.DTOs;

public sealed record SimularEstabilidadeRequest(
    decimal MediaRemuneratoria,
    int DiasBase,
    DateOnly Demissao,
    DateOnly FimEstabilidade,
    decimal Complementos);
