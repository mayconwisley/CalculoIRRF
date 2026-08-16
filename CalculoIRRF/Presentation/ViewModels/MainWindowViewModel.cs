using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Application.UseCases;
using CalculoIRRF.Presentation.Mvvm;
using CalculoIRRF.Presentation.Services;
using System.Globalization;
using System.Runtime.Versioning;
using System.Text;
using System.Windows.Input;

namespace CalculoIRRF.Presentation.ViewModels;

[SupportedOSPlatform("windows")]
public sealed class MainWindowViewModel : ViewModelBase
{
    private static readonly CultureInfo CulturaPtBr = CultureInfo.GetCultureInfo("pt-BR");
    private readonly ISimularImpostoUseCase _simularImposto;
    private readonly IUserNotifier _notificador;
    private readonly IWindowNavigator _navegador;
    private string _competencia = DateTime.Today.ToString("MM/yyyy");
    private string _valorBruto = "0,00";
    private string _baseInss = "0,00";
    private string _dependentes = "0";
    private string _resultado = "Informe os valores e selecione Calcular.";
    private bool _podeCalcularPensao;
    private ThemeMode _temaSelecionado = ThemeManager.CurrentMode;

    public MainWindowViewModel(ISimularImpostoUseCase simularImposto, IUserNotifier notificador, IWindowNavigator navegador)
    {
        _simularImposto = simularImposto;
        _notificador = notificador;
        _navegador = navegador;
        CalcularCommand = new AsyncRelayCommand(CalcularAsync);
        AtualizarBaseInssCommand = new AsyncRelayCommand(AtualizarBaseInssAsync);
        AbrirTabelaInssCommand = new RelayCommand(_ => _navegador.AbrirTabelaInss());
        AbrirTabelaIrrfCommand = new RelayCommand(_ => _navegador.AbrirTabelaIrrf());
        AbrirSimplificadoCommand = new RelayCommand(_ => _navegador.AbrirSimplificado());
        AbrirDependentesCommand = new RelayCommand(_ => _navegador.AbrirDependentes());
        AbrirDescontoMinimoCommand = new RelayCommand(_ => _navegador.AbrirDescontoMinimo());
        AbrirReducaoMensalIrrfCommand = new RelayCommand(_ => _navegador.AbrirReducaoMensalIrrf());
        AbrirPensaoCommand = new RelayCommand(_ => AbrirPensao(), _ => PodeCalcularPensao);
    }

    public IReadOnlyList<ThemeMode> Temas { get; } = [ThemeMode.Automatico, ThemeMode.Claro, ThemeMode.Escuro];
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
    public string Resultado { get => _resultado; private set => SetProperty(ref _resultado, value); }
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
    public ICommand AtualizarBaseInssCommand { get; }
    public ICommand AbrirTabelaInssCommand { get; }
    public ICommand AbrirTabelaIrrfCommand { get; }
    public ICommand AbrirSimplificadoCommand { get; }
    public ICommand AbrirDependentesCommand { get; }
    public ICommand AbrirDescontoMinimoCommand { get; }
    public ICommand AbrirReducaoMensalIrrfCommand { get; }
    public ICommand AbrirPensaoCommand { get; }

    private async Task CalcularAsync()
    {
        if (!TentarLerEntrada(out var entrada))
            return;

        try
        {
            var resultado = await _simularImposto.ExecutarAsync(entrada, CancellationToken.None);
            Resultado = Formatar(resultado);
            PodeCalcularPensao = true;
        }
        catch (Exception exception)
        {
            _notificador.MostrarErro("Não foi possível concluir o cálculo.", exception);
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

    private static string Formatar(SimulacaoImpostoDto simulacao)
    {
        var texto = new StringBuilder();
        texto.AppendLine("INFORMAÇÕES DO CÁLCULO").AppendLine();
        texto.AppendLine($"Valor bruto: {simulacao.Entrada.ValorBruto:N2}");
        texto.AppendLine($"Base INSS considerada: {simulacao.BaseInssConsiderada:N2}");
        texto.AppendLine($"Valor INSS: {simulacao.ValorInss:N2}").AppendLine();
        AdicionarModalidade(texto, simulacao.Normal, simulacao.Entrada.QuantidadeDependentes, simulacao.Entrada.ValorBruto);
        AdicionarModalidade(texto, simulacao.Simplificada, 0, simulacao.Entrada.ValorBruto);
        texto.AppendLine($"Vantagem: {simulacao.MensagemVantagem}").AppendLine();
        AdicionarDetalhes(texto, "IR normal progressivo", simulacao.Normal.DetalhesProgressivos);
        AdicionarDetalhes(texto, "IR simplificado progressivo", simulacao.Simplificada.DetalhesProgressivos);
        AdicionarDetalhes(texto, "INSS progressivo", simulacao.DetalhesInss);
        texto.AppendLine($"FGTS 8%: {simulacao.FgtsOitoPorCento:N2}");
        texto.AppendLine($"FGTS 2%: {simulacao.FgtsDoisPorCento:N2}");
        return texto.ToString();
    }

    private static void AdicionarModalidade(StringBuilder texto, ModalidadeIrrfDto modalidade, int dependentes, decimal valorBruto)
    {
        texto.AppendLine($"IR {modalidade.Nome}");
        texto.AppendLine($"Base de cálculo: {modalidade.BaseCalculo:N2}");
        texto.AppendLine($"Alíquota: {modalidade.Aliquota:N2}% | Dedução: {modalidade.Deducao:N2}");
        texto.AppendLine($"Imposto pela tabela progressiva: {modalidade.ImpostoAntesReducao:N2}");
        texto.AppendLine($"Redução mensal do IRRF: {modalidade.ReducaoMensal:N2}");
        texto.AppendLine($"Imposto final: {modalidade.Imposto:N2} | Alíquota efetiva: {modalidade.AliquotaEfetiva:N2}%").AppendLine();
    }

    private static void AdicionarDetalhes(StringBuilder texto, string titulo, IReadOnlyList<DetalheFaixaDto> detalhes)
    {
        texto.AppendLine(titulo.ToUpperInvariant());
        if (detalhes.Count == 0)
            texto.AppendLine("Sem cálculo aplicável.").AppendLine();
        else
        {
            foreach (var detalhe in detalhes)
                texto.AppendLine($"Faixa {detalhe.Faixa}: base {detalhe.BaseCalculada:N2} | {detalhe.Aliquota:N2}% | imposto {detalhe.Imposto:N2}");

            var totalImposto = detalhes.Sum(detalhe => detalhe.Imposto);
            texto.AppendLine($"Total do imposto: {totalImposto:N2}");
            texto.AppendLine();
        }
    }
}
