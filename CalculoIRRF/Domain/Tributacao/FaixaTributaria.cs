namespace CalculoIRRF.Domain.Tributacao;

/// <summary>Representa uma faixa de tributação cujo limite é inclusivo.</summary>
public sealed record FaixaTributaria(int Numero, decimal Limite, decimal Aliquota, decimal Deducao = 0m)
{
    public decimal AliquotaDecimal => Aliquota / 100m;
}
