using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using Microsoft.EntityFrameworkCore;

namespace CalculoIRRF.Infrastructure.Persistence;

/// <summary>Adaptador EF Core da porta de leitura consumida pela camada Application.</summary>
public sealed class EfTributacaoConsulta(CalculoIrrfDbContext context) : ITributacaoConsulta
{
    public async Task<PerfilTributarioDto> ObterPerfilAsync(DateOnly competencia, CancellationToken cancellationToken)
    {
        var data = competencia.ToDateTime(TimeOnly.MinValue);
        var competenciaInss = await ObterUltimaCompetenciaAsync(context.FaixasInss.Select(item => item.Competencia), data, cancellationToken, "INSS");
        var competenciaIrrf = await ObterUltimaCompetenciaAsync(context.FaixasIrrf.Select(item => item.Competencia), data, cancellationToken, "IRRF");
        var competenciaSimplificado = await ObterUltimaCompetenciaAsync(context.ParametrosSimplificados.Select(item => item.Competencia), data, cancellationToken, "desconto simplificado");
        var competenciaDependente = await ObterUltimaCompetenciaAsync(context.ParametrosDependentes.Select(item => item.Competencia), data, cancellationToken, "dedução por dependente");
        var competenciaMinimo = await ObterUltimaCompetenciaAsync(context.ParametrosDescontoMinimo.Select(item => item.Competencia), data, cancellationToken, "desconto mínimo");
        var competenciaReducao = await ObterUltimaCompetenciaOpcionalAsync(context.ReducoesMensaisIrrf.Select(item => item.Competencia), data, cancellationToken);

        var inss = await context.FaixasInss.AsNoTracking().Where(item => item.Competencia == competenciaInss).OrderBy(item => item.Faixa)
            .Select(item => new FaixaTributariaDto(item.Faixa, (decimal)item.Valor, (decimal)item.Porcentagem, 0m)).ToArrayAsync(cancellationToken);
        var irrf = await context.FaixasIrrf.AsNoTracking().Where(item => item.Competencia == competenciaIrrf).OrderBy(item => item.Faixa)
            .Select(item => new FaixaTributariaDto(item.Faixa, (decimal)item.Valor, (decimal)item.Porcentagem, (decimal)item.Deducao)).ToArrayAsync(cancellationToken);

        var simplificado = await context.ParametrosSimplificados.AsNoTracking().Where(item => item.Competencia == competenciaSimplificado).Select(item => (decimal)item.Valor).FirstAsync(cancellationToken);
        var dependente = await context.ParametrosDependentes.AsNoTracking().Where(item => item.Competencia == competenciaDependente).Select(item => (decimal)item.Valor).FirstAsync(cancellationToken);
        var minimo = await context.ParametrosDescontoMinimo.AsNoTracking().Where(item => item.Competencia == competenciaMinimo).Select(item => (decimal)item.Valor).FirstAsync(cancellationToken);
        var reducoes = competenciaReducao is null
            ? []
            : await context.ReducoesMensaisIrrf.AsNoTracking().Where(item => item.Competencia == competenciaReducao).OrderBy(item => item.Faixa)
                .Select(item => new RegraReducaoMensalIrrfDto(item.Faixa, (decimal)item.LimiteRendimentos, (decimal)item.Multiplicador, (decimal)item.ValorBase)).ToArrayAsync(cancellationToken);

        return new PerfilTributarioDto(inss, irrf, simplificado, dependente, minimo, reducoes);
    }

    private static async Task<DateTime> ObterUltimaCompetenciaAsync(IQueryable<DateTime> competencias, DateTime competencia, CancellationToken cancellationToken, string tabela)
    {
        var ultima = await competencias.Where(item => item <= competencia).OrderByDescending(item => item).FirstOrDefaultAsync(cancellationToken);
        return ultima == default
            ? throw new InvalidOperationException($"Não há dados de {tabela} cadastrados para a competência informada.")
            : ultima;
    }

    private static async Task<DateTime?> ObterUltimaCompetenciaOpcionalAsync(IQueryable<DateTime> competencias, DateTime competencia, CancellationToken cancellationToken) =>
        await competencias.Where(item => item <= competencia).OrderByDescending(item => item).Select(item => (DateTime?)item).FirstOrDefaultAsync(cancellationToken);
}
