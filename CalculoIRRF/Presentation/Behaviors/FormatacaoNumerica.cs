using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace CalculoIRRF.Presentation.Behaviors;

public static class FormatacaoNumerica
{
    private static readonly CultureInfo CulturaPtBr = CultureInfo.GetCultureInfo("pt-BR");

    public static readonly DependencyProperty FormatoProperty = DependencyProperty.RegisterAttached(
        "Formato",
        typeof(string),
        typeof(FormatacaoNumerica),
        new PropertyMetadata(null, AoAlterarFormato));

    public static string GetFormato(DependencyObject objeto) => (string)objeto.GetValue(FormatoProperty);

    public static void SetFormato(DependencyObject objeto, string valor) => objeto.SetValue(FormatoProperty, valor);

    private static void AoAlterarFormato(DependencyObject objeto, DependencyPropertyChangedEventArgs argumentos)
    {
        if (objeto is not TextBox campo)
            return;

        campo.GotKeyboardFocus -= LimparZeroAoReceberFoco;
        campo.LostKeyboardFocus -= FormatarAoPerderFoco;

        if (argumentos.NewValue is string { Length: > 0 })
        {
            campo.GotKeyboardFocus += LimparZeroAoReceberFoco;
            campo.LostKeyboardFocus += FormatarAoPerderFoco;
        }
    }

    private static void LimparZeroAoReceberFoco(object sender, RoutedEventArgs argumentos)
    {
        if (sender is not TextBox { Text: var texto } campo || !decimal.TryParse(texto, NumberStyles.Number, CulturaPtBr, out var valor) || valor != 0m)
            return;

        campo.Clear();
        BindingOperations.GetBindingExpression(campo, TextBox.TextProperty)?.UpdateSource();
    }

    private static void FormatarAoPerderFoco(object sender, RoutedEventArgs argumentos)
    {
        if (sender is not TextBox { Text: var texto } campo)
            return;

        if (string.IsNullOrWhiteSpace(texto))
        {
            campo.Text = 0m.ToString(GetFormato(campo), CulturaPtBr);
            BindingOperations.GetBindingExpression(campo, TextBox.TextProperty)?.UpdateSource();
            return;
        }

        if (!decimal.TryParse(texto, NumberStyles.Number, CulturaPtBr, out var valor))
            return;

        campo.Text = valor.ToString(GetFormato(campo), CulturaPtBr);
        BindingOperations.GetBindingExpression(campo, TextBox.TextProperty)?.UpdateSource();
    }
}
