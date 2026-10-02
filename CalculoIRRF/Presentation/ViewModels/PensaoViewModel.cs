#nullable enable

using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Presentation.Mvvm;
using CalculoIRRF.Presentation.Services;
using System.Globalization;
using System.Windows.Input;

namespace CalculoIRRF.Presentation.ViewModels;

public sealed class PensaoViewModel : ViewModelBase
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");
    private readonly EntradaPensaoViewModel _entrada;
    private readonly ISimularPensaoUseCase _simulador;
    private readonly IUserNotifier _notificador;
    private readonly IRelatorioPdfService _relatorioPdf;
    private readonly IArquivoDialogService _arquivoDialog;
    private string _percentual = "0,00";
    private string _outrosDescontos = "0,00";
    private SimulacaoPensaoDto? _ultimaSimulacao;
    private EntradaPensaoDto? _ultimaEntrada;
    private bool _ultimoCalculoDetalhado;
    private bool _temResultado;
    private bool _temMemoria;
    private string _mensagemMemoria = "Selecione Detalhar para exibir a memória de cálculo por iteração.";
    private IReadOnlyList<IndicadorResumoViewModel> _indicadoresResumo = [];
    private IReadOnlyList<ComparativoPensaoViewModel> _comparativoPensao = [];
    private IReadOnlyList<SecaoMemoriaPensaoViewModel> _memoriaPensao = [];

    public PensaoViewModel(EntradaPensaoViewModel entrada, ISimularPensaoUseCase simulador, IUserNotifier notificador, IRelatorioPdfService relatorioPdf, IArquivoDialogService arquivoDialog)
    {
        _entrada = entrada; _simulador = simulador; _notificador = notificador; _relatorioPdf = relatorioPdf; _arquivoDialog = arquivoDialog;
        CalcularResumoCommand = new AsyncRelayCommand(() => CalcularAsync(false));
        CalcularDetalheCommand = new AsyncRelayCommand(() => CalcularAsync(true));
        ExportarPdfCommand = new AsyncRelayCommand(ExportarPdfAsync, () => _ultimaSimulacao is not null && _ultimaEntrada is not null);
    }
    public string Percentual { get => _percentual; set => SetProperty(ref _percentual, value); }
    public string OutrosDescontos { get => _outrosDescontos; set => SetProperty(ref _outrosDescontos, value); }
    public string ValorBruto => _entrada.ValorBruto.ToString("N2", Cultura);
    public bool TemResultado { get => _temResultado; private set => SetProperty(ref _temResultado, value); }
    public bool TemMemoria { get => _temMemoria; private set => SetProperty(ref _temMemoria, value); }
    public string MensagemMemoria { get => _mensagemMemoria; private set => SetProperty(ref _mensagemMemoria, value); }
    public IReadOnlyList<IndicadorResumoViewModel> IndicadoresResumo { get => _indicadoresResumo; private set => SetProperty(ref _indicadoresResumo, value); }
    public IReadOnlyList<ComparativoPensaoViewModel> ComparativoPensao { get => _comparativoPensao; private set => SetProperty(ref _comparativoPensao, value); }
    public IReadOnlyList<SecaoMemoriaPensaoViewModel> MemoriaPensao { get => _memoriaPensao; private set => SetProperty(ref _memoriaPensao, value); }
    public ICommand CalcularResumoCommand { get; }
    public ICommand CalcularDetalheCommand { get; }
    public ICommand ExportarPdfCommand { get; }
    private async Task CalcularAsync(bool detalhar)
    {
        if (!decimal.TryParse(Percentual, NumberStyles.Number, Cultura, out var percentual) || !decimal.TryParse(OutrosDescontos, NumberStyles.Number, Cultura, out var descontos)) { _notificador.MostrarAviso("Informe valores monetários válidos."); return; }
        try
        {
            var resultado = await _simulador.ExecutarAsync(new SimularPensaoRequest(_entrada.Competencia, _entrada.ValorBruto, _entrada.BaseInss, _entrada.Dependentes, percentual, descontos), CancellationToken.None);
            _ultimaSimulacao = resultado;
            _ultimaEntrada = new EntradaPensaoDto(_entrada.Competencia, _entrada.ValorBruto, _entrada.BaseInss, _entrada.Dependentes, percentual, descontos);
            _ultimoCalculoDetalhado = detalhar;
            AtualizarApresentacao(resultado, _ultimaEntrada, detalhar);
            ((AsyncRelayCommand)ExportarPdfCommand).RaiseCanExecuteChanged();
        }
        catch (Exception ex) { _notificador.MostrarErro("Não foi possível calcular a pensão.", ex); }
    }

    private void AtualizarApresentacao(SimulacaoPensaoDto simulacao, EntradaPensaoDto entrada, bool detalhar)
    {
        IndicadoresResumo =
        [
            new("Rendimentos", Moeda(entrada.ValorBruto), $"Competência {entrada.Competencia:MM/yyyy}"),
            new("INSS", Moeda(simulacao.ValorInss), $"Base: {Moeda(entrada.BaseInss)}"),
            new("Pensão normal", Moeda(simulacao.Normal.Pensao), $"Total: {Moeda(simulacao.Normal.Total)}"),
            new("Pensão simplificada", Moeda(simulacao.Simplificada.Pensao), $"Total: {Moeda(simulacao.Simplificada.Total)}")
        ];
        ComparativoPensao =
        [
            CriarComparativo(simulacao.Normal),
            CriarComparativo(simulacao.Simplificada)
        ];
        MemoriaPensao = detalhar
            ? [CriarMemoria(simulacao.Normal, entrada, simulacao.ValorInss), CriarMemoria(simulacao.Simplificada, entrada, simulacao.ValorInss)]
            : [];
        TemResultado = true;
        TemMemoria = detalhar;
        MensagemMemoria = detalhar ? string.Empty : "Selecione Detalhar para exibir a memória de cálculo por iteração.";
    }

    private static ComparativoPensaoViewModel CriarComparativo(ModalidadePensaoDto modalidade) =>
        new(modalidade.Nome, Moeda(modalidade.Imposto), Moeda(modalidade.Pensao), Moeda(modalidade.Total));

    private static SecaoMemoriaPensaoViewModel CriarMemoria(ModalidadePensaoDto modalidade, EntradaPensaoDto entrada, decimal valorInss)
    {
        var rendimentosTributaveis = entrada.ValorBruto - entrada.OutrosDescontos;
        var baseIrrfInicial = modalidade.Detalhes.Count == 0 ? 0m : modalidade.Detalhes[0].BaseIrrf;
        var iteracoes = new List<IteracaoMemoriaPensaoViewModel>();

        for (var indice = 0; indice < modalidade.Detalhes.Count; indice++)
        {
            var detalhe = modalidade.Detalhes[indice];
            var pensaoAnterior = indice == 0 ? 0m : modalidade.Detalhes[indice - 1].Pensao;
            var origemBaseIrrf = indice == 0 ? rendimentosTributaveis : baseIrrfInicial;
            var deducaoBaseIrrf = indice == 0 ? rendimentosTributaveis - baseIrrfInicial : pensaoAnterior;
            var tituloBaseIrrf = indice == 0 ? "Rendimentos tributáveis - deduções do modelo" : "Base inicial de IR - pensão anterior";
            iteracoes.Add(new IteracaoMemoriaPensaoViewModel($"Iteração {detalhe.Sequencia}",
            [
                new(tituloBaseIrrf, $"{Moeda(origemBaseIrrf)} - {Moeda(deducaoBaseIrrf)} = {Moeda(detalhe.BaseIrrf)}"),
                new("IR progressivo", $"{Moeda(detalhe.BaseIrrf)} x {FormatarPercentual(detalhe.Aliquota)} - {Moeda(detalhe.Deducao)} = {Moeda(detalhe.ImpostoAntesReducao)}"),
                new("IRRF após redução mensal", $"{Moeda(detalhe.ImpostoAntesReducao)} - {Moeda(detalhe.ReducaoMensal)} = {Moeda(detalhe.Imposto)}"),
                new("Base da pensão", $"{Moeda(rendimentosTributaveis)} - {Moeda(valorInss)} - {Moeda(detalhe.Imposto)} = {Moeda(detalhe.BasePensao)}"),
                new("Pensão calculada", $"{Moeda(detalhe.BasePensao)} x {FormatarPercentual(entrada.Percentual)} = {Moeda(detalhe.Pensao)}")
            ]));
        }

        return new SecaoMemoriaPensaoViewModel($"{modalidade.Nome} - memória de cálculo", $"Iterações: {modalidade.Iteracoes} | Total: {Moeda(modalidade.Total)}", iteracoes);
    }

    private static string Moeda(decimal valor) => valor.ToString("C2", Cultura);
    private static string FormatarPercentual(decimal valor) => valor.ToString("N2", Cultura) + "%";

    private async Task ExportarPdfAsync()
    {
        if (_ultimaSimulacao is null || _ultimaEntrada is null)
            return;

        var caminhoArquivo = _arquivoDialog.SolicitarDestinoPdf($"relatorio-pensao-alimenticia-{_ultimaEntrada.Competencia:MM-yyyy}.pdf");
        if (caminhoArquivo is null)
            return;

        try
        {
            await _relatorioPdf.GerarRelatorioPensaoAsync(_ultimaSimulacao, _ultimaEntrada, _ultimoCalculoDetalhado, caminhoArquivo, CancellationToken.None);
        }
        catch (Exception exception)
        {
            _notificador.MostrarErro("Não foi possível gerar o relatório em PDF.", exception);
        }
    }
}
