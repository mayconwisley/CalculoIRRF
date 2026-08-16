using System.Windows;

namespace CalculoIRRF.Presentation.Services;

public sealed class WpfUserNotifier : IUserNotifier
{
    public void MostrarAviso(string mensagem, string titulo = "Dados inválidos") =>
        MessageBox.Show(mensagem, titulo, MessageBoxButton.OK, MessageBoxImage.Warning);

    public void MostrarErro(string mensagem, Exception exception) =>
        MessageBox.Show($"{mensagem}\n\n{exception.Message}", "Calculadora de Imposto", MessageBoxButton.OK, MessageBoxImage.Error);
}
