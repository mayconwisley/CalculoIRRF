#nullable enable

namespace CalculoIRRF.Application.DTOs;

public sealed record FaixaTributariaDto(int Numero, decimal Limite, decimal Aliquota, decimal Deducao = 0m);
public sealed record RegraReducaoMensalIrrfDto(int Faixa, decimal LimiteRendimentos, decimal Multiplicador, decimal ValorBase);
public sealed record FaixaSalarioFamiliaDto(int Faixa, decimal LimiteRemuneracao, decimal Cota);

/// <param name="SalarioMinimo">Nulo quando não há salário mínimo cadastrado até a competência.</param>
/// <param name="FaixasPlr">Tabela anual da PLR vigente; vazia quando não há tabela cadastrada.</param>
/// <param name="FaixasSalarioFamilia">Faixas do salário-família vigentes; vazias quando não há tabela cadastrada.</param>
public sealed record PerfilTributarioDto(
    IReadOnlyList<FaixaTributariaDto> FaixasInss,
    IReadOnlyList<FaixaTributariaDto> FaixasIrrf,
    decimal DeducaoSimplificada,
    decimal DeducaoPorDependente,
    decimal DescontoMinimo,
    IReadOnlyList<RegraReducaoMensalIrrfDto> ReducoesMensaisIrrf,
    decimal? SalarioMinimo = null,
    IReadOnlyList<FaixaTributariaDto>? FaixasPlr = null,
    IReadOnlyList<FaixaSalarioFamiliaDto>? FaixasSalarioFamilia = null);
