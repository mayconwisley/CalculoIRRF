namespace CalculoIRRF.Domain.Tributacao;

public sealed record ResultadoFaixaTributaria(
    int Faixa,
    decimal BaseCalculada,
    decimal Aliquota,
    decimal Imposto);
