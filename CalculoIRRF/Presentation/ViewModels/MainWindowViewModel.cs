#nullable enable

using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Presentation.Mvvm;
using CalculoIRRF.Presentation.Services;
using System.Globalization;
using System.Reflection;
using System.Runtime.Versioning;
using System.Windows.Input;

namespace CalculoIRRF.Presentation.ViewModels;

[SupportedOSPlatform("windows")]
public sealed class MainWindowViewModel : ViewModelBase
{
    private static readonly CultureInfo CulturaPtBr = CultureInfo.GetCultureInfo("pt-BR");
    private readonly ISimularImpostoUseCase _simularImposto;
    private readonly IUserNotifier _notificador;
    private readonly IWindowNavigator _navegador;
    private readonly IRelatorioPdfService _relatorioPdf;
    private readonly IArquivoDialogService _arquivoDialog;
    private string _competencia = DateTime.Today.ToString("MM/yyyy");
    private string _valorBruto = "0,00";
    private string _baseInss = "0,00";
    private string _dependentes = "0";
    private bool _podeCalcularPensao;
    private ThemeMode _temaSelecionado = ThemeManager.CurrentMode;
    private SimulacaoImpostoDto? _ultimaSimulacao;
    private bool _temResultado;
    private IReadOnlyList<IndicadorResumoViewModel> _indicadoresResumo = [];
    private IReadOnlyList<ComparativoIrrfViewModel> _comparativoIrrf = [];
    private IReadOnlyList<SecaoMemoriaIrrfViewModel> _memoriaIrrf = [];
    private IReadOnlyList<SecaoMemoriaTributariaViewModel> _memoriaTributaria = [];

    public MainWindowViewModel(ISimularImpostoUseCase simularImposto, IUserNotifier notificador, IWindowNavigator navegador, IRelatorioPdfService relatorioPdf, IArquivoDialogService arquivoDialog)
    {
        _simularImposto = simularImposto;
        _notificador = notificador;
        _navegador = navegador;
        _relatorioPdf = relatorioPdf;
        _arquivoDialog = arquivoDialog;
        CalcularCommand = new AsyncRelayCommand(CalcularAsync);
        ExportarPdfCommand = new AsyncRelayCommand(ExportarPdfAsync, () => _ultimaSimulacao is not null);
        AtualizarBaseInssCommand = new AsyncRelayCommand(AtualizarBaseInssAsync);
        AbrirTabelaInssCommand = new RelayCommand(_ => _navegador.AbrirTabelaInss());
        AbrirTabelaIrrfCommand = new RelayCommand(_ => _navegador.AbrirTabelaIrrf());
        AbrirSimplificadoCommand = new RelayCommand(_ => _navegador.AbrirSimplificado());
        AbrirDependentesCommand = new RelayCommand(_ => _navegador.AbrirDependentes());
        AbrirDescontoMinimoCommand = new RelayCommand(_ => _navegador.AbrirDescontoMinimo());
        AbrirReducaoMensalIrrfCommand = new RelayCommand(_ => _navegador.AbrirReducaoMensalIrrf());
        AbrirPensaoCommand = new RelayCommand(_ => AbrirPensao(), _ => PodeCalcularPensao);
        AbrirEstabilidadeCommand = new RelayCommand(_ => _navegador.AbrirEstabilidade());
    }

    public IReadOnlyList<ThemeMode> Temas { get; } = [ThemeMode.Automatico, ThemeMode.Claro, ThemeMode.Escuro];
    public string Versao { get; } = ObterVersao();
    public string Competencia { get => _competencia; set => SetProperty(ref _competencia, value); }
    public string ValorBruto
    {
        get => _valorBruto;
        set
        {
            if (SetProperty(ref _valorBruto, value) && TentarLerDecimal(value, out var valorBruto) && valorBruto >= 0m)
                BaseInss = valorBruto.ToString("N2", CulturaPtBr);
        }
    }
    public string BaseInss { get => _baseInss; set => SetProperty(ref _baseInss, value); }
    public string Dependentes { get => _dependentes; set => SetProperty(ref _dependentes, value); }
    public bool TemResultado { get => _temResultado; private set => SetProperty(ref _temResultado, value); }
    public IReadOnlyList<IndicadorResumoViewModel> IndicadoresResumo { get => _indicadoresResumo; private set => SetProperty(ref _indicadoresResumo, value); }
    public IReadOnlyList<ComparativoIrrfViewModel> ComparativoIrrf { get => _comparativoIrrf; private set => SetProperty(ref _comparativoIrrf, value); }
    public IReadOnlyList<SecaoMemoriaIrrfViewModel> MemoriaIrrf { get => _memoriaIrrf; private set => SetProperty(ref _memoriaIrrf, value); }
    public IReadOnlyList<SecaoMemoriaTributariaViewModel> MemoriaTributaria { get => _memoriaTributaria; private set => SetProperty(ref _memoriaTributaria, value); }
    public bool PodeCalcularPensao { get => _podeCalcularPensao; private set { if (SetProperty(ref _podeCalcularPensao, value)) ((RelayCommand)AbrirPensaoCommand).RaiseCanExecuteChanged(); } }
    public ThemeMode TemaSelecionado
    {
        get => _temaSelecionado;
        set
        {
            if (SetProperty(ref _temaSelecionado, value))
                ThemeManager.Apply(value);
        }
    }

