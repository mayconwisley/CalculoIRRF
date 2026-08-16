namespace CalculoIRRF.Domain.Tributacao;

/// <summary>Apura o INSS conforme o regime aplicável à competência.</summary>
public static class CalculadoraInss
{
    private static readonly DateOnly InicioRegimeProgressivo = new(2020, 3, 1);

    public static IReadOnlyList<ResultadoFaixaTributaria> CalcularDetalhes(
        DateOnly competencia,
        decimal baseCalculo,
        IReadOnlyList<FaixaTributaria> faixas)
    {
        if (competencia >= InicioRegimeProgressivo)
            return CalculadoraTributacao.CalcularProgressivo(baseCalculo, faixas);

        if (baseCalculo <= 0m)
            return [];

        var faixa = faixas.OrderBy(item => item.Numero).FirstOrDefault(item => baseCalculo <= item.Limite) ?? faixas.MaxBy(item => item.Limite)
            ?? throw new InvalidOperationException("Não há faixas de INSS cadastradas para a competência informada.");
        var contribuicao = CalculadoraTributacao.CalcularPorFaixa(baseCalculo, faixas);

        return [new ResultadoFaixaTributaria(faixa.Numero, baseCalculo, faixa.Aliquota, contribuicao)];
    }
}
