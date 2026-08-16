using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Presentation.Services;

namespace CalculoIRRF.Presentation.ViewModels;

public sealed class PensaoViewModelFactory(ISimularPensaoUseCase simulador, IUserNotifier notificador, IRelatorioPdfService relatorioPdf, IArquivoDialogService arquivoDialog) : IPensaoViewModelFactory
{
    public PensaoViewModel Criar(EntradaPensaoViewModel entrada) => new(entrada, simulador, notificador, relatorioPdf, arquivoDialog);
}
