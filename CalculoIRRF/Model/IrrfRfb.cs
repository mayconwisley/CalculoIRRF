using System;

namespace CalculoIRRF.Model;

public class IrrfRfb
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public int Sequencia { get; set; }
    public double BaseCaculo { get; set; }
    public double Aliquota { get; set; }
    public double Deducao { get; set; }
    public double Dependente { get; set; }
    public double Simplificado { get; set; }
}
