namespace CalculoIRRF.Services.Calculo;

public class FgtsCalculo(double baseInss)
{
    private readonly double _baseInss = baseInss;

    public double Normal8()
    {
        return _baseInss * 0.08d;
    }

    public double Normal2()
    {
        return _baseInss * 0.02d;
    }
}
