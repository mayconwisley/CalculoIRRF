#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;

namespace CalculoIRRF.Infrastructure.Persistence;

/// <summary>
/// Adaptador SQLite da porta de leitura consumida pela camada Application.
/// As tabelas são pequenas e só mudam pela manutenção ou pela atualização online, por isso são carregadas uma vez
/// e mantidas em memória até <see cref="Invalidar"/> ser chamado.
/// </summary>
public sealed class SqliteTributacaoConsulta(BancoTributario banco) : ITributacaoConsulta, ICacheTabelasTributarias
{
    private TabelasTributarias? _tabelas;
    private int _versao;

    public async Task<PerfilTributarioDto> ObterPerfilAsync(DateOnly competencia, CancellationToken cancellationToken)
    {
        var tabelas = await ObterTabelasAsync(cancellationToken);
        var data = competencia.ToDateTime(TimeOnly.MinValue);
        var competenciaInss = ObterUltimaCompetencia(tabelas.FaixasInss, data, "INSS");
        var competenciaIrrf = ObterUltimaCompetencia(tabelas.FaixasIrrf, data, "IRRF");
        var competenciaSimplificado = ObterUltimaCompetencia(tabelas.Simplificados, data, "desconto simplificado");
        var competenciaDependente = ObterUltimaCompetencia(tabelas.Dependentes, data, "dedução por dependente");
        var competenciaMinimo = ObterUltimaCompetencia(tabelas.DescontosMinimos, data, "desconto mínimo");
        var competenciaReducao = ObterUltimaCompetenciaOpcional(tabelas.ReducoesMensais, data);

        var inss = tabelas.FaixasInss.Where(item => item.Competencia == competenciaInss).Select(item => item.Valor).OrderBy(item => item.Numero).ToArray();
        var irrf = tabelas.FaixasIrrf.Where(item => item.Competencia == competenciaIrrf).Select(item => item.Valor).OrderBy(item => item.Numero).ToArray();
        var simplificado = tabelas.Simplificados.First(item => item.Competencia == competenciaSimplificado).Valor;
        var dependente = tabelas.Dependentes.First(item => item.Competencia == competenciaDependente).Valor;
        var minimo = tabelas.DescontosMinimos.First(item => item.Competencia == competenciaMinimo).Valor;
        var reducoes = competenciaReducao is null
            ? []
            : tabelas.ReducoesMensais.Where(item => item.Competencia == competenciaReducao).Select(item => item.Valor).OrderBy(item => item.Faixa).ToArray();

        return new PerfilTributarioDto(inss, irrf, simplificado, dependente, minimo, reducoes);
    }

    public void Invalidar()
    {
        Interlocked.Increment(ref _versao);
        Volatile.Write(ref _tabelas, null);
    }

    private async Task<TabelasTributarias> ObterTabelasAsync(CancellationToken cancellationToken)
    {
        if (Volatile.Read(ref _tabelas) is { } carregadas)
            return carregadas;

        var versao = Volatile.Read(ref _versao);
        await using var conexao = await banco.AbrirAsync(cancellationToken);

        // GetDecimal converte o texto que o SQLite gera para cada REAL, mantendo a mesma escala decimal lida pelo EF Core até então.
        // A ordenação por Id preserva a ordem física usada antes quando há mais de um registro na mesma competência.
        var tabelas = new TabelasTributarias(
            await conexao.ListarAsync(null, "SELECT Competencia, Faixa, Valor, Porcentagem FROM Inss ORDER BY Id",
                leitor => new Vigencia<FaixaTributariaDto>(leitor.GetDateTime(0), new FaixaTributariaDto(leitor.GetInt32(1), leitor.GetDecimal(2), leitor.GetDecimal(3), 0m)), cancellationToken),
            await conexao.ListarAsync(null, "SELECT Competencia, Faixa, Valor, Porcentagem, Deducao FROM Irrf ORDER BY Id",
                leitor => new Vigencia<FaixaTributariaDto>(leitor.GetDateTime(0), new FaixaTributariaDto(leitor.GetInt32(1), leitor.GetDecimal(2), leitor.GetDecimal(3), leitor.GetDecimal(4))), cancellationToken),
            await conexao.ListarAsync(null, "SELECT Competencia, Valor FROM Simplificado ORDER BY Id",
                leitor => new Vigencia<decimal>(leitor.GetDateTime(0), leitor.GetDecimal(1)), cancellationToken),
            await conexao.ListarAsync(null, "SELECT Competencia, Valor FROM Dependente ORDER BY Id",
                leitor => new Vigencia<decimal>(leitor.GetDateTime(0), leitor.GetDecimal(1)), cancellationToken),
            await conexao.ListarAsync(null, "SELECT Competencia, Valor FROM DescontoMinimo ORDER BY Id",
                leitor => new Vigencia<decimal>(leitor.GetDateTime(0), leitor.GetDecimal(1)), cancellationToken),
            await conexao.ListarAsync(null, "SELECT Competencia, Faixa, LimiteRendimentos, Multiplicador, ValorBase FROM ReducaoMensalIrrf ORDER BY Id",
                leitor => new Vigencia<RegraReducaoMensalIrrfDto>(leitor.GetDateTime(0), new RegraReducaoMensalIrrfDto(leitor.GetInt32(1), leitor.GetDecimal(2), leitor.GetDecimal(3), leitor.GetDecimal(4))), cancellationToken));

        // Se houve gravação durante a carga, o resultado atende esta chamada, mas não é guardado.
        if (versao == Volatile.Read(ref _versao))
            Volatile.Write(ref _tabelas, tabelas);
        return tabelas;
    }

    private static DateTime ObterUltimaCompetencia<T>(IEnumerable<Vigencia<T>> vigencias, DateTime competencia, string tabela) =>
        ObterUltimaCompetenciaOpcional(vigencias, competencia)
        ?? throw new InvalidOperationException($"Não há dados de {tabela} cadastrados para a competência informada.");

    private static DateTime? ObterUltimaCompetenciaOpcional<T>(IEnumerable<Vigencia<T>> vigencias, DateTime competencia) =>
        vigencias.Where(item => item.Competencia <= competencia).Max(item => (DateTime?)item.Competencia);

    private sealed record Vigencia<T>(DateTime Competencia, T Valor);

    private sealed record TabelasTributarias(
        Vigencia<FaixaTributariaDto>[] FaixasInss,
        Vigencia<FaixaTributariaDto>[] FaixasIrrf,
        Vigencia<decimal>[] Simplificados,
        Vigencia<decimal>[] Dependentes,
        Vigencia<decimal>[] DescontosMinimos,
        Vigencia<RegraReducaoMensalIrrfDto>[] ReducoesMensais);
}
