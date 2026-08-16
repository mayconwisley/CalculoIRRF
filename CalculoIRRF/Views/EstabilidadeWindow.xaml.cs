using CalculoIRRF.Presentation.ViewModels;
using System.Windows;

namespace CalculoIRRF.Views;

public partial class EstabilidadeWindow : Window
{
    public EstabilidadeWindow(EstabilidadeViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
