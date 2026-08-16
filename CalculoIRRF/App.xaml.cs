using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Infrastructure;
using CalculoIRRF.Infrastructure.Reporting;
using CalculoIRRF.Infrastructure.Persistence;
using CalculoIRRF.Presentation;
using CalculoIRRF.Presentation.Services;
using CalculoIRRF.Presentation.ViewModels;
using CalculoIRRF.Views;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.Versioning;
using System.Windows;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class App : System.Windows.Application
{
    private ServiceProvider _serviceProvider = null!;
    private IServiceScope _applicationScope = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
        ThemeManager.Initialize();
        _serviceProvider = ConfigureServices();
        _applicationScope = _serviceProvider.CreateScope();
        _applicationScope.ServiceProvider.GetRequiredService<IInicializadorBancoTributario>()
            .InicializarAsync(CancellationToken.None).GetAwaiter().GetResult();
        _applicationScope.ServiceProvider.GetRequiredService<MainWindow>().Show();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        ThemeManager.Dispose();
        _applicationScope?.Dispose();
        _serviceProvider?.Dispose();
        base.OnExit(e);
    }

    private static ServiceProvider ConfigureServices() => new ServiceCollection()
        .AddScoped<MainWindow>()
        .AddScoped<MainWindowViewModel>()
        .AddScoped<ITabelaManutencaoViewModelFactory, TabelaManutencaoViewModelFactory>()
        .AddScoped<IPensaoViewModelFactory, PensaoViewModelFactory>()
        .AddSingleton<IUserNotifier, WpfUserNotifier>()
        .AddSingleton<IArquivoDialogService, WpfArquivoDialogService>()
        .AddSingleton<IRelatorioPdfService, QuestPdfRelatorioPdfService>()
        .AddScoped<IWindowNavigator, WpfWindowNavigator>()
        .AddScoped<ISimularImpostoUseCase, SimularImpostoUseCase>()
        .AddScoped<ISimularPensaoUseCase, SimularPensaoUseCase>()
        .AddInfrastructure()
        .BuildServiceProvider();
}
