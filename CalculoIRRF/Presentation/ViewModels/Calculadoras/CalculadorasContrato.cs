#nullable enable

using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Application.UseCases;

namespace CalculoIRRF.Presentation.ViewModels.Calculadoras;

public sealed class CalculadoraRescisao : CalculadoraBase
{
    private readonly ISimularDemonstrativoUseCase<SimularRescisaoRequest> _simulador;
    private readonly CampoTextoViewModel _admissao = new("Data de admissão", TipoCampo.Data, DateTime.Today.AddYears(-2).ToString("dd/MM/yyyy", Cultura), "Data de início do contrato (dd/mm/aaaa).");
    private readonly CampoTextoViewModel _desligamento = new("Data de desligamento", TipoCampo.Data, DateTime.Today.ToString("dd/MM/yyyy", Cultura), "Último dia de trabalho; com aviso trabalhado, o último dia do aviso (dd/mm/aaaa).");
    private readonly CampoOpcaoViewModel _motivo = new("Motivo",
        [
            new("Dispensa sem justa causa", MotivoRescisao.DispensaSemJustaCausa),
            new("Pedido de demissão", MotivoRescisao.PedidoDeDemissao),
            new("Acordo (art. 484-A)", MotivoRescisao.Acordo),
            new("Dispensa por justa causa", MotivoRescisao.DispensaPorJustaCausa),
            new("Fim de contrato a prazo", MotivoRescisao.TerminoDeContratoPorPrazo)
        ], "Motivo do desligamento, que define as verbas devidas.");
    private readonly CampoOpcaoViewModel _aviso = new("Aviso prévio",
        [
            new("Indenizado", CumprimentoAvisoPrevio.Indenizado),
            new("Trabalhado ou dispensado", CumprimentoAvisoPrevio.TrabalhadoOuDispensado),
            new("Não cumprido (descontar)", CumprimentoAvisoPrevio.NaoCumpridoPeloEmpregado)
        ], "Indenizado: pago sem trabalhar. Não cumprido: o empregado pediu demissão e não trabalhou o aviso.");
    private readonly CampoTextoViewModel _salario = Moeda("Salário", "Último salário mensal.");
    private readonly CampoTextoViewModel _medias = Moeda("Médias de variáveis", "Média de horas extras, comissões e adicionais, que entra no aviso, no 13º e nas férias.");
    private readonly CampoOpcaoViewModel _feriasVencidas = new("Férias vencidas",
        [new("Nenhuma", 0), new("1 período", 1), new("2 períodos", 2)], "Períodos aquisitivos completos cujas férias não foram tiradas.");
    private readonly CampoTextoViewModel _faltas = Inteiro("Faltas no período atual", 0, "Faltas injustificadas no período aquisitivo em curso, que reduzem as férias proporcionais.");
    private readonly CampoTextoViewModel _saldoFgts = Moeda("Saldo do FGTS", "Saldo do extrato para fins rescisórios; deixe 0,00 para estimar pelo salário.");
    private readonly CampoTextoViewModel _adiantamento13 = Moeda("13º já adiantado", "1ª parcela do 13º paga neste ano, descontada na rescisão.");
    private readonly CampoTextoViewModel _dependentes = Inteiro("Dependentes", 0, "Dependentes para a dedução do IRRF.");

    public CalculadoraRescisao(ISimularDemonstrativoUseCase<SimularRescisaoRequest> simulador)
    {
        _simulador = simulador;
        _motivo.AoAlterar = AjustarAoMotivo;
        AjustarAoMotivo();
    }

    public override string Titulo => "Rescisão";
    public override string Descricao => "Calcule as verbas rescisórias conforme o motivo do desligamento, com aviso prévio proporcional, FGTS e multa.";
    public override string InstrucaoInicial => "Informe as datas, o motivo, o aviso prévio e o salário e selecione Calcular.";
    public override IReadOnlyList<CampoViewModel> Campos => [_admissao, _desligamento, _motivo, _aviso, _salario, _medias, _feriasVencidas, _faltas, _saldoFgts, _adiantamento13, _dependentes];
    public override string NomeArquivoPdf => $"rescisao-{_desligamento.Valor.Replace('/', '-')}.pdf";

