using CalculoIRRF.Presentation.ViewModels;
using CalculoIRRF.Presentation.ViewModels.Calculadoras;

namespace CalculoIRRF.Presentation.Services;

public interface IWindowNavigator
{
    void AbrirTabelaInss();
    void AbrirTabelaIrrf();
    void AbrirSimplificado();
    void AbrirDependentes();
    void AbrirDescontoMinimo();
    void AbrirReducaoMensalIrrf();
    void AbrirPensao(EntradaPensaoViewModel entrada);
    void AbrirEstabilidade();
    void AbrirCalculadora(TipoCalculadora tipo, ContextoCalculo contexto);
}
