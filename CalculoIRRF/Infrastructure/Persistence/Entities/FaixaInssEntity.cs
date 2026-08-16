namespace CalculoIRRF.Infrastructure.Persistence.Entities;

internal sealed class FaixaInssEntity
{
    public int Id { get; set; }
    public DateTime Competencia { get; set; }
    public int Faixa { get; set; }
    public double Valor { get; set; }
    public double Porcentagem { get; set; }
}
