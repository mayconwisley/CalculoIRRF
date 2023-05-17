using CalculoIRRF.Modelos.Validacao;
using System;
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

        private void TxtValor_TextChanged(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtValor.Text = validar.ValidarValor(TxtValor.Text);
            TxtValor.Select(TxtValor.Text.Length, 0);
        }

        private void TxtValor_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtValor.Text = validar.Zero(TxtValor.Text);
            TxtValor.Text = validar.Formatar(TxtValor.Text);
        }

        private void TxtValor_Enter(object sender, EventArgs e)
        {
            if (TxtValor.Text == "0,00")
            {
                TxtValor.Text = "";
            }

        }

        private void TxtDeducao_TextChanged(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtDeducao.Text = validar.ValidarValor(TxtDeducao.Text);
            TxtDeducao.Select(TxtDeducao.Text.Length, 0);
        }

        private void TxtDeducao_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtDeducao.Text = validar.Zero(TxtDeducao.Text);
            TxtDeducao.Text = validar.Formatar(TxtDeducao.Text);
        }

        private void TxtDeducao_Enter(object sender, EventArgs e)
        {
            if (TxtDeducao.Text == "0,00")
            {
                TxtDeducao.Text = "";
            }
        }

        private void TxtFaixa_TextChanged(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtFaixa.Text = validar.ValidarNumero(TxtFaixa.Text);
            TxtFaixa.Select(TxtFaixa.Text.Length, 0);
        }

        private void TxtFaixa_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtFaixa.Text = validar.ZeroNumero(TxtFaixa.Text);
            TxtFaixa.Text = validar.FormatarNumero(TxtFaixa.Text);
        }

        private void TxtFaixa_Enter(object sender, EventArgs e)
        {
            if (TxtFaixa.Text == "0")
            {
                TxtFaixa.Text = "";
            }
        }

        private void TxtPorcentagem_TextChanged(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtPorcentagem.Text = validar.ValidarValor(TxtPorcentagem.Text);
            TxtPorcentagem.Select(TxtPorcentagem.Text.Length, 0);
        }

        private void TxtPorcentagem_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtPorcentagem.Text = validar.Zero(TxtPorcentagem.Text);
            TxtPorcentagem.Text = validar.Formatar(TxtPorcentagem.Text);
        }

        private void TxtPorcentagem_Enter(object sender, EventArgs e)
        {
            if (TxtPorcentagem.Text == "0,00")
            {
                TxtPorcentagem.Text = "";
            }
        }
    }
}
