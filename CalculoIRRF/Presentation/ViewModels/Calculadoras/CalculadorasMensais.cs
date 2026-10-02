#nullable enable

using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Application.UseCases;

namespace CalculoIRRF.Presentation.ViewModels.Calculadoras;

public sealed class CalculadoraSalarioPeloLiquido(ISimularDemonstrativoUseCase<SimularSalarioPeloLiquidoRequest> simulador) : CalculadoraBase
{
    private readonly CampoTextoViewModel _competencia = Competencia();
    private readonly CampoTextoViewModel _liquido = Moeda("Salário líquido desejado", "Valor que o trabalhador deve receber, já descontados INSS e IRRF.");
    private readonly CampoTextoViewModel _dependentes = Inteiro("Dependentes", 0, "Dependentes para a dedução do IRRF.");

    public override string Titulo => "Salário bruto a partir do líquido";
    public override string Descricao => "Descubra o salário bruto necessário para que o trabalhador receba um líquido desejado.";
    public override string InstrucaoInicial => "Informe a competência, o salário líquido desejado e os dependentes e selecione Calcular.";
    public override IReadOnlyList<CampoViewModel> Campos => [_competencia, _liquido, _dependentes];
    public override string NomeArquivoPdf => $"salario-bruto-pelo-liquido-{_competencia.Valor.Replace('/', '-')}.pdf";

    public override void Preencher(ContextoCalculo contexto)
    {
        if (contexto.Competencia is { } competencia) _competencia.Valor = competencia.ToString("MM/yyyy", Cultura);
        if (contexto.Dependentes is { } dependentes) _dependentes.Valor = dependentes.ToString(Cultura);
    }

    public override Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken) =>
        simulador.ExecutarAsync(new SimularSalarioPeloLiquidoRequest(LerCompetencia(_competencia), LerMoeda(_liquido), LerInteiro(_dependentes)), cancellationToken);
}

public sealed class CalculadoraDecimoTerceiro : CalculadoraBase
{
    private readonly ISimularDemonstrativoUseCase<SimularDecimoTerceiroRequest> _simulador;

    public CalculadoraDecimoTerceiro(ISimularDemonstrativoUseCase<SimularDecimoTerceiroRequest> simulador)
    {
        _simulador = simulador;
        _adiantamento.AoAlterar = () => _valorAdiantamento.Visivel = _adiantamento.Valor<AdiantamentoDecimoTerceiro>() == AdiantamentoDecimoTerceiro.ValorInformado;
        _adiantamento.AoAlterar();
    }

    private readonly CampoTextoViewModel _competencia = new("Competência do pagamento", TipoCampo.Competencia, $"12/{DateTime.Today.Year}", "Mês da 2ª parcela, normalmente dezembro (MM/AAAA).");
    private readonly CampoTextoViewModel _salario = Moeda("Salário", "Salário mensal de dezembro.");
    private readonly CampoTextoViewModel _medias = Moeda("Médias de variáveis", "Média anual de horas extras, comissões e adicionais; deixe 0,00 se não houver.");
    private readonly CampoTextoViewModel _avos = Inteiro("Avos (meses trabalhados)", 12, "Meses do ano com 15 dias ou mais de trabalho, de 1 a 12.");
    private readonly CampoTextoViewModel _dependentes = Inteiro("Dependentes", 0, "Dependentes para a dedução do IRRF.");
    private readonly CampoOpcaoViewModel _adiantamento = new("1ª parcela (adiantamento)",
        [
            new("50% do 13º (padrão)", AdiantamentoDecimoTerceiro.CinquentaPorCento),
            new("Não houve adiantamento", AdiantamentoDecimoTerceiro.SemAdiantamento),
            new("Valor informado", AdiantamentoDecimoTerceiro.ValorInformado)
        ], "Como foi paga a 1ª parcela, descontada na 2ª.");
    private readonly CampoTextoViewModel _valorAdiantamento = Moeda("Valor da 1ª parcela", "Valor pago como adiantamento, descontado na 2ª parcela.");

