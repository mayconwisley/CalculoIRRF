﻿using CalculoIRRF.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Calculo;

public class PensaoCalculo(DateTime _competencia, int _qtdDependente, double _valorInss,
                           double _valorBruto, double _porcentagemPensao,
                           IIrrfServices _irrfServices, ISimplificadoServices _simplificadoServices,
                           IDependenteServices _dependenteServices, IDescontoMinimoServices _descontoMinimoServices)
{
    double pensaoSimplificado;
    double pensaoNormal;

    public List<string> DadosCalculoPensao { get; set; } = [];

    public async Task CalculoJudicialIrrfSimplificado(bool detalhe)
    {
        IrrfCalculo irrfCalculo = new(_competencia, _qtdDependente, _valorInss, _valorBruto,
                                  _simplificadoServices, _descontoMinimoServices, _irrfServices, _dependenteServices);

        double valorSimplificado = await _simplificadoServices.ValorSimplificado(_competencia);
        double valorPensao = 0d;
        double anteriorP = 0d;
        double descontoIrrfSimplificado = 0d;
        int seqCalculo = 0;

        DadosCalculoPensao.Add($"Calculo Pensão Alimentícia - Rendimentos {_porcentagemPensao:##0.00}%\n");
        DadosCalculoPensao.Add("\n\tSimplificado\n\n");

        do
        {
            seqCalculo++;
            double baseIrrf = await irrfCalculo.BaseIrrfSimplificado();
            double basePensao = _valorBruto - _valorInss;

            anteriorP = valorPensao;

            int faixaIrrf = await _irrfServices.FaixaIrrf(basePensao - anteriorP, _competencia);
            double aliquotaIrrf = await _irrfServices.PorcentagemIrrf(faixaIrrf, _competencia) / 100;
            double valorDeducao = await _irrfServices.DeducaoIrrf(faixaIrrf, _competencia);

            descontoIrrfSimplificado = await irrfCalculo.Simplificado(valorPensao);
            basePensao = basePensao - descontoIrrfSimplificado;
            valorPensao = basePensao * (_porcentagemPensao / 100);

            if (detalhe)
            {
                DadosCalculoPensao.Add($"\t{seqCalculo}º: Calculo\n" +
                                       $"{_valorBruto:#,##0.00} - {valorSimplificado:#,##0.00} = {baseIrrf:#,##0.00}\n" +
                                       $"(({baseIrrf:#,##0.00} - {anteriorP:#,##0.00}) * {aliquotaIrrf * 100:#,##0.00}%) - {valorDeducao:#,##0.00} = {descontoIrrfSimplificado:#,##0.00}\n\n" +
                                       $"\tBase Pensão\n" +
                                       $"{_valorBruto:#,##0.00} - {_valorInss:#,##0.00} - {descontoIrrfSimplificado:#,##0.00} = {basePensao:#,##0.00}\n" +
                                       $"{basePensao:#,##0.00} * {_porcentagemPensao:#,##0.00}% = {valorPensao:#,##0.00}\n\n");

            }
            else
            {
                DadosCalculoPensao.Add($"{seqCalculo}º: " +
                                       $"Base IR: {baseIrrf - anteriorP:#,##0.00}, " +
                                       $"Aliquota: {aliquotaIrrf * 100:#,##0.00}%, " +
                                       $"Dedução: {valorDeducao:#,##0.00}, " +
                                       $"IR: {descontoIrrfSimplificado:#,##0.00}, " +
                                       $"Base Pensão: {basePensao:#,##0.00}, " +
                                       $"Valor Pensão: {valorPensao:#,##0.00}\n");
            }
        } while (Math.Abs(valorPensao - anteriorP) > 0.01d);

        pensaoSimplificado = valorPensao + descontoIrrfSimplificado;
        DadosCalculoPensao.Add($"\nTotal IR: {descontoIrrfSimplificado:#,##0.00}\n");
        DadosCalculoPensao.Add($"\nTotal Pensão: {valorPensao:#,##0.00}\n");
        DadosCalculoPensao.Add($"\nTotal: Pensão + IR: {pensaoSimplificado:#,##0.00}\n");
    }
    public async Task CalculoJudicialIrrfNormal(bool detalhe)
    {

        IrrfCalculo irrfCalculo = new(_competencia, _qtdDependente, _valorInss, _valorBruto,
                                      _simplificadoServices, _descontoMinimoServices, _irrfServices, _dependenteServices);


        double baseCalculo = await irrfCalculo.BaseIrrfNormal();
        double valorDependente = await _dependenteServices.VlrDependente(_competencia) * _qtdDependente;

        double valorPensao = 0d;
        double anteriorP = 0d;
        double descontoIrrfNormal = 0d;
        int seqCalculo = 0;

        DadosCalculoPensao.Add("\n\tNormal\n\n");
        do
        {
            seqCalculo++;

            anteriorP = valorPensao;

            int faixaIrrf = await _irrfServices.FaixaIrrf(baseCalculo - anteriorP, _competencia);
            double aliquotaIrrf = await _irrfServices.PorcentagemIrrf(faixaIrrf, _competencia) / 100;
            double parcelaDeduzirIrrf = await _irrfServices.DeducaoIrrf(faixaIrrf, _competencia);
            double valorDeducao = await _irrfServices.DeducaoIrrf(faixaIrrf, _competencia);
            descontoIrrfNormal = await irrfCalculo.Normal(valorPensao);

            double novaBaseCalculo = baseCalculo - anteriorP;
            double calculoAliquotaIrPensao = novaBaseCalculo * aliquotaIrrf;

            valorPensao = (baseCalculo - calculoAliquotaIrPensao + parcelaDeduzirIrrf) * (_porcentagemPensao / 100);

            if (detalhe)
            {
                DadosCalculoPensao.Add($"\t{seqCalculo}º: Calculo\n" +
                                       $"{_valorBruto:#,##0.00} - {_valorInss:#,##0.00} = {baseCalculo:#,##0.00}\n" +
                                       $"(({baseCalculo:#,##0.00} - {valorDependente:#,##0.00} - {anteriorP:#,##0.00}) * {aliquotaIrrf * 100:#,##0.00}%) - {valorDeducao:#,##0.00} = {descontoIrrfNormal:#,##0.00}\n\n" +
                                       $"\tBase Pensão\n" +
                                       $"{_valorBruto:#,##0.00} - {_valorInss:#,##0.00} - {descontoIrrfNormal:#,##0.00} = {baseCalculo - descontoIrrfNormal:#,##0.00}\n" +
                                       $"{baseCalculo - descontoIrrfNormal:#,##0.00} * {_porcentagemPensao:#,##0.00}% = {valorPensao:#,##0.00}\n\n");

            }
            else
            {
                DadosCalculoPensao.Add($"{seqCalculo}º: " +
                                       $"Base IR: {baseCalculo - anteriorP:#,##0.00}, " +
                                       $"Aliquota: {aliquotaIrrf * 100:#,##0.00}%, " +
                                       $"Dedução: {valorDeducao:#,##0.00}, " +
                                       $"IR: {descontoIrrfNormal:#,##0.00}, " +
                                       $"Base Pensão: {baseCalculo - descontoIrrfNormal:#,##0.00} " +
                                       $"Valor Pensão: {valorPensao:#,##0.00}\n");
            }
        } while (Math.Abs(valorPensao - anteriorP) > 0.01d);

        pensaoNormal = valorPensao + descontoIrrfNormal;
        DadosCalculoPensao.Add($"\nTotal IR: {descontoIrrfNormal:#,##0.00}\n");
        DadosCalculoPensao.Add($"\nTotal Pensão: {valorPensao:#,##0.00}\n");
        DadosCalculoPensao.Add($"\nTotal: Pensão + IR: {pensaoNormal:#,##0.00}\n");
    }
    public void Vantagem()
    {
        if (pensaoSimplificado < pensaoNormal)
        {
            DadosCalculoPensao.Add("\n\n\tCalculo Simplicado é vantajoso");
        }
        else
        {
            DadosCalculoPensao.Add("\n\n\tCalculo Normal é vantajoso");
        }
    }
}
