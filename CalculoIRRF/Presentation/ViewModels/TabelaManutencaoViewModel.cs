#nullable enable

using CalculoIRRF.Application.Management;
using CalculoIRRF.Presentation.Mvvm;
using CalculoIRRF.Presentation.Services;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Input;

namespace CalculoIRRF.Presentation.ViewModels;

public sealed class TabelaManutencaoViewModel : ViewModelBase
{
    private static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");
    // Até seis casas: o multiplicador da redução mensal (ex.: 0,133145) não pode ser arredondado ao ser editado.
    private const string FormatoAliquota = "#,##0.00####";
    private readonly ITabelaTributariaService _service;
    private readonly IAtualizadorTabelas _atualizador;
    private readonly IUserNotifier _notificador;
    private RegistroTabelaDto? _selecionado;
    private string _competencia = DateTime.Today.ToString("MM/yyyy");
    private string _faixa = "1";
    private string _valor = "0,00";
    private string _aliquota = "0,00";
    private string _deducao = "0,00";
    private string _statusAtualizacao = "Dados exibidos localmente.";

    public TabelaManutencaoViewModel(
        TipoTabelaTributaria tipo,
        ITabelaTributariaService service,
        IAtualizadorTabelas atualizador,
        IUserNotifier notificador)
    {
        Tipo = tipo; _service = service; _atualizador = atualizador; _notificador = notificador;
        SalvarCommand = new AsyncRelayCommand(SalvarAsync);
        ExcluirCommand = new AsyncRelayCommand(ExcluirAsync, () => Selecionado is not null);
        CarregarCommand = new AsyncRelayCommand(() => CarregarAsync(Selecionado?.Id));
        NovoCommand = new RelayCommand(_ => NovoRegistro());
        AtualizarDaReceitaCommand = new AsyncRelayCommand(AtualizarDaReceitaAsync, () => ExibeAtualizacaoOnline);
        AbrirFonteOficialCommand = new RelayCommand(_ => AbrirFonteOficial(), _ => ExibeAtualizacaoOnline);
    }