    public ICommand CalcularCommand { get; }
    public ICommand ExportarPdfCommand { get; }
    public ICommand AtualizarBaseInssCommand { get; }
    public ICommand AbrirTabelaInssCommand { get; }
    public ICommand AbrirTabelaIrrfCommand { get; }
    public ICommand AbrirSimplificadoCommand { get; }
    public ICommand AbrirDependentesCommand { get; }
    public ICommand AbrirDescontoMinimoCommand { get; }
    public ICommand AbrirReducaoMensalIrrfCommand { get; }
    public ICommand AbrirPensaoCommand { get; }
    public ICommand AbrirEstabilidadeCommand { get; }

    private async Task CalcularAsync()
    {
        if (!TentarLerEntrada(out var entrada))
            return;

        try
        {
            var resultado = await _simularImposto.ExecutarAsync(entrada, CancellationToken.None);
            _ultimaSimulacao = resultado;
            AtualizarApresentacao(resultado);
            PodeCalcularPensao = true;
            ((AsyncRelayCommand)ExportarPdfCommand).RaiseCanExecuteChanged();
        }
        catch (Exception exception)
        {
            _notificador.MostrarErro("Não foi possível concluir o cálculo.", exception);
        }
    }

    private async Task ExportarPdfAsync()
    {
        if (_ultimaSimulacao is null)
            return;

        var caminhoArquivo = _arquivoDialog.SolicitarDestinoPdf($"relatorio-simulacao-tributaria-{_ultimaSimulacao.Entrada.Competencia:MM-yyyy}.pdf");
        if (caminhoArquivo is null)
            return;

        try
        {
            await _relatorioPdf.GerarRelatorioImpostoAsync(_ultimaSimulacao, caminhoArquivo, CancellationToken.None);
        }
        catch (Exception exception)
        {
            _notificador.MostrarErro("Não foi possível gerar o relatório em PDF.", exception);
        }
    }

    private Task AtualizarBaseInssAsync()
    {
        if (!TentarLerCompetencia(out _) || !TentarLerDecimal(ValorBruto, out var valorBruto))
            return Task.CompletedTask;

        BaseInss = valorBruto.ToString("N2", CulturaPtBr);
        return Task.CompletedTask;
    }

    private void AbrirPensao()
    {
        if (!TentarLerEntrada(out var entrada))
            return;

        _navegador.AbrirPensao(new EntradaPensaoViewModel(entrada.Competencia, entrada.ValorBruto, entrada.BaseInss, entrada.QuantidadeDependentes));
    }

    private bool TentarLerEntrada(out SimularImpostoRequest entrada)
    {
        entrada = default!;
        if (!TentarLerCompetencia(out var competencia) || !TentarLerDecimal(ValorBruto, out var valorBruto) || !TentarLerDecimal(BaseInss, out var baseInss) || !int.TryParse(Dependentes, out var dependentes) || valorBruto < 0m || baseInss < 0m || dependentes < 0)
        {
            _notificador.MostrarAviso("Informe uma competência válida (MM/AAAA), valores monetários válidos e dependentes maior ou igual a zero.");
            return false;
        }

        entrada = new SimularImpostoRequest(competencia, valorBruto, baseInss, dependentes);
        return true;
    }

    private bool TentarLerCompetencia(out DateOnly competencia)
    {
        competencia = default;
        if (!DateTime.TryParseExact(Competencia.Trim(), "MM/yyyy", CulturaPtBr, DateTimeStyles.None, out var data))
            return false;

        competencia = DateOnly.FromDateTime(data);
        return true;
    }

