using CalculoIRRF.Application.Management;
using CalculoIRRF.Presentation.Services;

namespace CalculoIRRF.Presentation.ViewModels;

public sealed class TabelaManutencaoViewModelFactory(
    ITabelaTributariaService service,
    IAtualizadorTabelas atualizador,
    IUserNotifier notificador) : ITabelaManutencaoViewModelFactory
{
    public TabelaManutencaoViewModel Criar(TipoTabelaTributaria tipo) => new(tipo, service, atualizador, notificador);
}
