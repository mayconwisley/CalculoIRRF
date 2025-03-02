﻿using CalculoIRRF.Services.Interface;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Calculo;

public class IrrfCalculo(DateTime _competencia, int _qtdDependente, decimal _valorInss, decimal _valorBruto,
                         ISimplificadoServices _simplificadoServices, IDescontoMinimoServices _descontoMinimoServices,
                         IIrrfServices _irrfServices, IDependenteServices _dependenteServices)
{
    public async Task<decimal> BaseIrrfNormal()
    {
        decimal valorDependente = await _dependenteServices.VlrDependente(_competencia);
        decimal baseIrrf = _valorBruto - _valorInss - _qtdDependente * valorDependente;

        return baseIrrf;
    }
    public async Task<decimal> BaseIrrfSimplificado()
    {
        decimal valorDeducao = await _simplificadoServices.ValorSimplificado(_competencia);
        decimal baseIrrf = _valorBruto - valorDeducao;

        return baseIrrf;
    }
    public async Task<decimal> Normal(decimal valorPensao = 0)
    {
        decimal baseIrrf = await BaseIrrfNormal() - valorPensao;
        int faixaIrrf = await _irrfServices.FaixaIrrf(baseIrrf, _competencia);
        decimal porcentagemInss = await _irrfServices.PorcentagemIrrf(faixaIrrf, _competencia);
        decimal deducaoIrrf = await _irrfServices.DeducaoIrrf(faixaIrrf, _competencia);
        decimal desconto = baseIrrf * (porcentagemInss / 100) - deducaoIrrf;
        desconto = Math.Round(desconto, 2);

        return desconto;
    }
    public async Task<string> DescricaoCalculoNormal()
    {
        decimal valorDependente = await _dependenteServices.VlrDependente(_competencia);
        decimal baseIrrf = await BaseIrrfNormal();
        int faixaIrrf = await _irrfServices.FaixaIrrf(baseIrrf, _competencia);
        decimal porcentagemInss = await _irrfServices.PorcentagemIrrf(faixaIrrf, _competencia);
        decimal deducaoIrrf = await _irrfServices.DeducaoIrrf(faixaIrrf, _competencia);
        decimal desconto = baseIrrf * (porcentagemInss / 100) - deducaoIrrf;
        desconto = Math.Round(desconto, 2);
        decimal aliquotaEfetiva;

        /*Se ocorrer divisão por zero, retornar o valor 0(zero)*/
        try
        {
            aliquotaEfetiva = desconto / _valorBruto * 100;
            aliquotaEfetiva = Math.Truncate(aliquotaEfetiva * 100) / 100;
        }
        catch
        {
            aliquotaEfetiva = 0m;
        }

        StringBuilder strMensagem = new StringBuilder();
        strMensagem.Append("Informações de Calculo do IR Normal\n\n");
        strMensagem.Append($"Valor Bruto: {_valorBruto:#,##0.00}\n");
        strMensagem.Append($"Valor INSS: {_valorInss:#,##0.00}\n");
        strMensagem.Append($"Quantidade Dependente: {_qtdDependente} Valor: {valorDependente:#,##0.00} Total: {_qtdDependente * valorDependente:#,##0.00}\n");
        strMensagem.Append($"Valor Base do IR: {baseIrrf:#,##0.00}\n");
        strMensagem.Append($"Porcentagem: {porcentagemInss:#,##0.00}% - Dedução: {deducaoIrrf:#,##0.00}\n");
        strMensagem.Append($"Valor do Desconto: {desconto:#,##0.00}\n");
        strMensagem.Append($"Alíquota Efetiva: {aliquotaEfetiva:#,##0.00}%\n\n");

        return strMensagem.ToString();
    }
    public async Task<decimal> NormalProgressivo()
    {
        decimal baseIrrf = await BaseIrrfNormal();
        int faixaIrrf = await _irrfServices.FaixaIrrf(baseIrrf, _competencia);
        decimal desconto = 0;
        decimal valorInssAnterior = 0;
        for (int i = 1; i <= faixaIrrf; i++)
        {
            decimal porcentagemInss = await _irrfServices.PorcentagemIrrf(i, _competencia);
            decimal valorIrrf = await _irrfServices.ValorIrrf(i, _competencia);
            decimal baseIrrfCalculo = valorIrrf - valorInssAnterior;

            if (valorIrrf > baseIrrf)
            {
                baseIrrfCalculo = baseIrrf - valorInssAnterior;
            }

            if (baseIrrfCalculo > _valorBruto)
            {
                baseIrrfCalculo = baseIrrf - valorInssAnterior;
            }

            desconto += baseIrrfCalculo * (porcentagemInss / 100);
            valorInssAnterior = valorIrrf;
        }
        desconto = Math.Round(desconto, 2);

        return desconto;
    }
    public async Task<string> DescricaoCalculoNormalProgrssivo()
    {
        StringBuilder strMensagem = new();

        decimal valorDependente = await _dependenteServices.VlrDependente(_competencia);
        decimal baseIrrf = await BaseIrrfNormal();
        int faixaIrrf = await _irrfServices.FaixaIrrf(baseIrrf, _competencia);
        decimal totalDesconto = 0;
        decimal valorInssAnterior = 0;

        strMensagem.Append("Informações de Calculo do IR Normal Progressivo\n\n");
        strMensagem.Append($"Valor Bruto: {_valorBruto:#,##0.00}\n");
        strMensagem.Append($"Valor INSS: {_valorInss:#,##0.00}\n");
        strMensagem.Append($"Quantidade Dependente: {_qtdDependente} Valor: {valorDependente:#,##0.00} Total: {_qtdDependente * valorDependente:#,##0.00}\n");
        strMensagem.Append($"Base de IR: {baseIrrf:#,##0.00}\n");

        for (int i = 1; i <= faixaIrrf; i++)
        {
            decimal porcentagemInss = await _irrfServices.PorcentagemIrrf(i, _competencia);
            decimal valorIrrf = await _irrfServices.ValorIrrf(i, _competencia);
            decimal baseIrrfCalculo = valorIrrf - valorInssAnterior;

            if (valorIrrf > baseIrrf)
            {
                baseIrrfCalculo = baseIrrf - valorInssAnterior;
            }

            if (baseIrrfCalculo > _valorBruto)
            {
                baseIrrfCalculo = baseIrrf - valorInssAnterior;
            }

            decimal desconto = baseIrrfCalculo * (porcentagemInss / 100);
            totalDesconto += desconto;

            valorInssAnterior = valorIrrf;

            strMensagem.Append($"{i}º Faixa ");
            strMensagem.Append($"Base do IR: {baseIrrfCalculo:#,##0.00} ");
            strMensagem.Append($"Porcentagem: {porcentagemInss:#,##0.00}% ");
            strMensagem.Append($"Imposto: {desconto:#,##0.00}\n");
        }
        strMensagem.Append($"Valor do Desconto: {totalDesconto:#,##0.00}\n\n");

        return strMensagem.ToString();
    }
    public async Task<decimal> Simplificado(decimal valorPensao = 0)
    {
        if (_competencia < DateTime.Parse("01/05/2023"))
        {
            return 0;
        }

        decimal valorDeducao = await _simplificadoServices.ValorSimplificado(_competencia);
        decimal baseIrrf = await BaseIrrfSimplificado() - valorPensao;
        int faixaIrrf = await _irrfServices.FaixaIrrf(baseIrrf, _competencia);
        decimal porcentagemInss = await _irrfServices.PorcentagemIrrf(faixaIrrf, _competencia);
        decimal deducaoIrrf = await _irrfServices.DeducaoIrrf(faixaIrrf, _competencia);
        decimal desconto = baseIrrf * (porcentagemInss / 100) - deducaoIrrf;
        desconto = Math.Round(desconto, 2);

        return desconto;
    }
    public async Task<string> DescricaoCalculoSimplificado()
    {
        if (_competencia < DateTime.Parse("01/05/2023"))
        {
            return "Calculo Simplificado é a partir de 05/2023!\n\n";
        }

        decimal valorDeducao = await _simplificadoServices.ValorSimplificado(_competencia);
        decimal baseIrrf = await BaseIrrfSimplificado();
        int faixaIrrf = await _irrfServices.FaixaIrrf(baseIrrf, _competencia);
        decimal porcentagemInss = await _irrfServices.PorcentagemIrrf(faixaIrrf, _competencia);
        decimal deducaoIrrf = await _irrfServices.DeducaoIrrf(faixaIrrf, _competencia);
        decimal desconto = baseIrrf * (porcentagemInss / 100) - deducaoIrrf;
        desconto = Math.Round(desconto, 2);
        decimal aliquotaEfetiica;
        /*Se ocorrer divisão por zero, retornar o valor 0(zero)*/
        try
        {
            aliquotaEfetiica = desconto / _valorBruto * 100;
            aliquotaEfetiica = Math.Truncate(aliquotaEfetiica * 100) / 100;
        }
        catch
        {
            aliquotaEfetiica = 0m;
        }

        StringBuilder strMensagem = new();
        strMensagem.Append("Informações de Calculo do IR Simplificado\n\n");
        strMensagem.Append($"Valor Bruto: {_valorBruto:#,##0.00}\n");
        strMensagem.Append($"Valor Dedução: {valorDeducao:#,##0.00}\n");
        strMensagem.Append($"Valor Base do IR: {baseIrrf:#,##0.00}\n");
        strMensagem.Append($"Porcentagem: {porcentagemInss:#,##0.00}% - Dedução: {deducaoIrrf:#,##0.00}\n");
        strMensagem.Append($"Valor do Desconto: {desconto:#,##0.00}\n");
        strMensagem.Append($"Alíquota Efetiva: {aliquotaEfetiica:#,##0.00}%\n\n");
        return strMensagem.ToString();

    }
    public async Task<string> DescricaoVantagem()
    {
        decimal vlrDescontoMinimo = await _descontoMinimoServices.ValorDescontoMinimo(_competencia);

        if (_competencia < DateTime.Parse("01/05/2023"))
        {
            return "";
        }

        decimal valorNormal = await Normal();
        decimal valorSimplificado = await Simplificado();

        if (valorNormal < vlrDescontoMinimo || valorSimplificado < vlrDescontoMinimo)
        {
            return $"Não tem desconto de IR\n\n" +
                   $"Valor abaixo do valor de desconto minimo {vlrDescontoMinimo:#,##0.00}\n\n";
        }

        if (valorNormal > valorSimplificado)
        {
            decimal total = valorNormal - valorSimplificado;
            return $"Calculo Simplificado é mais vantajoso!\n" +
                   $"Diferença: {total:#,##0.00}\n\n";
        }
        else
        {
            decimal total = valorSimplificado - valorNormal;
            return $"Calculo Normal é mais vantajoso!\n" +
                   $"Diferença: {total:#,##0.00}\n\n";
        }
    }
}
