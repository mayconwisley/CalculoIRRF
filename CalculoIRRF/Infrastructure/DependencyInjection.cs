using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.Management;
using CalculoIRRF.Infrastructure.Persistence;
using CalculoIRRF.Infrastructure.Tributacao;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net.Http;

namespace CalculoIRRF.Infrastructure;

public static class DependencyInjection
{
    // O banco fica na pasta do executável, e não na pasta de trabalho, que muda conforme o atalho ou o terminal que abriu o app.
    private static readonly string CaminhoBanco = Path.Combine(AppContext.BaseDirectory, "BancoDados", "calculoIrrf.db");

    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
        .AddSingleton(new BancoTributario($"Data Source={CaminhoBanco}"))
        .AddSingleton<Func<HttpClient>>(static () => new HttpClient())
        .AddSingleton<SqliteTributacaoConsulta>()
        .AddSingleton<ITributacaoConsulta>(provider => provider.GetRequiredService<SqliteTributacaoConsulta>())
        .AddSingleton<ICacheTabelasTributarias>(provider => provider.GetRequiredService<SqliteTributacaoConsulta>())
        .AddScoped<ITabelaTributariaService, SqliteTabelaTributariaService>()
        .AddScoped<IAtualizadorTabelaIrrf, AtualizadorTabelaIrrfDaReceitaFederal>()
        .AddScoped<IAtualizadorTabelaInss, AtualizadorTabelaInssDoGoverno>()
        .AddScoped<IInicializadorBancoTributario, InicializadorBancoTributario>();
}
