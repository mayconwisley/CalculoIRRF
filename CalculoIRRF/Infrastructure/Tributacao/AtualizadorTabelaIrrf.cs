#nullable enable

using CalculoIRRF.Application.Management;
using CalculoIRRF.Infrastructure.Persistence;
using Microsoft.Data.Sqlite;
using System.Globalization;
using System.Net.Http;

namespace CalculoIRRF.Infrastructure.Tributacao;

/// <summary>
/// Importa as faixas do IRRF e os parâmetros mensais publicados com elas. A gravação só ocorre depois de a tabela ser
/// validada e confirmada (veja <see cref="ConsultaDeFontes"/>), preservando os dados locais em caso de falha.
/// </summary>
public sealed class AtualizadorTabelaIrrf(
    Func<HttpClient> criarHttpClient,
    IEnumerable<IFonteTabela<TabelaIrrfPublicada>> fontes,
    BancoTributario banco,
    ICacheTabelasTributarias cache) : IAtualizadorTabelaIrrf
{
    // A Lei 9.250/1995, com a redação da Lei 14.663/2023, fixa o desconto simplificado mensal em 25% do valor máximo
    // da faixa com alíquota zero da tabela progressiva.
    private const decimal PercentualDescontoSimplificado = 0.25m;
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");

    private readonly IFonteTabela<TabelaIrrfPublicada>[] _fontes = fontes.ToArray();

    public Uri FonteOficial => _fontes.First(fonte => fonte.Oficial).Endereco;

    public async Task<AtualizacaoTabelaResultado> AtualizarAsync(CancellationToken cancellationToken)
    {
        var escolha = await ConsultaDeFontes.EscolherAsync(_fontes, criarHttpClient, "IRRF", cancellationToken);
        var tabela = escolha.Tabela;
        var observacoes = escolha.Observacoes.ToList();

        // As fontes alternativas publicam só as faixas: o desconto simplificado sai da regra da lei, e a dedução por
        // dependente e a redução mensal continuam com os valores já cadastrados.
        if (tabela.ValorSimplificado is null)
        {
            tabela = tabela with { ValorSimplificado = Math.Round(tabela.Faixas[0].Limite * PercentualDescontoSimplificado, 2, MidpointRounding.AwayFromZero) };
            observacoes.Add($"O desconto simplificado foi calculado em 25% da faixa isenta (R$ {tabela.ValorSimplificado.Value.ToString("N2", Cultura)}), como prevê a lei; a dedução por dependente e a redução mensal seguem os valores já cadastrados.");
        }

        await PersistirAsync(tabela, cancellationToken);
        return new AtualizacaoTabelaResultado(tabela.Competencia, tabela.Faixas.Count, escolha.Fontes, escolha.Oficial, observacoes);
    }

    private async Task PersistirAsync(TabelaIrrfPublicada tabela, CancellationToken cancellationToken)
    {
        var competencia = tabela.Competencia.ToDateTime(TimeOnly.MinValue);
        await using var conexao = await banco.AbrirAsync(cancellationToken);
        await using var transacao = conexao.BeginTransaction();

        await conexao.ExecutarAsync(transacao, "DELETE FROM Irrf WHERE Competencia = $competencia", cancellationToken, ("$competencia", competencia));
        for (var indice = 0; indice < tabela.Faixas.Count; indice++)
        {
            var faixa = tabela.Faixas[indice];
            await conexao.ExecutarAsync(transacao, "INSERT INTO Irrf (Competencia, Faixa, Valor, Porcentagem, Deducao) VALUES ($competencia, $faixa, $valor, $porcentagem, $deducao)", cancellationToken,
                ("$competencia", competencia), ("$faixa", indice + 1), ("$valor", (double)faixa.Limite), ("$porcentagem", (double)faixa.Aliquota), ("$deducao", (double)faixa.Deducao));
        }

        if (tabela.ValorDependente is { } dependente)
            await AtualizarParametroAsync(conexao, transacao, "Dependente", competencia, dependente, cancellationToken);
        if (tabela.ValorSimplificado is { } simplificado)
            await AtualizarParametroAsync(conexao, transacao, "Simplificado", competencia, simplificado, cancellationToken);

        if (tabela.ReducoesMensais is { } reducoes)
        {
            await conexao.ExecutarAsync(transacao, "DELETE FROM ReducaoMensalIrrf WHERE Competencia = $competencia", cancellationToken, ("$competencia", competencia));
            foreach (var reducao in reducoes)
                await conexao.ExecutarAsync(transacao, "INSERT INTO ReducaoMensalIrrf (Competencia, Faixa, LimiteRendimentos, Multiplicador, ValorBase) VALUES ($competencia, $faixa, $limite, $multiplicador, $valorBase)", cancellationToken,
                    ("$competencia", competencia), ("$faixa", reducao.Faixa), ("$limite", (double)reducao.LimiteRendimentos), ("$multiplicador", (double)reducao.Multiplicador), ("$valorBase", (double)reducao.ValorBase));
        }

        await transacao.CommitAsync(cancellationToken);
        cache.Invalidar();
    }

    private static async Task AtualizarParametroAsync(SqliteConnection conexao, SqliteTransaction transacao, string tabela, DateTime competencia, decimal valor, CancellationToken cancellationToken)
    {
        var ids = await conexao.ListarAsync(transacao, $"SELECT Id FROM {tabela} WHERE Competencia = $competencia LIMIT 2", leitor => leitor.GetInt32(0), cancellationToken, ("$competencia", competencia));
        if (ids.Length > 1)
            throw new InvalidOperationException($"Há mais de um registro em {tabela} para a competência {competencia:MM/yyyy}. Nenhum dado foi alterado.");

        if (ids.Length == 0)
            await conexao.ExecutarAsync(transacao, $"INSERT INTO {tabela} (Competencia, Valor) VALUES ($competencia, $valor)", cancellationToken, ("$competencia", competencia), ("$valor", (double)valor));
        else
            await conexao.ExecutarAsync(transacao, $"UPDATE {tabela} SET Valor = $valor WHERE Id = $id", cancellationToken, ("$valor", (double)valor), ("$id", ids[0]));
    }
}
