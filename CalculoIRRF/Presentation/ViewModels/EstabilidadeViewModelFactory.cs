using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Presentation.Services;

namespace CalculoIRRF.Presentation.ViewModels;

public sealed class EstabilidadeViewModelFactory(ISimularEstabilidadeUseCase simulador, IUserNotifier notificador, IRelatorioPdfService relatorioPdf, IArquivoDialogService arquivoDialog) : IEstabilidadeViewModelFactory
{
    public EstabilidadeViewModel Criar() => new(simulador, notificador, relatorioPdf, arquivoDialog);
}