    public override string Titulo => "13º salário";
    public override string Descricao => "Calcule a 1ª e a 2ª parcela do 13º, com o INSS e o IRRF descontados em dezembro.";
    public override string InstrucaoInicial => "Informe o salário, as médias, os avos e os dependentes e selecione Calcular.";
    public override IReadOnlyList<CampoViewModel> Campos => [_competencia, _salario, _medias, _avos, _dependentes, _adiantamento, _valorAdiantamento];
    public override string NomeArquivoPdf => $"decimo-terceiro-{_competencia.Valor.Replace('/', '-')}.pdf";

    public override void Preencher(ContextoCalculo contexto)
    {
        PreencherMoeda(_salario, contexto.Salario);
        if (contexto.Dependentes is { } dependentes) _dependentes.Valor = dependentes.ToString(Cultura);
    }

    public override Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken) =>
        _simulador.ExecutarAsync(new SimularDecimoTerceiroRequest(
            LerCompetencia(_competencia), LerMoeda(_salario), LerMoeda(_medias), LerInteiro(_avos), LerInteiro(_dependentes),
            _adiantamento.Valor<AdiantamentoDecimoTerceiro>(), LerMoeda(_valorAdiantamento)), cancellationToken);
}

public sealed class CalculadoraFerias(ISimularDemonstrativoUseCase<SimularFeriasRequest> simulador) : CalculadoraBase
{
    private readonly CampoTextoViewModel _competencia = Competencia("Competência do pagamento");
    private readonly CampoTextoViewModel _salario = Moeda("Salário", "Salário mensal na data das férias.");
    private readonly CampoTextoViewModel _medias = Moeda("Médias de variáveis", "Média de horas extras, comissões e adicionais do período aquisitivo; deixe 0,00 se não houver.");
    private readonly CampoTextoViewModel _faltas = Inteiro("Faltas injustificadas", 0, "Faltas no período aquisitivo: até 5 dão 30 dias; de 6 a 14, 24; de 15 a 23, 18; de 24 a 32, 12.");
    private readonly CampoTextoViewModel _diasGozo = Inteiro("Dias de descanso", 0, "Deixe 0 para usar todos os dias de direito que não forem vendidos; informe menos para dividir as férias.");
    private readonly CampoOpcaoViewModel _abono = CampoOpcaoViewModel.SimNao("Vender 1/3 (abono)", false, "Converte 1/3 dos dias em dinheiro, sem INSS e sem IRRF.");
    private readonly CampoOpcaoViewModel _adiantamento13 = CampoOpcaoViewModel.SimNao("Adiantar 13º (1ª parcela)", false, "Paga metade do 13º junto com as férias, quando solicitado.");
    private readonly CampoTextoViewModel _dependentes = Inteiro("Dependentes", 0, "Dependentes para a dedução do IRRF.");

    public override string Titulo => "Férias";
    public override string Descricao => "Calcule as férias com o terço constitucional, a venda de dias (abono) e o adiantamento do 13º.";
    public override string InstrucaoInicial => "Informe o salário, as médias, as faltas e as opções de abono e selecione Calcular.";
    public override IReadOnlyList<CampoViewModel> Campos => [_competencia, _salario, _medias, _faltas, _diasGozo, _abono, _adiantamento13, _dependentes];
    public override string NomeArquivoPdf => $"ferias-{_competencia.Valor.Replace('/', '-')}.pdf";

    public override void Preencher(ContextoCalculo contexto)
    {
        if (contexto.Competencia is { } competencia) _competencia.Valor = competencia.ToString("MM/yyyy", Cultura);
        PreencherMoeda(_salario, contexto.Salario);
        if (contexto.Dependentes is { } dependentes) _dependentes.Valor = dependentes.ToString(Cultura);
    }

