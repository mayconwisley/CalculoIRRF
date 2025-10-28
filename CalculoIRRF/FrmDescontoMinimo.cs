using CalculoIRRF.Model;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Services.Validacao;
using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculoIRRF;
[SupportedOSPlatform("windows")]
public partial class FrmDescontoMinimo : Form
{
	private readonly IDescontoMinimoServices _descontoMinimoServices;
	public FrmDescontoMinimo(IDescontoMinimoServices descontoMinimoServices)
	{
		InitializeComponent();
		_descontoMinimoServices = descontoMinimoServices;
	}
	int idDescontoMinimo = 0;
	private async Task ListarTabelaDescontoMinimo()
	{
		try
		{
			DgvValorDescontoMinimo.DataSource = await _descontoMinimoServices.ListarTodos();
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
			DescontoMinimo dados = new()
			{
				Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
				Valor = double.Parse(TxtValor.Text.Trim())
			};
			await _descontoMinimoServices.Gravar(dados);

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
			DescontoMinimo dados = new()
			{
				Id = idDescontoMinimo,
				Competencia = DateTime.Parse(MktCompetencia.Text.Trim()),
				Valor = double.Parse(TxtValor.Text.Trim())
			};
			await _descontoMinimoServices.Gravar(dados);
			AtivarBotoes(false);
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
			await _descontoMinimoServices.Excluir(idDescontoMinimo);
			AtivarBotoes(false);
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
			TxtValor.Text = double.Parse(DgvValorDescontoMinimo.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");
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
			TxtValor.Text = "";
	}
}
