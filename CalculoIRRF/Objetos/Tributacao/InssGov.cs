using System;

namespace CalculoIRRF.Objetos.Tributacao
{
    public class InssGov
    {
        public int Id { get; set; }
        public DateTime DataCriacao { get; set; }
        public DateTime DataAtualizacao { get; set; }
        public int Sequencia { get; set; }
        public decimal BaseCaculo { get; set; }
        public decimal Aliquota { get; set; }
    }
}
