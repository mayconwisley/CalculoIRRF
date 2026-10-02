using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Presentation.ViewModels.Calculadoras;
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
using System.Windows.Input;
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
        // ApplicationCommands.Help já responde ao F1; registrado em Window, vale para todas as janelas e para o botão "Manual".
        var manual = _serviceProvider.GetRequiredService<IManualUsuarioService>();
        CommandManager.RegisterClassCommandBinding(typeof(Window), new CommandBinding(ApplicationCommands.Help, (_, _) => manual.Abrir()));
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
        .AddSingleton<IManualUsuarioService, WpfManualUsuarioService>()
        .AddSingleton<IRelatorioPdfService, QuestPdfRelatorioPdfService>()
        .AddScoped<IWindowNavigator, WpfWindowNavigator>()
        .AddScoped<ISimularImpostoUseCase, SimularImpostoUseCase>()
        .AddScoped<ISimularPensaoUseCase, SimularPensaoUseCase>()
        .AddScoped<ISimularEstabilidadeUseCase, SimularEstabilidadeUseCase>()
        .AddScoped<ICalculadoraViewModelFactory, CalculadoraViewModelFactory>()
        .AddScoped<ISimularDemonstrativoUseCase<SimularSalarioPeloLiquidoRequest>, SimularSalarioPeloLiquidoUseCase>()
        .AddScoped<ISimularDemonstrativoUseCase<SimularDecimoTerceiroRequest>, SimularDecimoTerceiroUseCase>()
        .AddScoped<ISimularDemonstrativoUseCase<SimularFeriasRequest>, SimularFeriasUseCase>()
        .AddScoped<ISimularDemonstrativoUseCase<SimularHorasExtrasRequest>, SimularHorasExtrasUseCase>()
        .AddScoped<ISimularDemonstrativoUseCase<SimularRescisaoRequest>, SimularRescisaoUseCase>()
        .AddScoped<ISimularDemonstrativoUseCase<SimularCustoFuncionarioRequest>, SimularCustoFuncionarioUseCase>()
        .AddScoped<ISimularDemonstrativoUseCase<SimularProLaboreRequest>, SimularProLaboreUseCase>()
        // Cada janela de calculadora recebe uma calculadora nova, com o formulário em branco.
        .AddKeyedTransient<ICalculadora, CalculadoraSalarioPeloLiquido>(TipoCalculadora.SalarioPeloLiquido)
        .AddKeyedTransient<ICalculadora, CalculadoraDecimoTerceiro>(TipoCalculadora.DecimoTerceiro)
        .AddKeyedTransient<ICalculadora, CalculadoraFerias>(TipoCalculadora.Ferias)
        .AddKeyedTransient<ICalculadora, CalculadoraHorasExtras>(TipoCalculadora.HorasExtras)
        .AddKeyedTransient<ICalculadora, CalculadoraRescisao>(TipoCalculadora.Rescisao)
        .AddKeyedTransient<ICalculadora, CalculadoraCustoFuncionario>(TipoCalculadora.CustoFuncionario)
        .AddKeyedTransient<ICalculadora, CalculadoraProLabore>(TipoCalculadora.ProLaboreAutonomo)
        .AddInfrastructure()
        .BuildServiceProvider();
}
