#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Presentation.Mvvm;
using CalculoIRRF.Presentation.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Globalization;
using System.Windows.Input;

namespace CalculoIRRF.Presentation.ViewModels.Calculadoras;

/// <param name="Provento">Valor formatado na coluna de proventos; vazio nas linhas de desconto.</param>
public sealed record LinhaDemonstrativoViewModel(string Descricao, string Referencia, string Provento, string Desconto);

/// <summary>Janela padrão das calculadoras: formulário, demonstrativo no formato de holerite, memória de cálculo e PDF.</summary>
public sealed class CalculadoraViewModel : ViewModelBase
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");
    private readonly ICalculadora _calculadora;
    private readonly IUserNotifier _notificador;
    private readonly IRelatorioPdfService _relatorioPdf;
    private readonly IArquivoDialogService _arquivoDialog;
    private DemonstrativoDto? _demonstrativo;
    private string _nomeArquivoPdf = string.Empty;
    private bool _temResultado;
    private string _referencia = string.Empty;
    private IReadOnlyList<IndicadorResumoViewModel> _destaques = [];
    private IReadOnlyList<LinhaDemonstrativoViewModel> _linhas = [];
    private IReadOnlyList<LinhaDemonstrativoViewModel> _informativos = [];
    private IReadOnlyList<SecaoMemoriaIrrfViewModel> _memoria = [];
    private IReadOnlyList<string> _observacoes = [];
    private string _totalProventos = string.Empty;
    private string _totalDescontos = string.Empty;
    private string _resultado = string.Empty;
    private string _rotuloProventos = "Proventos";
    private string _rotuloResultado = "Líquido a receber";

    public CalculadoraViewModel(ICalculadora calculadora, IUserNotifier notificador, IRelatorioPdfService relatorioPdf, IArquivoDialogService arquivoDialog)
    {
        _calculadora = calculadora;
        _notificador = notificador;
        _relatorioPdf = relatorioPdf;
        _arquivoDialog = arquivoDialog;
        Campos = calculadora.Campos;
        CalcularCommand = new AsyncRelayCommand(CalcularAsync);
        ExportarPdfCommand = new AsyncRelayCommand(ExportarPdfAsync, () => _demonstrativo is not null);
    }

    public string Titulo => _calculadora.Titulo;
    public string Descricao => _calculadora.Descricao;
    public string InstrucaoInicial => _calculadora.InstrucaoInicial;
    public IReadOnlyList<CampoViewModel> Campos { get; }
    public ICommand CalcularCommand { get; }
    public ICommand ExportarPdfCommand { get; }

    public bool TemResultado { get => _temResultado; private set => SetProperty(ref _temResultado, value); }
    public string Referencia { get => _referencia; private set => SetProperty(ref _referencia, value); }
    public IReadOnlyList<IndicadorResumoViewModel> Destaques { get => _destaques; private set => SetProperty(ref _destaques, value); }
    public IReadOnlyList<LinhaDemonstrativoViewModel> Linhas { get => _linhas; private set => SetProperty(ref _linhas, value); }
    public IReadOnlyList<LinhaDemonstrativoViewModel> Informativos { get => _informativos; private set { SetProperty(ref _informativos, value); OnPropertyChanged(nameof(TemInformativos)); } }
    public bool TemInformativos => Informativos.Count > 0;
    public IReadOnlyList<SecaoMemoriaIrrfViewModel> Memoria { get => _memoria; private set => SetProperty(ref _memoria, value); }
    public IReadOnlyList<string> Observacoes { get => _observacoes; private set { SetProperty(ref _observacoes, value); OnPropertyChanged(nameof(TemObservacoes)); } }
    public bool TemObservacoes => Observacoes.Count > 0;
    public string TotalProventos { get => _totalProventos; private set => SetProperty(ref _totalProventos, value); }
    public string TotalDescontos { get => _totalDescontos; private set { SetProperty(ref _totalDescontos, value); OnPropertyChanged(nameof(TemDescontos)); } }
    public bool TemDescontos => TotalDescontos.Length > 0;
    public string Resultado { get => _resultado; private set => SetProperty(ref _resultado, value); }
    public string RotuloProventos { get => _rotuloProventos; private set => SetProperty(ref _rotuloProventos, value); }
    public string RotuloResultado { get => _rotuloResultado; private set => SetProperty(ref _rotuloResultado, value); }

    private async Task CalcularAsync()
    {
        try
        {
            var demonstrativo = await _calculadora.CalcularAsync(CancellationToken.None);
            _nomeArquivoPdf = _calculadora.NomeArquivoPdf;
            Apresentar(demonstrativo);
        }
        catch (ArgumentException exception)
        {
            _notificador.MostrarAviso(MensagemUsuario.De(exception));
        }
        catch (Exception exception)
        {
            _notificador.MostrarErro($"Não foi possível concluir o cálculo de {Titulo.ToLower(Cultura)}.", exception);
        }
    }

    private void Apresentar(DemonstrativoDto demonstrativo)
    {
        _demonstrativo = demonstrativo;
        Referencia = demonstrativo.Referencia;
        Destaques = demonstrativo.Destaques.Select(destaque => new IndicadorResumoViewModel(destaque.Rotulo, destaque.Valor, destaque.Complemento)).ToArray();
        Linhas = demonstrativo.Proventos.Select(verba => new LinhaDemonstrativoViewModel(verba.Descricao, verba.Referencia, Moeda(verba.Valor), string.Empty))
            .Concat(demonstrativo.Descontos.Select(verba => new LinhaDemonstrativoViewModel(verba.Descricao, verba.Referencia, string.Empty, Moeda(verba.Valor))))
            .ToArray();
        Informativos = demonstrativo.Informativos.Select(verba => new LinhaDemonstrativoViewModel(verba.Descricao, verba.Referencia, Moeda(verba.Valor), string.Empty)).ToArray();
        Memoria = demonstrativo.Memoria.Select(grupo => new SecaoMemoriaIrrfViewModel(grupo.Titulo, grupo.Destaque, grupo.Formulas.Select(formula => new FormulaCalculoViewModel(formula.Titulo, formula.Formula)).ToArray())).ToArray();
        Observacoes = demonstrativo.Observacoes;
        RotuloProventos = demonstrativo.RotuloProventos;
        RotuloResultado = demonstrativo.RotuloResultado;
        TotalProventos = Moeda(demonstrativo.TotalProventos);
        TotalDescontos = demonstrativo.Descontos.Count > 0 ? Moeda(demonstrativo.TotalDescontos) : string.Empty;
        Resultado = Moeda(demonstrativo.Resultado);
        TemResultado = true;
        ((AsyncRelayCommand)ExportarPdfCommand).RaiseCanExecuteChanged();
    }

    private async Task ExportarPdfAsync()
    {
        if (_demonstrativo is null)
            return;

        var caminhoArquivo = _arquivoDialog.SolicitarDestinoPdf(_nomeArquivoPdf);
        if (caminhoArquivo is null)
            return;

        try
        {
            await _relatorioPdf.GerarDemonstrativoAsync(_demonstrativo, caminhoArquivo, CancellationToken.None);
        }
        catch (Exception exception)
        {
            _notificador.MostrarErro("Não foi possível gerar o relatório em PDF.", exception);
        }
    }

    private static string Moeda(decimal valor) => valor.ToString("C2", Cultura);
}

public interface ICalculadoraViewModelFactory
{
    CalculadoraViewModel Criar(TipoCalculadora tipo, ContextoCalculo contexto);
}

/// <summary>Cada calculadora é registrada na injeção de dependências com o seu <see cref="TipoCalculadora"/> como chave.</summary>
public sealed class CalculadoraViewModelFactory(IServiceProvider servicos, IUserNotifier notificador, IRelatorioPdfService relatorioPdf, IArquivoDialogService arquivoDialog) : ICalculadoraViewModelFactory
{
    public CalculadoraViewModel Criar(TipoCalculadora tipo, ContextoCalculo contexto)
    {
        var calculadora = servicos.GetRequiredKeyedService<ICalculadora>(tipo);
        calculadora.Preencher(contexto);
        return new CalculadoraViewModel(calculadora, notificador, relatorioPdf, arquivoDialog);
    }
}
