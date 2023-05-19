using CalculoIRRF.Modelos.Validacao;
using System;
using System.Windows.Forms;

namespace CalculoIRRF
{
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

        private void BtnCalcular_Click(object sender, EventArgs e)
        {
            DateTime competencia = DateTime.Parse(MktCompetencia.Text.Trim());
            decimal valorBruto = decimal.Parse(TxtValorBruto.Text.Trim());
            decimal baseInss = decimal.Parse(TxtBaseInss.Text.Trim());
            int qtdDependente = int.Parse(TxtQtdDependente.Text.Trim());

            try
            {
                Modelos.Calculo.Inss inss = new Modelos.Calculo.Inss(competencia, baseInss);
                decimal valorInss = inss.NormalProgressivo();

                Modelos.Calculo.Irrf irrf = new Modelos.Calculo.Irrf(competencia, qtdDependente, valorInss, valorBruto);

                string str = irrf.DescricaoCalculoNormal();
                str += "--------------------------------------------------------------------\n";
                str += irrf.DescricaoCalculoSimplificado();
                str += "--------------------------------------------------------------------\n";
                str += irrf.DescricaoVantagem();
                str += "--------------------------------------------------------------------\n";
                str += irrf.DescricaoCalculoNormalProgrssivo();
                str += "--------------------------------------------------------------------\n";
                str += inss.DescricaoCalculoNormalProgressivo();

                RTxtResultado.Text = str;

                irrf.NormalProgressivo();
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
            Validar validar = new Validar();
            TxtValorBruto.Text = validar.ValidarValor(TxtValorBruto.Text);
            TxtValorBruto.Select(TxtValorBruto.Text.Length, 0);
        }

        private void TxtValorBruto_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtValorBruto.Text = validar.Zero(TxtValorBruto.Text);
            TxtValorBruto.Text = validar.Formatar(TxtValorBruto.Text);

            TxtBaseInss.Text = TxtValorBruto.Text;
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
            Validar validar = new Validar();
            TxtBaseInss.Text = validar.ValidarValor(TxtBaseInss.Text);
            TxtBaseInss.Select(TxtBaseInss.Text.Length, 0);
        }

        private void TxtDescInss_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtBaseInss.Text = validar.Zero(TxtBaseInss.Text);
            TxtBaseInss.Text = validar.Formatar(TxtBaseInss.Text);
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
            Validar validar = new Validar();
            TxtQtdDependente.Text = validar.ValidarNumero(TxtQtdDependente.Text);
            TxtQtdDependente.Select(TxtQtdDependente.Text.Length, 0);
        }

        private void TxtQtdDependente_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtQtdDependente.Text = validar.ZeroNumero(TxtQtdDependente.Text);
            TxtQtdDependente.Text = validar.FormatarNumero(TxtQtdDependente.Text);
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
            FrmTabelaINSS tabelaINSS = new FrmTabelaINSS();
            tabelaINSS.ShowDialog();
        }

        private void BtnDescMinimo_Click(object sender, EventArgs e)
        {
            FrmDescontoMinimo descontoMinimo = new FrmDescontoMinimo();
            descontoMinimo.ShowDialog();
        }
    }
}