    public override Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken) =>
        simulador.ExecutarAsync(new SimularFeriasRequest(
            LerCompetencia(_competencia), LerMoeda(_salario), LerMoeda(_medias), LerInteiro(_faltas), LerInteiro(_diasGozo),
            _abono.Valor<bool>(), _adiantamento13.Valor<bool>(), LerInteiro(_dependentes)), cancellationToken);
}

public sealed class CalculadoraHorasExtras(ISimularDemonstrativoUseCase<SimularHorasExtrasRequest> simulador) : CalculadoraBase
{
    private readonly CampoTextoViewModel _competencia = Competencia(dica: "Mês das horas, que define os domingos para o DSR e as tabelas de INSS e IRRF (MM/AAAA).");
    private readonly CampoTextoViewModel _salario = Moeda("Salário", "Salário mensal.");
    private readonly CampoTextoViewModel _divisor = new("Divisor de horas", TipoCampo.Numero, "220", "220 para 44 horas semanais; 200 para 40; 180 para 36; 150 para 30.");
    private readonly CampoTextoViewModel _horas1 = new("Horas extras (faixa 1)", TipoCampo.Horas, "0:00", "Horas extras com o primeiro adicional, como 10:30 ou 10,5.");
    private readonly CampoTextoViewModel _percentual1 = new("Adicional da faixa 1 (%)", TipoCampo.Numero, "50", "Mínimo de 50%; convenções coletivas podem prever mais.");
    private readonly CampoTextoViewModel _horas2 = new("Horas extras (faixa 2)", TipoCampo.Horas, "0:00", "Horas com o segundo adicional, como as de domingos e feriados.");
    private readonly CampoTextoViewModel _percentual2 = new("Adicional da faixa 2 (%)", TipoCampo.Numero, "100", "Normalmente 100% para domingos e feriados.");
    private readonly CampoTextoViewModel _horasNoturnas = new("Horas noturnas (relógio)", TipoCampo.Horas, "0:00", "Horas de relógio trabalhadas entre 22h e 5h; a conversão para a hora reduzida é automática.");
    private readonly CampoTextoViewModel _percentualNoturno = new("Adicional noturno (%)", TipoCampo.Numero, "20", "20% para o trabalho urbano (CLT, art. 73).");
    private readonly CampoTextoViewModel _feriados = Inteiro("Feriados no mês", 0, "Feriados que caem em dias úteis, para o cálculo do DSR.");
    private readonly CampoTextoViewModel _dependentes = Inteiro("Dependentes", 0, "Dependentes para a dedução do IRRF.");

    public override string Titulo => "Horas extras e adicionais";
    public override string Descricao => "Calcule horas extras, adicional noturno e o reflexo no DSR, com o salário líquido do mês.";
    public override string InstrucaoInicial => "Informe o salário, o divisor e as horas do mês e selecione Calcular.";
    public override IReadOnlyList<CampoViewModel> Campos => [_competencia, _salario, _divisor, _horas1, _percentual1, _horas2, _percentual2, _horasNoturnas, _percentualNoturno, _feriados, _dependentes];
    public override string NomeArquivoPdf => $"horas-extras-{_competencia.Valor.Replace('/', '-')}.pdf";

    public override void Preencher(ContextoCalculo contexto)
    {
        if (contexto.Competencia is { } competencia) _competencia.Valor = competencia.ToString("MM/yyyy", Cultura);
        PreencherMoeda(_salario, contexto.Salario);
        if (contexto.Dependentes is { } dependentes) _dependentes.Valor = dependentes.ToString(Cultura);
    }

    public override Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken) =>
        simulador.ExecutarAsync(new SimularHorasExtrasRequest(
            LerCompetencia(_competencia), LerMoeda(_salario), LerNumero(_divisor),
            LerHoras(_horas1), LerNumero(_percentual1), LerHoras(_horas2), LerNumero(_percentual2),
            LerHoras(_horasNoturnas), LerNumero(_percentualNoturno), LerInteiro(_feriados), LerInteiro(_dependentes)), cancellationToken);
}
