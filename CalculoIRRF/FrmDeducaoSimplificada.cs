using CalculoIRRF.Model;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Services.Validacao;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class FrmDeducaoSimplificada : Form
{
	private readonly ISimplificadoServices _simplificadoServices;
	public FrmDeducaoSimplificada(ISimplificadoServices simplificadoServices)
	{
		InitializeComponent();
		_simplificadoServices = simplificadoServices;
	}

	int idSimplificado = 0;
	private async Task ListarTabelaSimplificado()
	{
		try
		{
			DgvValorSimplificado.DataSource = await _simplificadoServices.ListarTodos();
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
	private void AtivarBotoes(bool ativar = true)
	{
		BtnGravar.Enabled = !ativar;
		BtnAlterar.Enabled = ativar;
		BtnExcluir.Enabled = ativar;
	}
	private async void BtnGravar_Click(object sender, EventArgs e)
	{
		try
		{
			Simplificado dados = new()
			{
				Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
				Valor = double.Parse(TxtValor.Text.Trim())
			};
			await _simplificadoServices.Gravar(dados);
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
			Simplificado dados = new()
			{
				Id = idSimplificado,
				Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
				Valor = double.Parse(TxtValor.Text.Trim())
			};
			await _simplificadoServices.Alterar(dados);
			AtivarBotoes(false);
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
			await _simplificadoServices.Excluir(idSimplificado);
			AtivarBotoes(false);
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
			TxtValor.Text = double.Parse(DgvValorSimplificado.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");

			AtivarBotoes();
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
