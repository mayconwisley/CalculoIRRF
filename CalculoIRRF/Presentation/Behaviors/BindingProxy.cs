using System.Windows;

namespace CalculoIRRF.Presentation.Behaviors;

/// <summary>
/// Leva o DataContext da janela a elementos que não fazem parte da árvore visual, como as colunas do DataGrid.
/// </summary>
public sealed class BindingProxy : Freezable
{
    public static readonly DependencyProperty DataProperty = DependencyProperty.Register(nameof(Data), typeof(object), typeof(BindingProxy));

    public object Data { get => GetValue(DataProperty); set => SetValue(DataProperty, value); }

    protected override Freezable CreateInstanceCore() => new BindingProxy();
}
