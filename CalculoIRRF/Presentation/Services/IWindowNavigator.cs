using CalculoIRRF.Presentation.ViewModels;

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
}