    public TipoTabelaTributaria Tipo { get; }
    public string Titulo => Tipo switch
    {
        TipoTabelaTributaria.Inss => "Tabela INSS",
        TipoTabelaTributaria.Irrf => "Tabela IRRF",
        TipoTabelaTributaria.Simplificado => "Valor simplificado",
        TipoTabelaTributaria.Dependente => "Dedução por dependente",
        TipoTabelaTributaria.DescontoMinimo => "Desconto mínimo",
        TipoTabelaTributaria.SalarioMinimo => "Salário mínimo",
        TipoTabelaTributaria.SalarioFamilia => "Salário-família",
        TipoTabelaTributaria.Plr => "Tabela PLR",
        _ => "Redução mensal do IRRF"
    };
    public string Descricao => Tipo == TipoTabelaTributaria.ReducaoMensalIrrf
        ? "Configure a redução aplicada ao imposto após a tabela progressiva."
        : ExibeFaixa ? "Cadastre, consulte e mantenha as faixas por competência." : "Cadastre, consulte e mantenha os valores por competência.";
    public string TituloLista => ExibeFaixa ? "Faixas cadastradas" : "Valores cadastrados";
    public string LabelValor => Tipo switch
    {
        TipoTabelaTributaria.ReducaoMensalIrrf => "Limite de rendimentos (R$)",
        TipoTabelaTributaria.Inss or TipoTabelaTributaria.Irrf => "Limite da base (R$)",
        TipoTabelaTributaria.Plr => "Limite da PLR anual (R$)",
        TipoTabelaTributaria.SalarioFamilia => "Limite de remuneração (R$)",
        _ => "Valor (R$)"
    };
    public string LabelAliquota => Tipo == TipoTabelaTributaria.ReducaoMensalIrrf ? "Multiplicador" : "Alíquota (%)";
    public string LabelDeducao => Tipo switch
    {
        TipoTabelaTributaria.ReducaoMensalIrrf => "Valor-base da redução (R$)",
        TipoTabelaTributaria.SalarioFamilia => "Cota por filho (R$)",
        _ => "Parcela a deduzir (R$)"
    };
    public bool ExibeFaixa => Tipo is TipoTabelaTributaria.Inss or TipoTabelaTributaria.Irrf or TipoTabelaTributaria.ReducaoMensalIrrf or TipoTabelaTributaria.Plr or TipoTabelaTributaria.SalarioFamilia;
    // O salário-família tem faixas, mas cada uma é um limite de remuneração com uma cota, sem alíquota.
    public bool ExibeAliquota => Tipo is TipoTabelaTributaria.Inss or TipoTabelaTributaria.Irrf or TipoTabelaTributaria.ReducaoMensalIrrf or TipoTabelaTributaria.Plr;
    public bool ExibeDeducao => Tipo is TipoTabelaTributaria.Irrf or TipoTabelaTributaria.ReducaoMensalIrrf or TipoTabelaTributaria.Plr or TipoTabelaTributaria.SalarioFamilia;
    public bool ExibeAtualizacaoOnline => _atualizador.TemAtualizacaoOnline(Tipo);
    public string TextoLinkFonteOficial => ExibeAtualizacaoOnline ? (_atualizador.NomeFonteOficial(Tipo) == "INSS" ? "Abrir página oficial do INSS ↗" : "Abrir página oficial da Receita Federal ↗") : string.Empty;
    public string FonteOficial => ExibeAtualizacaoOnline ? _atualizador.FonteOficial(Tipo).AbsoluteUri : string.Empty;
    public ObservableCollection<RegistroTabelaDto> Registros { get; } = [];
    public RegistroTabelaDto? Selecionado
    {
        get => _selecionado;
        set
        {
            if (SetProperty(ref _selecionado, value) && value is not null) Preencher(value);
            ((AsyncRelayCommand)ExcluirCommand).RaiseCanExecuteChanged();
            OnPropertyChanged(nameof(TextoSalvar));
        }
    }
    // Sem linha selecionada, salvar cria um registro; o rótulo deixa claro qual das duas operações será feita.
    public string TextoSalvar => Selecionado is null ? "Incluir registro" : "Salvar alteração";
    public string Competencia { get => _competencia; set => SetProperty(ref _competencia, value); }
    public string Faixa { get => _faixa; set => SetProperty(ref _faixa, value); }
    public string Valor { get => _valor; set => SetProperty(ref _valor, value); }
    public string Aliquota { get => _aliquota; set => SetProperty(ref _aliquota, value); }
    public string Deducao { get => _deducao; set => SetProperty(ref _deducao, value); }
    public string StatusAtualizacao { get => _statusAtualizacao; private set => SetProperty(ref _statusAtualizacao, value); }
    public ICommand SalvarCommand { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand CarregarCommand { get; }
    public ICommand NovoCommand { get; }
    public ICommand AtualizarDaReceitaCommand { get; }
    public ICommand AbrirFonteOficialCommand { get; }

    /// <summary>Recarrega a lista mantendo selecionado o registro informado; se ele não existir mais, volta ao modo de inclusão.</summary>
    private async Task CarregarAsync(int? idSelecionado)
    {
        var registros = await _service.ListarAsync(Tipo, CancellationToken.None);
        Registros.Clear();
        foreach (var item in registros) Registros.Add(item);
        // Os registros são records (igualdade por valor): limpar antes garante que o formulário seja preenchido de novo com o que está no banco.
        Selecionado = null;
        Selecionado = Registros.FirstOrDefault(item => item.Id == idSelecionado);
        if (Selecionado is null) LimparFormulario();
    }
    private void NovoRegistro()
    {
        Selecionado = null;
        LimparFormulario();
    }
    private void LimparFormulario()
    {
        Competencia = DateTime.Today.ToString("MM/yyyy"); Faixa = "1"; Valor = "0,00"; Aliquota = "0,00"; Deducao = "0,00";
    }
    private async Task AtualizarDaReceitaAsync()
    {
        try
        {
            StatusAtualizacao = "Consultando a fonte oficial e as fontes alternativas...";
            var resultado = await _atualizador.AtualizarAsync(Tipo, CancellationToken.None);
            await CarregarAsync(Selecionado?.Id);
            StatusAtualizacao = DescreverAtualizacao(resultado);
        }
        catch (Exception ex)
        {
            StatusAtualizacao = "A atualização não foi concluída; os dados locais foram preservados.";
            _notificador.MostrarErro("Não foi possível atualizar a tabela pela internet.", ex);
        }
    }
    private static string DescreverAtualizacao(AtualizacaoTabelaResultado resultado)
    {
        var origem = resultado.Oficial ? "pela fonte oficial" : $"por {string.Join(" e ", resultado.Fontes)}, que já publicaram a tabela";
        var texto = $"Dados de {resultado.Competencia:MM/yyyy} atualizados {origem} ({(resultado.QuantidadeFaixas == 1 ? "1 valor importado" : $"{resultado.QuantidadeFaixas} faixas tributárias importadas")}).";
        return resultado.Observacoes.Count == 0 ? texto : $"{texto} {string.Join(" ", resultado.Observacoes)}";
    }
    private void AbrirFonteOficial()
    {
        Process.Start(new ProcessStartInfo(FonteOficial) { UseShellExecute = true });
    }
    private async Task SalvarAsync()
    {
        if (!Ler(out var request)) return;
        try
        {
            await _service.SalvarAsync(Tipo, request, CancellationToken.None);
            await CarregarAsync(request.Id == 0 ? null : request.Id);
            // Após incluir, o registro criado (o de maior Id com a mesma competência e faixa) fica selecionado.
            if (request.Id == 0)
                Selecionado = Registros.Where(item => item.Competencia == request.Competencia && item.Faixa == request.Faixa).MaxBy(item => item.Id);
        }
        catch (Exception ex) { _notificador.MostrarErro("Não foi possível salvar o registro.", ex); }
    }
    private async Task ExcluirAsync()
    {
        if (Selecionado is null) return;
        try { await _service.ExcluirAsync(Tipo, Selecionado.Id, CancellationToken.None); await CarregarAsync(null); }
        catch (Exception ex) { _notificador.MostrarErro("Não foi possível excluir o registro.", ex); }
    }
    private bool Ler(out SalvarRegistroTabelaRequest request)
    {
        request = default!;
        var competencia = default(DateTime);
        var valor = 0m;
        var valido = DateTime.TryParseExact(Competencia, "MM/yyyy", Cultura, DateTimeStyles.None, out competencia) && decimal.TryParse(Valor, NumberStyles.Number, Cultura, out valor) && valor >= 0m;
        var f = 0; var a = 0m; var d = 0m;
        int? faixa = null; decimal? aliquota = null; decimal? deducao = null;
        if (ExibeFaixa) { valido &= int.TryParse(Faixa, out f) && f > 0; faixa = f; }
        if (ExibeAliquota) { valido &= decimal.TryParse(Aliquota, NumberStyles.Number, Cultura, out a) && a >= 0m; aliquota = a; }
        if (ExibeDeducao) { valido &= decimal.TryParse(Deducao, NumberStyles.Number, Cultura, out d) && d >= 0m; deducao = d; }
        if (!valido) { _notificador.MostrarAviso("Informe valores válidos para competência e campos numéricos."); return false; }
        request = new SalvarRegistroTabelaRequest(Selecionado?.Id ?? 0, DateOnly.FromDateTime(competencia), faixa, valor, aliquota, deducao); return true;
    }
    private void Preencher(RegistroTabelaDto item)
    {
        Competencia = item.Competencia.ToString("MM/yyyy"); Faixa = item.Faixa?.ToString() ?? "1"; Valor = item.Valor.ToString("N2", Cultura); Aliquota = item.Aliquota?.ToString(FormatoAliquota, Cultura) ?? "0,00"; Deducao = item.Deducao?.ToString("N2", Cultura) ?? "0,00";
    }
}
