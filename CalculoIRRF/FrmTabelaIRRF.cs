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
    public partial class FrmTabelaIRRF : Form
    {
        public FrmTabelaIRRF()
        {
            InitializeComponent();
        }
        int idIrrf = 0;

        private void ListarTabelaIrrf()
        {
            try
            {
                Modelos.Irrf.Listar listar = new Modelos.Irrf.Listar();
                DgvTabelaIRRF.DataSource = listar.TodosItens();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                Objetos.Irrf irrf = new Objetos.Irrf();
                irrf.Competencia = DateTime.Parse(MktCompetencia.Text);
                irrf.Faixa = int.Parse(TxtFaixa.Text.Trim());
                irrf.Valor = decimal.Parse(TxtValor.Text.Trim());
                irrf.Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim());
                irrf.Deducao = decimal.Parse(TxtDeducao.Text.Trim());

                Modelos.Irrf.Gravar gravar = new Modelos.Irrf.Gravar();
                gravar.Item(irrf);
                ListarTabelaIrrf();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnAlterar_Click(object sender, EventArgs e)
        {
            try
            {
                Objetos.Irrf irrf = new Objetos.Irrf();
                irrf.Id = idIrrf;
                irrf.Competencia = DateTime.Parse(MktCompetencia.Text);
                irrf.Faixa = int.Parse(TxtFaixa.Text.Trim());
                irrf.Valor = decimal.Parse(TxtValor.Text.Trim());
                irrf.Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim());
                irrf.Deducao = decimal.Parse(TxtDeducao.Text.Trim());

                Modelos.Irrf.Alterar alterar = new Modelos.Irrf.Alterar();
                alterar.Item(irrf);
                ListarTabelaIrrf();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void BtnExcluir_Click(object sender, EventArgs e)
        {
            try
            {
                Modelos.Irrf.Excluir excluir = new Modelos.Irrf.Excluir();
                excluir.Item(idIrrf);
                ListarTabelaIrrf();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTabelaIRRF_Load(object sender, EventArgs e)
        {
            ListarTabelaIrrf();
        }

        private void DgvTabelaIRRF_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                idIrrf = int.Parse(DgvTabelaIRRF.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                MktCompetencia.Text = DateTime.Parse(DgvTabelaIRRF.Rows[e.RowIndex].Cells["Competencia"].Value.ToString()).ToString("MM/yyyy");
                TxtFaixa.Text = decimal.Parse(DgvTabelaIRRF.Rows[e.RowIndex].Cells["Faixa"].Value.ToString()).ToString("0");
                TxtValor.Text = decimal.Parse(DgvTabelaIRRF.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");
                TxtPorcentagem.Text = decimal.Parse(DgvTabelaIRRF.Rows[e.RowIndex].Cells["Porcentagem"].Value.ToString()).ToString("#,##0.00");
                TxtDeducao.Text = decimal.Parse(DgvTabelaIRRF.Rows[e.RowIndex].Cells["Deducao"].Value.ToString()).ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
