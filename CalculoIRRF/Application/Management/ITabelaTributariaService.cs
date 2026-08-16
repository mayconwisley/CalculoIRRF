namespace CalculoIRRF.Application.Management;

public interface ITabelaTributariaService
{
    Task<IReadOnlyList<RegistroTabelaDto>> ListarAsync(TipoTabelaTributaria tipo, CancellationToken cancellationToken);
    Task SalvarAsync(TipoTabelaTributaria tipo, SalvarRegistroTabelaRequest request, CancellationToken cancellationToken);
    Task ExcluirAsync(TipoTabelaTributaria tipo, int id, CancellationToken cancellationToken);
}
