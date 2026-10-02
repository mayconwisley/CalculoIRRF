#nullable enable

namespace CalculoIRRF.Application.DTOs;

public sealed record DetalheFaixaDto(int Faixa, decimal BaseCalculada, decimal Aliquota, decimal Imposto);

public sealed record ModalidadeIrrfDto(
    string Nome,
    decimal BaseCalculo,
    decimal Aliquota,
    decimal Deducao,
    decimal ImpostoAntesReducao,
    decimal ReducaoMensal,
    decimal Imposto,
    decimal AliquotaEfetiva,
    IReadOnlyList<DetalheFaixaDto> DetalhesProgressivos);

/// <param name="DeducaoPorDependente">Valor da tabela deduzido da base normal para cada dependente.</param>
/// <param name="DescontoSimplificado">Valor da tabela deduzido da base simplificada; nulo antes de 05/2023, quando a modalidade não existia.</param>
public sealed record SimulacaoImpostoDto(
    SimularImpostoRequest Entrada,
    decimal BaseInssConsiderada,
    decimal ValorInss,
    ModalidadeIrrfDto Normal,
    ModalidadeIrrfDto Simplificada,
    decimal DescontoMinimo,
    decimal FgtsOitoPorCento,
    decimal FgtsDoisPorCento,
    string MensagemVantagem,
    string? ModalidadeMaisVantajosa,
    IReadOnlyList<DetalheFaixaDto> DetalhesInss,
    decimal DeducaoPorDependente,
    decimal? DescontoSimplificado);
