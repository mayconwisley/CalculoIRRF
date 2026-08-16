namespace CalculoIRRF.Infrastructure.Persistence;

public interface IInicializadorBancoTributario
{
    Task InicializarAsync(CancellationToken cancellationToken);
}
