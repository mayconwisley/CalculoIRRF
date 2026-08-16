using CalculoIRRF.Services.Calculo;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Presentation;
using System;
using System.Globalization;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

namespace CalculoIRRF.Views;

[SupportedOSPlatform("windows")]
public partial class MainWindow : Window
{
    private static readonly CultureInfo PtBr = CultureInfo.GetCultureInfo("pt-BR");
    private readonly IServiceProvider _services;
    private readonly IInssServices _inssServices;
    private readonly CalculoImposto _calculoImposto;

    public MainWindow(IServiceProvider services, IInssServices inssServices, CalculoImposto calculoImposto)
    {
        InitializeComponent();
        _services = services;
        _inssServices = inssServices;
        _calculoImposto = calculoImposto;
        CompetenciaTextBox.Text = DateTime.Today.ToString("MM/yyyy");
        ValorBrutoTextBox.Text = BaseInssTextBox.Text = "0,00";
        DependentesTextBox.Text = "0";
        ThemeComboBox.SelectedIndex = (int)ThemeManager.CurrentMode;
    }

    private async void ValorBrutoTextBox_LostKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
    {
        if (!TryReadCompetencia(out var competencia) || !TryReadDecimal(ValorBrutoTextBox.Text, out var valorBruto)) return;
        BaseInssTextBox.Text = Math.Min(valorBruto, await _inssServices.TetoInss(competencia)).ToString("N2", PtBr);
    }

    private void ThemeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (ThemeComboBox.SelectedIndex >= 0)
            ThemeManager.Apply((CalculoIRRF.Presentation.ThemeMode)ThemeComboBox.SelectedIndex);
    }

    private async void Calcular_Click(object sender, RoutedEventArgs e)
    {
        if (!TryReadInputs(out var competencia, out var valorBruto, out var baseInss, out var dependentes)) return;
        try
        {
            ResultadoRichTextBox.Document.Blocks.Clear();
            var resultado = await _calculoImposto.Calcular(competencia, valorBruto, baseInss, dependentes);
            foreach (var item in resultado)
                ResultadoRichTextBox.Document.Blocks.Add(new Paragraph(new Run(item.Texto)) { Foreground = ToBrush(item.Tipo), Margin = new Thickness(0, 0, 0, 8) });

            PensaoButton.IsEnabled = true;
        }
        catch (Exception ex)
        {
            ShowError("Não foi possível concluir o cálculo.", ex);
        }
    }

    private void AbrirTabelaInss_Click(object sender, RoutedEventArgs e) => new TaxTableWindow(TaxTableKind.Inss, _services) { Owner = this }.ShowDialog();
    private void AbrirTabelaIrrf_Click(object sender, RoutedEventArgs e) => new TaxTableWindow(TaxTableKind.Irrf, _services) { Owner = this }.ShowDialog();
    private void AbrirSimplificado_Click(object sender, RoutedEventArgs e) => OpenValueTable(ValueTableKind.Simplificado);
    private void AbrirDependente_Click(object sender, RoutedEventArgs e) => OpenValueTable(ValueTableKind.Dependente);
    private void AbrirDescontoMinimo_Click(object sender, RoutedEventArgs e) => OpenValueTable(ValueTableKind.DescontoMinimo);

    private void OpenValueTable(ValueTableKind kind) => new ValueTableWindow(kind, _services) { Owner = this }.ShowDialog();

    private void AbrirPensao_Click(object sender, RoutedEventArgs e)
    {
        if (!TryReadInputs(out var competencia, out var valorBruto, out var baseInss, out var dependentes)) return;
        new PensionWindow(competencia, baseInss, dependentes, valorBruto, _services) { Owner = this }.ShowDialog();
    }

    private bool TryReadInputs(out DateTime competencia, out double valorBruto, out double baseInss, out int dependentes)
    {
        competencia = default; valorBruto = baseInss = 0; dependentes = 0;
        if (!TryReadCompetencia(out competencia) || !TryReadDecimal(ValorBrutoTextBox.Text, out valorBruto) || !TryReadDecimal(BaseInssTextBox.Text, out baseInss) || !int.TryParse(DependentesTextBox.Text, out dependentes) || valorBruto < 0 || baseInss < 0 || dependentes < 0)
        {
            MessageBox.Show("Informe uma competência válida (MM/AAAA), valores monetários válidos e uma quantidade de dependentes maior ou igual a zero.", "Dados inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);
            return false;
        }
        return true;
    }

    private bool TryReadCompetencia(out DateTime competencia) => DateTime.TryParseExact(CompetenciaTextBox.Text.Trim(), "MM/yyyy", PtBr, DateTimeStyles.None, out competencia);
    private static bool TryReadDecimal(string value, out double result) => double.TryParse(value, NumberStyles.Number, PtBr, out result);
    private static Brush ToBrush(ResultadoCalculoTipo tipo) => tipo switch
    {
        ResultadoCalculoTipo.Normal => Brushes.DodgerBlue,
        ResultadoCalculoTipo.Simplificado => Brushes.IndianRed,
        ResultadoCalculoTipo.Vantagem => Brushes.MediumSeaGreen,
        _ => (Brush)Application.Current.Resources["TextBrush"]
    };
    private static void ShowError(string message, Exception exception) => MessageBox.Show($"{message}\n\n{exception.Message}", "Calculadora IRRF", MessageBoxButton.OK, MessageBoxImage.Error);
}
