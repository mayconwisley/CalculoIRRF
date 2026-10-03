#nullable enable

using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Application.UseCases;

namespace CalculoIRRF.Presentation.ViewModels.Calculadoras;

public sealed class CalculadoraPlr(ISimularDemonstrativoUseCase<SimularPlrRequest> simulador) : CalculadoraBase
{
    private readonly CampoTextoViewModel _competencia = Competencia("Competência do pagamento", "Mês do pagamento da PLR, que define a tabela anual usada (MM/AAAA).");
    private readonly CampoTextoViewModel _valor = Moeda("Valor da PLR", "Valor bruto desta parcela da participação nos lucros ou resultados.");
    private readonly CampoTextoViewModel _anterior = Moeda("PLR já paga no ano", "Outra parcela paga no mesmo ano; o imposto é recalculado sobre o total.");
    private readonly CampoTextoViewModel _impostoAnterior = Moeda("IRRF já retido no ano", "Imposto retido na parcela anterior, que é descontado do imposto recalculado.");
    private readonly CampoTextoViewModel _pensao = Moeda("Pensão sobre a PLR", "Pensão alimentícia judicial descontada desta PLR, que reduz a base do imposto.");

    public override string Titulo => "PLR (participação nos lucros)";
    public override string Descricao => "Calcule o IRRF da participação nos lucros ou resultados pela tabela anual exclusiva, sem INSS e sem FGTS.";
    public override string InstrucaoInicial => "Informe a competência do pagamento e o valor da PLR e selecione Calcular.";
    public override IReadOnlyList<CampoViewModel> Campos => [_competencia, _valor, _anterior, _impostoAnterior, _pensao];
    public override string NomeArquivoPdf => $"plr-{_competencia.Valor.Replace('/', '-')}.pdf";

    public override void Preencher(ContextoCalculo contexto)
    {
        if (contexto.Competencia is { } competencia) _competencia.Valor = competencia.ToString("MM/yyyy", Cultura);
    }

    public override Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken) =>
        simulador.ExecutarAsync(new SimularPlrRequest(LerCompetencia(_competencia), LerMoeda(_valor), LerMoeda(_anterior), LerMoeda(_impostoAnterior), LerMoeda(_pensao)), cancellationToken);
}

public sealed class CalculadoraSalarioFamilia(ISimularDemonstrativoUseCase<SimularSalarioFamiliaRequest> simulador) : CalculadoraBase
{
    private readonly CampoTextoViewModel _competencia = Competencia(dica: "Mês do pagamento, que define o limite e a cota do salário-família (MM/AAAA).");
    private readonly CampoTextoViewModel _remuneracao = Moeda("Remuneração do mês", "Remuneração total do mês, comparada ao limite do salário-família.");
    private readonly CampoTextoViewModel _filhos = Inteiro("Filhos com direito", 1, "Filhos ou equiparados de até 14 anos, ou inválidos de qualquer idade.");
    private readonly CampoTextoViewModel _dias = Inteiro("Dias trabalhados", 30, "30 no mês completo; nos meses de admissão e desligamento, os dias trabalhados.");

    public override string Titulo => "Salário-família";
    public override string Descricao => "Verifique o direito e calcule o salário-família pela remuneração e pela quantidade de filhos.";
    public override string InstrucaoInicial => "Informe a competência, a remuneração do mês e os filhos com direito e selecione Calcular.";
    public override IReadOnlyList<CampoViewModel> Campos => [_competencia, _remuneracao, _filhos, _dias];
    public override string NomeArquivoPdf => $"salario-familia-{_competencia.Valor.Replace('/', '-')}.pdf";

    public override void Preencher(ContextoCalculo contexto)
    {
        if (contexto.Competencia is { } competencia) _competencia.Valor = competencia.ToString("MM/yyyy", Cultura);
        PreencherMoeda(_remuneracao, contexto.Salario);
    }

    public override Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken) =>
        simulador.ExecutarAsync(new SimularSalarioFamiliaRequest(LerCompetencia(_competencia), LerMoeda(_remuneracao), LerInteiro(_filhos), LerInteiro(_dias)), cancellationToken);
}

