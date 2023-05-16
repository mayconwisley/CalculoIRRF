using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CalculoIRRF.Modelos.Calculo
{
    public class Irrf
    {
        DateTime _competencia;
        int _qtdDependente = 0;
        decimal _valorInss = 0;
        decimal _valorBruto = 0;


        public Irrf(DateTime competencia, int qtdDependente, decimal valorInss, decimal valorBruto)
        {
            _competencia = competencia;
            _qtdDependente = qtdDependente;
            _valorInss = valorInss;
            _valorBruto = valorBruto;
        }
        public decimal Normal()
        {
            Modelos.Irrf.Listar listar = new Modelos.Irrf.Listar();

            decimal baseIrrf = _valorBruto - _valorInss - (_qtdDependente * 189.59m);
            int faixaIrrf = listar.Faixa(baseIrrf);
            decimal porcentagemIrrf = listar.Porcentagem(faixaIrrf);
            decimal deducaoIrrf = listar.Deducao(faixaIrrf);

            decimal desconto = (baseIrrf * (porcentagemIrrf / 100)) - deducaoIrrf;
            desconto = Math.Round(desconto, 2);

            return desconto;
        }
        public decimal Simplificado()
        {
            Modelos.Simplificado.Listar listar = new Simplificado.Listar();
            decimal valorDeducao = listar.Valor(_competencia);
            decimal baseIrrf = _valorBruto - valorDeducao;

            Modelos.Irrf.Listar listarIrrf = new Modelos.Irrf.Listar();
            int faixaIrrf = listarIrrf.Faixa(baseIrrf);
            decimal porcentagemIrrf = listarIrrf.Porcentagem(faixaIrrf);
            decimal deducaoIrrf = listarIrrf.Deducao(faixaIrrf);

            decimal desconto = (baseIrrf * (porcentagemIrrf / 100)) - deducaoIrrf;
            desconto = Math.Round(desconto, 2);

            return desconto;
        }
    }
}
