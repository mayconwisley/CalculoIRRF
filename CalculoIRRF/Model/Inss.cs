using System;

namespace CalculoIRRF.Model;

public class Inss
{
    public int Id { get; set; }
    public DateTime Competencia { get; set; }
    public int Faixa { get; set; }
    public decimal Valor { get; set; }
    public decimal Porcentagem { get; set; }
    public decimal Teto { get; set; }

}
