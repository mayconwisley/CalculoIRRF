using System;

namespace CalculoIRRF.Model;

public class IrrfRfb
{
    public int Id { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime DataAtualizacao { get; set; }
    public int Sequencia { get; set; }
    public decimal BaseCaculo { get; set; }
    public decimal Aliquota { get; set; }
    public decimal Deducao { get; set; }
    public decimal Dependente { get; set; }
    public decimal Simplificado { get; set; }
}
