using System.Threading.Tasks;

namespace CalculoIRRF.Services.Calculo.Interface;

public interface IInssCalculo
{
    Task<double> NormalProgressivo();
    Task<string> DescricaoCalculoNormalProgressivo();
}
