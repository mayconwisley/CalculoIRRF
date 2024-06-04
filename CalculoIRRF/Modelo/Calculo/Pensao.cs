using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoIRRF.Modelo.Calculo
{
    public class Pensao
    {
        readonly DateTime _competencia;
        readonly int _qtdDependente = 0;
        readonly decimal _valorBruto;
        readonly decimal _valorInss;
        readonly decimal _porcentagemPensao;
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
            Modelo.Calculo.Irrf irrf = new Modelo.Calculo.Irrf(_competencia, _qtdDependente, _valorInss, _valorBruto);

            decimal baseIrrf = irrf.BaseIrrfNormal();
            decimal descontoIrrf = irrf.Simplificado();

            decimal basePensao = baseIrrf - descontoIrrf;
            decimal valorPensao = basePensao * (_porcentagemPensao / 100);

            return 00;

        }
    }
}
