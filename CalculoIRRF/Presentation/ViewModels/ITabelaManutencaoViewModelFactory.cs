using CalculoIRRF.Application.Management;

namespace CalculoIRRF.Presentation.ViewModels;

public interface ITabelaManutencaoViewModelFactory
{
    TabelaManutencaoViewModel Criar(TipoTabelaTributaria tipo);
}
