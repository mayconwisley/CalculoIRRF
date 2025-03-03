using CalculoIRRF.Model;
using CalculoIRRF.Services;
using CalculoIRRF.Services.Validacao;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculoIRRF;
[SupportedOSPlatform("windows")]
public partial class FrmDeducaoSimplificada : Form
{
    public FrmDeducaoSimplificada()
    {
        InitializeComponent();
    }

    int idSimplificado = 0;
    private async Task ListarTabelaSimplificado()
    {
        try
        {
            SimplificadoServices simplificado = new();
            DgvValorSimplificado.DataSource = await simplificado.ListarTodos();
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
            SimplificadoServices simplificado = new();
            Simplificado dados = new()
            {
                Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                Valor = decimal.Parse(TxtValor.Text.Trim())
            };
            await simplificado.Gravar(dados);

            await ListarTabelaSimplificado();
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
            SimplificadoServices simplificado = new();
            Simplificado dados = new()
            {
                Id = idSimplificado,
                Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                Valor = decimal.Parse(TxtValor.Text.Trim())
            };
            await simplificado.Alterar(dados);

            await ListarTabelaSimplificado();
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
            SimplificadoServices simplificado = new();

            await simplificado.Excluir(idSimplificado);

            await ListarTabelaSimplificado();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void FrmDeducaoSimplificada_Load(object sender, EventArgs e)
    {
        MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
        await ListarTabelaSimplificado();
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
        TxtValor.Text = Validar.ValidarValor(TxtValor.Text);
        TxtValor.Select(TxtValor.Text.Length, 0);
    }

    private void TxtValor_Leave(object sender, EventArgs e)
    {
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
}
