using System.Runtime.Versioning;
using System.Windows;
using CalculoIRRF.Presentation.ViewModels;

namespace CalculoIRRF.Views;

[SupportedOSPlatform("windows")]
public partial class MainWindow : Window
{
    public MainWindow(MainWindowViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
