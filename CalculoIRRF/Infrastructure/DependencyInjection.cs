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
        .AddDbContext<CalculoIrrfDbContext>(options => options.UseSqlite("Data Source=BancoDados/calculoIrrf.db"))
        .AddSingleton(new HttpClient())
        .AddScoped<ITributacaoConsulta, EfTributacaoConsulta>()
        .AddScoped<ITabelaTributariaService, EfTabelaTributariaService>()
        .AddScoped<IAtualizadorTabelaIrrf, AtualizadorTabelaIrrfDaReceitaFederal>()
        .AddScoped<IInicializadorBancoTributario, InicializadorBancoTributario>();
}
