using CalculoIRRF.Services.Interface;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Calculo;

public class CalculoImposto(IInssServices _inssServices, IIrrfServices _irrfServices,
                            ISimplificadoServices _simplificadoServices, IDescontoMinimoServices _descontoMinimoServices,
                            IDependenteServices _dependenteServices)
{
    public async Task<List<(Color, string)>> Calcular(DateTime competencia, double valorBruto, double baseInss, int qtdDependente)
    {
        var inssCalculo = new InssCalculo(competencia, baseInss, _inssServices);
        double valorInss = await inssCalculo.NormalProgressivo();

        var irrfCalculo = new IrrfCalculo(competencia, qtdDependente, valorInss, valorBruto,
                                          _simplificadoServices, _descontoMinimoServices,
                                          _irrfServices, _dependenteServices);

        var fgtsCalculo = new FgtsCalculo(baseInss);

        var resultado = new List<(Color, string)>
        {
            (Color.Blue, $"{await irrfCalculo.DescricaoCalculoNormal()}\n--------------------------------------------------\n"),
            (Color.Red, $"{await irrfCalculo.DescricaoCalculoSimplificado()}\n--------------------------------------------------\n"),
            (Color.Green, $"{await irrfCalculo.DescricaoVantagem()}\n--------------------------------------------------\n"),
            (Color.Black, $"{await irrfCalculo.DescricaoCalculoNormalProgrssivo()}\n--------------------------------------------------\n"),
            (Color.Black, $"{await irrfCalculo.DescricaoCalculoSimplificadoProgrssivo()}\n--------------------------------------------------\n"),
            (Color.Black, $"{await inssCalculo.DescricaoCalculoNormalProgressivo()}\n--------------------------------------------------\n"),
            (Color.Black, $"FGTS 8% {fgtsCalculo.Normal8():#,##0.00}\n"),
            (Color.Black, $"FGTS 2% {fgtsCalculo.Normal2():#,##0.00}")
        };

        return resultado;
    }
}
