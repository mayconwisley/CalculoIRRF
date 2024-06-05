using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CalculoIRRF.Modelo.Calculo
{
    public class Pensao
    {
        readonly DateTime _competencia;
        readonly int _qtdDependente = 0;
        readonly decimal _valorBruto;
        readonly decimal _valorInss;
        readonly decimal _porcentagemPensao;

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

            decimal baseCalculo = calculoIrrf.BaseIrrfNormal();

            decimal valorPensao = 0m;
            decimal anteriorP = 0m;
            int seqCalculo = 0;
            DadosCalculoPensao.Add("Calculo Pensão Alimentencia - Rendimentos\n");

            do
            {
                seqCalculo++;

                anteriorP = valorPensao;

                int faixaIrrf = cadastroIrrf.FaixaIrrf((baseCalculo - anteriorP), _competencia);
                decimal aliquotaIrrf = cadastroIrrf.PorcentagemIrrf(faixaIrrf, _competencia) / 100;
                decimal parcelaDeduzirIrrf = cadastroIrrf.DeducaoIrrf(faixaIrrf, _competencia);

                decimal descontoIrrfNormal = calculoIrrf.Normal(valorPensao);
                decimal descontoIrrfSimplificado = calculoIrrf.Simplificado(valorPensao);

                valorPensao = (baseCalculo - (aliquotaIrrf * (baseCalculo - anteriorP)) + parcelaDeduzirIrrf) * (_porcentagemPensao / 100);

                DadosCalculoPensao.Add($"{seqCalculo}º Calculo: Base {(baseCalculo - anteriorP):#,##0.00}, IR: {descontoIrrfNormal:#,##0.00}, Valor Pensão {valorPensao:#,##0.00}\n");
            } while (Math.Abs(valorPensao - anteriorP) > 0.01m);

            return valorPensao;

        }
    }
}
