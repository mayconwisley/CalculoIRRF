namespace CalculoIRRF.Services.Calculo;

public class FgtsCalculo(decimal baseInss)
{
    private readonly decimal _baseInss = baseInss;

    public decimal Normal8()
    {
        return _baseInss * 0.08m;
    }

    public decimal Normal2()
    {
        return _baseInss * 0.02m;
    }
}
