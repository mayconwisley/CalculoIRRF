using CalculoIRRF.Services.Calculo;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Services.Validacao;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class FrmPrincipal : Form
{
    readonly IServiceProvider _serviceProvider;
    readonly IInssServices _inssServices;
    readonly ISimplificadoServices _simplificadoServices;
    readonly IDescontoMinimoServices _descontoMinimoServices;
    readonly IIrrfServices _irrfServices;
    readonly IDependenteServices _dependenteServices;

    public FrmPrincipal(IServiceProvider serviceProvider)
    {
        InitializeComponent();
        _serviceProvider = serviceProvider;
        _inssServices = _serviceProvider.GetRequiredService<IInssServices>();
        _irrfServices = _serviceProvider.GetRequiredService<IIrrfServices>();
        _simplificadoServices = _serviceProvider.GetRequiredService<ISimplificadoServices>();
        _descontoMinimoServices = _serviceProvider.GetRequiredService<IDescontoMinimoServices>();
        _dependenteServices = _serviceProvider.GetRequiredService<IDependenteServices>();
    }

    private void BtnTabelaIRRF_Click(object sender, EventArgs e)
    {
        FrmTabelaIRRF tabelaIRRF = new(_irrfServices);
        tabelaIRRF.ShowDialog();
    }

    private void BtnTabelaValSimplificado_Click(object sender, EventArgs e)
    {
        FrmDeducaoSimplificada deducaoSimplificada = new(_simplificadoServices);
        deducaoSimplificada.ShowDialog();
    }

    private async void BtnCalcular_Click(object sender, EventArgs e)
    {
        RTxtResultado.Clear();

        if (!DateTime.TryParse(MktCompetencia.Text.Trim(), out DateTime competencia) ||
         !double.TryParse(TxtValorBruto.Text.Trim(), out double valorBruto) ||
         !double.TryParse(TxtBaseInss.Text.Trim(), out double baseInss) ||
         !int.TryParse(TxtQtdDependente.Text.Trim(), out int qtdDependente))
        {
            MessageBox.Show("Por favor, insira valores válidos.");
            return;
        }

        try
        {
            var calculoImposto = _serviceProvider.GetRequiredService<CalculoImposto>();
            var resultado = await calculoImposto.Calcular(competencia, valorBruto, baseInss, qtdDependente);

            foreach (var (color, text) in resultado)
            {
                RTxtResultado.SelectionColor = color;
                RTxtResultado.AppendText(text);
            }

            BtnPensao.Enabled = true;
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao calcular: {ex.Message}");
        }
    }

    private void BtnDependente_Click(object sender, EventArgs e)
    {
        FrmDependente dependente = new(_dependenteServices);
        dependente.ShowDialog();
    }

    private void TxtValorBruto_TextChanged(object sender, EventArgs e)
    {
        TxtValorBruto.Text = Validar.ValidarValor(TxtValorBruto.Text);
        TxtValorBruto.Select(TxtValorBruto.Text.Length, 0);
    }

    private async void TxtValorBruto_Leave(object sender, EventArgs e)
    {
        TxtValorBruto.Text = Validar.Zero(TxtValorBruto.Text);
        TxtValorBruto.Text = Validar.Formatar(TxtValorBruto.Text);

        if (!DateTime.TryParse(MktCompetencia.Text, out DateTime competencia) ||
            !double.TryParse(TxtValorBruto.Text, out double valorBruto))
        {
            return;
        }

        double tetoInss = await _inssServices.TetoInss(competencia);
        TxtBaseInss.Text = Math.Min(valorBruto, tetoInss).ToString("#,##0.00");
    }

    private void TxtValorBruto_Enter(object sender, EventArgs e)
    {
        if (TxtValorBruto.Text == "0,00")
        {
            TxtValorBruto.Text = "";
        }
        BtnPensao.Enabled = false;
    }

    private void TxtDescInss_TextChanged(object sender, EventArgs e)
    {
        TxtBaseInss.Text = Validar.ValidarValor(TxtBaseInss.Text);
        TxtBaseInss.Select(TxtBaseInss.Text.Length, 0);
    }

    private void TxtDescInss_Leave(object sender, EventArgs e)
    {
        TxtBaseInss.Text = Validar.Zero(TxtBaseInss.Text);
        TxtBaseInss.Text = Validar.Formatar(TxtBaseInss.Text);
    }

    private void TxtDescInss_Enter(object sender, EventArgs e)
    {
        if (TxtBaseInss.Text == "0,00")
        {
            TxtBaseInss.Text = "";
        }
        BtnPensao.Enabled = false;
    }

    private void TxtQtdDependente_TextChanged(object sender, EventArgs e)
    {
        TxtQtdDependente.Text = Validar.ValidarNumero(TxtQtdDependente.Text);
        TxtQtdDependente.Select(TxtQtdDependente.Text.Length, 0);
    }

    private void TxtQtdDependente_Leave(object sender, EventArgs e)
    {
        TxtQtdDependente.Text = Validar.ZeroNumero(TxtQtdDependente.Text);
        TxtQtdDependente.Text = Validar.FormatarNumero(TxtQtdDependente.Text);
    }

    private void TxtQtdDependente_Enter(object sender, EventArgs e)
    {
        if (TxtQtdDependente.Text == "0")
        {
            TxtQtdDependente.Text = "";
        }
        BtnPensao.Enabled = false;
    }

    private void FrmPrincipal_Load(object sender, EventArgs e)
    {
        MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
    }

    private void BtnTabelaINSS_Click(object sender, EventArgs e)
    {
        FrmTabelaINSS tabelaINSS = new(_inssServices);
        tabelaINSS.ShowDialog();
    }

    private void BtnDescMinimo_Click(object sender, EventArgs e)
    {
        FrmDescontoMinimo descontoMinimo = new(_descontoMinimoServices);
        descontoMinimo.ShowDialog();
    }

    private void BtnPensao_Click(object sender, EventArgs e)
    {
        DateTime competencia = DateTime.Parse(MktCompetencia.Text.Trim());
        double baseInss = double.Parse(TxtBaseInss.Text.Trim());
        double valorBruto = double.Parse(TxtValorBruto.Text.Trim());
        int qtdDependente = int.Parse(TxtQtdDependente.Text.Trim());
        FrmPensao pensao = new(competencia, baseInss, qtdDependente, valorBruto,
                               _inssServices, _irrfServices, _simplificadoServices,
                               _dependenteServices, _descontoMinimoServices);
        pensao.ShowDialog();
    }
}
