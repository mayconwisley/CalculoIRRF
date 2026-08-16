using CalculoIRRF.DataBase;
using CalculoIRRF.Repository;
using CalculoIRRF.Repository.Interface;
using CalculoIRRF.Presentation;
using CalculoIRRF.Services;
using CalculoIRRF.Services.Calculo;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Views;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.Versioning;
using System.Windows;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class App : Application
{
    private ServiceProvider _serviceProvider = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ThemeManager.Initialize();
        _serviceProvider = ConfigureServices();
        _serviceProvider.GetRequiredService<MainWindow>().Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        ThemeManager.Dispose();
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }

    private static ServiceProvider ConfigureServices() => new ServiceCollection()
        .AddDbContext<CalculoImpostoContext>(options => options.UseSqlite("Data Source=BancoDados/calculoIrrf.db"))
        .AddSingleton<MainWindow>()
        .AddScoped<IDependenteRepository, DependenteRepository>()
        .AddScoped<IDescontoMinimoRepository, DescontoMinimoRepository>()
        .AddScoped<IInssRepository, InssRepository>()
        .AddScoped<IIrrfRepository, IrrfRepository>()
        .AddScoped<ISimplificadoRepository, SimplificadoRepository>()
        .AddScoped<IDependenteServices, DependenteServices>()
        .AddScoped<IDescontoMinimoServices, DescontoMinimoServices>()
        .AddScoped<IInssServices, InssServices>()
        .AddScoped<IIrrfServices, IrrfServices>()
        .AddScoped<ISimplificadoServices, SimplificadoServices>()
        .AddScoped<CalculoImposto>()
        .BuildServiceProvider();
}
