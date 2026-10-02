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
    private readonly IAtualizadorTabelaIrrf _atualizadorIrrf;
    private readonly IAtualizadorTabelaInss _atualizadorInss;
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
        IAtualizadorTabelaIrrf atualizadorIrrf,
        IAtualizadorTabelaInss atualizadorInss,
        IUserNotifier notificador)
    {
        Tipo = tipo; _service = service; _atualizadorIrrf = atualizadorIrrf; _atualizadorInss = atualizadorInss; _notificador = notificador;
        SalvarCommand = new AsyncRelayCommand(SalvarAsync);
        ExcluirCommand = new AsyncRelayCommand(ExcluirAsync, () => Selecionado is not null);
        CarregarCommand = new AsyncRelayCommand(CarregarAsync);
        AtualizarDaReceitaCommand = new AsyncRelayCommand(AtualizarDaReceitaAsync, () => ExibeAtualizacaoOnline);
        AbrirFonteOficialCommand = new RelayCommand(_ => AbrirFonteOficial(), _ => ExibeAtualizacaoOnline);
    }

    public TipoTabelaTributaria Tipo { get; }
    public string Titulo => Tipo switch { TipoTabelaTributaria.Inss => "Tabela INSS", TipoTabelaTributaria.Irrf => "Tabela IRRF", TipoTabelaTributaria.Simplificado => "Valor simplificado", TipoTabelaTributaria.Dependente => "Dedução por dependente", TipoTabelaTributaria.DescontoMinimo => "Desconto mínimo", _ => "Redução mensal do IRRF" };
    public string Descricao => Tipo == TipoTabelaTributaria.ReducaoMensalIrrf
        ? "Configure a redução aplicada ao imposto após a tabela progressiva."
        : ExibeFaixa ? "Cadastre, consulte e mantenha as faixas por competência." : "Cadastre, consulte e mantenha os valores por competência.";
    public string TituloLista => ExibeFaixa ? "Faixas cadastradas" : "Valores cadastrados";
    public string LabelValor => Tipo switch { TipoTabelaTributaria.ReducaoMensalIrrf => "Limite de rendimentos (R$)", TipoTabelaTributaria.Inss or TipoTabelaTributaria.Irrf => "Limite da base (R$)", _ => "Valor (R$)" };
    public string LabelAliquota => Tipo == TipoTabelaTributaria.ReducaoMensalIrrf ? "Multiplicador" : "Alíquota (%)";
    public string LabelDeducao => Tipo == TipoTabelaTributaria.ReducaoMensalIrrf ? "Valor-base da redução (R$)" : "Parcela a deduzir (R$)";
    public bool ExibeFaixa => Tipo is TipoTabelaTributaria.Inss or TipoTabelaTributaria.Irrf or TipoTabelaTributaria.ReducaoMensalIrrf;
    public bool ExibeDeducao => Tipo is TipoTabelaTributaria.Irrf or TipoTabelaTributaria.ReducaoMensalIrrf;
    public bool ExibeAtualizacaoOnline => Tipo is TipoTabelaTributaria.Inss or TipoTabelaTributaria.Irrf or TipoTabelaTributaria.Simplificado or TipoTabelaTributaria.Dependente or TipoTabelaTributaria.ReducaoMensalIrrf;
    public string TextoLinkFonteOficial => Tipo == TipoTabelaTributaria.Inss ? "Abrir página oficial do INSS ↗" : "Abrir página oficial da Receita Federal ↗";
    public string FonteOficial => (Tipo == TipoTabelaTributaria.Inss ? _atualizadorInss.FonteOficial : _atualizadorIrrf.FonteOficial).AbsoluteUri;
    public ObservableCollection<RegistroTabelaDto> Registros { get; } = [];
    public RegistroTabelaDto? Selecionado { get => _selecionado; set { if (SetProperty(ref _selecionado, value) && value is not null) Preencher(value); ((AsyncRelayCommand)ExcluirCommand).RaiseCanExecuteChanged(); } }
    public string Competencia { get => _competencia; set => SetProperty(ref _competencia, value); }
    public string Faixa { get => _faixa; set => SetProperty(ref _faixa, value); }
    public string Valor { get => _valor; set => SetProperty(ref _valor, value); }
    public string Aliquota { get => _aliquota; set => SetProperty(ref _aliquota, value); }
    public string Deducao { get => _deducao; set => SetProperty(ref _deducao, value); }
    public string StatusAtualizacao { get => _statusAtualizacao; private set => SetProperty(ref _statusAtualizacao, value); }
    public ICommand SalvarCommand { get; }
    public ICommand ExcluirCommand { get; }
    public ICommand CarregarCommand { get; }
    public ICommand AtualizarDaReceitaCommand { get; }
    public ICommand AbrirFonteOficialCommand { get; }

    private async Task CarregarAsync()
    {
        Registros.Clear();
        foreach (var item in await _service.ListarAsync(Tipo, CancellationToken.None)) Registros.Add(item);
        Selecionado = null;
    }
    private async Task AtualizarDaReceitaAsync()
    {
        try
        {
            StatusAtualizacao = "Consultando a tabela oficial...";
            var resultado = Tipo == TipoTabelaTributaria.Inss
                ? await AtualizarInssAsync()
                : await AtualizarIrrfAsync();
            await CarregarAsync();
            StatusAtualizacao = $"Dados de {resultado.Competencia:MM/yyyy} atualizados pela fonte oficial ({resultado.QuantidadeFaixas} faixas tributárias importadas).";
        }
        catch (Exception ex)
        {
            StatusAtualizacao = "A atualização não foi concluída; os dados locais foram preservados.";
            _notificador.MostrarErro("Não foi possível atualizar a tabela pelo site oficial.", ex);
        }
    }
    private void AbrirFonteOficial()
    {
        Process.Start(new ProcessStartInfo(FonteOficial) { UseShellExecute = true });
    }
    private async Task<AtualizacaoOnlineResultado> AtualizarIrrfAsync()
    {
        var resultado = await _atualizadorIrrf.AtualizarAsync(CancellationToken.None);
        return new AtualizacaoOnlineResultado(resultado.Competencia, resultado.QuantidadeFaixas);
    }
    private async Task<AtualizacaoOnlineResultado> AtualizarInssAsync()
    {
        var resultado = await _atualizadorInss.AtualizarAsync(CancellationToken.None);
        return new AtualizacaoOnlineResultado(resultado.Competencia, resultado.QuantidadeFaixas);
    }
    private async Task SalvarAsync()
    {
        if (!Ler(out var request)) return;
        try { await _service.SalvarAsync(Tipo, request, CancellationToken.None); await CarregarAsync(); }
        catch (Exception ex) { _notificador.MostrarErro("Não foi possível salvar o registro.", ex); }
    }
    private async Task ExcluirAsync()
    {
        if (Selecionado is null) return;
        try { await _service.ExcluirAsync(Tipo, Selecionado.Id, CancellationToken.None); await CarregarAsync(); }
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
        if (ExibeFaixa) valido &= int.TryParse(Faixa, out f) && f > 0 && decimal.TryParse(Aliquota, NumberStyles.Number, Cultura, out a) && a >= 0m;
        if (ExibeFaixa) { faixa = f; aliquota = a; }
        if (ExibeDeducao) { valido &= decimal.TryParse(Deducao, NumberStyles.Number, Cultura, out d) && d >= 0m; deducao = d; }
        if (!valido) { _notificador.MostrarAviso("Informe valores válidos para competência e campos numéricos."); return false; }
        request = new SalvarRegistroTabelaRequest(Selecionado?.Id ?? 0, DateOnly.FromDateTime(competencia), faixa, valor, aliquota, deducao); return true;
    }
    private void Preencher(RegistroTabelaDto item)
    {
        Competencia = item.Competencia.ToString("MM/yyyy"); Faixa = item.Faixa?.ToString() ?? "1"; Valor = item.Valor.ToString("N2", Cultura); Aliquota = item.Aliquota?.ToString(FormatoAliquota, Cultura) ?? "0,00"; Deducao = item.Deducao?.ToString("N2", Cultura) ?? "0,00";
    }
    private sealed record AtualizacaoOnlineResultado(DateOnly Competencia, int QuantidadeFaixas);
}
