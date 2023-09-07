using CalculoIRRF.Modelo.Validacao;
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
                Modelo.Irrf.Cadastro irrf = new Modelo.Irrf.Cadastro();
                DgvTabelaIRRF.DataSource = irrf.ListarTodos();
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

            Modelo.Irrf.Cadastro irrf = new Modelo.Irrf.Cadastro();

            TxtFaixa.Text = (irrf.UltimaFaixaIrrf(competencia) + 1).ToString();
            TxtValor.Text = "0,00";
            TxtPorcentagem.Text = "0,00";
            TxtDeducao.Text = "0,00";
            TxtValor.Focus();

        }
        private void BtnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                Modelo.Irrf.Cadastro irrf = new Modelo.Irrf.Cadastro();
                Objetos.Irrf dados = new Objetos.Irrf
                {
                    Competencia = DateTime.Parse(MktCompetencia.Text),
                    Faixa = int.Parse(TxtFaixa.Text.Trim()),
                    Valor = decimal.Parse(TxtValor.Text.Trim()),
                    Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim()),
                    Deducao = decimal.Parse(TxtDeducao.Text.Trim())
                };

                irrf.Gravar(dados);

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
                Modelo.Irrf.Cadastro irrf = new Modelo.Irrf.Cadastro();
                Objetos.Irrf dados = new Objetos.Irrf
                {
                    Id = idIrrf,
                    Competencia = DateTime.Parse(MktCompetencia.Text),
                    Faixa = int.Parse(TxtFaixa.Text.Trim()),
                    Valor = decimal.Parse(TxtValor.Text.Trim()),
                    Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim()),
                    Deducao = decimal.Parse(TxtDeducao.Text.Trim())
                };

                irrf.Alterar(dados);
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
                Modelo.Irrf.Cadastro irrf = new Modelo.Irrf.Cadastro();

                irrf.Excluir(idIrrf);
                ListarTabelaIrrf();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmTabelaIRRF_Load(object sender, EventArgs e)
        {
            MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
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

        private void MktCompetencia_Leave(object sender, EventArgs e)
        {
            try
            {
                DateTime competencia = DateTime.Parse(MktCompetencia.Text);
                Modelo.Irrf.Cadastro irrf = new Modelo.Irrf.Cadastro();
                int faixa = irrf.UltimaFaixaIrrf(competencia) + 1;

                TxtFaixa.Text = faixa.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
