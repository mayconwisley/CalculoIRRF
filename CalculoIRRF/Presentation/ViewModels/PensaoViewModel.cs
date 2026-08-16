using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Presentation.Mvvm;
using CalculoIRRF.Presentation.Services;
using System.Globalization;
using System.Text;
using System.Windows.Input;

namespace CalculoIRRF.Presentation.ViewModels;

public sealed class PensaoViewModel : ViewModelBase
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");
    private readonly EntradaPensaoViewModel _entrada;
    private readonly ISimularPensaoUseCase _simulador;
    private readonly IUserNotifier _notificador;
    private string _percentual = "0,00";
    private string _outrosDescontos = "0,00";
    private string _resultado = "Informe o percentual da pensão para calcular.";

    public PensaoViewModel(EntradaPensaoViewModel entrada, ISimularPensaoUseCase simulador, IUserNotifier notificador)
    {
        _entrada = entrada; _simulador = simulador; _notificador = notificador;
        CalcularResumoCommand = new AsyncRelayCommand(() => CalcularAsync(false));
        CalcularDetalheCommand = new AsyncRelayCommand(() => CalcularAsync(true));
    }
    public string Percentual { get => _percentual; set => SetProperty(ref _percentual, value); }
    public string OutrosDescontos { get => _outrosDescontos; set => SetProperty(ref _outrosDescontos, value); }
    public string ValorBruto => _entrada.ValorBruto.ToString("N2", Cultura);
    public string Resultado { get => _resultado; private set => SetProperty(ref _resultado, value); }
    public ICommand CalcularResumoCommand { get; }
    public ICommand CalcularDetalheCommand { get; }
    private async Task CalcularAsync(bool detalhar)
    {
        if (!decimal.TryParse(Percentual, NumberStyles.Number, Cultura, out var percentual) || !decimal.TryParse(OutrosDescontos, NumberStyles.Number, Cultura, out var descontos)) { _notificador.MostrarAviso("Informe valores monetários válidos."); return; }
        try
        {
            var resultado = await _simulador.ExecutarAsync(new SimularPensaoRequest(_entrada.Competencia, _entrada.ValorBruto, _entrada.BaseInss, _entrada.Dependentes, percentual, descontos), CancellationToken.None);
            Resultado = Formatar(resultado, detalhar);
        }
        catch (Exception ex) { _notificador.MostrarErro("Não foi possível calcular a pensão.", ex); }
    }
    private static string Formatar(SimulacaoPensaoDto resultado, bool detalhar)
    {
        var texto = new StringBuilder($"INSS: {resultado.ValorInss:N2}\n\n");
        foreach (var item in new[] { resultado.Normal, resultado.Simplificada })
        {
            texto.AppendLine($"{item.Nome}\nIRRF progressivo: {item.ImpostoAntesReducao:N2}\nRedução mensal: {item.ReducaoMensal:N2}\nIRRF final: {item.Imposto:N2}\nPensão: {item.Pensao:N2}\nTotal: {item.Total:N2}\nIterações: {item.Iteracoes}");
            if (detalhar)
            {
                texto.AppendLine();
                foreach (var detalhe in item.Detalhes)
                    texto.AppendLine($"{detalhe.Sequencia}º: Base IR {detalhe.BaseIrrf:N2} | {detalhe.Aliquota:N2}% | Dedução {detalhe.Deducao:N2}\n    IR progressivo {detalhe.ImpostoAntesReducao:N2} | Redução {detalhe.ReducaoMensal:N2} | IR final {detalhe.Imposto:N2}\n    Base pensão {detalhe.BasePensao:N2} | Pensão {detalhe.Pensao:N2}");
            }
            texto.AppendLine();
        }
        return texto.Append($"{resultado.MensagemVantagem}").ToString();
    }
}
