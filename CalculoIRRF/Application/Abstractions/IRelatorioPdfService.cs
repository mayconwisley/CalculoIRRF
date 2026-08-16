using CalculoIRRF.Application.DTOs;

namespace CalculoIRRF.Application.Abstractions;

public interface IRelatorioPdfService
{
    Task GerarRelatorioImpostoAsync(SimulacaoImpostoDto simulacao, string caminhoArquivo, CancellationToken cancellationToken);
    Task GerarRelatorioPensaoAsync(SimulacaoPensaoDto simulacao, EntradaPensaoDto entrada, bool incluirDetalhes, string caminhoArquivo, CancellationToken cancellationToken);
}

public sealed record EntradaPensaoDto(DateOnly Competencia, decimal ValorBruto, decimal BaseInss, int Dependentes, decimal Percentual, decimal OutrosDescontos);