    public override void Preencher(ContextoCalculo contexto)
    {
        PreencherMoeda(_salario, contexto.Salario);
        if (contexto.Dependentes is { } dependentes) _dependentes.Valor = dependentes.ToString(Cultura);
    }

    public override Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken) =>
        _simulador.ExecutarAsync(new SimularRescisaoRequest(
            LerData(_admissao), LerData(_desligamento), _motivo.Valor<MotivoRescisao>(), _aviso.Valor<CumprimentoAvisoPrevio>(),
            LerMoeda(_salario), LerMoeda(_medias), _feriasVencidas.Valor<int>(), LerInteiro(_faltas),
            _saldoFgts.Visivel ? LerMoeda(_saldoFgts) : 0m, LerMoeda(_adiantamento13), LerInteiro(_dependentes)), cancellationToken);

    // O aviso só existe na dispensa sem justa causa, no pedido de demissão e no acordo; o saldo do FGTS só importa quando há multa ou saque.
    private void AjustarAoMotivo()
    {
        var motivo = _motivo.Valor<MotivoRescisao>();
        _aviso.Visivel = motivo is MotivoRescisao.DispensaSemJustaCausa or MotivoRescisao.PedidoDeDemissao or MotivoRescisao.Acordo;
        _saldoFgts.Visivel = motivo is MotivoRescisao.DispensaSemJustaCausa or MotivoRescisao.Acordo or MotivoRescisao.TerminoDeContratoPorPrazo;
        var aviso = _aviso.Valor<CumprimentoAvisoPrevio>();
        if (motivo == MotivoRescisao.PedidoDeDemissao && aviso == CumprimentoAvisoPrevio.Indenizado)
            _aviso.Selecionada = _aviso.Opcoes[1];
        else if (motivo != MotivoRescisao.PedidoDeDemissao && aviso == CumprimentoAvisoPrevio.NaoCumpridoPeloEmpregado)
            _aviso.Selecionada = _aviso.Opcoes[0];
    }
}

public sealed class CalculadoraCustoFuncionario : CalculadoraBase
{
    private readonly ISimularDemonstrativoUseCase<SimularCustoFuncionarioRequest> _simulador;
    private readonly CampoTextoViewModel _salario = Moeda("Salário", "Salário mensal do empregado.");
    private readonly CampoOpcaoViewModel _regime = new("Regime da empresa",
        [
            new("Lucro Real ou Presumido", RegimeTributario.LucroRealOuPresumido),
            new("Simples (anexos I a III e V)", RegimeTributario.SimplesNacional),
            new("Simples (anexo IV)", RegimeTributario.SimplesNacionalAnexoIV)
        ], "Define se a empresa paga a contribuição patronal sobre a folha.");
    private readonly CampoOpcaoViewModel _rat = new("RAT", [new("1% (risco leve)", 1m), new("2% (risco médio)", 2m), new("3% (risco grave)", 3m)], "Risco ambiental do trabalho, conforme a atividade da empresa.");
    private readonly CampoTextoViewModel _fap = new("FAP", TipoCampo.Numero, "1,0000", "Fator acidentário de prevenção, de 0,5 a 2, informado anualmente à empresa.");
    private readonly CampoTextoViewModel _terceiros = new("Terceiros (%)", TipoCampo.Numero, "5,8", "Sistema S, salário-educação e Incra; 5,8% é o mais comum.");
    private readonly CampoTextoViewModel _beneficios = Moeda("Benefícios", "Custo mensal com vale-transporte, alimentação e planos, já descontada a parte do empregado.");
    private readonly CampoOpcaoViewModel _provisoes = CampoOpcaoViewModel.SimNao("Incluir provisões", true, "Distribui o 13º e as férias com 1/3 mês a mês.");

    public CalculadoraCustoFuncionario(ISimularDemonstrativoUseCase<SimularCustoFuncionarioRequest> simulador)
    {
        _simulador = simulador;
        _rat.Selecionada = _rat.Opcoes[1];
        _regime.AoAlterar = () =>
        {
            var regime = _regime.Valor<RegimeTributario>();
            _rat.Visivel = _fap.Visivel = regime != RegimeTributario.SimplesNacional;
            _terceiros.Visivel = regime == RegimeTributario.LucroRealOuPresumido;
        };
        _regime.AoAlterar();
    }

