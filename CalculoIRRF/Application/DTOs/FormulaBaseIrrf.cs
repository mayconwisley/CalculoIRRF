#nullable enable

namespace CalculoIRRF.Application.DTOs;

/// <summary>
/// Linha da memória de cálculo que mostra como a base do IRRF foi obtida, com cada dedução identificada.
/// A tela e o relatório PDF usam o mesmo texto, para explicarem a base da mesma forma.
/// </summary>
public static class FormulaBaseIrrf
{
    public static string Normal(SimulacaoImpostoDto simulacao, Func<decimal, string> moeda)
    {
        var dependentes = simulacao.Entrada.QuantidadeDependentes;
        var termos = $"{moeda(simulacao.Entrada.ValorBruto)} (valor bruto) - {moeda(simulacao.ValorInss)} (INSS)";
        if (dependentes > 0)
            termos += $" - {dependentes} x {moeda(simulacao.DeducaoPorDependente)} ({(dependentes == 1 ? "dependente" : "dependentes")})";

        var resultado = simulacao.Entrada.ValorBruto - simulacao.ValorInss - dependentes * simulacao.DeducaoPorDependente;
        return Concluir(termos, resultado, simulacao.Normal.BaseCalculo, "as deduções superam o valor bruto", moeda);
    }

    public static string Simplificada(SimulacaoImpostoDto simulacao, Func<decimal, string> moeda)
    {
        if (simulacao.DescontoSimplificado is not { } desconto)
            return "Não se aplica: o desconto simplificado vale a partir de 05/2023.";

        var termos = $"{moeda(simulacao.Entrada.ValorBruto)} (valor bruto) - {moeda(desconto)} (desconto simplificado)";
        return Concluir(termos, simulacao.Entrada.ValorBruto - desconto, simulacao.Simplificada.BaseCalculo, "o desconto supera o valor bruto", moeda);
    }

    // A base não fica negativa: quando as deduções superam o valor bruto, ela é zero.
    private static string Concluir(string termos, decimal resultado, decimal baseCalculo, string motivoBaseZero, Func<decimal, string> moeda) =>
        resultado < 0m
            ? $"{termos} = {moeda(baseCalculo)} ({motivoBaseZero})"
            : $"{termos} = {moeda(baseCalculo)}";
}
