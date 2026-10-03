using CalculoIRRF.Application.Management;
using CalculoIRRF.Presentation.ViewModels;
using CalculoIRRF.Presentation.ViewModels.Calculadoras;

namespace CalculoIRRF.Presentation.Services;

public interface IWindowNavigator
{
    void AbrirTabela(TipoTabelaTributaria tipo);
    void AbrirPensao(EntradaPensaoViewModel entrada);
    void AbrirEstabilidade();
    void AbrirCalculadora(TipoCalculadora tipo, ContextoCalculo contexto);
}
