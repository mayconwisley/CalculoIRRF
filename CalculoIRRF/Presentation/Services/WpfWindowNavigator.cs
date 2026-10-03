using CalculoIRRF.Presentation.ViewModels;
using CalculoIRRF.Presentation.ViewModels.Calculadoras;
using CalculoIRRF.Application.Management;
using CalculoIRRF.Views;
using System.Windows;

namespace CalculoIRRF.Presentation.Services;

/// <summary>Detalhe de navegação WPF, isolado dos ViewModels.</summary>
public sealed class WpfWindowNavigator(
    ITabelaManutencaoViewModelFactory tabelaViewModelFactory,
    IPensaoViewModelFactory pensaoViewModelFactory,
    IEstabilidadeViewModelFactory estabilidadeViewModelFactory,
    ICalculadoraViewModelFactory calculadoraViewModelFactory) : IWindowNavigator
{
    public void AbrirTabela(TipoTabelaTributaria tipo) => Abrir(new TabelaManutencaoWindow(tabelaViewModelFactory.Criar(tipo)));
    public void AbrirPensao(EntradaPensaoViewModel entrada) => Abrir(new PensaoWindow(pensaoViewModelFactory.Criar(entrada)));
    public void AbrirEstabilidade() => Abrir(new EstabilidadeWindow(estabilidadeViewModelFactory.Criar()));
    public void AbrirCalculadora(TipoCalculadora tipo, ContextoCalculo contexto) => Abrir(new CalculadoraWindow(calculadoraViewModelFactory.Criar(tipo, contexto)));

    private static void Abrir(Window janela)
    {
        janela.Owner = System.Windows.Application.Current.MainWindow;
        janela.ShowDialog();
    }
}
