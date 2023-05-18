using CalculoIRRF.Modelos.Validacao;
using System;
using System.Windows.Forms;

namespace CalculoIRRF
{
    public partial class FrmDependente : Form
    {
        public FrmDependente()
        {
            InitializeComponent();
        }
        int idDependente = 0;
        private void ListarTabelaDependente()
        {
            try
            {
                Modelos.Dependente.Listar listar = new Modelos.Dependente.Listar();
                DgvValorDependente.DataSource = listar.TodosItens();
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
        }
        private void BtnGravar_Click(object sender, EventArgs e)
        {
            try
            {
                Objetos.Dependente dependente = new Objetos.Dependente();
                dependente.Competencia = DateTime.Parse(MktCompetencia.Text.Trim());
                dependente.Valor = decimal.Parse(TxtValor.Text.Trim());
                Modelos.Dependente.Gravar gravar = new Modelos.Dependente.Gravar();
                gravar.Item(dependente);
                ListarTabelaDependente();
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
                Objetos.Dependente dependente = new Objetos.Dependente();
                dependente.Id = idDependente;
                dependente.Competencia = DateTime.Parse(MktCompetencia.Text.Trim());
                dependente.Valor = decimal.Parse(TxtValor.Text.Trim());
                Modelos.Dependente.Alterar alterar = new Modelos.Dependente.Alterar();
                alterar.Item(dependente);
                ListarTabelaDependente();
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
                Modelos.Dependente.Excluir excluir = new Modelos.Dependente.Excluir();
                excluir.Item(idDependente);
                ListarTabelaDependente();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DgvValorSimplificado_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                idDependente = int.Parse(DgvValorDependente.Rows[e.RowIndex].Cells["Id"].Value.ToString());
                MktCompetencia.Text = DateTime.Parse(DgvValorDependente.Rows[e.RowIndex].Cells["Competencia"].Value.ToString()).ToString("MM/yyyy");
                TxtValor.Text = decimal.Parse(DgvValorDependente.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void FrmDependente_Load(object sender, EventArgs e)
        {
            MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
            ListarTabelaDependente();
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
