namespace CalculoIRRF.Application.DTOs;

public sealed record IteracaoPensaoDto(int Sequencia, decimal BaseIrrf, decimal Aliquota, decimal Deducao, decimal ImpostoAntesReducao, decimal ReducaoMensal, decimal Imposto, decimal BasePensao, decimal Pensao);
public sealed record ModalidadePensaoDto(string Nome, decimal ImpostoAntesReducao, decimal ReducaoMensal, decimal Imposto, decimal Pensao, decimal Total, int Iteracoes, IReadOnlyList<IteracaoPensaoDto> Detalhes);
public sealed record SimulacaoPensaoDto(decimal ValorInss, ModalidadePensaoDto Normal, ModalidadePensaoDto Simplificada, string MensagemVantagem);
