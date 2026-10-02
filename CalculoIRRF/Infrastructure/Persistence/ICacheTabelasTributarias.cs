namespace CalculoIRRF.Infrastructure.Persistence;

/// <summary>Descarta as tabelas tributárias mantidas em memória; deve ser chamado após qualquer gravação nessas tabelas.</summary>
public interface ICacheTabelasTributarias
{
    void Invalidar();
}
