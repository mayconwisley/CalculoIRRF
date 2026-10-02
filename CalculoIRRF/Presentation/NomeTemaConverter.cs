using System.Globalization;
using System.Windows.Data;

namespace CalculoIRRF.Presentation;

/// <summary>Exibe o nome de cada <see cref="ThemeMode"/> como o usuário lê, com acentuação.</summary>
public sealed class NomeTemaConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value switch
    {
        ThemeMode.Automatico => "Automático",
        ThemeMode.Claro => "Claro",
        ThemeMode.Escuro => "Escuro",
        _ => value?.ToString() ?? string.Empty
    };

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => Binding.DoNothing;
}
