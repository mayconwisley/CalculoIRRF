using CalculoIRRF.Model;
using CalculoIRRF.Modelo.Validacao;
using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
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
            Modelo.DescontoMinimo.Cadastro descontoMinimo = new();
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

    private async void BtnGravar_Click(object sender, EventArgs e)
    {
        try
        {
            Modelo.DescontoMinimo.Cadastro descontoMinimo = new();
            DescontoMinimo dados = new()
            {
                Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                Valor = decimal.Parse(TxtValor.Text.Trim())
            };
            await descontoMinimo.Gravar(dados);

            ListarTabelaDescontoMinimo();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void BtnAlterar_Click(object sender, EventArgs e)
    {
        try
        {
            Modelo.DescontoMinimo.Cadastro descontoMinimo = new();
            DescontoMinimo dados = new()
            {
                Id = idDescontoMinimo,
                Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                Valor = decimal.Parse(TxtValor.Text.Trim())
            };
            await descontoMinimo.Gravar(dados);
            ListarTabelaDescontoMinimo();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void BtnExcluir_Click(object sender, EventArgs e)
    {
        try
        {
            Modelo.DescontoMinimo.Cadastro descontoMinimo = new();
            await descontoMinimo.Excluir(idDescontoMinimo);
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
        Validar validar = new();
        TxtValor.Text = validar.ValidarValor(TxtValor.Text);
        TxtValor.Select(TxtValor.Text.Length, 0);
    }

    private void TxtValor_Leave(object sender, EventArgs e)
    {
        Validar validar = new();
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
