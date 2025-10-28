using CalculoIRRF.Model;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Services.Validacao;
using CalculoIRRF.Tributacao.AcessarSite;
using CalculoIRRF.Tributacao.IRRF;
using System;
using System.Linq;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class FrmTabelaIRRF : Form
{
	readonly IIrrfServices _irrfServices;
	readonly IDependenteServices _dependenteServices;
	readonly ISimplificadoServices _simplificadoServices;
	public FrmTabelaIRRF(IIrrfServices irrfServices, IDependenteServices dependenteServices, ISimplificadoServices simplificadoServices)
	{
		InitializeComponent();
		_irrfServices = irrfServices;
		_dependenteServices = dependenteServices;
		_simplificadoServices = simplificadoServices;
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
			Irrf dados = new()
			{
				Competencia = DateTime.Parse(MktCompetencia.Text),
				Faixa = int.Parse(TxtFaixa.Text.Trim()),
				Valor = double.Parse(TxtValor.Text.Trim()),
				Porcentagem = double.Parse(TxtPorcentagem.Text.Trim()),
				Deducao = double.Parse(TxtDeducao.Text.Trim())
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
				Valor = double.Parse(TxtValor.Text.Trim()),
				Porcentagem = double.Parse(TxtPorcentagem.Text.Trim()),
				Deducao = double.Parse(TxtDeducao.Text.Trim())
			};
			AtivarBotoes(false);
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
			AtivarBotoes(false);
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
			TxtFaixa.Text = double.Parse(DgvTabelaIRRF.Rows[e.RowIndex].Cells["Faixa"].Value.ToString()).ToString("0");
			TxtValor.Text = double.Parse(DgvTabelaIRRF.Rows[e.RowIndex].Cells["Valor"].Value.ToString()).ToString("#,##0.00");
			TxtPorcentagem.Text = double.Parse(DgvTabelaIRRF.Rows[e.RowIndex].Cells["Porcentagem"].Value.ToString()).ToString("#,##0.00");
			TxtDeducao.Text = double.Parse(DgvTabelaIRRF.Rows[e.RowIndex].Cells["Deducao"].Value.ToString()).ToString("#,##0.00");

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
			TxtDeducao.Text = "";
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
			TxtFaixa.Text = "";
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
			TxtPorcentagem.Text = "";
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
		try
		{
			TributacaoRFB tributacaoRFB = new(_irrfServices);
			var listIrrf = await tributacaoRFB.AtualizarOnline();

			if (listIrrf is null)
			{
				MessageBox.Show("Tabela já está atualizada", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
				return;
			}

			var valorDependente = listIrrf.Select(s => s.Dependente).FirstOrDefault();
			var vigencia = listIrrf.Select(s => s.Vigencia).FirstOrDefault();
			var dependente = new Dependente()
			{
				Competencia = vigencia,
				Valor = valorDependente

			};
			if (!await _dependenteServices.Gravar(dependente))
			{
				MessageBox.Show("Erro ao gravar dados do dependente", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			var valorSimplificado = listIrrf.Select(s => s.Simplificado).FirstOrDefault();

			var simplificado = new Simplificado()
			{
				Competencia = vigencia,
				Valor = valorDependente
			};
			if (!await _simplificadoServices.Gravar(simplificado))
			{
				MessageBox.Show("Erro ao gravar dados simplificado", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			int countItem = 0;
			foreach (var irrfGov in listIrrf)
			{
				countItem++;
				var irrf = new Irrf
				{
					Competencia = DateTime.Parse(MktCompetencia.Text),
					Faixa = irrfGov.Sequencia,
					Valor = irrfGov.BaseCaculo,
					Porcentagem = irrfGov.Aliquota,
					Deducao = irrfGov.Deducao
				};

				if (countItem == irrfGov.Sequencia)
				{
					irrf.Valor = double.Parse("99999999999.99");
				}

				if (!await _irrfServices.Gravar(irrf))
				{
					MessageBox.Show("Erro ao gravar dados irrf", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
					return;
				}
			}
			await ListarTabelaIrrf();
			MessageBox.Show("Tabela atualizada com sucesso", this.Text);
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, this.Text);
		}
	}

	private void LblLinkSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
	{
		string url = "https://www.contabeis.com.br/tabelas/imposto-renda/";

		try
		{
			AcessarUrl.Acessar(url);
		}
		catch (Exception ex)
		{
			MessageBox.Show($"Erro ao acessar o site: {ex.Message}", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
		}
	}
}
