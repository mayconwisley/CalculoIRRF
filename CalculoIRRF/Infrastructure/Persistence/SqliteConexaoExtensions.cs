#nullable enable

using Microsoft.Data.Sqlite;

namespace CalculoIRRF.Infrastructure.Persistence;

internal static class SqliteConexaoExtensions
{
    public static SqliteCommand CriarComando(this SqliteConnection conexao, SqliteTransaction? transacao, string sql, params (string Nome, object Valor)[] parametros)
    {
        var comando = conexao.CreateCommand();
        comando.Transaction = transacao;
        comando.CommandText = sql;
        foreach (var (nome, valor) in parametros)
            comando.Parameters.AddWithValue(nome, valor);
        return comando;
    }

    public static async Task<int> ExecutarAsync(this SqliteConnection conexao, SqliteTransaction? transacao, string sql, CancellationToken cancellationToken, params (string Nome, object Valor)[] parametros)
    {
        await using var comando = conexao.CriarComando(transacao, sql, parametros);
        return await comando.ExecuteNonQueryAsync(cancellationToken);
    }

    public static async Task<T[]> ListarAsync<T>(this SqliteConnection conexao, SqliteTransaction? transacao, string sql, Func<SqliteDataReader, T> ler, CancellationToken cancellationToken, params (string Nome, object Valor)[] parametros)
    {
        await using var comando = conexao.CriarComando(transacao, sql, parametros);
        await using var leitor = await comando.ExecuteReaderAsync(cancellationToken);
        var itens = new List<T>();
        while (await leitor.ReadAsync(cancellationToken))
            itens.Add(ler(leitor));
        return itens.ToArray();
    }
}
