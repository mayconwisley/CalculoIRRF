using CalculoIRRF.Presentation.ViewModels;
using CalculoIRRF.Application.Management;
using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Views;
using System.Windows;

namespace CalculoIRRF.Presentation.Services;

/// <summary>Detalhe de navegação WPF, isolado dos ViewModels.</summary>
public sealed class WpfWindowNavigator(ITabelaManutencaoViewModelFactory tabelaViewModelFactory, IPensaoViewModelFactory pensaoViewModelFactory, IEstabilidadeViewModelFactory estabilidadeViewModelFactory) : IWindowNavigator
{
    public void AbrirTabelaInss() => Abrir(new TabelaManutencaoWindow(tabelaViewModelFactory.Criar(TipoTabelaTributaria.Inss)));
    public void AbrirTabelaIrrf() => Abrir(new TabelaManutencaoWindow(tabelaViewModelFactory.Criar(TipoTabelaTributaria.Irrf)));
    public void AbrirSimplificado() => Abrir(new TabelaManutencaoWindow(tabelaViewModelFactory.Criar(TipoTabelaTributaria.Simplificado)));
    public void AbrirDependentes() => Abrir(new TabelaManutencaoWindow(tabelaViewModelFactory.Criar(TipoTabelaTributaria.Dependente)));
    public void AbrirDescontoMinimo() => Abrir(new TabelaManutencaoWindow(tabelaViewModelFactory.Criar(TipoTabelaTributaria.DescontoMinimo)));
    public void AbrirReducaoMensalIrrf() => Abrir(new TabelaManutencaoWindow(tabelaViewModelFactory.Criar(TipoTabelaTributaria.ReducaoMensalIrrf)));
    public void AbrirPensao(EntradaPensaoViewModel entrada) => Abrir(new PensaoWindow(pensaoViewModelFactory.Criar(entrada)));
    public void AbrirEstabilidade() => Abrir(new EstabilidadeWindow(estabilidadeViewModelFactory.Criar()));

    private static void Abrir(Window janela)
    {
        janela.Owner = System.Windows.Application.Current.MainWindow;
        janela.ShowDialog();
    }
}
