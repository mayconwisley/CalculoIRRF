using Microsoft.Data.Sqlite;

namespace CalculoIRRF.Infrastructure.Persistence;

/// <summary>Abre conexões com o banco SQLite local das tabelas tributárias. As conexões são reaproveitadas pelo pool do provedor.</summary>
public sealed class BancoTributario(string connectionString)
{
    public async Task<SqliteConnection> AbrirAsync(CancellationToken cancellationToken)
    {
        var conexao = new SqliteConnection(connectionString);
        await conexao.OpenAsync(cancellationToken);
        return conexao;
    }
}
