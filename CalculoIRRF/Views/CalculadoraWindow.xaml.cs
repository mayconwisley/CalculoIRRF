using CalculoIRRF.Presentation.ViewModels.Calculadoras;
using System.Windows;

namespace CalculoIRRF.Views;

public partial class CalculadoraWindow : Window
{
    public CalculadoraWindow(CalculadoraViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
