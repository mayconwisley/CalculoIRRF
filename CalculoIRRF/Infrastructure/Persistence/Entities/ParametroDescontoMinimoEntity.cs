namespace CalculoIRRF.Infrastructure.Persistence.Entities;

internal sealed class ParametroDescontoMinimoEntity
{
    public int Id { get; set; }
    public DateTime Competencia { get; set; }
    public double Valor { get; set; }
}
