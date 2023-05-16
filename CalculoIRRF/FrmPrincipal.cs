using CalculoIRRF.Modelos.Validacao;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            decimal valorInss = decimal.Parse(TxtDescInss.Text.Trim());
            int qtdDependente = int.Parse(TxtQtdDependente.Text.Trim());

            Modelos.Calculo.Irrf irrf = new Modelos.Calculo.Irrf(competencia, qtdDependente, valorInss, valorBruto);

            string str = irrf.DescricaoCalculoNormal();
            str += "--------------------------------------------------------------------\n";
            str += irrf.DescricaoCalculoSimplificado();
            str += "--------------------------------------------------------------------\n";
            str += irrf.Vantagem();
            str += "--------------------------------------------------------------------\n";
            str += irrf.DescricaoCalculoNormalProgrssivo();

            RTxtResultado.Text = str;

            irrf.NormalProgressivo();
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
            TxtDescInss.Text = validar.ValidarValor(TxtDescInss.Text);
            TxtDescInss.Select(TxtDescInss.Text.Length, 0);
        }

        private void TxtDescInss_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtDescInss.Text = validar.Zero(TxtDescInss.Text);
            TxtDescInss.Text = validar.Formatar(TxtDescInss.Text);
        }

        private void TxtDescInss_Enter(object sender, EventArgs e)
        {
            if (TxtDescInss.Text == "0,00")
            {
                TxtDescInss.Text = "";
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
    }
}
