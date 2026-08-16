namespace CalculoIRRF.Presentation.ViewModels;

public interface IPensaoViewModelFactory
{
    PensaoViewModel Criar(EntradaPensaoViewModel entrada);
}