    public override string Titulo => "Custo do funcionário";
    public override string Descricao => "Calcule quanto um empregado custa para a empresa, com encargos, provisões e benefícios.";
    public override string InstrucaoInicial => "Informe o salário, o regime da empresa e os benefícios e selecione Calcular.";
    public override IReadOnlyList<CampoViewModel> Campos => [_salario, _regime, _rat, _fap, _terceiros, _beneficios, _provisoes];
    public override string NomeArquivoPdf => "custo-do-funcionario.pdf";

    public override void Preencher(ContextoCalculo contexto) => PreencherMoeda(_salario, contexto.Salario);

    public override Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken) =>
        _simulador.ExecutarAsync(new SimularCustoFuncionarioRequest(
            LerMoeda(_salario), _regime.Valor<RegimeTributario>(), _rat.Valor<decimal>(),
            _fap.Visivel ? LerNumero(_fap) : 1m, _terceiros.Visivel ? LerNumero(_terceiros) : 0m,
            LerMoeda(_beneficios), _provisoes.Valor<bool>()), cancellationToken);
}

public sealed class CalculadoraProLabore : CalculadoraBase
{
    private readonly ISimularDemonstrativoUseCase<SimularProLaboreRequest> _simulador;
    private readonly CampoTextoViewModel _competencia = Competencia();
    private readonly CampoOpcaoViewModel _tipo = new("Tipo",
        [new("Pró-labore (sócio)", TipoContribuinteIndividual.ProLabore), new("Autônomo (RPA)", TipoContribuinteIndividual.Autonomo)],
        "Pró-labore é a remuneração do sócio; RPA é o recibo de pagamento a autônomo.");
    private readonly CampoTextoViewModel _valor = Moeda("Valor bruto", "Pró-labore ou valor do serviço.");
    private readonly CampoTextoViewModel _dependentes = Inteiro("Dependentes", 0, "Dependentes para a dedução do IRRF.");
    private readonly CampoTextoViewModel _iss = new("ISS retido (%)", TipoCampo.Numero, "0", "Alíquota do ISS do município, de 2% a 5%, quando a lei municipal exige a retenção; 0 para não reter.");
    private readonly CampoOpcaoViewModel _regime = new("Regime da empresa",
        [
            new("Lucro Real ou Presumido", RegimeTributario.LucroRealOuPresumido),
            new("Simples (anexos I a III e V)", RegimeTributario.SimplesNacional),
            new("Simples (anexo IV)", RegimeTributario.SimplesNacionalAnexoIV)
        ], "Define se a empresa paga o INSS patronal de 20%.");

    public CalculadoraProLabore(ISimularDemonstrativoUseCase<SimularProLaboreRequest> simulador)
    {
        _simulador = simulador;
        _tipo.AoAlterar = () => _iss.Visivel = _tipo.Valor<TipoContribuinteIndividual>() == TipoContribuinteIndividual.Autonomo;
        _tipo.AoAlterar();
    }

    public override string Titulo => "Pró-labore e autônomo";
    public override string Descricao => "Calcule o líquido do pró-labore do sócio ou do pagamento a autônomo (RPA) e o custo para a empresa.";
    public override string InstrucaoInicial => "Informe a competência, o tipo, o valor bruto e os dependentes e selecione Calcular.";
    public override IReadOnlyList<CampoViewModel> Campos => [_competencia, _tipo, _valor, _dependentes, _iss, _regime];
    public override string NomeArquivoPdf => $"{(_tipo.Valor<TipoContribuinteIndividual>() == TipoContribuinteIndividual.Autonomo ? "rpa" : "pro-labore")}-{_competencia.Valor.Replace('/', '-')}.pdf";

    public override void Preencher(ContextoCalculo contexto)
    {
        if (contexto.Competencia is { } competencia) _competencia.Valor = competencia.ToString("MM/yyyy", Cultura);
        PreencherMoeda(_valor, contexto.Salario);
        if (contexto.Dependentes is { } dependentes) _dependentes.Valor = dependentes.ToString(Cultura);
    }

    public override Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken) =>
        _simulador.ExecutarAsync(new SimularProLaboreRequest(
            LerCompetencia(_competencia), _tipo.Valor<TipoContribuinteIndividual>(), LerMoeda(_valor), LerInteiro(_dependentes),
            _iss.Visivel ? LerNumero(_iss) : 0m, _regime.Valor<RegimeTributario>()), cancellationToken);
}
