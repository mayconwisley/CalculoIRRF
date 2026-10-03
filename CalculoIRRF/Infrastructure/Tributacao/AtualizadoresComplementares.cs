#nullable enable

using CalculoIRRF.Application.Management;
using CalculoIRRF.Infrastructure.Persistence;
using System.Net.Http;

namespace CalculoIRRF.Infrastructure.Tributacao;

/// <summary>Importa a tabela anual da PLR da competência confirmada, substituindo as faixas dessa competência.</summary>
public sealed class AtualizadorTabelaPlr(
    Func<HttpClient> criarHttpClient,
    IEnumerable<IFonteTabela<TabelaPlrPublicada>> fontes,
    BancoTributario banco,
    ICacheTabelasTributarias cache)
{
    private readonly IFonteTabela<TabelaPlrPublicada>[] _fontes = fontes.ToArray();

    public Uri FonteOficial => _fontes.First(fonte => fonte.Oficial).Endereco;

    public async Task<AtualizacaoTabelaResultado> AtualizarAsync(CancellationToken cancellationToken)
    {
        var escolha = await ConsultaDeFontes.EscolherAsync(_fontes, criarHttpClient, "PLR", cancellationToken);
        var tabela = escolha.Tabela;
        var competencia = tabela.Competencia.ToDateTime(TimeOnly.MinValue);

        await using var conexao = await banco.AbrirAsync(cancellationToken);
        await using var transacao = conexao.BeginTransaction();
        await conexao.ExecutarAsync(transacao, "DELETE FROM Plr WHERE Competencia = $competencia", cancellationToken, ("$competencia", competencia));
        for (var indice = 0; indice < tabela.Faixas.Count; indice++)
        {
            var faixa = tabela.Faixas[indice];
            await conexao.ExecutarAsync(transacao, "INSERT INTO Plr (Competencia, Faixa, Valor, Porcentagem, Deducao) VALUES ($competencia, $faixa, $valor, $porcentagem, $deducao)", cancellationToken,
                ("$competencia", competencia), ("$faixa", indice + 1), ("$valor", (double)faixa.Limite), ("$porcentagem", (double)faixa.Aliquota), ("$deducao", (double)faixa.Deducao));
        }
        await transacao.CommitAsync(cancellationToken);
        cache.Invalidar();

        return new AtualizacaoTabelaResultado(tabela.Competencia, tabela.Faixas.Count, escolha.Fontes, escolha.Oficial, escolha.Observacoes);
    }
}

/// <summary>Importa o limite de remuneração e a cota do salário-família da competência confirmada.</summary>
public sealed class AtualizadorSalarioFamilia(
    Func<HttpClient> criarHttpClient,
    IEnumerable<IFonteTabela<TabelaSalarioFamiliaPublicada>> fontes,
    BancoTributario banco,
    ICacheTabelasTributarias cache)
{
    private readonly IFonteTabela<TabelaSalarioFamiliaPublicada>[] _fontes = fontes.ToArray();

    public Uri FonteOficial => _fontes.First(fonte => fonte.Oficial).Endereco;

    public async Task<AtualizacaoTabelaResultado> AtualizarAsync(CancellationToken cancellationToken)
    {
        var escolha = await ConsultaDeFontes.EscolherAsync(_fontes, criarHttpClient, "salário-família", cancellationToken);
        var tabela = escolha.Tabela;
        var competencia = tabela.Competencia.ToDateTime(TimeOnly.MinValue);

        await using var conexao = await banco.AbrirAsync(cancellationToken);
        await using var transacao = conexao.BeginTransaction();
        await conexao.ExecutarAsync(transacao, "DELETE FROM SalarioFamilia WHERE Competencia = $competencia", cancellationToken, ("$competencia", competencia));
        for (var indice = 0; indice < tabela.Faixas.Count; indice++)
        {
            var faixa = tabela.Faixas[indice];
            await conexao.ExecutarAsync(transacao, "INSERT INTO SalarioFamilia (Competencia, Faixa, LimiteRemuneracao, Cota) VALUES ($competencia, $faixa, $limite, $cota)", cancellationToken,
                ("$competencia", competencia), ("$faixa", indice + 1), ("$limite", (double)faixa.LimiteRemuneracao), ("$cota", (double)faixa.Cota));
        }
        await transacao.CommitAsync(cancellationToken);
        cache.Invalidar();

        return new AtualizacaoTabelaResultado(tabela.Competencia, tabela.Faixas.Count, escolha.Fontes, escolha.Oficial, escolha.Observacoes);
    }
}

