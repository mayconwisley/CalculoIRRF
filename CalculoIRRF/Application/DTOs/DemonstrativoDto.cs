namespace CalculoIRRF.Application.DTOs;

/// <param name="Referencia">Quantidade que originou o valor, como "15 dias" ou "7/12"; vazio quando não se aplica.</param>
public sealed record VerbaDto(string Descricao, string Referencia, decimal Valor);

public sealed record DestaqueDto(string Rotulo, string Valor, string Complemento);

public sealed record FormulaDto(string Titulo, string Formula);

public sealed record GrupoMemoriaDto(string Titulo, string Destaque, IReadOnlyList<FormulaDto> Formulas);

/// <summary>
/// Resultado das calculadoras no formato de holerite: proventos, descontos e o resultado (o líquido, ou o custo total),
/// além de valores informativos que não são pagos ao trabalhador, como o FGTS, e da memória de cálculo passo a passo.
/// </summary>
/// <param name="Referencia">Período ou data do cálculo, exibido abaixo do título.</param>
/// <param name="Observacoes">Premissas e limites do cálculo que o usuário precisa conhecer.</param>
public sealed record DemonstrativoDto(
    string Titulo,
    string Referencia,
    IReadOnlyList<DestaqueDto> Destaques,
    IReadOnlyList<VerbaDto> Proventos,
    IReadOnlyList<VerbaDto> Descontos,
    IReadOnlyList<VerbaDto> Informativos,
    IReadOnlyList<GrupoMemoriaDto> Memoria,
    IReadOnlyList<string> Observacoes,
    string RotuloProventos = "Proventos",
    string RotuloResultado = "Líquido a receber")
{
    public decimal TotalProventos => Proventos.Sum(verba => verba.Valor);
    public decimal TotalDescontos => Descontos.Sum(verba => verba.Valor);
    public decimal Resultado => TotalProventos - TotalDescontos;
}
