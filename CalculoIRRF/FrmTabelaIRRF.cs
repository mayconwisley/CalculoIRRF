using CalculoIRRF.Model;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Services.Validacao;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class FrmTabelaIRRF : Form
{
    private readonly IIrrfServices _irrfServices;
    public FrmTabelaIRRF(IIrrfServices irrfServices)
    {
        InitializeComponent();
        _irrfServices = irrfServices;
    }
    int idIrrf = 0;

    private async Task ListarTabelaIrrf()
    {
        try
        {
            DgvTabelaIRRF.DataSource = await _irrfServices.ListarTodos();
            await LimparCampos();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }
    private async Task LimparCampos()
    {
        DateTime competencia = DateTime.Parse(MktCompetencia.Text);

        TxtFaixa.Text = (await _irrfServices.UltimaFaixaIrrf(competencia) + 1).ToString();
        TxtValor.Text = "0,00";
        TxtPorcentagem.Text = "0,00";
        TxtDeducao.Text = "0,00";
        TxtValor.Focus();

    }
    private async void BtnGravar_Click(object sender, EventArgs e)
    {
        try
        {
            Irrf dados = new()
            {
                Competencia = DateTime.Parse(MktCompetencia.Text),
                Faixa = int.Parse(TxtFaixa.Text.Trim()),
                Valor = decimal.Parse(TxtValor.Text.Trim()),
                Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim()),
                Deducao = decimal.Parse(TxtDeducao.Text.Trim())
            };

            await _irrfServices.Gravar(dados);

            await ListarTabelaIrrf();
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
            Irrf dados = new()
            {
                Id = idIrrf,
                Competencia = DateTime.Parse(MktCompetencia.Text),
                Faixa = int.Parse(TxtFaixa.Text.Trim()),
                Valor = decimal.Parse(TxtValor.Text.Trim()),
                Porcentagem = decimal.Parse(TxtPorcentagem.Text.Trim()),
                Deducao = decimal.Parse(TxtDeducao.Text.Trim())
            };

            await _irrfServices.Alterar(dados);
            await ListarTabelaIrrf();
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
            await _irrfServices.Excluir(idIrrf);
            await ListarTabelaIrrf();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void FrmTabelaIRRF_Load(object sender, EventArgs e)
    {
        MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
        await ListarTabelaIrrf();
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

    private void TxtDeducao_TextChanged(object sender, EventArgs e)
    {
        TxtDeducao.Text = Validar.ValidarValor(TxtDeducao.Text);
        TxtDeducao.Select(TxtDeducao.Text.Length, 0);
    }

    private void TxtDeducao_Leave(object sender, EventArgs e)
    {
        TxtDeducao.Text = Validar.Zero(TxtDeducao.Text);
        TxtDeducao.Text = Validar.Formatar(TxtDeducao.Text);
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
        TxtFaixa.Text = Validar.ValidarNumero(TxtFaixa.Text);
        TxtFaixa.Select(TxtFaixa.Text.Length, 0);
    }

    private void TxtFaixa_Leave(object sender, EventArgs e)
    {
        TxtFaixa.Text = Validar.ZeroNumero(TxtFaixa.Text);
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
        TxtPorcentagem.Text = Validar.ValidarValor(TxtPorcentagem.Text);
        TxtPorcentagem.Select(TxtPorcentagem.Text.Length, 0);
    }

    private void TxtPorcentagem_Leave(object sender, EventArgs e)
    {
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

    private async void MktCompetencia_Leave(object sender, EventArgs e)
    {
        try
        {
            DateTime competencia = DateTime.Parse(MktCompetencia.Text);
            int faixa = await _irrfServices.UltimaFaixaIrrf(competencia) + 1;

            TxtFaixa.Text = faixa.ToString();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void LblLinkOnline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        //TributacaoRFB tributacaoRFB = new();
        //await tributacaoRFB.AtualizarOnline();
        //await ListarTabelaIrrf();
    }
}
