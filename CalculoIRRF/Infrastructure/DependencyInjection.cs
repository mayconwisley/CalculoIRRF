using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.Management;
using CalculoIRRF.Infrastructure.Persistence;
using CalculoIRRF.Infrastructure.Tributacao;
using CalculoIRRF.Infrastructure.Tributacao.Fontes;
using Microsoft.Extensions.DependencyInjection;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace CalculoIRRF.Infrastructure;

public static class DependencyInjection
{
    // O banco fica na pasta do executável, e não na pasta de trabalho, que muda conforme o atalho ou o terminal que abriu o app.
    private static readonly string CaminhoBanco = Path.Combine(AppContext.BaseDirectory, "BancoDados", "calculoIrrf.db");

    // As fontes de cada tabela aparecem nas mensagens ao usuário na ordem em que estão registradas.
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
        .AddSingleton(new BancoTributario($"Data Source={CaminhoBanco}"))
        .AddSingleton<Func<HttpClient>>(static () => CriarHttpClient())
        .AddSingleton<SqliteTributacaoConsulta>()
        .AddSingleton<ITributacaoConsulta>(provider => provider.GetRequiredService<SqliteTributacaoConsulta>())
        .AddSingleton<ICacheTabelasTributarias>(provider => provider.GetRequiredService<SqliteTributacaoConsulta>())
        .AddScoped<ITabelaTributariaService, SqliteTabelaTributariaService>()
        .AddSingleton<IFonteTabela<TabelaIrrfPublicada>, FonteIrrfReceitaFederal>()
        .AddSingleton<IFonteTabela<TabelaIrrfPublicada>, FonteIrrfDebit>()
        .AddSingleton<IFonteTabela<TabelaIrrfPublicada>, FonteIrrfContabeis>()
        .AddSingleton<IFonteTabela<TabelaInssPublicada>, FonteInssGovBr>()
        .AddSingleton<IFonteTabela<TabelaInssPublicada>, FonteInssDebit>()
        .AddSingleton<IFonteTabela<TabelaInssPublicada>, FonteInssContabeis>()
        .AddScoped<IAtualizadorTabelaIrrf, AtualizadorTabelaIrrf>()
        .AddScoped<IAtualizadorTabelaInss, AtualizadorTabelaInss>()
        .AddScoped<IInicializadorBancoTributario, InicializadorBancoTributario>();

    // Identifica o aplicativo para os sites consultados e limita a espera: uma fonte fora do ar não pode travar a atualização.
    private static HttpClient CriarHttpClient()
    {
        var httpClient = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };
        httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("CalculadoraDeImposto", typeof(DependencyInjection).Assembly.GetName().Version?.ToString(3) ?? "1.0.0"));
        httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("(+https://github.com/mayconwisley/CalculoIRRF)"));
        return httpClient;
    }
}
