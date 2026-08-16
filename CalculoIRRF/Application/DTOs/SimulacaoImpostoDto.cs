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
    IReadOnlyList<DetalheFaixaDto> DetalhesInss);
