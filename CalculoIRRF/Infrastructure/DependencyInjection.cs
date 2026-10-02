using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.Management;
using CalculoIRRF.Infrastructure.Persistence;
using CalculoIRRF.Infrastructure.Tributacao;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace CalculoIRRF.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
        .AddDbContextFactory<CalculoIrrfDbContext>(options => options.UseSqlite("Data Source=BancoDados/calculoIrrf.db"))
        .AddSingleton<Func<HttpClient>>(static () => new HttpClient())
        .AddSingleton<EfTributacaoConsulta>()
        .AddSingleton<ITributacaoConsulta>(provider => provider.GetRequiredService<EfTributacaoConsulta>())
        .AddSingleton<ICacheTabelasTributarias>(provider => provider.GetRequiredService<EfTributacaoConsulta>())
        .AddScoped<ITabelaTributariaService, EfTabelaTributariaService>()
        .AddScoped<IAtualizadorTabelaIrrf, AtualizadorTabelaIrrfDaReceitaFederal>()
        .AddScoped<IAtualizadorTabelaInss, AtualizadorTabelaInssDoGoverno>()
        .AddScoped<IInicializadorBancoTributario, InicializadorBancoTributario>();
}
