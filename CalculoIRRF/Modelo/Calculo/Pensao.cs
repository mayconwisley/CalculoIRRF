using System;
using System.Collections.Generic;

namespace CalculoIRRF.Modelo.Calculo
{
    public class Pensao
    {
        readonly DateTime _competencia;
        readonly int _qtdDependente = 0;
        readonly decimal _valorBruto;
        readonly decimal _valorInss;
        readonly decimal _porcentagemPensao;

        decimal pensaoSimplificado;
        decimal pensaoNormal;

        public List<string> DadosCalculoPensao { get; set; } = new List<string>();
        public Pensao(DateTime competencia, int qtdDependente, decimal valorInss, decimal valorBruto, decimal porcentagemPensao)
        {
            _competencia = competencia;
            _valorBruto = valorBruto;
            _valorInss = valorInss;
            _qtdDependente = qtdDependente;
            _porcentagemPensao = porcentagemPensao;
        }
        public decimal CalculoJudicialIrrfSimplificado()
        {
            Modelo.Irrf.Cadastro cadastroIrrf = new Modelo.Irrf.Cadastro();
            Modelo.Simplificado.Cadastro cadastroSimplificado = new Simplificado.Cadastro();
            Modelo.Calculo.Irrf calculoIrrf = new Irrf(_competencia, _qtdDependente, _valorInss, _valorBruto);

            decimal valorSimplificado = cadastroSimplificado.ValorSimplificado(_competencia);

            decimal valorPensao = 0m;
            decimal anteriorP = 0m;
            decimal descontoIrrfSimplificado = 0m;
            int seqCalculo = 0;

            DadosCalculoPensao.Add($"Calculo Pensão Alimentencia - Rendimentos {_porcentagemPensao:##0.00}%\n");
            DadosCalculoPensao.Add("\n\tSimplificado\n\n");

            do
            {
                seqCalculo++;
                decimal baseIrrf = calculoIrrf.BaseIrrfSimplificado();
                decimal baseCalculo = _valorBruto - _valorInss;

                anteriorP = valorPensao;

                int faixaIrrf = cadastroIrrf.FaixaIrrf((baseCalculo - anteriorP), _competencia);
                decimal aliquotaIrrf = cadastroIrrf.PorcentagemIrrf(faixaIrrf, _competencia) / 100;
                decimal parcelaDeduzirIrrf = cadastroIrrf.DeducaoIrrf(faixaIrrf, _competencia);
                decimal valorDeducao = cadastroIrrf.DeducaoIrrf(faixaIrrf, _competencia);

                descontoIrrfSimplificado = calculoIrrf.Simplificado(valorPensao);
                baseCalculo = baseCalculo - descontoIrrfSimplificado;
                valorPensao = baseCalculo * (_porcentagemPensao / 100);

                DadosCalculoPensao.Add($"{seqCalculo}º: " +
                                       $"Base IR: {(baseIrrf - anteriorP):#,##0.00}, " +
                                       $"Aliquota: {(aliquotaIrrf * 100):#,##0.00}%, " +
                                       $"Dedução: {valorDeducao:#,##0.00}, " +
                                       $"IR: {descontoIrrfSimplificado:#,##0.00}, " +
                                       $"Base Pensão: {baseCalculo:#,##0.00}, " +
                                       $"Valor Pensão: {valorPensao:#,##0.00}\n");
            } while (Math.Abs(valorPensao - anteriorP) > 0.01m);

            pensaoSimplificado = valorPensao + descontoIrrfSimplificado;
            DadosCalculoPensao.Add($"\nTotal: Pensão + IR: {pensaoSimplificado:#,##0.00}\n");
            return valorPensao;
        }
        public decimal CalculoJudicialIrrfSimplificadoDetalhe()
        {
            Modelo.Irrf.Cadastro cadastroIrrf = new Modelo.Irrf.Cadastro();
            Modelo.Simplificado.Cadastro cadastroSimplificado = new Simplificado.Cadastro();
            Modelo.Calculo.Irrf calculoIrrf = new Irrf(_competencia, _qtdDependente, _valorInss, _valorBruto);

            decimal valorSimplificado = cadastroSimplificado.ValorSimplificado(_competencia);

            decimal valorPensao = 0m;
            decimal anteriorP = 0m;
            int seqCalculo = 0;

            DadosCalculoPensao.Add($"Calculo Pensão Alimentencia - Rendimentos {_porcentagemPensao:##0.00}%\n");
            DadosCalculoPensao.Add("\n\t\tSimplificado\n\n");

            do
            {
                seqCalculo++;
                decimal baseIrrf = calculoIrrf.BaseIrrfSimplificado();
                decimal baseCalculo = _valorBruto - _valorInss;

                anteriorP = valorPensao;

                int faixaIrrf = cadastroIrrf.FaixaIrrf((baseCalculo - anteriorP), _competencia);
                decimal aliquotaIrrf = cadastroIrrf.PorcentagemIrrf(faixaIrrf, _competencia) / 100;
                decimal parcelaDeduzirIrrf = cadastroIrrf.DeducaoIrrf(faixaIrrf, _competencia);
                decimal valorDeducao = cadastroIrrf.DeducaoIrrf(faixaIrrf, _competencia);

                decimal descontoIrrfSimplificado = calculoIrrf.Simplificado(valorPensao);
                baseCalculo = baseCalculo - descontoIrrfSimplificado;
                valorPensao = baseCalculo * (_porcentagemPensao / 100);

                DadosCalculoPensao.Add($"\t{seqCalculo}º: Calculo\n" +
                                       $"{_valorBruto:#,##0.00} - {valorSimplificado:#,##0.00} = {baseIrrf:#,##0.00}\n" +
                                       $"{valorSimplificado:#,##0.00} * {(aliquotaIrrf * 100):#,##0.00}% - {valorDeducao:#,##0.00} = {descontoIrrfSimplificado:#,##0.00}\n\n" +
                                       $"\tBase Pensão\n" +
                                       $"{_valorBruto:#,##0.00} - {_valorInss:#,##0.00} - {descontoIrrfSimplificado:#,##0.00} = {baseCalculo:#,##0.00}\n" +
                                       $"{baseCalculo:#,##0.00} * {_porcentagemPensao:#,##0.00}% = {valorPensao:#,##0.00}\n\n");

            } while (Math.Abs(valorPensao - anteriorP) > 0.01m);


            return valorPensao;
        }
        public decimal CalculoJudicialIrrfNormal()
        {
            Modelo.Irrf.Cadastro cadastroIrrf = new Modelo.Irrf.Cadastro();
            Modelo.Calculo.Irrf calculoIrrf = new Irrf(_competencia, _qtdDependente, _valorInss, _valorBruto);
            Modelo.Dependente.Cadastro cadastroDependente = new Dependente.Cadastro();

            decimal baseCalculo = calculoIrrf.BaseIrrfNormal();
            decimal valorDependente = cadastroDependente.VlrDependente(_competencia) * _qtdDependente;

            decimal valorPensao = 0m;
            decimal anteriorP = 0m;
            decimal descontoIrrfNormal = 0m;
            int seqCalculo = 0;

            DadosCalculoPensao.Add("\n\tNormal\n\n");
            do
            {
                seqCalculo++;

                anteriorP = valorPensao;

                int faixaIrrf = cadastroIrrf.FaixaIrrf((baseCalculo - anteriorP), _competencia);
                decimal aliquotaIrrf = cadastroIrrf.PorcentagemIrrf(faixaIrrf, _competencia) / 100;
                decimal parcelaDeduzirIrrf = cadastroIrrf.DeducaoIrrf(faixaIrrf, _competencia);
                decimal valorDeducao = cadastroIrrf.DeducaoIrrf(faixaIrrf, _competencia);
                descontoIrrfNormal = calculoIrrf.Normal(valorPensao);
                valorPensao = (baseCalculo - (aliquotaIrrf * (baseCalculo - valorDependente - anteriorP)) + parcelaDeduzirIrrf) * (_porcentagemPensao / 100);

                DadosCalculoPensao.Add($"{seqCalculo}º: " +
                                       $"Base IR: {(baseCalculo - anteriorP):#,##0.00}, " +
                                       $"Aliquota: {(aliquotaIrrf * 100):#,##0.00}%, " +
                                       $"Dedução: {valorDeducao:#,##0.00}, " +
                                       $"IR: {descontoIrrfNormal:#,##0.00}, " +
                                       $"Base Pensão: {(baseCalculo - descontoIrrfNormal):#,##0.00} " +
                                       $"Valor Pensão: {valorPensao:#,##0.00}\n");
            } while (Math.Abs(valorPensao - anteriorP) > 0.01m);

            pensaoNormal = valorPensao + descontoIrrfNormal;
            DadosCalculoPensao.Add($"\nTotal: Pensão + IR: {pensaoNormal:#,##0.00}\n");
            return valorPensao;
        }
        public decimal CalculoJudicialIrrfNormalDetalhe()
        {
            Modelo.Irrf.Cadastro cadastroIrrf = new Modelo.Irrf.Cadastro();
            Modelo.Calculo.Irrf calculoIrrf = new Irrf(_competencia, _qtdDependente, _valorInss, _valorBruto);
            Modelo.Dependente.Cadastro cadastroDependente = new Dependente.Cadastro();

            decimal baseCalculo = calculoIrrf.BaseIrrfNormal();
            decimal valorDependente = cadastroDependente.VlrDependente(_competencia) * _qtdDependente;

            decimal valorPensao = 0m;
            decimal anteriorP = 0m;
            int seqCalculo = 0;

            DadosCalculoPensao.Add("\n\t\tNormal\n\n");
            do
            {
                seqCalculo++;

                anteriorP = valorPensao;

                int faixaIrrf = cadastroIrrf.FaixaIrrf((baseCalculo - anteriorP), _competencia);
                decimal aliquotaIrrf = cadastroIrrf.PorcentagemIrrf(faixaIrrf, _competencia) / 100;
                decimal parcelaDeduzirIrrf = cadastroIrrf.DeducaoIrrf(faixaIrrf, _competencia);
                decimal valorDeducao = cadastroIrrf.DeducaoIrrf(faixaIrrf, _competencia);
                decimal descontoIrrfNormal = calculoIrrf.Normal(valorPensao);
                valorPensao = (baseCalculo - (aliquotaIrrf * (baseCalculo - valorDependente - anteriorP)) + parcelaDeduzirIrrf) * (_porcentagemPensao / 100);

                DadosCalculoPensao.Add($"\t{seqCalculo}º: Calculo\n" +
                                      $"{_valorBruto:#,##0.00} - {_valorInss:#,##0.00} = {baseCalculo:#,##0.00}\n" +
                                      $"{(baseCalculo - anteriorP):#,##0.00} * {(aliquotaIrrf * 100):#,##0.00}% - {valorDeducao:#,##0.00} = {descontoIrrfNormal:#,##0.00}\n\n" +
                                      $"\tBase Pensão\n" +
                                      $"{_valorBruto:#,##0.00} - {_valorInss:#,##0.00} - {descontoIrrfNormal:#,##0.00} = {(baseCalculo - descontoIrrfNormal):#,##0.00}\n" +
                                      $"{(baseCalculo - descontoIrrfNormal):#,##0.00} * {_porcentagemPensao:#,##0.00}% = {valorPensao:#,##0.00}\n\n");



            } while (Math.Abs(valorPensao - anteriorP) > 0.01m);

            return valorPensao;
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
}
