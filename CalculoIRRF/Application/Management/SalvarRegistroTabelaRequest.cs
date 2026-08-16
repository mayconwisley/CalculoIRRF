namespace CalculoIRRF.Application.Management;

public sealed record SalvarRegistroTabelaRequest(int Id, DateOnly Competencia, int? Faixa, decimal Valor, decimal? Aliquota, decimal? Deducao);
