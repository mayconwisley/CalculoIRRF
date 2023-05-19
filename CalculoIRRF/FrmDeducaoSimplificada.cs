using CalculoIRRF.Modelos.Validacao;
using System;
using System.Windows.Forms;

namespace CalculoIRRF
{
    public partial class FrmDeducaoSimplificada : Form
    {
        public FrmDeducaoSimplificada()
        {
            InitializeComponent();
        }

        int idSimplificado = 0;
        private void ListarTabelaSimplificado()
        {
            try
            {
                Modelos.Simplificado.Listar listar = new Modelos.Simplificado.Listar();
                DgvValorSimplificado.DataSource = listar.TodosItens();
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
                Objetos.Simplificado simplificado = new Objetos.Simplificado();
                simplificado.Competencia = DateTime.Parse(MktCompetencia.Text.Trim());
                simplificado.Valor = decimal.Parse(TxtValor.Text.Trim());
                Modelos.Simplificado.Gravar gravar = new Modelos.Simplificado.Gravar();
                gravar.Item(simplificado);
                ListarTabelaSimplificado();
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
                Objetos.Simplificado simplificado = new Objetos.Simplificado();
                simplificado.Id = idSimplificado;
                simplificado.Competencia = DateTime.Parse(MktCompetencia.Text.Trim());
                simplificado.Valor = decimal.Parse(TxtValor.Text.Trim());
                Modelos.Simplificado.Alterar alterar = new Modelos.Simplificado.Alterar();
                alterar.Item(simplificado);
                ListarTabelaSimplificado();
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
                Modelos.Simplificado.Excluir excluir = new Modelos.Simplificado.Excluir();
                excluir.Item(idSimplificado);
                ListarTabelaSimplificado();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void FrmDeducaoSimplificada_Load(object sender, EventArgs e)
        {
            MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
            ListarTabelaSimplificado();
        }

        private void DgvValorSimplificado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                idSimplificado = int.Parse(DgvValorSimplificado.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                MktCompetencia.Text = DateTime.Parse(DgvValorSimplificado.Rows[e.RowIndex].Cells["Competencia"].Value.ToString()).ToString("MM/yyyy");
                TxtValor.Text = decimal.Parse(DgvValorSimplificado.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");
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
