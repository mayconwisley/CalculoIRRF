using CalculoIRRF.Modelo.Validacao;
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
                Objetos.Inss inss = new Objetos.Inss();
                DgvTabelaINSS.DataSource = inss.ListarTodos();
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LimparCampos()
        {
            DateTime competencia = DateTime.Parse(MktCompetencia.Text);
            Objetos.Inss inss = new Objetos.Inss(competencia);

            TxtFaixa.Text = (inss.UltimaFaixa + 1).ToString();
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
                Objetos.Inss inss = new Objetos.Inss
                {
                    Competencia = DateTime.Parse(MktCompetencia.Text),
                    Faixa = int.Parse(TxtFaixa.Text.Trim()),
                    Valor = decimal.Parse(TxtValor.Text.Trim()),
                    Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim())
                };
                inss.Gravar();

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
                Objetos.Inss inss = new Objetos.Inss
                {
                    Id = idInss,
                    Competencia = DateTime.Parse(MktCompetencia.Text),
                    Faixa = int.Parse(TxtFaixa.Text.Trim()),
                    Valor = decimal.Parse(TxtValor.Text.Trim()),
                    Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim())
                };
                inss.Alterar();
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
                Objetos.Inss inss = new Objetos.Inss
                {
                    Id = idInss
                };
                inss.Excluir();
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
                DateTime competencia = DateTime.Parse(MktCompetencia.Text);
                Objetos.Inss inss = new Objetos.Inss(competencia);
                int faixa = inss.UltimaFaixa + 1;

                TxtFaixa.Text = faixa.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
