using CalculoIRRF.Model;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Services.Validacao;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class FrmDependente : Form
{
    private readonly IDependenteServices _dependenteServices;
    public FrmDependente(IDependenteServices dependenteServices)
    {
        InitializeComponent();
        _dependenteServices = dependenteServices;
    }
    int idDependente = 0;
    private async Task ListarTabelaDependente()
    {
        try
        {
            DgvValorDependente.DataSource = await _dependenteServices.ListarTodos();
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
            Dependente dados = new()
            {
                Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                Valor = double.Parse(TxtValor.Text.Trim())
            };

            await _dependenteServices.Gravar(dados);
            await ListarTabelaDependente();
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
            Dependente dados = new()
            {
                Id = idDependente,
                Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
                Valor = double.Parse(TxtValor.Text.Trim())
            };
            await _dependenteServices.Alterar(dados);
            await ListarTabelaDependente();
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
            await _dependenteServices.Excluir(idDependente);
            await ListarTabelaDependente();
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
            TxtValor.Text = double.Parse(DgvValorDependente.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private async void FrmDependente_Load(object sender, EventArgs e)
    {
        MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
        await ListarTabelaDependente();
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
