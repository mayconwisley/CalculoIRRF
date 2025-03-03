using CalculoIRRF.DataBase;
using CalculoIRRF.Repository;
using CalculoIRRF.Repository.Interface;
using CalculoIRRF.Services;
using CalculoIRRF.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace CalculoIRRF;
[SupportedOSPlatform("windows")]
internal static class Program
{
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        ServiceProvider serviceProvider = SettingsService();
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        FrmPrincipal frmPrincipal = serviceProvider.GetRequiredService<FrmPrincipal>();
        Application.Run(frmPrincipal);
    }

    static ServiceProvider SettingsService()
    {
        var serviceProvider = new ServiceCollection()
        .AddDbContext<CalculoImpostoContext>(op => op.UseSqlite("Data Source = BancoDados/calculoIrrf.db"))
        .AddSingleton<FrmPrincipal>()
        .AddSingleton<FrmDeducaoSimplificada>()
        .AddSingleton<FrmDependente>()
        .AddSingleton<FrmPensao>()
        .AddSingleton<FrmTabelaINSS>()
        .AddSingleton<FrmTabelaIRRF>()
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
        .BuildServiceProvider();

        return serviceProvider;
    }
}
