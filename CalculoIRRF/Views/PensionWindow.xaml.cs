using CalculoIRRF.Services.Calculo;
using CalculoIRRF.Services.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;

namespace CalculoIRRF.Views;

public partial class PensionWindow : Window
{
    private static readonly CultureInfo PtBr = CultureInfo.GetCultureInfo("pt-BR");
    private readonly DateTime _competencia;
    private readonly double _baseInss;
    private readonly int _dependentes;
    private readonly IInssServices _inssServices;
    private readonly IIrrfServices _irrfServices;
    private readonly ISimplificadoServices _simplificadoServices;
    private readonly IDependenteServices _dependenteServices;
    private readonly IDescontoMinimoServices _descontoMinimoServices;

    public PensionWindow(DateTime competencia, double baseInss, int dependentes, double valorBruto, IServiceProvider services)
    {
        InitializeComponent();
        _competencia = competencia;
        _baseInss = baseInss;
        _dependentes = dependentes;
        _inssServices = services.GetRequiredService<IInssServices>();
        _irrfServices = services.GetRequiredService<IIrrfServices>();
        _simplificadoServices = services.GetRequiredService<ISimplificadoServices>();
        _dependenteServices = services.GetRequiredService<IDependenteServices>();
        _descontoMinimoServices = services.GetRequiredService<IDescontoMinimoServices>();
        ValorBrutoTextBox.Text = valorBruto.ToString("N2", PtBr);
        PorcentagemTextBox.Text = OutrosDescontosTextBox.Text = "0,00";
    }

    private async void CalcularResumo_Click(object sender, RoutedEventArgs e) => await CalcularAsync(false);
    private async void CalcularDetalhe_Click(object sender, RoutedEventArgs e) => await CalcularAsync(true);

    private async Task CalcularAsync(bool detalhar)
    {
        if (!TryReadValues(out var valorBruto, out var percentual, out var outrosDescontos)) return;
        try
        {
            DescricaoRichTextBox.Document.Blocks.Clear();
            var inss = new InssCalculo(_competencia, _baseInss, _inssServices);
            var valorInss = await inss.NormalProgressivo();
            var pensao = new CalculoPensao(_competencia, _dependentes, valorInss, valorBruto - outrosDescontos, percentual, _irrfServices, _simplificadoServices, _dependenteServices, _descontoMinimoServices);
            await pensao.CalculoJudicialIrrfNormal(detalhar);
            await pensao.CalculoJudicialIrrfSimplificado(detalhar);
            pensao.Vantagem();
            DescricaoRichTextBox.Document.Blocks.Add(new Paragraph(new Run($"Cálculo de pensão alimentícia — rendimentos de {percentual:N2}%\n\n{string.Concat(pensao.DadosCalculoPensao)}")));
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Não foi possível calcular a pensão.\n\n{ex.Message}", "Calculadora IRRF", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool TryReadValues(out double valorBruto, out double percentual, out double outrosDescontos)
    {
        valorBruto = percentual = outrosDescontos = 0;
        var valid = double.TryParse(ValorBrutoTextBox.Text, NumberStyles.Number, PtBr, out valorBruto)
                    && double.TryParse(PorcentagemTextBox.Text, NumberStyles.Number, PtBr, out percentual)
                    && double.TryParse(OutrosDescontosTextBox.Text, NumberStyles.Number, PtBr, out outrosDescontos)
                    && valorBruto >= 0 && percentual >= 0 && percentual <= 100 && outrosDescontos >= 0 && outrosDescontos <= valorBruto;
        if (!valid) MessageBox.Show("Informe valores válidos. O percentual deve estar entre 0 e 100 e os descontos não podem exceder os rendimentos.", "Dados inválidos", MessageBoxButton.OK, MessageBoxImage.Warning);
        return valid;
    }
}
