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
    }
}
