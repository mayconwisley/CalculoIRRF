using CalculoIRRF.Model;
using CalculoIRRF.Services;
using CalculoIRRF.Services.Validacao;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
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
    private async Task ListarTabelaDescontoMinimo()
    {
        try
        {
            DecontoMinimoService descontoMinimoServices = new();
            DgvValorDescontoMinimo.DataSource = await descontoMinimoServices.ListarTodos();
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
            DecontoMinimoService descontoMinimoServices = new();
            DescontoMinimo dados = new()
            {
                Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                Valor = decimal.Parse(TxtValor.Text.Trim())
            };
            await descontoMinimoServices.Gravar(dados);

            await ListarTabelaDescontoMinimo();
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
            DecontoMinimoService descontoMinimoServices = new();
            DescontoMinimo dados = new()
            {
                Id = idDescontoMinimo,
                Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                Valor = decimal.Parse(TxtValor.Text.Trim())
            };
            await descontoMinimoServices.Gravar(dados);
            await ListarTabelaDescontoMinimo();
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
            DecontoMinimoService descontoMinimoServices = new();
            await descontoMinimoServices.Excluir(idDescontoMinimo);
            await ListarTabelaDescontoMinimo();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void FrmDescontoMinimo_Load(object sender, EventArgs e)
    {

        MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
        await ListarTabelaDescontoMinimo();
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
