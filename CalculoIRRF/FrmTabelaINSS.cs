using CalculoIRRF.Services;
using CalculoIRRF.Services.Validacao;
using CalculoIRRF.Tributacao.INSS;
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
                InssServices inss = new Modelo.Inss.Cadastro();
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
            InssServices inss = new Modelo.Inss.Cadastro();

            TxtFaixa.Text = (inss.UltimaFaixaInss(competencia) + 1).ToString();
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
                InssServices inss = new Modelo.Inss.Cadastro();
                Objetos.Inss dados = new Objetos.Inss
                {
                    Competencia = DateTime.Parse(MktCompetencia.Text),
                    Faixa = int.Parse(TxtFaixa.Text.Trim()),
                    Valor = decimal.Parse(TxtValor.Text.Trim()),
                    Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim())
                };
                inss.Gravar(dados);

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
                InssServices inss = new Modelo.Inss.Cadastro();
                Objetos.Inss dados = new Objetos.Inss
                {
                    Id = idInss,
                    Competencia = DateTime.Parse(MktCompetencia.Text),
                    Faixa = int.Parse(TxtFaixa.Text.Trim()),
                    Valor = decimal.Parse(TxtValor.Text.Trim()),
                    Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim())
                };
                inss.Alterar(dados);
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
                InssServices inss = new Modelo.Inss.Cadastro();

                inss.Excluir(idInss);
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
            TxtValor.Text = Validar.ValidarValor(TxtValor.Text);
            TxtValor.Select(TxtValor.Text.Length, 0);
        }

        private void TxtValor_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtValor.Text = Validar.Zero(TxtValor.Text);
            TxtValor.Text = Validar.Formatar(TxtValor.Text);
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
            TxtFaixa.Text = Validar.ValidarNumero(TxtFaixa.Text);
            TxtFaixa.Select(TxtFaixa.Text.Length, 0);
        }

        private void TxtFaixa_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtFaixa.Text = validar.ZeroNumero(TxtFaixa.Text);
            TxtFaixa.Text = Validar.FormatarNumero(TxtFaixa.Text);
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
            TxtPorcentagem.Text = Validar.ValidarValor(TxtPorcentagem.Text);
            TxtPorcentagem.Select(TxtPorcentagem.Text.Length, 0);
        }

        private void TxtPorcentagem_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtPorcentagem.Text = Validar.Zero(TxtPorcentagem.Text);
            TxtPorcentagem.Text = Validar.Formatar(TxtPorcentagem.Text);
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
                InssServices inss = new Modelo.Inss.Cadastro();
                int faixa = inss.UltimaFaixaInss(competencia) + 1;

                TxtFaixa.Text = faixa.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void LkLblOnline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            TributacaoINSS tributacaoINSS = new TributacaoINSS();
            await tributacaoINSS.AtualizarOnline();
        }
    }
}
