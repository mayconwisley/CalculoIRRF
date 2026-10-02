#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CalculoIRRF.Infrastructure.Persistence;

/// <summary>
/// Adaptador EF Core da porta de leitura consumida pela camada Application.
/// As tabelas são pequenas e só mudam pela manutenção ou pela atualização online, por isso são carregadas uma vez
/// e mantidas em memória até <see cref="Invalidar"/> ser chamado.
/// </summary>
public sealed class EfTributacaoConsulta(IDbContextFactory<CalculoIrrfDbContext> contextFactory) : ITributacaoConsulta, ICacheTabelasTributarias
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
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);

        // As conversões para decimal ficam na consulta para manter a mesma leitura feita pelo provedor SQLite.
        // A ordenação por Id preserva a ordem física usada antes quando há mais de um registro na mesma competência.
        var tabelas = new TabelasTributarias(
            await context.FaixasInss.OrderBy(item => item.Id)
                .Select(item => new Vigencia<FaixaTributariaDto>(item.Competencia, new FaixaTributariaDto(item.Faixa, (decimal)item.Valor, (decimal)item.Porcentagem, 0m))).ToArrayAsync(cancellationToken),
            await context.FaixasIrrf.OrderBy(item => item.Id)
                .Select(item => new Vigencia<FaixaTributariaDto>(item.Competencia, new FaixaTributariaDto(item.Faixa, (decimal)item.Valor, (decimal)item.Porcentagem, (decimal)item.Deducao))).ToArrayAsync(cancellationToken),
            await context.ParametrosSimplificados.OrderBy(item => item.Id)
                .Select(item => new Vigencia<decimal>(item.Competencia, (decimal)item.Valor)).ToArrayAsync(cancellationToken),
            await context.ParametrosDependentes.OrderBy(item => item.Id)
                .Select(item => new Vigencia<decimal>(item.Competencia, (decimal)item.Valor)).ToArrayAsync(cancellationToken),
            await context.ParametrosDescontoMinimo.OrderBy(item => item.Id)
                .Select(item => new Vigencia<decimal>(item.Competencia, (decimal)item.Valor)).ToArrayAsync(cancellationToken),
            await context.ReducoesMensaisIrrf.OrderBy(item => item.Id)
                .Select(item => new Vigencia<RegraReducaoMensalIrrfDto>(item.Competencia, new RegraReducaoMensalIrrfDto(item.Faixa, (decimal)item.LimiteRendimentos, (decimal)item.Multiplicador, (decimal)item.ValorBase))).ToArrayAsync(cancellationToken));

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
