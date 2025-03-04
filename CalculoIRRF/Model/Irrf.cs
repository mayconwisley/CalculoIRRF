using System;

namespace CalculoIRRF.Model;

public class Irrf
{
    public int Id { get; set; }
    public DateTime Competencia { get; set; }
    public int Faixa { get; set; }
    public double Valor { get; set; }
    public double Porcentagem { get; set; }
    public double Deducao { get; set; }

}
