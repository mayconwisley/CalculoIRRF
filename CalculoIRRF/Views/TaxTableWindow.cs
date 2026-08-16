using CalculoIRRF.Model;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Tributacao.INSS;
using CalculoIRRF.Tributacao.IRRF;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace CalculoIRRF.Views;

public enum TaxTableKind { Inss, Irrf }

public sealed class TaxTableWindow : Window
{
    private static readonly CultureInfo PtBr = CultureInfo.GetCultureInfo("pt-BR");
    private readonly TaxTableKind _kind;
    private readonly IInssServices _inss = null!;
    private readonly IIrrfServices _irrf = null!;
    private readonly IDependenteServices _dependente;
    private readonly ISimplificadoServices _simplificado;
    private readonly DataGrid _grid = new() { AutoGenerateColumns = true, Margin = new Thickness(0, 16, 0, 16) };
    private readonly TextBox _competencia = new() { TextAlignment = TextAlignment.Center };
    private readonly TextBox _faixa = new() { TextAlignment = TextAlignment.Right };
    private readonly TextBox _valor = new() { TextAlignment = TextAlignment.Right };
    private readonly TextBox _porcentagem = new() { TextAlignment = TextAlignment.Right };
    private readonly TextBox _deducao = new() { TextAlignment = TextAlignment.Right };
    private int _id;

    public TaxTableWindow(TaxTableKind kind, IServiceProvider services)
    {
        _kind = kind;
        if (kind == TaxTableKind.Inss) _inss = services.GetRequiredService<IInssServices>();
        if (kind == TaxTableKind.Irrf) _irrf = services.GetRequiredService<IIrrfServices>();
        _dependente = services.GetRequiredService<IDependenteServices>();
        _simplificado = services.GetRequiredService<ISimplificadoServices>();
        Title = kind == TaxTableKind.Inss ? "Tabela INSS" : "Tabela IRRF";
        Width = 920; Height = 600; MinWidth = 720; MinHeight = 490; ShowInTaskbar = false;
        SetResourceReference(BackgroundProperty, "ApplicationBackgroundBrush");
        SetResourceReference(ForegroundProperty, "TextBrush");
        _competencia.Text = DateTime.Today.ToString("MM/yyyy"); _faixa.Text = "1"; _valor.Text = _porcentagem.Text = _deducao.Text = "0,00";
        _grid.SelectionChanged += SelectionChanged;
        Content = BuildContent();
        Loaded += async (_, _) => await LoadAsync();
    }

