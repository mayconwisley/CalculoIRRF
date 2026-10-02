using CalculoIRRF.Application.Management;
using CalculoIRRF.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace CalculoIRRF.Infrastructure.Persistence;

public sealed class EfTabelaTributariaService(IDbContextFactory<CalculoIrrfDbContext> contextFactory, ICacheTabelasTributarias cache) : ITabelaTributariaService
{
    public async Task<IReadOnlyList<RegistroTabelaDto>> ListarAsync(TipoTabelaTributaria tipo, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        return tipo switch
        {
            TipoTabelaTributaria.Inss => await context.FaixasInss.AsNoTracking().OrderByDescending(x => x.Competencia).ThenBy(x => x.Faixa).Select(x => new RegistroTabelaDto(x.Id, DateOnly.FromDateTime(x.Competencia), x.Faixa, (decimal)x.Valor, (decimal)x.Porcentagem, null)).ToArrayAsync(cancellationToken),
            TipoTabelaTributaria.Irrf => await context.FaixasIrrf.AsNoTracking().OrderByDescending(x => x.Competencia).ThenBy(x => x.Faixa).Select(x => new RegistroTabelaDto(x.Id, DateOnly.FromDateTime(x.Competencia), x.Faixa, (decimal)x.Valor, (decimal)x.Porcentagem, (decimal)x.Deducao)).ToArrayAsync(cancellationToken),
            TipoTabelaTributaria.Simplificado => await context.ParametrosSimplificados.AsNoTracking().OrderByDescending(x => x.Competencia).Select(x => new RegistroTabelaDto(x.Id, DateOnly.FromDateTime(x.Competencia), null, (decimal)x.Valor, null, null)).ToArrayAsync(cancellationToken),
            TipoTabelaTributaria.Dependente => await context.ParametrosDependentes.AsNoTracking().OrderByDescending(x => x.Competencia).Select(x => new RegistroTabelaDto(x.Id, DateOnly.FromDateTime(x.Competencia), null, (decimal)x.Valor, null, null)).ToArrayAsync(cancellationToken),
            TipoTabelaTributaria.DescontoMinimo => await context.ParametrosDescontoMinimo.AsNoTracking().OrderByDescending(x => x.Competencia).Select(x => new RegistroTabelaDto(x.Id, DateOnly.FromDateTime(x.Competencia), null, (decimal)x.Valor, null, null)).ToArrayAsync(cancellationToken),
            _ => await context.ReducoesMensaisIrrf.AsNoTracking().OrderByDescending(x => x.Competencia).ThenBy(x => x.Faixa).Select(x => new RegistroTabelaDto(x.Id, DateOnly.FromDateTime(x.Competencia), x.Faixa, (decimal)x.LimiteRendimentos, (decimal)x.Multiplicador, (decimal)x.ValorBase)).ToArrayAsync(cancellationToken)
        };
    }

