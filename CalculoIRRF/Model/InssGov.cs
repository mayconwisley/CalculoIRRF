using System;

namespace CalculoIRRF.Model;

public class InssGov
{
	public int Id { get; set; }
	public DateTime Vigencia { get; set; }
	public int Sequencia { get; set; }
	public double BaseCaculo { get; set; }
	public double Aliquota { get; set; }
}
