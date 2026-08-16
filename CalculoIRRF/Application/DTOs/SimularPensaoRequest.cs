namespace CalculoIRRF.Application.DTOs;

public sealed record SimularPensaoRequest(DateOnly Competencia, decimal ValorBruto, decimal BaseInss, int Dependentes, decimal PercentualPensao, decimal OutrosDescontos);