public sealed class CalculadoraAdicionais : CalculadoraBase
{
    private readonly ISimularDemonstrativoUseCase<SimularAdicionaisRequest> _simulador;
    private readonly CampoTextoViewModel _competencia = Competencia(dica: "Mês do cálculo, que define o salário mínimo e as tabelas de INSS e IRRF (MM/AAAA).");
    private readonly CampoTextoViewModel _salario = Moeda("Salário", "Salário-base, sem gratificações, prêmios ou outros adicionais.");
    private readonly CampoOpcaoViewModel _grau = new("Insalubridade",
        [
            new("Não há", GrauInsalubridade.Nenhum),
            new("Grau mínimo (10%)", GrauInsalubridade.Minimo),
            new("Grau médio (20%)", GrauInsalubridade.Medio),
            new("Grau máximo (40%)", GrauInsalubridade.Maximo)
        ], "Grau definido pelo laudo técnico, conforme a NR-15.");
    private readonly CampoOpcaoViewModel _base = new("Base da insalubridade",
        [
            new("Salário mínimo", BaseInsalubridade.SalarioMinimo),
            new("Salário", BaseInsalubridade.Salario),
            new("Valor informado", BaseInsalubridade.ValorInformado)
        ], "Salário mínimo, salvo previsão diferente em convenção coletiva, como o piso da categoria.");
    private readonly CampoTextoViewModel _valorBase = Moeda("Valor da base", "Piso da categoria ou outra base prevista em convenção coletiva.");
    private readonly CampoOpcaoViewModel _periculosidade = CampoOpcaoViewModel.SimNao("Periculosidade (30%)", false, "Atividade perigosa: inflamáveis, explosivos, energia elétrica, segurança, motocicleta e outras (CLT, art. 193).");
    private readonly CampoTextoViewModel _dependentes = Inteiro("Dependentes", 0, "Dependentes para a dedução do IRRF.");

    public CalculadoraAdicionais(ISimularDemonstrativoUseCase<SimularAdicionaisRequest> simulador)
    {
        _simulador = simulador;
        _grau.Selecionada = _grau.Opcoes[2];
        _grau.AoAlterar = _base.AoAlterar = AjustarCampos;
        AjustarCampos();
    }

    public override string Titulo => "Insalubridade e periculosidade";
    public override string Descricao => "Calcule os adicionais de insalubridade e de periculosidade, com o salário líquido do mês.";
    public override string InstrucaoInicial => "Informe o salário, o grau de insalubridade ou a periculosidade e selecione Calcular.";
    public override IReadOnlyList<CampoViewModel> Campos => [_competencia, _salario, _grau, _base, _valorBase, _periculosidade, _dependentes];
    public override string NomeArquivoPdf => $"adicionais-{_competencia.Valor.Replace('/', '-')}.pdf";

    public override void Preencher(ContextoCalculo contexto)
    {
        if (contexto.Competencia is { } competencia) _competencia.Valor = competencia.ToString("MM/yyyy", Cultura);
        PreencherMoeda(_salario, contexto.Salario);
        if (contexto.Dependentes is { } dependentes) _dependentes.Valor = dependentes.ToString(Cultura);
    }

    public override Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken) =>
        _simulador.ExecutarAsync(new SimularAdicionaisRequest(
            LerCompetencia(_competencia), LerMoeda(_salario), _grau.Valor<GrauInsalubridade>(), _base.Valor<BaseInsalubridade>(),
            _valorBase.Visivel ? LerMoeda(_valorBase) : 0m, _periculosidade.Valor<bool>(), LerInteiro(_dependentes)), cancellationToken);

    // A base só importa quando há insalubridade, e o valor só quando a base é informada.
    private void AjustarCampos()
    {
        var temInsalubridade = _grau.Valor<GrauInsalubridade>() != GrauInsalubridade.Nenhum;
        _base.Visivel = temInsalubridade;
        _valorBase.Visivel = temInsalubridade && _base.Valor<BaseInsalubridade>() == BaseInsalubridade.ValorInformado;
    }
}
