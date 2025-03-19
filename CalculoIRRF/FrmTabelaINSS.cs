using CalculoIRRF.Model;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Services.Validacao;
using CalculoIRRF.Tributacao.INSS;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class FrmTabelaINSS : Form
{
    private readonly IInssServices _inssServices;
    public FrmTabelaINSS(IInssServices inssServices)
    {
        InitializeComponent();
        _inssServices = inssServices;
    }

    int idInss = 0;

    private async Task ListarTabelaInss()
    {
        try
        {
            DgvTabelaINSS.DataSource = await _inssServices.ListarTodos();
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

        TxtFaixa.Text = (await _inssServices.UltimaFaixaInss(competencia) + 1).ToString();
        TxtValor.Text = "0,00";
        TxtPorcentagem.Text = "0,00";
        TxtValor.Focus();
    }

    private async void FrmTabelaINSS_Load(object sender, EventArgs e)
    {
        MktCompetencia.Text = DateTime.Now.ToString("MM/yyyy");
        await ListarTabelaInss();
    }

    private async void BtnGravar_Click(object sender, EventArgs e)
    {
        try
        {
            Inss dados = new()
            {
                Competencia = DateTime.Parse(MktCompetencia.Text),
                Faixa = int.Parse(TxtFaixa.Text.Trim()),
                Valor = double.Parse(TxtValor.Text.Trim()),
                Porcentagem = double.Parse(TxtPorcentagem.Text.Trim())
            };
            await _inssServices.Gravar(dados);
            await ListarTabelaInss();
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
            Inss dados = new()
            {
                Id = idInss,
                Competencia = DateTime.Parse(MktCompetencia.Text),
                Faixa = int.Parse(TxtFaixa.Text.Trim()),
                Valor = double.Parse(TxtValor.Text.Trim()),
                Porcentagem = double.Parse(TxtPorcentagem.Text.Trim())
            };
            await _inssServices.Alterar(dados);
            await ListarTabelaInss();
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
            await _inssServices.Excluir(idInss);
            await ListarTabelaInss();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private void DgvTabelaINSS_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        try
        {
            idInss = int.Parse(DgvTabelaINSS.Rows[e.RowIndex].Cells["Id"].Value.ToString());
            MktCompetencia.Text = DateTime.Parse(DgvTabelaINSS.Rows[e.RowIndex].Cells["Competencia"].Value.ToString()).ToString("MM/yyyy");
            TxtFaixa.Text = double.Parse(DgvTabelaINSS.Rows[e.RowIndex].Cells["Faixa"].Value.ToString()).ToString("0");
            TxtValor.Text = double.Parse(DgvTabelaINSS.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");
            TxtPorcentagem.Text = double.Parse(DgvTabelaINSS.Rows[e.RowIndex].Cells["Porcentagem"].Value.ToString()).ToString("#,##0.00");
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
            int faixa = await _inssServices.UltimaFaixaInss(competencia) + 1;

            TxtFaixa.Text = faixa.ToString();
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message);
        }
    }

    private async void LkLblOnline_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        try
        {
            TributacaoINSS tributacaoINSS = new(_inssServices);
            var listInssGov = await tributacaoINSS.AtualizarOnline();

            if (listInssGov is null)
            {
                MessageBox.Show("Tabela já atualizada", this.Text);
                return;
            }

            foreach (var inss in listInssGov)
            {
                Inss dados = new()
                {
                    Competencia = DateTime.Parse(MktCompetencia.Text),
                    Faixa = inss.Sequencia,
                    Valor = inss.BaseCaculo,
                    Porcentagem = inss.Aliquota
                };

                if (!await _inssServices.Gravar(dados))
                {
                    MessageBox.Show("Erro ao gravar dados", this.Text);
                    return;
                }
            }
            await ListarTabelaInss();
            MessageBox.Show("Tabela atualizada com sucesso", this.Text);
        }
        catch (Exception ex)
        {
            MessageBox.Show(ex.Message, this.Text);
        }
    }
}
