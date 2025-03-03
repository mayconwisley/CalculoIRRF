using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Calculo;

public class PensaoCalculo(DateTime _competencia, int _qtdDependente, decimal _valorInss, decimal _valorBruto, decimal _porcentagemPensao)
{
    decimal pensaoSimplificado;
    decimal pensaoNormal;

    public List<string> DadosCalculoPensao { get; set; } = [];

    public async Task CalculoJudicialIrrfSimplificado(bool detalhe)
    {
        IrrfServices irrfServices = new();
        SimplificadoServices simplificadoServices = new();
        IrrfCalculo irrfCalculo = new(_competencia, _qtdDependente, _valorInss, _valorBruto);

        decimal valorSimplificado = await simplificadoServices.ValorSimplificado(_competencia);
        decimal valorPensao = 0m;
        decimal anteriorP = 0m;
        decimal descontoIrrfSimplificado = 0m;
        int seqCalculo = 0;

        DadosCalculoPensao.Add($"Calculo Pensão Alimentícia - Rendimentos {_porcentagemPensao:##0.00}%\n");
        DadosCalculoPensao.Add("\n\tSimplificado\n\n");

        do
        {
            seqCalculo++;
            decimal baseIrrf = await irrfCalculo.BaseIrrfSimplificado();
            decimal basePensao = _valorBruto - _valorInss;

            anteriorP = valorPensao;

            int faixaIrrf = await irrfServices.FaixaIrrf(basePensao - anteriorP, _competencia);
            decimal aliquotaIrrf = await irrfServices.PorcentagemIrrf(faixaIrrf, _competencia) / 100;
            decimal valorDeducao = await irrfServices.DeducaoIrrf(faixaIrrf, _competencia);

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
        } while (Math.Abs(valorPensao - anteriorP) > 0.01m);

        pensaoSimplificado = valorPensao + descontoIrrfSimplificado;
        DadosCalculoPensao.Add($"\nTotal IR: {descontoIrrfSimplificado:#,##0.00}\n");
        DadosCalculoPensao.Add($"\nTotal Pensão: {valorPensao:#,##0.00}\n");
        DadosCalculoPensao.Add($"\nTotal: Pensão + IR: {pensaoSimplificado:#,##0.00}\n");
    }
    public async Task CalculoJudicialIrrfNormal(bool detalhe)
    {
        IrrfServices irrfServices = new();
        IrrfCalculo irrfCalculo = new(_competencia, _qtdDependente, _valorInss, _valorBruto);
        DependenteServices dependenteServices = new();

        decimal baseCalculo = await irrfCalculo.BaseIrrfNormal();
        decimal valorDependente = await dependenteServices.VlrDependente(_competencia) * _qtdDependente;

        decimal valorPensao = 0m;
        decimal anteriorP = 0m;
        decimal descontoIrrfNormal = 0m;
        int seqCalculo = 0;

        DadosCalculoPensao.Add("\n\tNormal\n\n");
        do
        {
            seqCalculo++;

            anteriorP = valorPensao;

            int faixaIrrf = await irrfServices.FaixaIrrf(baseCalculo - anteriorP, _competencia);
            decimal aliquotaIrrf = await irrfServices.PorcentagemIrrf(faixaIrrf, _competencia) / 100;
            decimal parcelaDeduzirIrrf = await irrfServices.DeducaoIrrf(faixaIrrf, _competencia);
            decimal valorDeducao = await irrfServices.DeducaoIrrf(faixaIrrf, _competencia);
            descontoIrrfNormal = await irrfCalculo.Normal(valorPensao);

            decimal novaBaseCalculo = baseCalculo - anteriorP;
            decimal calculoAliquotaIrPensao = novaBaseCalculo * aliquotaIrrf;

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
        } while (Math.Abs(valorPensao - anteriorP) > 0.01m);

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
