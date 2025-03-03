using CalculoIRRF.Services;
using CalculoIRRF.Services.Calculo;
using CalculoIRRF.Services.Validacao;
using System;
using System.Drawing;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class FrmPrincipal : Form
{
    public FrmPrincipal()
    {
        InitializeComponent();
    }

    private void BtnTabelaIRRF_Click(object sender, EventArgs e)
    {
        FrmTabelaIRRF tabelaIRRF = new FrmTabelaIRRF();
        tabelaIRRF.ShowDialog();
    }

    private void BtnTabelaValSimplificado_Click(object sender, EventArgs e)
    {
        FrmDeducaoSimplificada deducaoSimplificada = new FrmDeducaoSimplificada();
        deducaoSimplificada.ShowDialog();
    }

    private async void BtnCalcular_Click(object sender, EventArgs e)
    {

        DateTime competencia = DateTime.Parse(MktCompetencia.Text.Trim());
        decimal valorBruto = decimal.Parse(TxtValorBruto.Text.Trim());
        decimal baseInss = decimal.Parse(TxtBaseInss.Text.Trim());
        int qtdDependente = int.Parse(TxtQtdDependente.Text.Trim());
        RTxtResultado.Clear();

        try
        {
            InssCalculo inssCalculo = new(competencia, baseInss);
            decimal valorInss = await inssCalculo.NormalProgressivo();

            IrrfCalculo irrfCalculo = new(competencia, qtdDependente, valorInss, valorBruto);
            FgtsCalculo fgtsCalculo = new(baseInss);

            Color colorIrNormal = Color.Blue;
            RTxtResultado.SelectionColor = colorIrNormal;

            string str = await irrfCalculo.DescricaoCalculoNormal();
            str += "--------------------------------------------------------------------\n";
            RTxtResultado.SelectedText = str;

            Color colorIrSimplificado = Color.Red;
            RTxtResultado.SelectionColor = colorIrSimplificado;

            str = await irrfCalculo.DescricaoCalculoSimplificado();
            str += "--------------------------------------------------------------------\n";
            RTxtResultado.SelectedText = str;

            Color colorVantagem = Color.Green;
            RTxtResultado.SelectionColor = colorVantagem;

            str = await irrfCalculo.DescricaoVantagem();
            str += "--------------------------------------------------------------------\n";
            RTxtResultado.SelectedText = str;

            str = await irrfCalculo.DescricaoCalculoNormalProgrssivo();
            str += "--------------------------------------------------------------------\n";
            str += inssCalculo.DescricaoCalculoNormalProgressivo();

            str += "--------------------------------------------------------------------\n";
            str += "FGTS 8% " + fgtsCalculo.Normal8().ToString("#,##0.00") + "\n";
            str += "FGTS 2% " + fgtsCalculo.Normal2().ToString("#,##0.00");


            RTxtResultado.SelectedText = str;
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void BtnDependente_Click(object sender, EventArgs e)
    {
        FrmDependente dependente = new FrmDependente();
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
        InssServices cadastroInss = new();

        decimal tetoInss = 0;
        decimal valorBruto = decimal.Parse(TxtValorBruto.Text);

        _ = new DateTime();

        if (DateTime.TryParse(MktCompetencia.Text, out DateTime competencia))
        {
            tetoInss = await cadastroInss.TetoInss(competencia);
        }

        if (valorBruto > tetoInss)
        {
            TxtBaseInss.Text = tetoInss.ToString();
        }
        else
        {
            TxtBaseInss.Text = TxtValorBruto.Text;
        }
    }

    private void TxtValorBruto_Enter(object sender, EventArgs e)
    {
        if (TxtValorBruto.Text == "0,00")
        {
            TxtValorBruto.Text = "";
        }
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
    }

    private void FrmPrincipal_Load(object sender, EventArgs e)
    {
        MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
    }

    private void BtnTabelaINSS_Click(object sender, EventArgs e)
    {
        FrmTabelaINSS tabelaINSS = new();
        tabelaINSS.ShowDialog();
    }

    private void BtnDescMinimo_Click(object sender, EventArgs e)
    {
        FrmDescontoMinimo descontoMinimo = new();
        descontoMinimo.ShowDialog();
    }

    private void BtnPensao_Click(object sender, EventArgs e)
    {
        DateTime competencia = DateTime.Parse(MktCompetencia.Text.Trim());
        decimal baseInss = decimal.Parse(TxtBaseInss.Text.Trim());
        decimal valorBruto = decimal.Parse(TxtValorBruto.Text.Trim());
        int qtdDependente = int.Parse(TxtQtdDependente.Text.Trim());
        FrmPensao pensao = new(competencia, baseInss, qtdDependente, valorBruto);
        pensao.ShowDialog();
    }
}
