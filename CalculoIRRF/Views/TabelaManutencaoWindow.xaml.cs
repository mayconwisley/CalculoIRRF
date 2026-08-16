using CalculoIRRF.Presentation.ViewModels;
using System.Windows;

namespace CalculoIRRF.Views;

public partial class TabelaManutencaoWindow : Window
{
    public TabelaManutencaoWindow(TabelaManutencaoViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        Loaded += (_, _) => ((TabelaManutencaoViewModel)DataContext).CarregarCommand.Execute(null);
    }
}
