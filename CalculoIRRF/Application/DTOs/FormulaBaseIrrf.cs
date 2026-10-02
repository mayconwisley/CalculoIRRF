#nullable enable

namespace CalculoIRRF.Application.DTOs;

/// <summary>
/// Linha da memória de cálculo que mostra como a base do IRRF foi obtida, com cada dedução identificada.
/// A tela, o relatório PDF e os demonstrativos das calculadoras usam o mesmo texto, para explicarem a base da mesma forma.
/// </summary>
public static class FormulaBaseIrrf
{
    public static string Normal(SimulacaoImpostoDto simulacao, Func<decimal, string> moeda) =>
        Normal(simulacao.Entrada.ValorBruto, "valor bruto", simulacao.ValorInss, simulacao.Entrada.QuantidadeDependentes, simulacao.DeducaoPorDependente, simulacao.Normal.BaseCalculo, moeda);

    public static string Simplificada(SimulacaoImpostoDto simulacao, Func<decimal, string> moeda) =>
        Simplificada(simulacao.Entrada.ValorBruto, "valor bruto", simulacao.DescontoSimplificado, simulacao.Simplificada.BaseCalculo, moeda);

    /// <param name="rotuloRendimentos">Como o rendimento aparece na fórmula, por exemplo "valor bruto" ou "férias + 1/3".</param>
    public static string Normal(decimal rendimentos, string rotuloRendimentos, decimal inss, int dependentes, decimal deducaoPorDependente, decimal baseCalculo, Func<decimal, string> moeda)
    {
        var termos = $"{moeda(rendimentos)} ({rotuloRendimentos}) - {moeda(inss)} (INSS)";
        if (dependentes > 0)
            termos += $" - {dependentes} x {moeda(deducaoPorDependente)} ({(dependentes == 1 ? "dependente" : "dependentes")})";

        return Concluir(termos, rendimentos - inss - dependentes * deducaoPorDependente, baseCalculo, "as deduções superam o rendimento", moeda);
    }

    public static string Simplificada(decimal rendimentos, string rotuloRendimentos, decimal? descontoSimplificado, decimal baseCalculo, Func<decimal, string> moeda)
    {
        if (descontoSimplificado is not { } desconto)
            return "Não se aplica: o desconto simplificado vale a partir de 05/2023.";

        var termos = $"{moeda(rendimentos)} ({rotuloRendimentos}) - {moeda(desconto)} (desconto simplificado)";
        return Concluir(termos, rendimentos - desconto, baseCalculo, "o desconto supera o rendimento", moeda);
    }

    // A base não fica negativa: quando as deduções superam o rendimento, ela é zero.
    private static string Concluir(string termos, decimal resultado, decimal baseCalculo, string motivoBaseZero, Func<decimal, string> moeda) =>
        resultado < 0m
            ? $"{termos} = {moeda(baseCalculo)} ({motivoBaseZero})"
            : $"{termos} = {moeda(baseCalculo)}";
}
