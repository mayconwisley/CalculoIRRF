using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Presentation.Services;

namespace CalculoIRRF.Presentation.ViewModels;

public sealed class PensaoViewModelFactory(ISimularPensaoUseCase simulador, IUserNotifier notificador) : IPensaoViewModelFactory
{
    public PensaoViewModel Criar(EntradaPensaoViewModel entrada) => new(entrada, simulador, notificador);
}
