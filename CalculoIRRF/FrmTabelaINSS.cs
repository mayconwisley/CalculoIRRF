using CalculoIRRF.Modelos.Validacao;
using System;
using System.Windows.Forms;

namespace CalculoIRRF
{
    public partial class FrmTabelaINSS : Form
    {
        public FrmTabelaINSS()
        {
            InitializeComponent();
        }
        int idInss = 0;

        private void ListarTabelaInss()
        {
            try
            {
                Modelos.Inss.Listar listar = new Modelos.Inss.Listar();
                DgvTabelaINSS.DataSource = listar.TodosItens();
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LimparCampos()
        {
            Modelos.Inss.Listar listar = new Modelos.Inss.Listar();
            DateTime competencia = DateTime.Parse(MktCompetencia.Text);
            TxtFaixa.Text = (listar.UltimaFaixa(competencia) + 1).ToString();
            TxtValor.Text = "0,00";
            TxtPorcentagem.Text = "0,00";
            TxtValor.Focus();
        }

        private void FrmTabelaINSS_Load(object sender, EventArgs e)
        {
            MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
            ListarTabelaInss();
        }

        private void BtnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                Objetos.Inss inss = new Objetos.Inss();
                inss.Competencia = DateTime.Parse(MktCompetencia.Text);
                inss.Faixa = int.Parse(TxtFaixa.Text.Trim());
                inss.Valor = decimal.Parse(TxtValor.Text.Trim());
                inss.Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim());

                Modelos.Inss.Gravar gravar = new Modelos.Inss.Gravar();
                gravar.Item(inss);
                ListarTabelaInss();
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
                Objetos.Inss inss = new Objetos.Inss();
                inss.Id = idInss;
                inss.Competencia = DateTime.Parse(MktCompetencia.Text);
                inss.Faixa = int.Parse(TxtFaixa.Text.Trim());
                inss.Valor = decimal.Parse(TxtValor.Text.Trim());
                inss.Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim());

                Modelos.Inss.Alterar alterar = new Modelos.Inss.Alterar();
                alterar.Item(inss);
                ListarTabelaInss();
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
                Modelos.Inss.Excluir excluir = new Modelos.Inss.Excluir();
                excluir.Item(idInss);
                ListarTabelaInss();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void DgvTabelaINSS_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                idInss = int.Parse(DgvTabelaINSS.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                MktCompetencia.Text = DateTime.Parse(DgvTabelaINSS.Rows[e.RowIndex].Cells["Competencia"].Value.ToString()).ToString("MM/yyyy");
                TxtFaixa.Text = decimal.Parse(DgvTabelaINSS.Rows[e.RowIndex].Cells["Faixa"].Value.ToString()).ToString("0");
                TxtValor.Text = decimal.Parse(DgvTabelaINSS.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");
                TxtPorcentagem.Text = decimal.Parse(DgvTabelaINSS.Rows[e.RowIndex].Cells["Porcentagem"].Value.ToString()).ToString("#,##0.00");
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

        private void MktCompetencia_Leave(object sender, EventArgs e)
        {
            try
            {
                Modelos.Irrf.Listar listar = new Modelos.Irrf.Listar();
                DateTime competencia = DateTime.Parse(MktCompetencia.Text);
                int faixa = listar.UltimaFaixa(competencia) + 1;

                TxtFaixa.Text = faixa.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void MktCompetencia_Leave_1(object sender, EventArgs e)
        {
            try
            {
                Modelos.Inss.Listar listar = new Modelos.Inss.Listar();
                DateTime competencia = DateTime.Parse(MktCompetencia.Text);
                int faixa = listar.UltimaFaixa(competencia) + 1;

                TxtFaixa.Text = faixa.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
