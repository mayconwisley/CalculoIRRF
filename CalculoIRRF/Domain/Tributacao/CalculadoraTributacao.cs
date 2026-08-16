namespace CalculoIRRF.Domain.Tributacao;

/// <summary>Regras puras e reutilizáveis para cálculo por tabela tributária.</summary>
public static class CalculadoraTributacao
{
    public static decimal CalcularPorFaixa(decimal baseCalculo, IReadOnlyList<FaixaTributaria> faixas)
    {
        var faixa = ObterFaixa(baseCalculo, faixas);
        return Arredondar(Math.Max(0m, baseCalculo) * faixa.AliquotaDecimal - faixa.Deducao);
    }

    public static IReadOnlyList<ResultadoFaixaTributaria> CalcularProgressivo(decimal baseCalculo, IReadOnlyList<FaixaTributaria> faixas)
    {
        if (baseCalculo <= 0m)
            return [];

        ValidarFaixas(faixas);
        var resultado = new List<ResultadoFaixaTributaria>();
        var limiteAnterior = 0m;

        foreach (var faixa in faixas.OrderBy(item => item.Numero))
        {
            var baseDaFaixa = Math.Min(baseCalculo, faixa.Limite) - limiteAnterior;
            if (baseDaFaixa > 0m)
            {
                resultado.Add(new ResultadoFaixaTributaria(
                    faixa.Numero,
                    baseDaFaixa,
                    faixa.Aliquota,
                    Arredondar(baseDaFaixa * faixa.AliquotaDecimal)));
            }

            if (baseCalculo <= faixa.Limite)
                break;

            limiteAnterior = faixa.Limite;
        }

        return resultado;
    }

    public static decimal Arredondar(decimal valor) => Math.Round(valor, 2, MidpointRounding.AwayFromZero);

    private static FaixaTributaria ObterFaixa(decimal baseCalculo, IReadOnlyList<FaixaTributaria> faixas)
    {
        ValidarFaixas(faixas);
        var basePositiva = Math.Max(0m, baseCalculo);
        return faixas.OrderBy(item => item.Numero).FirstOrDefault(item => basePositiva <= item.Limite) ?? faixas.MaxBy(item => item.Limite)!;
    }

    private static void ValidarFaixas(IReadOnlyList<FaixaTributaria> faixas)
    {
        if (faixas.Count == 0)
            throw new InvalidOperationException("Não há faixas tributárias cadastradas para a competência informada.");
    }
}
