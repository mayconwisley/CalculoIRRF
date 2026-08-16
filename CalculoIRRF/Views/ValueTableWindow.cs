using CalculoIRRF.Model;
using CalculoIRRF.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CalculoIRRF.Views;

public enum ValueTableKind { Simplificado, Dependente, DescontoMinimo }

public sealed class ValueTableWindow : Window
{
    private static readonly CultureInfo PtBr = CultureInfo.GetCultureInfo("pt-BR");
    private readonly ValueTableKind _kind;
    private readonly ISimplificadoServices _simplificado = null!;
    private readonly IDependenteServices _dependente = null!;
    private readonly IDescontoMinimoServices _descontoMinimo = null!;
    private readonly DataGrid _grid = new() { AutoGenerateColumns = true, Margin = new Thickness(0, 16, 0, 16) };
    private readonly TextBox _competencia = new() { TextAlignment = TextAlignment.Center };
    private readonly TextBox _valor = new() { TextAlignment = TextAlignment.Right };
    private int _id;

    public ValueTableWindow(ValueTableKind kind, IServiceProvider services)
    {
        _kind = kind;
        if (kind == ValueTableKind.Simplificado) _simplificado = services.GetRequiredService<ISimplificadoServices>();
        if (kind == ValueTableKind.Dependente) _dependente = services.GetRequiredService<IDependenteServices>();
        if (kind == ValueTableKind.DescontoMinimo) _descontoMinimo = services.GetRequiredService<IDescontoMinimoServices>();
        Title = kind switch { ValueTableKind.Simplificado => "Valor simplificado", ValueTableKind.Dependente => "Dedução por dependente", _ => "Desconto mínimo" };
        Width = 660; Height = 560; MinWidth = 540; MinHeight = 480; ShowInTaskbar = false;
        SetResourceReference(BackgroundProperty, "ApplicationBackgroundBrush");
        SetResourceReference(ForegroundProperty, "TextBrush");
        _competencia.Text = DateTime.Today.ToString("MM/yyyy"); _valor.Text = "0,00";
        _grid.SelectionChanged += SelectionChanged;
        Content = BuildContent();
        Loaded += async (_, _) => await LoadAsync();
    }

    private UIElement BuildContent()
    {
        var root = new Grid { Margin = new Thickness(28) };
        root.SetResourceReference(BackgroundProperty, "ApplicationBackgroundBrush");
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); root.RowDefinitions.Add(new RowDefinition());
        var header = new StackPanel(); header.Children.Add(new TextBlock { Text = Title, FontSize = 24, FontWeight = FontWeights.SemiBold }); header.Children.Add(new TextBlock { Text = "Cadastre os valores por competência.", Opacity = .7, Margin = new Thickness(0, 5, 0, 0) }); root.Children.Add(header);
        var form = new Border { Style = (Style)Application.Current.Resources["Card"], Margin = new Thickness(0, 20, 0, 0) };
        var formGrid = new Grid(); formGrid.ColumnDefinitions.Add(new ColumnDefinition()); formGrid.ColumnDefinitions.Add(new ColumnDefinition()); formGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Auto });
        formGrid.Children.Add(Field("Competência", _competencia, 0)); formGrid.Children.Add(Field("Valor", _valor, 1)); var save = new Button { Content = "Salvar", Style = (Style)Application.Current.Resources["PrimaryButton"], Margin = new Thickness(12, 25, 0, 0) }; save.Click += async (_, _) => await SaveAsync(); Grid.SetColumn(save, 2); formGrid.Children.Add(save); form.Child = formGrid; Grid.SetRow(form, 1); root.Children.Add(form);
        var data = new Grid(); data.RowDefinitions.Add(new RowDefinition()); data.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); data.Children.Add(_grid); var actions = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right }; var remove = new Button { Content = "Excluir selecionado" }; remove.Click += async (_, _) => await DeleteAsync(); actions.Children.Add(remove); Grid.SetRow(actions, 1); data.Children.Add(actions); Grid.SetRow(data, 2); root.Children.Add(data);
        return root;
    }

    private static FrameworkElement Field(string label, Control input, int column)
    {
        var panel = new StackPanel { Margin = new Thickness(column == 0 ? 0 : 0, 0, 12, 0) }; panel.Children.Add(new TextBlock { Text = label, Opacity = .7, Margin = new Thickness(0, 0, 0, 6) }); panel.Children.Add(input); Grid.SetColumn(panel, column); return panel;
    }

    private async Task LoadAsync()
    {
        _grid.ItemsSource = _kind switch
        {
            ValueTableKind.Simplificado => (await _simplificado.ListarTodos()).OrderByDescending(x => x.Competencia).ToList(),
            ValueTableKind.Dependente => (await _dependente.ListarTodos()).OrderByDescending(x => x.Competencia).ToList(),
            _ => (await _descontoMinimo.ListarTodos()).OrderByDescending(x => x.Competencia).ToList()
        };
        _id = 0; _valor.Text = "0,00";
    }

    private async Task SaveAsync()
    {
        if (!TryRead(out var competencia, out var valor)) return;
        try
        {
            switch (_kind)
            {
                case ValueTableKind.Simplificado: var simplificado = new Simplificado { Id = _id, Competencia = competencia, Valor = valor }; if (_id == 0) await _simplificado.Gravar(simplificado); else await _simplificado.Alterar(simplificado); break;
                case ValueTableKind.Dependente: var dependente = new Dependente { Id = _id, Competencia = competencia, Valor = valor }; if (_id == 0) await _dependente.Gravar(dependente); else await _dependente.Alterar(dependente); break;
                default: var desconto = new DescontoMinimo { Id = _id, Competencia = competencia, Valor = valor }; if (_id == 0) await _descontoMinimo.Gravar(desconto); else await _descontoMinimo.Alterar(desconto); break;
            }
            await LoadAsync();
        }
        catch (Exception ex) { ShowError(ex); }
    }

    private async Task DeleteAsync()
    {
        if (_id == 0) return;
        if (MessageBox.Show("Excluir o registro selecionado?", Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
        try { if (_kind == ValueTableKind.Simplificado) await _simplificado.Excluir(_id); else if (_kind == ValueTableKind.Dependente) await _dependente.Excluir(_id); else await _descontoMinimo.Excluir(_id); await LoadAsync(); } catch (Exception ex) { ShowError(ex); }
    }

    private void SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (_grid.SelectedItem)
        {
            case Simplificado item: _id = item.Id; _competencia.Text = item.Competencia.ToString("MM/yyyy"); _valor.Text = item.Valor.ToString("N2", PtBr); break;
            case Dependente item: _id = item.Id; _competencia.Text = item.Competencia.ToString("MM/yyyy"); _valor.Text = item.Valor.ToString("N2", PtBr); break;
            case DescontoMinimo item: _id = item.Id; _competencia.Text = item.Competencia.ToString("MM/yyyy"); _valor.Text = item.Valor.ToString("N2", PtBr); break;
        }
    }

    private bool TryRead(out DateTime competencia, out double valor)
    {
        competencia = default;
        valor = 0;
        var valid = DateTime.TryParseExact(_competencia.Text.Trim(), "MM/yyyy", PtBr, DateTimeStyles.None, out competencia) && double.TryParse(_valor.Text, NumberStyles.Number, PtBr, out valor) && valor >= 0;
        if (!valid) MessageBox.Show("Informe competência no formato MM/AAAA e um valor maior ou igual a zero.", "Dados inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);
        return valid;
    }
    private void ShowError(Exception ex) => MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
}
