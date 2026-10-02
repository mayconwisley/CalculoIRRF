#nullable enable

using CalculoIRRF.Application.Management;
using CalculoIRRF.Infrastructure.Persistence;
using System.Net.Http;

namespace CalculoIRRF.Infrastructure.Tributacao;

/// <summary>
/// Importa a tabela progressiva dos segurados empregado, doméstico e trabalhador avulso. A gravação só ocorre depois de a
/// tabela ser validada e confirmada (veja <see cref="ConsultaDeFontes"/>), preservando os dados locais em caso de falha.
/// </summary>
public sealed class AtualizadorTabelaInss(
    Func<HttpClient> criarHttpClient,
    IEnumerable<IFonteTabela<TabelaInssPublicada>> fontes,
    BancoTributario banco,
    ICacheTabelasTributarias cache) : IAtualizadorTabelaInss
{
    private readonly IFonteTabela<TabelaInssPublicada>[] _fontes = fontes.ToArray();

    public Uri FonteOficial => _fontes.First(fonte => fonte.Oficial).Endereco;

    public async Task<AtualizacaoTabelaResultado> AtualizarAsync(CancellationToken cancellationToken)
    {
        var escolha = await ConsultaDeFontes.EscolherAsync(_fontes, criarHttpClient, "INSS", cancellationToken);
        await PersistirAsync(escolha.Tabela, cancellationToken);

        return new AtualizacaoTabelaResultado(escolha.Tabela.Competencia, escolha.Tabela.Faixas.Count, escolha.Fontes, escolha.Oficial, escolha.Observacoes);
    }

    private async Task PersistirAsync(TabelaInssPublicada tabela, CancellationToken cancellationToken)
    {
        var competencia = tabela.Competencia.ToDateTime(TimeOnly.MinValue);
        await using var conexao = await banco.AbrirAsync(cancellationToken);
        await using var transacao = conexao.BeginTransaction();
        await conexao.ExecutarAsync(transacao, "DELETE FROM Inss WHERE Competencia = $competencia", cancellationToken, ("$competencia", competencia));
        for (var indice = 0; indice < tabela.Faixas.Count; indice++)
        {
            var faixa = tabela.Faixas[indice];
            await conexao.ExecutarAsync(transacao, "INSERT INTO Inss (Competencia, Faixa, Valor, Porcentagem) VALUES ($competencia, $faixa, $valor, $porcentagem)", cancellationToken,
                ("$competencia", competencia), ("$faixa", indice + 1), ("$valor", (double)faixa.Limite), ("$porcentagem", (double)faixa.Aliquota));
        }
        await transacao.CommitAsync(cancellationToken);
        cache.Invalidar();
    }
}
