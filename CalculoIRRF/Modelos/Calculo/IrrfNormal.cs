using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CalculoIRRF.Modelos.Calculo
{
    public class IrrfNormal
    {
        DateTime _competencia;
        int _qtdDependente = 0;
        decimal _valorInss = 0;
        decimal _valorBase = 0;
        decimal _valorSimplificado = 0;

        public IrrfNormal(DateTime competencia, int qtdDependente, decimal valorInss, decimal valorBase, decimal valorSimplificado)
        {
            _competencia = competencia;
            _qtdDependente = qtdDependente;
            _valorInss = valorInss;
            _valorBase = valorBase;
            _valorSimplificado = valorSimplificado;
        }

        public decimal Normal(decimal porcentagem, decimal deducao)
        {
            decimal resultado = ((_valorBase - _valorInss - (_qtdDependente * 189.59m)) * (porcentagem / 100)) - deducao;
            return resultado;
        }
        public decimal Simplificado(decimal deducao)
        {
            decimal resultado = (_valorBase - _valorSimplificado) - deducao;
            return resultado;
        }
    }
}