    private UIElement BuildContent()
    {
        var root = new Grid { Margin = new Thickness(28) };
        root.SetResourceReference(BackgroundProperty, "ApplicationBackgroundBrush");
        root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); root.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); root.RowDefinitions.Add(new RowDefinition());
        var header = new StackPanel(); header.Children.Add(new TextBlock { Text = Title, FontSize = 24, FontWeight = FontWeights.SemiBold }); header.Children.Add(new TextBlock { Text = "Manutenção das faixas de tributação por competência.", Opacity = .7, Margin = new Thickness(0, 5, 0, 0) }); root.Children.Add(header);
        var form = new Border { Style = (Style)Application.Current.Resources["Card"], Margin = new Thickness(0, 20, 0, 0) };
        var formGrid = new Grid();
        for (var i = 0; i < (_kind == TaxTableKind.Irrf ? 6 : 5); i++) formGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = i == (_kind == TaxTableKind.Irrf ? 5 : 4) ? GridLength.Auto : new GridLength(1, GridUnitType.Star) });
        formGrid.Children.Add(Field("Competência", _competencia, 0)); formGrid.Children.Add(Field("Faixa", _faixa, 1)); formGrid.Children.Add(Field("Limite da faixa", _valor, 2)); formGrid.Children.Add(Field("Alíquota (%)", _porcentagem, 3));
        if (_kind == TaxTableKind.Irrf) formGrid.Children.Add(Field("Dedução", _deducao, 4));
        var saveColumn = _kind == TaxTableKind.Irrf ? 5 : 4; var save = new Button { Content = "Salvar", Style = (Style)Application.Current.Resources["PrimaryButton"], Margin = new Thickness(12, 25, 0, 0) }; save.Click += async (_, _) => await SaveAsync(); Grid.SetColumn(save, saveColumn); formGrid.Children.Add(save); form.Child = formGrid; Grid.SetRow(form, 1); root.Children.Add(form);
        var data = new Grid(); data.RowDefinitions.Add(new RowDefinition()); data.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto }); data.Children.Add(_grid); var actions = new StackPanel { Orientation = Orientation.Horizontal, HorizontalAlignment = HorizontalAlignment.Right }; var update = new Button { Content = "Atualizar online", Margin = new Thickness(0, 0, 8, 0) }; update.Click += async (_, _) => await UpdateOnlineAsync(); actions.Children.Add(update); var delete = new Button { Content = "Excluir selecionado" }; delete.Click += async (_, _) => await DeleteAsync(); actions.Children.Add(delete); Grid.SetRow(actions, 1); data.Children.Add(actions); Grid.SetRow(data, 2); root.Children.Add(data);
        return root;
    }

    private static FrameworkElement Field(string label, Control input, int column)
    {
        var panel = new StackPanel { Margin = new Thickness(0, 0, 12, 0) }; panel.Children.Add(new TextBlock { Text = label, Opacity = .7, Margin = new Thickness(0, 0, 0, 6) }); panel.Children.Add(input); Grid.SetColumn(panel, column); return panel;
    }

    private async Task LoadAsync()
    {
        _grid.ItemsSource = _kind == TaxTableKind.Inss
            ? (await _inss.ListarTodos()).OrderByDescending(x => x.Competencia).ThenBy(x => x.Faixa).ToList()
            : (await _irrf.ListarTodos()).OrderByDescending(x => x.Competencia).ThenBy(x => x.Faixa).ToList();
        _id = 0; _valor.Text = _porcentagem.Text = _deducao.Text = "0,00";
        if (_kind == TaxTableKind.Inss && DateTime.TryParseExact(_competencia.Text, "MM/yyyy", PtBr, DateTimeStyles.None, out var competencia)) _faixa.Text = ((await _inss.UltimaFaixaInss(competencia)) + 1).ToString();
    }

    private async Task SaveAsync()
    {
        if (!TryRead(out var competencia, out var faixa, out var valor, out var porcentagem, out var deducao)) return;
        try
        {
            if (_kind == TaxTableKind.Inss)
            {
                var item = new Inss { Id = _id, Competencia = competencia, Faixa = faixa, Valor = valor, Porcentagem = porcentagem };
                if (_id == 0) await _inss.Gravar(item); else await _inss.Alterar(item);
            }
            else
            {
                var item = new Irrf { Id = _id, Competencia = competencia, Faixa = faixa, Valor = valor, Porcentagem = porcentagem, Deducao = deducao };
                if (_id == 0) await _irrf.Gravar(item); else await _irrf.Alterar(item);
            }
            await LoadAsync();
        }
        catch (Exception ex) { ShowError(ex); }
    }

    private async Task DeleteAsync()
    {
        if (_id == 0) return;
        if (MessageBox.Show("Excluir o registro selecionado?", Title, MessageBoxButton.YesNo, MessageBoxImage.Warning) != MessageBoxResult.Yes) return;
        try { if (_kind == TaxTableKind.Inss) await _inss.Excluir(_id); else await _irrf.Excluir(_id); await LoadAsync(); } catch (Exception ex) { ShowError(ex); }
    }

    private async Task UpdateOnlineAsync()
    {
        try
        {
            if (_kind == TaxTableKind.Inss)
            {
                var tabela = await new TributacaoINSS(_inss).AtualizarOnline();
                if (tabela is null) { ShowInfo("A tabela online já está cadastrada."); return; }
                foreach (var item in tabela) await _inss.Gravar(new Inss { Competencia = item.Vigencia, Faixa = item.Sequencia, Valor = item.BaseCaculo, Porcentagem = item.Aliquota });
            }
            else
            {
                var tabela = await new TributacaoRFB(_irrf).AtualizarOnline();
                if (tabela is null) { ShowInfo("A tabela online já está cadastrada."); return; }
                var referencia = tabela.First();
                await _dependente.Gravar(new Dependente { Competencia = referencia.Vigencia, Valor = referencia.Dependente });
                await _simplificado.Gravar(new Simplificado { Competencia = referencia.Vigencia, Valor = referencia.Simplificado });
                foreach (var item in tabela) await _irrf.Gravar(new Irrf { Competencia = item.Vigencia, Faixa = item.Sequencia, Valor = item.BaseCaculo, Porcentagem = item.Aliquota, Deducao = item.Deducao });
            }
            await LoadAsync();
            ShowInfo("Tabela atualizada com sucesso.");
        }
        catch (Exception ex) { ShowError(ex); }
    }

    private void SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        switch (_grid.SelectedItem)
        {
            case Inss item: _id = item.Id; SetFields(item.Competencia, item.Faixa, item.Valor, item.Porcentagem, 0); break;
            case Irrf item: _id = item.Id; SetFields(item.Competencia, item.Faixa, item.Valor, item.Porcentagem, item.Deducao); break;
        }
    }

    private void SetFields(DateTime competencia, int faixa, double valor, double porcentagem, double deducao)
    {
        _competencia.Text = competencia.ToString("MM/yyyy"); _faixa.Text = faixa.ToString(); _valor.Text = valor.ToString("N2", PtBr); _porcentagem.Text = porcentagem.ToString("N2", PtBr); _deducao.Text = deducao.ToString("N2", PtBr);
    }

    private bool TryRead(out DateTime competencia, out int faixa, out double valor, out double porcentagem, out double deducao)
    {
        competencia = default;
        faixa = 0;
        valor = porcentagem = deducao = 0;
        var valid = DateTime.TryParseExact(_competencia.Text.Trim(), "MM/yyyy", PtBr, DateTimeStyles.None, out competencia)
                    && int.TryParse(_faixa.Text, out faixa) && faixa > 0
                    && double.TryParse(_valor.Text, NumberStyles.Number, PtBr, out valor) && valor >= 0
                    && double.TryParse(_porcentagem.Text, NumberStyles.Number, PtBr, out porcentagem) && porcentagem >= 0
                    && (_kind == TaxTableKind.Inss || double.TryParse(_deducao.Text, NumberStyles.Number, PtBr, out deducao)) && deducao >= 0;
        if (!valid) MessageBox.Show("Preencha os campos com valores válidos. A faixa deve ser maior que zero.", "Dados inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);
        return valid;
    }

    private void ShowError(Exception ex) => MessageBox.Show(ex.Message, Title, MessageBoxButton.OK, MessageBoxImage.Error);
    private void ShowInfo(string message) => MessageBox.Show(message, Title, MessageBoxButton.OK, MessageBoxImage.Information);
}
