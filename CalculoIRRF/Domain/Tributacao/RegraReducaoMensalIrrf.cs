namespace CalculoIRRF.Domain.Tributacao;

/// <summary>Regra de redução aplicada ao IRRF já apurado pela tabela progressiva.</summary>
public sealed record RegraReducaoMensalIrrf(int Faixa, decimal LimiteRendimentos, decimal Multiplicador, decimal ValorBase);

public static class CalculadoraReducaoMensalIrrf
{
    public static decimal Calcular(
        decimal rendimentosTributaveis,
        decimal impostoApurado,
        IReadOnlyList<RegraReducaoMensalIrrf> regras)
    {
        if (rendimentosTributaveis < 0m || impostoApurado <= 0m || regras.Count == 0)
            return 0m;

        var regra = regras.OrderBy(item => item.Faixa)
            .FirstOrDefault(item => rendimentosTributaveis <= item.LimiteRendimentos);
        if (regra is null)
            return 0m;

        var reducaoCalculada = regra.Multiplicador == 0m
            ? regra.ValorBase
            : regra.ValorBase - regra.Multiplicador * rendimentosTributaveis;

        return Math.Min(impostoApurado, Math.Max(0m, Math.Round(reducaoCalculada, 2, MidpointRounding.AwayFromZero)));
    }
}
