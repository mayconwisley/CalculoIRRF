namespace CalculoIRRF.Application.DTOs;

public sealed record FaixaTributariaDto(int Numero, decimal Limite, decimal Aliquota, decimal Deducao = 0m);
public sealed record RegraReducaoMensalIrrfDto(int Faixa, decimal LimiteRendimentos, decimal Multiplicador, decimal ValorBase);

public sealed record PerfilTributarioDto(
    IReadOnlyList<FaixaTributariaDto> FaixasInss,
    IReadOnlyList<FaixaTributariaDto> FaixasIrrf,
    decimal DeducaoSimplificada,
    decimal DeducaoPorDependente,
    decimal DescontoMinimo,
    IReadOnlyList<RegraReducaoMensalIrrfDto> ReducoesMensaisIrrf);
