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
            Modelos.Calculo.Irrf irrf = new Modelos.Calculo.Irrf(DateTime.Parse(DateTime.Now.Date.ToString("MM/yyyy")), 1, 526.17m, 5000m);
            irrf.Normal();

            irrf.Simplificado();



            Modelos.Calculo.Irrf irrf1 = new Modelos.Calculo.Irrf(DateTime.Parse(DateTime.Now.Date.ToString("MM/yyyy")), 1, 220.12m, 2640m);
            irrf1.Normal();

            irrf1.Simplificado();
        }
    }
}
