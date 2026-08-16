namespace CalculoIRRF.Application.DTOs;

public sealed record SimularImpostoRequest(
    DateOnly Competencia,
    decimal ValorBruto,
    decimal BaseInss,
    int QuantidadeDependentes);
