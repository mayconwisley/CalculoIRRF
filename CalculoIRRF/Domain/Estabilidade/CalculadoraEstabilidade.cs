namespace CalculoIRRF.Domain.Estabilidade;

/// <summary>
/// Regras puras para a apuração de indenização do período de estabilidade.
/// </summary>
public static class CalculadoraEstabilidade
{
    public static ResultadoEstabilidade Calcular(decimal mediaRemuneratoria, int diasBase, DateOnly demissao, DateOnly fimEstabilidade, decimal complementos = 0m)
    {
        if (mediaRemuneratoria < 0m)
            throw new ArgumentOutOfRangeException(nameof(mediaRemuneratoria), "A média remuneratória não pode ser negativa.");
        if (diasBase <= 0)
            throw new ArgumentOutOfRangeException(nameof(diasBase), "Os dias-base devem ser maiores que zero.");

        var diasEstabilidade = fimEstabilidade.DayNumber - demissao.DayNumber;
        if (diasEstabilidade <= 0)
            throw new ArgumentException("O fim da estabilidade deve ser posterior à data de demissão.", nameof(fimEstabilidade));

        var indenizacao = Arredondar(mediaRemuneratoria / diasBase * diasEstabilidade);
        var avos = diasEstabilidade / 30 + (diasEstabilidade % 30 >= 15 ? 1 : 0);
        var decimoTerceiro = Arredondar(mediaRemuneratoria / 12m * avos);
        var ferias = Arredondar(mediaRemuneratoria / 12m * avos);
        var tercoFerias = Arredondar(ferias / 3m);
        var fgtsOitoPorCento = Arredondar((indenizacao + decimoTerceiro) * .08m);
        var multaFgtsQuarentaPorCento = Arredondar(fgtsOitoPorCento * .40m);
        var total = Arredondar(indenizacao + decimoTerceiro + ferias + tercoFerias + fgtsOitoPorCento + multaFgtsQuarentaPorCento + complementos);

        return new ResultadoEstabilidade(diasEstabilidade, avos, indenizacao, decimoTerceiro, ferias, tercoFerias, fgtsOitoPorCento, multaFgtsQuarentaPorCento, complementos, total);
    }

    private static decimal Arredondar(decimal valor) => decimal.Round(valor, 2, MidpointRounding.AwayFromZero);
}
