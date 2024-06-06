using CalculoIRRF.Modelo.Validacao;
using System;
using System.Windows.Forms;

namespace CalculoIRRF
{
    public partial class FrmPensao : Form
    {
        readonly DateTime _competencia;
        readonly decimal _baseInss;
        readonly decimal _valorBruto;
        readonly int _qtdDependente;

        public FrmPensao(DateTime competencia, decimal baseInss, int qtdDependente, decimal valorBruto)
        {
            InitializeComponent();
            _competencia = competencia;
            _baseInss = baseInss;
            _qtdDependente = qtdDependente;
            _valorBruto = valorBruto;
            TxtValorBruto.Text = valorBruto.ToString();
        }

        private void BtnCalcular_Click(object sender, EventArgs e)
        {
            RtxDescricao.Clear();
            decimal valorBruto = decimal.Parse(TxtValorBruto.Text.Trim());
            decimal porcenPensao = decimal.Parse(TxtPorcentagem.Text.Trim());
            decimal outroDesconto = decimal.Parse(TxtOutrosDescontos.Text.Trim());

            Modelo.Calculo.Inss inss = new Modelo.Calculo.Inss(_competencia, _baseInss);
            decimal valorInss = inss.NormalProgressivo();
        }

        private void TxtValorBruto_TextChanged(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtValorBruto.Text = validar.ValidarValor(TxtValorBruto.Text);
            TxtValorBruto.Select(TxtValorBruto.Text.Length, 0);

        }

        private void TxtValorBruto_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtValorBruto.Text = validar.Zero(TxtValorBruto.Text);
            TxtValorBruto.Text = validar.Formatar(TxtValorBruto.Text);
        }

        private void TxtValorBruto_Enter(object sender, EventArgs e)
        {
            if (TxtValorBruto.Text == "0,00")
            {
                TxtValorBruto.Text = "";
            }
        }

        private void TxtPorcentagem_TextChanged(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtPorcentagem.Text = validar.ValidarValor(TxtPorcentagem.Text);
            TxtPorcentagem.Select(TxtPorcentagem.Text.Length, 0);
        }

        private void TxtPorcentagem_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtPorcentagem.Text = validar.Zero(TxtPorcentagem.Text);
            TxtPorcentagem.Text = validar.Formatar(TxtPorcentagem.Text);
        }

        private void TxtPorcentagem_Enter(object sender, EventArgs e)
        {
            if (TxtPorcentagem.Text == "0,00")
            {
                TxtPorcentagem.Text = "";
            }
        }

        private void TxtOutrosDescontos_TextChanged(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtOutrosDescontos.Text = validar.ValidarValor(TxtOutrosDescontos.Text);
            TxtOutrosDescontos.Select(TxtOutrosDescontos.Text.Length, 0);
        }

        private void TxtOutrosDescontos_Leave(object sender, EventArgs e)
        {
            Validar validar = new Validar();
            TxtOutrosDescontos.Text = validar.Zero(TxtOutrosDescontos.Text);
            TxtOutrosDescontos.Text = validar.Formatar(TxtOutrosDescontos.Text);
        }

        private void TxtOutrosDescontos_Enter(object sender, EventArgs e)
        {
            if (TxtOutrosDescontos.Text == "0,00")
            {
                TxtOutrosDescontos.Text = "";
            }
        }

        private void BtnDetalhar_Click(object sender, EventArgs e)
        {
            RtxDescricao.Clear();
            decimal valorBruto = decimal.Parse(TxtValorBruto.Text.Trim());
            decimal porcenPensao = decimal.Parse(TxtPorcentagem.Text.Trim());
            decimal outroDesconto = decimal.Parse(TxtOutrosDescontos.Text.Trim());

            Modelo.Calculo.Inss inss = new Modelo.Calculo.Inss(_competencia, _baseInss);
            decimal valorInss = inss.NormalProgressivo();

            Modelo.Calculo.Pensao pensao = new Modelo.Calculo.Pensao(_competencia, _qtdDependente, valorInss, (valorBruto - outroDesconto), porcenPensao);
            pensao.CalculoJudicialIrrfSimplificado(true);
            pensao.CalculoJudicialIrrfNormal(true);

            foreach (var item in pensao.DadosCalculoPensao)
            {
                RtxDescricao.AppendText(item);
            }
        }
    }
}
