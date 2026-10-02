#nullable enable

using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Presentation.Mvvm;
using CalculoIRRF.Presentation.Services;
using System.Globalization;
using System.Windows.Input;

namespace CalculoIRRF.Presentation.ViewModels;

public sealed class EstabilidadeViewModel : ViewModelBase
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");
    private readonly ISimularEstabilidadeUseCase _simulador;
    private readonly IUserNotifier _notificador;
    private readonly IRelatorioPdfService _relatorioPdf;
    private readonly IArquivoDialogService _arquivoDialog;
    private string _mediaRemuneratoria = "0,00";
    private string _diasBase = "30";
    private string _demissao = DateTime.Today.ToString("dd/MM/yyyy");
    private string _fimEstabilidade = DateTime.Today.AddDays(30).ToString("dd/MM/yyyy");
    private string _complementos = "0,00";
    private string _diasRestantes = "30 dias restantes";
    private bool _temResultado;
    private IReadOnlyList<IndicadorResumoViewModel> _indicadoresResumo = [];
    private IReadOnlyList<FormulaCalculoViewModel> _memoriaCalculo = [];
    private SimulacaoEstabilidadeDto? _ultimaSimulacao;
    private EntradaEstabilidadeDto? _ultimaEntrada;

    public EstabilidadeViewModel(ISimularEstabilidadeUseCase simulador, IUserNotifier notificador, IRelatorioPdfService relatorioPdf, IArquivoDialogService arquivoDialog)
    {
        _simulador = simulador;
        _notificador = notificador;
        _relatorioPdf = relatorioPdf;
        _arquivoDialog = arquivoDialog;
        CalcularCommand = new RelayCommand(_ => Calcular());
        ExportarPdfCommand = new AsyncRelayCommand(ExportarPdfAsync, () => _ultimaSimulacao is not null && _ultimaEntrada is not null);
    }

    public string MediaRemuneratoria { get => _mediaRemuneratoria; set => SetProperty(ref _mediaRemuneratoria, value); }
    public string DiasBase { get => _diasBase; set => SetProperty(ref _diasBase, value); }
    public string Demissao
    {
        get => _demissao;
        set
        {
            if (SetProperty(ref _demissao, value))
                AtualizarDiasRestantes();
        }
    }
    public string FimEstabilidade
    {
        get => _fimEstabilidade;
        set
        {
            if (SetProperty(ref _fimEstabilidade, value))
                AtualizarDiasRestantes();
        }
    }
    public string Complementos { get => _complementos; set => SetProperty(ref _complementos, value); }
    public string DiasRestantes { get => _diasRestantes; private set => SetProperty(ref _diasRestantes, value); }
    public bool TemResultado { get => _temResultado; private set => SetProperty(ref _temResultado, value); }
    public IReadOnlyList<IndicadorResumoViewModel> IndicadoresResumo { get => _indicadoresResumo; private set => SetProperty(ref _indicadoresResumo, value); }
    public IReadOnlyList<FormulaCalculoViewModel> MemoriaCalculo { get => _memoriaCalculo; private set => SetProperty(ref _memoriaCalculo, value); }
    public ICommand CalcularCommand { get; }
    public ICommand ExportarPdfCommand { get; }

    private void Calcular()
    {
        if (!TentarLerEntrada(out var entrada))
            return;

        try
        {
            var resultado = _simulador.Executar(entrada);
            _ultimaSimulacao = resultado;
            _ultimaEntrada = new EntradaEstabilidadeDto(entrada.MediaRemuneratoria, entrada.DiasBase, entrada.Demissao, entrada.FimEstabilidade, entrada.Complementos);
            DiasRestantes = $"{resultado.DiasEstabilidade:N0} dias restantes";
            IndicadoresResumo =
            [
                new("Indenização", Moeda(resultado.Indenizacao), $"{resultado.DiasEstabilidade:N0} dias proporcionais"),
                new("13º salário", Moeda(resultado.DecimoTerceiro), $"{resultado.Avos} avo(s)"),
                new("Férias + 1/3", Moeda(resultado.Ferias + resultado.TercoFerias), $"Férias: {Moeda(resultado.Ferias)}"),
                new("FGTS + multa", Moeda(resultado.FgtsOitoPorCento + resultado.MultaFgtsQuarentaPorCento), "8% + multa de 40%"),
                new("Complementos", Moeda(resultado.Complementos), "Ajuste adicional informado"),
                new("Total estimado", Moeda(resultado.Total), "Soma das verbas calculadas")
            ];
            MemoriaCalculo =
            [
                new("Indenização", $"{Moeda(entrada.MediaRemuneratoria)} ÷ {entrada.DiasBase} dias × {resultado.DiasEstabilidade:N0} dias = {Moeda(resultado.Indenizacao)}"),
                new("13º salário", $"{Moeda(entrada.MediaRemuneratoria)} ÷ 12 × {resultado.Avos} avo(s) = {Moeda(resultado.DecimoTerceiro)}"),
                new("Férias proporcionais", $"{Moeda(entrada.MediaRemuneratoria)} ÷ 12 × {resultado.Avos} avo(s) = {Moeda(resultado.Ferias)}"),
                new("Adicional de férias", $"{Moeda(resultado.Ferias)} ÷ 3 = {Moeda(resultado.TercoFerias)}"),
                new("FGTS", $"({Moeda(resultado.Indenizacao)} + {Moeda(resultado.DecimoTerceiro)}) × 8% = {Moeda(resultado.FgtsOitoPorCento)}"),
                new("Multa rescisória do FGTS", $"{Moeda(resultado.FgtsOitoPorCento)} × 40% = {Moeda(resultado.MultaFgtsQuarentaPorCento)}"),
                new("Total estimado", $"Verbas calculadas + complementos de {Moeda(resultado.Complementos)} = {Moeda(resultado.Total)}")
            ];
            TemResultado = true;
            ((AsyncRelayCommand)ExportarPdfCommand).RaiseCanExecuteChanged();
        }
        catch (ArgumentException exception)
        {
            _notificador.MostrarAviso(MensagemUsuario.De(exception));
        }
        catch (Exception exception)
        {
            _notificador.MostrarErro("Não foi possível concluir o cálculo de estabilidade.", exception);
        }
    }

    private bool TentarLerEntrada(out SimularEstabilidadeRequest entrada)
    {
        entrada = default!;
        if (!decimal.TryParse(MediaRemuneratoria, NumberStyles.Number, Cultura, out var media) ||
            !int.TryParse(DiasBase, out var diasBase) ||
            !DateOnly.TryParseExact(Demissao.Trim(), "dd/MM/yyyy", Cultura, DateTimeStyles.None, out var demissao) ||
            !DateOnly.TryParseExact(FimEstabilidade.Trim(), "dd/MM/yyyy", Cultura, DateTimeStyles.None, out var fimEstabilidade) ||
            !decimal.TryParse(Complementos, NumberStyles.Number, Cultura, out var complementos))
        {
            _notificador.MostrarAviso("Informe média, dias-base, datas (dd/MM/aaaa) e complementos em formatos válidos.");
            return false;
        }

        entrada = new SimularEstabilidadeRequest(media, diasBase, demissao, fimEstabilidade, complementos);
        return true;
    }

    private async Task ExportarPdfAsync()
    {
        if (_ultimaSimulacao is null || _ultimaEntrada is null)
            return;

        var caminhoArquivo = _arquivoDialog.SolicitarDestinoPdf($"demonstrativo-estabilidade-{_ultimaEntrada.Demissao:yyyy-MM-dd}.pdf");
        if (caminhoArquivo is null)
            return;

        try
        {
            await _relatorioPdf.GerarRelatorioEstabilidadeAsync(_ultimaSimulacao, _ultimaEntrada, caminhoArquivo, CancellationToken.None);
        }
        catch (Exception exception)
        {
            _notificador.MostrarErro("Não foi possível gerar o demonstrativo de estabilidade.", exception);
        }
    }

    private void AtualizarDiasRestantes()
    {
        if (DateOnly.TryParseExact(Demissao.Trim(), "dd/MM/yyyy", Cultura, DateTimeStyles.None, out var demissao) &&
            DateOnly.TryParseExact(FimEstabilidade.Trim(), "dd/MM/yyyy", Cultura, DateTimeStyles.None, out var fimEstabilidade))
        {
            var dias = fimEstabilidade.DayNumber - demissao.DayNumber;
            DiasRestantes = dias > 0 ? $"{dias:N0} dias restantes" : "A data final deve ser posterior à demissão";
            return;
        }

        DiasRestantes = "Informe as duas datas";
    }

    private static string Moeda(decimal valor) => valor.ToString("C2", Cultura);
}
