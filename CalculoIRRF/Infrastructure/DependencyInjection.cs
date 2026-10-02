using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.Management;
using CalculoIRRF.Infrastructure.Persistence;
using CalculoIRRF.Infrastructure.Tributacao;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;

namespace CalculoIRRF.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
        .AddSingleton(new BancoTributario("Data Source=BancoDados/calculoIrrf.db"))
        .AddSingleton<Func<HttpClient>>(static () => new HttpClient())
        .AddSingleton<SqliteTributacaoConsulta>()
        .AddSingleton<ITributacaoConsulta>(provider => provider.GetRequiredService<SqliteTributacaoConsulta>())
        .AddSingleton<ICacheTabelasTributarias>(provider => provider.GetRequiredService<SqliteTributacaoConsulta>())
        .AddScoped<ITabelaTributariaService, SqliteTabelaTributariaService>()
        .AddScoped<IAtualizadorTabelaIrrf, AtualizadorTabelaIrrfDaReceitaFederal>()
        .AddScoped<IAtualizadorTabelaInss, AtualizadorTabelaInssDoGoverno>()
        .AddScoped<IInicializadorBancoTributario, InicializadorBancoTributario>();
}
