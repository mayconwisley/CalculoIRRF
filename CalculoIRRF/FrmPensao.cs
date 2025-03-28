﻿using CalculoIRRF.Services.Calculo;
using CalculoIRRF.Services.Interface;
using CalculoIRRF.Services.Validacao;
using System;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace CalculoIRRF;

[SupportedOSPlatform("windows")]
public partial class FrmPensao : Form
{
    readonly DateTime _competencia;
    readonly double _baseInss;
    readonly double _valorBruto;
    readonly int _qtdDependente;
    readonly IInssServices _inssServices;
    readonly IIrrfServices _irrfServices;
    readonly ISimplificadoServices _simplificadoServices;
    readonly IDependenteServices _dependenteServices;
    readonly IDescontoMinimoServices _descontoMinimoServices;


    public FrmPensao(DateTime competencia, double baseInss, int qtdDependente, double valorBruto,
                     IInssServices inssServices, IIrrfServices irrfServices, ISimplificadoServices simplificadoServices,
                     IDependenteServices dependenteServices, IDescontoMinimoServices descontoMinimoServices)
    {
        InitializeComponent();
        _competencia = competencia;
        _baseInss = baseInss;
        _qtdDependente = qtdDependente;
        _valorBruto = valorBruto;
        _inssServices = inssServices;
        _irrfServices = irrfServices;
        _simplificadoServices = simplificadoServices;
        _dependenteServices = dependenteServices;
        _descontoMinimoServices = descontoMinimoServices;
        TxtValorBruto.Text = valorBruto.ToString();
    }

    private async void BtnCalcular_Click(object sender, EventArgs e)
    {
        RtxDescricao.Clear();
        double valorBruto = double.Parse(TxtValorBruto.Text.Trim());
        double porcenPensao = double.Parse(TxtPorcentagem.Text.Trim());
        double outroDesconto = double.Parse(TxtOutrosDescontos.Text.Trim());

        InssCalculo inssCalculo = new(_competencia, _baseInss, _inssServices);
        double valorInss = await inssCalculo.NormalProgressivo();

        CalculoPensao pensaoCalculo = new(_competencia, _qtdDependente, valorInss, (valorBruto - outroDesconto), porcenPensao,
                                          _irrfServices, _simplificadoServices, _dependenteServices, _descontoMinimoServices);

        RtxDescricao.AppendText($"Calculo Pensão Alimentícia - Rendimentos {porcenPensao:##0.00}%\n");

        await pensaoCalculo.CalculoJudicialIrrfNormal(false);
        await pensaoCalculo.CalculoJudicialIrrfSimplificado(false);

        pensaoCalculo.Vantagem();

        foreach (var item in pensaoCalculo.DadosCalculoPensao)
        {
            RtxDescricao.AppendText(item);
        }
    }

    private void TxtValorBruto_TextChanged(object sender, EventArgs e)
    {
        TxtValorBruto.Text = Validar.ValidarValor(TxtValorBruto.Text);
        TxtValorBruto.Select(TxtValorBruto.Text.Length, 0);

    }

    private void TxtValorBruto_Leave(object sender, EventArgs e)
    {
        TxtValorBruto.Text = Validar.Zero(TxtValorBruto.Text);
        TxtValorBruto.Text = Validar.Formatar(TxtValorBruto.Text);
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

    private void TxtOutrosDescontos_TextChanged(object sender, EventArgs e)
    {
        TxtOutrosDescontos.Text = Validar.ValidarValor(TxtOutrosDescontos.Text);
        TxtOutrosDescontos.Select(TxtOutrosDescontos.Text.Length, 0);
    }

    private void TxtOutrosDescontos_Leave(object sender, EventArgs e)
    {
        TxtOutrosDescontos.Text = Validar.Zero(TxtOutrosDescontos.Text);
        TxtOutrosDescontos.Text = Validar.Formatar(TxtOutrosDescontos.Text);
    }

    private void TxtOutrosDescontos_Enter(object sender, EventArgs e)
    {
        if (TxtOutrosDescontos.Text == "0,00")
        {
            TxtOutrosDescontos.Text = "";
        }
    }

    private async void BtnDetalhar_Click(object sender, EventArgs e)
    {
        RtxDescricao.Clear();
        double valorBruto = double.Parse(TxtValorBruto.Text.Trim());
        double porcenPensao = double.Parse(TxtPorcentagem.Text.Trim());
        double outroDesconto = double.Parse(TxtOutrosDescontos.Text.Trim());

        InssCalculo inssCalculo = new(_competencia, _baseInss, _inssServices);
        double valorInss = await inssCalculo.NormalProgressivo();

        CalculoPensao pensaoCalculo = new(_competencia, _qtdDependente, valorInss, (valorBruto - outroDesconto), porcenPensao,
                                          _irrfServices, _simplificadoServices, _dependenteServices, _descontoMinimoServices);
        await pensaoCalculo.CalculoJudicialIrrfNormal(true);
        await pensaoCalculo.CalculoJudicialIrrfSimplificado(true);


        foreach (var item in pensaoCalculo.DadosCalculoPensao)
        {
            RtxDescricao.AppendText(item);
        }
    }
}
