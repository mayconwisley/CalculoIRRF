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
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class App : System.Windows.Application
{
    private ServiceProvider _serviceProvider = null!;
    private IServiceScope _applicationScope = null!;

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        // O WPF formata bindings (StringFormat) em en-US por padrão; o app inteiro exibe valores no padrão brasileiro.
        FrameworkElement.LanguageProperty.OverrideMetadata(typeof(FrameworkElement), new FrameworkPropertyMetadata(XmlLanguage.GetLanguage("pt-BR")));
        ThemeManager.Initialize();
        if (!ThemeManager.UseHardwareAcceleration)
            RenderOptions.ProcessRenderMode = RenderMode.SoftwareOnly;
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
        .AddScoped<IEstabilidadeViewModelFactory, EstabilidadeViewModelFactory>()
        .AddSingleton<IUserNotifier, WpfUserNotifier>()
        .AddSingleton<IArquivoDialogService, WpfArquivoDialogService>()
        .AddSingleton<IRelatorioPdfService, QuestPdfRelatorioPdfService>()
        .AddScoped<IWindowNavigator, WpfWindowNavigator>()
        .AddScoped<ISimularImpostoUseCase, SimularImpostoUseCase>()
        .AddScoped<ISimularPensaoUseCase, SimularPensaoUseCase>()
        .AddScoped<ISimularEstabilidadeUseCase, SimularEstabilidadeUseCase>()
        .AddInfrastructure()
        .BuildServiceProvider();
}
