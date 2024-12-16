using System;

namespace CalculoIRRF.Objetos.Tributacao
{
    public class IrrfRfb
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public decimal BaseCaculo { get; set; }
        public decimal Aliquota { get; set; }
        public decimal Deducao { get; set; }
        public decimal Dependente { get; set; }
        public decimal Simplificado { get; set; }
    }
}