    public async Task SalvarAsync(TipoTabelaTributaria tipo, SalvarRegistroTabelaRequest request, CancellationToken cancellationToken)
    {
        Validar(tipo, request);
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        var competencia = request.Competencia.ToDateTime(TimeOnly.MinValue);
        switch (tipo)
        {
            case TipoTabelaTributaria.Inss:
                var inss = request.Id == 0 ? new FaixaInssEntity() : await context.FaixasInss.FindAsync([request.Id], cancellationToken) ?? throw new KeyNotFoundException("Faixa INSS não encontrada.");
                inss.Competencia = competencia; inss.Faixa = request.Faixa!.Value; inss.Valor = (double)request.Valor; inss.Porcentagem = (double)request.Aliquota!.Value; if (request.Id == 0) context.FaixasInss.Add(inss); break;
            case TipoTabelaTributaria.Irrf:
                var irrf = request.Id == 0 ? new FaixaIrrfEntity() : await context.FaixasIrrf.FindAsync([request.Id], cancellationToken) ?? throw new KeyNotFoundException("Faixa IRRF não encontrada.");
                irrf.Competencia = competencia; irrf.Faixa = request.Faixa!.Value; irrf.Valor = (double)request.Valor; irrf.Porcentagem = (double)request.Aliquota!.Value; irrf.Deducao = (double)request.Deducao!.Value; if (request.Id == 0) context.FaixasIrrf.Add(irrf); break;
            case TipoTabelaTributaria.Simplificado:
                var simplificado = request.Id == 0 ? new ParametroSimplificadoEntity() : await context.ParametrosSimplificados.FindAsync([request.Id], cancellationToken) ?? throw new KeyNotFoundException();
                simplificado.Competencia = competencia; simplificado.Valor = (double)request.Valor; if (request.Id == 0) context.ParametrosSimplificados.Add(simplificado); break;
            case TipoTabelaTributaria.Dependente:
                var dependente = request.Id == 0 ? new ParametroDependenteEntity() : await context.ParametrosDependentes.FindAsync([request.Id], cancellationToken) ?? throw new KeyNotFoundException();
                dependente.Competencia = competencia; dependente.Valor = (double)request.Valor; if (request.Id == 0) context.ParametrosDependentes.Add(dependente); break;
            case TipoTabelaTributaria.DescontoMinimo:
                var minimo = request.Id == 0 ? new ParametroDescontoMinimoEntity() : await context.ParametrosDescontoMinimo.FindAsync([request.Id], cancellationToken) ?? throw new KeyNotFoundException();
                minimo.Competencia = competencia; minimo.Valor = (double)request.Valor; if (request.Id == 0) context.ParametrosDescontoMinimo.Add(minimo); break;
            default:
                var reducao = request.Id == 0 ? new ReducaoIrrfMensalEntity() : await context.ReducoesMensaisIrrf.FindAsync([request.Id], cancellationToken) ?? throw new KeyNotFoundException("Faixa de redução mensal não encontrada.");
                reducao.Competencia = competencia; reducao.Faixa = request.Faixa!.Value; reducao.LimiteRendimentos = (double)request.Valor; reducao.Multiplicador = (double)request.Aliquota!.Value; reducao.ValorBase = (double)request.Deducao!.Value; if (request.Id == 0) context.ReducoesMensaisIrrf.Add(reducao); break;
        }
        await context.SaveChangesAsync(cancellationToken);
        cache.Invalidar();
    }

    public async Task ExcluirAsync(TipoTabelaTributaria tipo, int id, CancellationToken cancellationToken)
    {
        await using var context = await contextFactory.CreateDbContextAsync(cancellationToken);
        switch (tipo)
        {
            case TipoTabelaTributaria.Inss: context.Remove(await context.FaixasInss.FindAsync([id], cancellationToken) ?? throw new KeyNotFoundException()); break;
            case TipoTabelaTributaria.Irrf: context.Remove(await context.FaixasIrrf.FindAsync([id], cancellationToken) ?? throw new KeyNotFoundException()); break;
            case TipoTabelaTributaria.Simplificado: context.Remove(await context.ParametrosSimplificados.FindAsync([id], cancellationToken) ?? throw new KeyNotFoundException()); break;
            case TipoTabelaTributaria.Dependente: context.Remove(await context.ParametrosDependentes.FindAsync([id], cancellationToken) ?? throw new KeyNotFoundException()); break;
            case TipoTabelaTributaria.DescontoMinimo: context.Remove(await context.ParametrosDescontoMinimo.FindAsync([id], cancellationToken) ?? throw new KeyNotFoundException()); break;
            default: context.Remove(await context.ReducoesMensaisIrrf.FindAsync([id], cancellationToken) ?? throw new KeyNotFoundException()); break;
        }
        await context.SaveChangesAsync(cancellationToken);
        cache.Invalidar();
    }

    private static void Validar(TipoTabelaTributaria tipo, SalvarRegistroTabelaRequest request)
    {
        if (request.Valor < 0m || (request.Aliquota is < 0m) || (request.Deducao is < 0m) || (tipo is TipoTabelaTributaria.Inss or TipoTabelaTributaria.Irrf or TipoTabelaTributaria.ReducaoMensalIrrf && (request.Faixa is null or <= 0 || request.Aliquota is null)) || (tipo is TipoTabelaTributaria.Irrf or TipoTabelaTributaria.ReducaoMensalIrrf && request.Deducao is null))
            throw new ArgumentException("Os dados informados são inválidos.");
    }
}
