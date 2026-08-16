using CalculoIRRF.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Calculo;

public class CalculoImposto(IInssServices _inssServices, IIrrfServices _irrfServices,
                            ISimplificadoServices _simplificadoServices, IDescontoMinimoServices _descontoMinimoServices,
                            IDependenteServices _dependenteServices)
{
    public async Task<IReadOnlyList<ResultadoCalculo>> Calcular(DateTime competencia, double valorBruto, double baseInss, int qtdDependente)
    {
        var inssCalculo = new InssCalculo(competencia, baseInss, _inssServices);
        double valorInss = await inssCalculo.NormalProgressivo();

        var irrfCalculo = new IrrfCalculo(competencia, qtdDependente, valorInss, valorBruto,
                                          _simplificadoServices, _descontoMinimoServices,
                                          _irrfServices, _dependenteServices);

        var fgtsCalculo = new FgtsCalculo(baseInss);

        IReadOnlyList<ResultadoCalculo> resultado = new List<ResultadoCalculo>
        {
            new(ResultadoCalculoTipo.Normal, $"{await irrfCalculo.DescricaoCalculoNormal()}\n--------------------------------------------------\n"),
            new(ResultadoCalculoTipo.Simplificado, $"{await irrfCalculo.DescricaoCalculoSimplificado()}\n--------------------------------------------------\n"),
            new(ResultadoCalculoTipo.Vantagem, $"{await irrfCalculo.DescricaoVantagem()}\n--------------------------------------------------\n"),
            new(ResultadoCalculoTipo.Neutro, $"{await irrfCalculo.DescricaoCalculoNormalProgrssivo()}\n--------------------------------------------------\n"),
            new(ResultadoCalculoTipo.Neutro, $"{await irrfCalculo.DescricaoCalculoSimplificadoProgrssivo()}\n--------------------------------------------------\n"),
            new(ResultadoCalculoTipo.Neutro, $"{await inssCalculo.DescricaoCalculoNormalProgressivo()}\n--------------------------------------------------\n"),
            new(ResultadoCalculoTipo.Neutro, $"FGTS 8% {fgtsCalculo.Normal8():#,##0.00}\n"),
            new(ResultadoCalculoTipo.Neutro, $"FGTS 2% {fgtsCalculo.Normal2():#,##0.00}")
        };

        return resultado;
    }
}

public enum ResultadoCalculoTipo { Normal, Simplificado, Vantagem, Neutro }

public sealed record ResultadoCalculo(ResultadoCalculoTipo Tipo, string Texto);
