using System.Threading.Tasks;

namespace CalculoIRRF.Services.Calculo.Interface;

public interface IIrrfCalculo
{
    Task<double> BaseIrrfNormal();
    Task<double> BaseIrrfSimplificado();
    Task<double> Normal(double valorPensao = 0);
    Task<string> DescricaoCalculoNormal();
    Task<double> NormalProgressivo();
    Task<string> DescricaoCalculoNormalProgrssivo();
    Task<string> DescricaoCalculoSimplificadoProgrssivo();
    Task<double> Simplificado(double valorPensao = 0);
    Task<string> DescricaoCalculoSimplificado();
    Task<string> DescricaoVantagem();
}
