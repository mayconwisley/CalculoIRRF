using System;

namespace CalculoIRRF.Objetos
{
    public class Irrf
    {
        public int Id { get; set; }
        public DateTime Competencia { get; set; }
        public int Faixa { get; set; }
        public decimal Valor { get; set; }
        public decimal Porcentagem { get; set; }
        public decimal Deducao { get; set; }

    }
}
