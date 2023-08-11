using CalculoIRRF.Modelo.Validacao;
using System;
using System.Windows.Forms;

namespace CalculoIRRF
{
    public partial class FrmDescontoMinimo : Form
    {
        public FrmDescontoMinimo()
        {
            InitializeComponent();
        }
        int idDescontoMinimo = 0;
        private void ListarTabelaDescontoMinimo()
        {
            try
            {
                Objetos.DescontoMinimo descontoMinimo = new Objetos.DescontoMinimo();
                DgvValorDescontoMinimo.DataSource = descontoMinimo.ListarTodos();
                LimparCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void LimparCampos()
        {
            TxtValor.Text = "0,00";
            TxtValor.Focus();
        }

        private void BtnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                Objetos.DescontoMinimo descontoMinimo = new Objetos.DescontoMinimo
                {
                    Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                    Valor = decimal.Parse(TxtValor.Text.Trim())
                };
                descontoMinimo.Gravar();

                ListarTabelaDescontoMinimo();
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
                Objetos.DescontoMinimo descontoMinimo = new Objetos.DescontoMinimo
                {
                    Id = idDescontoMinimo,
                    Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                    Valor = decimal.Parse(TxtValor.Text.Trim())
                };
                descontoMinimo.Gravar();
                ListarTabelaDescontoMinimo();
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
                Objetos.DescontoMinimo descontoMinimo = new Objetos.DescontoMinimo
                {
                    Id = idDescontoMinimo
                };
                descontoMinimo.Excluir();
                ListarTabelaDescontoMinimo();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmDescontoMinimo_Load(object sender, EventArgs e)
        {

            MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
            ListarTabelaDescontoMinimo();
        }

        private void DgvValorDescontoMinimo_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                idDescontoMinimo = int.Parse(DgvValorDescontoMinimo.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                MktCompetencia.Text = DateTime.Parse(DgvValorDescontoMinimo.Rows[e.RowIndex].Cells["Competencia"].Value.ToString()).ToString("MM/yyyy");
                TxtValor.Text = decimal.Parse(DgvValorDescontoMinimo.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");
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
    }
}