/// <summary>Importa o salário mínimo da competência confirmada.</summary>
public sealed class AtualizadorSalarioMinimo(
    Func<HttpClient> criarHttpClient,
    IEnumerable<IFonteTabela<SalarioMinimoPublicado>> fontes,
    BancoTributario banco,
    ICacheTabelasTributarias cache)
{
    private readonly IFonteTabela<SalarioMinimoPublicado>[] _fontes = fontes.ToArray();

    public Uri FonteOficial => _fontes.First(fonte => fonte.Oficial).Endereco;

    public async Task<AtualizacaoTabelaResultado> AtualizarAsync(CancellationToken cancellationToken)
    {
        var escolha = await ConsultaDeFontes.EscolherAsync(_fontes, criarHttpClient, "salário mínimo", cancellationToken);
        var salario = escolha.Tabela;
        var competencia = salario.Competencia.ToDateTime(TimeOnly.MinValue);

        await using var conexao = await banco.AbrirAsync(cancellationToken);
        await using var transacao = conexao.BeginTransaction();
        var ids = await conexao.ListarAsync(transacao, "SELECT Id FROM SalarioMinimo WHERE Competencia = $competencia LIMIT 2", leitor => leitor.GetInt32(0), cancellationToken, ("$competencia", competencia));
        if (ids.Length > 1)
            throw new InvalidOperationException($"Há mais de um salário mínimo cadastrado para a competência {competencia:MM/yyyy}. Nenhum dado foi alterado.");
        if (ids.Length == 0)
            await conexao.ExecutarAsync(transacao, "INSERT INTO SalarioMinimo (Competencia, Valor) VALUES ($competencia, $valor)", cancellationToken, ("$competencia", competencia), ("$valor", (double)salario.Valor));
        else
            await conexao.ExecutarAsync(transacao, "UPDATE SalarioMinimo SET Valor = $valor WHERE Id = $id", cancellationToken, ("$valor", (double)salario.Valor), ("$id", ids[0]));
        await transacao.CommitAsync(cancellationToken);
        cache.Invalidar();

        return new AtualizacaoTabelaResultado(salario.Competencia, 1, escolha.Fontes, escolha.Oficial, escolha.Observacoes);
    }
}

/// <summary>Encaminha a atualização de cada tabela ao atualizador da sua fonte.</summary>
public sealed class AtualizadorTabelas(
    IAtualizadorTabelaInss inss,
    IAtualizadorTabelaIrrf irrf,
    AtualizadorTabelaPlr plr,
    AtualizadorSalarioFamilia salarioFamilia,
    AtualizadorSalarioMinimo salarioMinimo) : IAtualizadorTabelas
{
    public bool TemAtualizacaoOnline(TipoTabelaTributaria tipo) => tipo != TipoTabelaTributaria.DescontoMinimo;

    public Uri FonteOficial(TipoTabelaTributaria tipo) => tipo switch
    {
        TipoTabelaTributaria.Inss => inss.FonteOficial,
        TipoTabelaTributaria.Plr => plr.FonteOficial,
        TipoTabelaTributaria.SalarioFamilia => salarioFamilia.FonteOficial,
        TipoTabelaTributaria.SalarioMinimo => salarioMinimo.FonteOficial,
        TipoTabelaTributaria.DescontoMinimo => throw new NotSupportedException("O desconto mínimo não tem atualização pela internet."),
        _ => irrf.FonteOficial
    };

    public string NomeFonteOficial(TipoTabelaTributaria tipo) =>
        tipo is TipoTabelaTributaria.Inss or TipoTabelaTributaria.SalarioFamilia or TipoTabelaTributaria.SalarioMinimo ? "INSS" : "Receita Federal";

    // Valor simplificado, dedução por dependente e redução mensal vêm da mesma página da Receita que as faixas do IRRF.
    public Task<AtualizacaoTabelaResultado> AtualizarAsync(TipoTabelaTributaria tipo, CancellationToken cancellationToken) => tipo switch
    {
        TipoTabelaTributaria.Inss => inss.AtualizarAsync(cancellationToken),
        TipoTabelaTributaria.Plr => plr.AtualizarAsync(cancellationToken),
        TipoTabelaTributaria.SalarioFamilia => salarioFamilia.AtualizarAsync(cancellationToken),
        TipoTabelaTributaria.SalarioMinimo => salarioMinimo.AtualizarAsync(cancellationToken),
        TipoTabelaTributaria.DescontoMinimo => throw new NotSupportedException("O desconto mínimo não tem atualização pela internet."),
        _ => irrf.AtualizarAsync(cancellationToken)
    };
}
