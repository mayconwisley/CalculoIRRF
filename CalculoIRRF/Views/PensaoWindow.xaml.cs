using CalculoIRRF.Presentation.ViewModels;
using System.Windows;

namespace CalculoIRRF.Views;

public partial class PensaoWindow : Window
{
    public PensaoWindow(PensaoViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