    private static bool TentarLerDecimal(string valor, out decimal resultado) => decimal.TryParse(valor, NumberStyles.Number, CulturaPtBr, out resultado);

    private void AtualizarApresentacao(SimulacaoImpostoDto simulacao)
    {
        IndicadoresResumo =
        [
            new("Valor bruto", Moeda(simulacao.Entrada.ValorBruto), $"Competência {simulacao.Entrada.Competencia:MM/yyyy}"),
            new("INSS", Moeda(simulacao.ValorInss), $"Base: {Moeda(simulacao.BaseInssConsiderada)}"),
            new("IRRF normal", Moeda(simulacao.Normal.Imposto), $"Alíquota efetiva: {Percentual(simulacao.Normal.AliquotaEfetiva)}"),
            new("IRRF simplificado", Moeda(simulacao.Simplificada.Imposto), $"Alíquota efetiva: {Percentual(simulacao.Simplificada.AliquotaEfetiva)}"),
            new("FGTS padrão", Moeda(simulacao.FgtsOitoPorCento), "Alíquota de 8%"),
            new("FGTS Jovem Aprendiz", Moeda(simulacao.FgtsDoisPorCento), "Alíquota de 2%")
        ];

        ComparativoIrrf =
        [
            CriarComparativo(simulacao.Normal, simulacao.ModalidadeMaisVantajosa),
            CriarComparativo(simulacao.Simplificada, simulacao.ModalidadeMaisVantajosa)
        ];

        MemoriaIrrf =
        [
            CriarMemoriaIrrf(simulacao.Normal, FormulaBaseIrrf.Normal(simulacao, Moeda)),
            CriarMemoriaIrrf(simulacao.Simplificada, FormulaBaseIrrf.Simplificada(simulacao, Moeda))
        ];

        MemoriaTributaria =
        [
            CriarSecaoMemoria("INSS progressivo", simulacao.DetalhesInss),
            CriarSecaoMemoria("IRRF normal - cálculo progressivo", simulacao.Normal.DetalhesProgressivos),
            CriarSecaoMemoria("IRRF simplificado - cálculo progressivo", simulacao.Simplificada.DetalhesProgressivos)
        ];
        TemResultado = true;
    }

    private static ComparativoIrrfViewModel CriarComparativo(ModalidadeIrrfDto modalidade, string? modalidadeMaisVantajosa) =>
        new(
            modalidade.Nome,
            Moeda(modalidade.BaseCalculo),
            Moeda(modalidade.ReducaoMensal),
            Moeda(modalidade.Imposto),
            modalidade.Nome.Equals(modalidadeMaisVantajosa, StringComparison.OrdinalIgnoreCase));

    private static SecaoMemoriaTributariaViewModel CriarSecaoMemoria(string titulo, IReadOnlyList<DetalheFaixaDto> detalhes) =>
        new(titulo, Moeda(detalhes.Sum(detalhe => detalhe.Imposto)), detalhes.Select(detalhe => new LinhaFaixaTributariaViewModel($"Faixa {detalhe.Faixa}", Moeda(detalhe.BaseCalculada), Percentual(detalhe.Aliquota), Moeda(detalhe.Imposto))).ToArray());

    private static SecaoMemoriaIrrfViewModel CriarMemoriaIrrf(ModalidadeIrrfDto modalidade, string formulaBase) =>
        new($"IRRF {modalidade.Nome}", $"IRRF final: {Moeda(modalidade.Imposto)}",
        [
            new("Base de cálculo", formulaBase),
            new("IR progressivo", $"{Moeda(modalidade.BaseCalculo)} x {Percentual(modalidade.Aliquota)} - {Moeda(modalidade.Deducao)} = {Moeda(modalidade.ImpostoAntesReducao)}"),
            new("Redução mensal", $"{Moeda(modalidade.ImpostoAntesReducao)} - {Moeda(modalidade.ReducaoMensal)} = {Moeda(modalidade.Imposto)}")
        ]);

    private static string Moeda(decimal valor) => valor.ToString("C2", CulturaPtBr);
    private static string Percentual(decimal valor) => valor.ToString("N2", CulturaPtBr) + "%";

    // A versão vem da tag usada na publicação; o SDK acrescenta "+<commit>" à versão informativa, que não interessa ao usuário.
    private static string ObterVersao()
    {
        var versao = typeof(MainWindowViewModel).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0";
        var indiceCommit = versao.IndexOf('+');
        return indiceCommit >= 0 ? versao[..indiceCommit] : versao;
    }
}
