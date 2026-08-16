namespace CalculoIRRF.Infrastructure.Persistence.Entities;

/// <summary>Faixa da redução mensal do imposto prevista para uma competência.</summary>
internal sealed class ReducaoIrrfMensalEntity
{
    public int Id { get; set; }
    public DateTime Competencia { get; set; }
    public int Faixa { get; set; }
    public double LimiteRendimentos { get; set; }
    public double Multiplicador { get; set; }
    public double ValorBase { get; set; }
}
