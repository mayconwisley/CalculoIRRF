#nullable enable

using CalculoIRRF.Application.DTOs;

namespace CalculoIRRF.Application.UseCases;

/// <summary>Grupos da memória de cálculo do INSS e do IRRF, comuns aos demonstrativos das calculadoras.</summary>
internal static class MemoriaTributaria
{
    /// <summary>Nome da verba do IRRF no demonstrativo, com a modalidade aplicada.</summary>
    public static string DescricaoIrrf(string verba, ApuracaoIrrf irrf) => $"{verba} ({(irrf.SimplificadaAplicada ? "desconto simplificado" : "deduções legais")})";

    /// <summary>Alíquota da faixa aplicada; vazia quando não há imposto, por exemplo pela redução mensal.</summary>
    public static string ReferenciaIrrf(ApuracaoIrrf irrf) => irrf.Imposto > 0m ? Formato.PercentualCurto(irrf.Aplicada.Aliquota) : string.Empty;

    public static GrupoMemoriaDto Inss(string titulo, ApuracaoInss inss, string rotuloBase)
    {
        var formulas = new List<FormulaDto>
        {
            new("Base de cálculo", inss.LimitadaAoTeto
                ? $"{Formato.Moeda(inss.BaseInformada)} ({rotuloBase}), limitada ao teto de {Formato.Moeda(inss.BaseConsiderada)}"
                : $"{Formato.Moeda(inss.BaseConsiderada)} ({rotuloBase})")
        };
        formulas.AddRange(inss.Detalhes.Select(faixa => new FormulaDto($"Faixa {faixa.Faixa}", $"{Formato.Moeda(faixa.BaseCalculada)} x {Formato.Percentual(faixa.Aliquota)} = {Formato.Moeda(faixa.Imposto)}")));
        if (inss.Detalhes.Count > 1)
            formulas.Add(new("Total", $"{string.Join(" + ", inss.Detalhes.Select(faixa => Formato.Moeda(faixa.Imposto)))} = {Formato.Moeda(inss.Valor)}"));
        return new GrupoMemoriaDto(titulo, $"INSS: {Formato.Moeda(inss.Valor)}", formulas);
    }

    /// <param name="rotuloRendimentos">Como o rendimento aparece na fórmula da base, por exemplo "férias + 1/3".</param>
    public static GrupoMemoriaDto Irrf(string titulo, ApuracaoIrrf irrf, string rotuloRendimentos)
    {
        var formulas = new List<FormulaDto>
        {
            new("Base com deduções legais", FormulaBaseIrrf.Normal(irrf.Rendimentos, rotuloRendimentos, irrf.Inss, irrf.Dependentes, irrf.DeducaoPorDependente, irrf.Normal.BaseCalculo, Formato.Moeda)),
            new("Imposto com deduções legais", Imposto(irrf.Normal))
        };
        if (irrf.DescontoSimplificado is not null)
        {
            formulas.Add(new("Base com desconto simplificado", FormulaBaseIrrf.Simplificada(irrf.Rendimentos, rotuloRendimentos, irrf.DescontoSimplificado, irrf.Simplificada.BaseCalculo, Formato.Moeda)));
            formulas.Add(new("Imposto com desconto simplificado", Imposto(irrf.Simplificada)));
        }
        formulas.Add(new("Modalidade aplicada", DescreverModalidade(irrf)));
        return new GrupoMemoriaDto(titulo, $"IRRF: {Formato.Moeda(irrf.Imposto)}", formulas);
    }

    private static string Imposto(ModalidadeIrrfDto modalidade) =>
        modalidade.Aliquota == 0m
            ? $"{Formato.Moeda(modalidade.BaseCalculo)} está na faixa isenta: {Formato.Moeda(0m)}"
            : modalidade.ReducaoMensal > 0m
            ? $"{Formato.Moeda(modalidade.BaseCalculo)} x {Formato.Percentual(modalidade.Aliquota)} - {Formato.Moeda(modalidade.Deducao)} = {Formato.Moeda(modalidade.ImpostoAntesReducao)}; menos a redução mensal de {Formato.Moeda(modalidade.ReducaoMensal)} = {Formato.Moeda(modalidade.Imposto)}"
            : $"{Formato.Moeda(modalidade.BaseCalculo)} x {Formato.Percentual(modalidade.Aliquota)} - {Formato.Moeda(modalidade.Deducao)} = {Formato.Moeda(modalidade.Imposto)}";

    private static string DescreverModalidade(ApuracaoIrrf irrf)
    {
        if (irrf.DescontoSimplificado is null)
            return "Deduções legais: o desconto simplificado só vale a partir de 05/2023.";
        if (irrf.Normal.Imposto == irrf.Simplificada.Imposto)
            return $"As duas modalidades resultam em {Formato.Moeda(irrf.Imposto)}.";
        return irrf.SimplificadaAplicada
            ? $"Desconto simplificado, que resulta em imposto menor ({Formato.Moeda(irrf.Simplificada.Imposto)} contra {Formato.Moeda(irrf.Normal.Imposto)})."
            : $"Deduções legais, que resultam em imposto menor ({Formato.Moeda(irrf.Normal.Imposto)} contra {Formato.Moeda(irrf.Simplificada.Imposto)}).";
    }
}
