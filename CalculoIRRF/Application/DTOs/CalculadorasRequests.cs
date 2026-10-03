namespace CalculoIRRF.Application.DTOs;

public sealed record SimularSalarioPeloLiquidoRequest(DateOnly Competencia, decimal LiquidoDesejado, int Dependentes);

public enum AdiantamentoDecimoTerceiro { CinquentaPorCento, SemAdiantamento, ValorInformado }

/// <param name="Avos">Meses do ano com 15 dias ou mais de trabalho, de 1 a 12.</param>
/// <param name="ValorAdiantamento">Usado apenas quando <paramref name="Adiantamento"/> é <see cref="AdiantamentoDecimoTerceiro.ValorInformado"/>.</param>
public sealed record SimularDecimoTerceiroRequest(
    DateOnly Competencia,
    decimal Salario,
    decimal Medias,
    int Avos,
    int Dependentes,
    AdiantamentoDecimoTerceiro Adiantamento,
    decimal ValorAdiantamento);

/// <param name="Faltas">Faltas injustificadas no período aquisitivo, que definem os dias de direito.</param>
/// <param name="DiasGozo">Dias de descanso; zero usa todos os dias de direito que não forem vendidos.</param>
public sealed record SimularFeriasRequest(
    DateOnly Competencia,
    decimal Salario,
    decimal Medias,
    int Faltas,
    int DiasGozo,
    bool VenderAbono,
    bool AdiantarDecimoTerceiro,
    int Dependentes);

public enum MotivoRescisao { DispensaSemJustaCausa, PedidoDeDemissao, Acordo, DispensaPorJustaCausa, TerminoDeContratoPorPrazo }

public enum CumprimentoAvisoPrevio { Indenizado, TrabalhadoOuDispensado, NaoCumpridoPeloEmpregado }

/// <param name="Desligamento">Último dia de trabalho; com aviso trabalhado, o fim do aviso.</param>
/// <param name="FaltasPeriodoAtual">Faltas injustificadas no período aquisitivo em curso, que reduzem as férias proporcionais.</param>
/// <param name="SaldoFgts">Saldo da conta do FGTS para fins rescisórios; zero estima pelo salário atual.</param>
/// <param name="AdiantamentoDecimoTerceiro">1ª parcela do 13º já paga no ano, descontada na rescisão.</param>
public sealed record SimularRescisaoRequest(
    DateOnly Admissao,
    DateOnly Desligamento,
    MotivoRescisao Motivo,
    CumprimentoAvisoPrevio Aviso,
    decimal Salario,
    decimal Medias,
    int PeriodosFeriasVencidas,
    int FaltasPeriodoAtual,
    decimal SaldoFgts,
    decimal AdiantamentoDecimoTerceiro,
    int Dependentes);

/// <summary>Regime da empresa, que define a contribuição patronal sobre a folha.</summary>
public enum RegimeTributario { LucroRealOuPresumido, SimplesNacional, SimplesNacionalAnexoIV }

/// <param name="Rat">Alíquota do risco ambiental do trabalho (RAT), de 1% a 3%, antes do FAP.</param>
/// <param name="Fap">Fator acidentário de prevenção, de 0,5 a 2.</param>
/// <param name="Terceiros">Contribuições a terceiros (Sistema S, salário-educação, Incra), normalmente 5,8%.</param>
/// <param name="Beneficios">Custo mensal da empresa com benefícios, já descontada a parte do empregado.</param>
public sealed record SimularCustoFuncionarioRequest(
    decimal Salario,
    RegimeTributario Regime,
    decimal Rat,
    decimal Fap,
    decimal Terceiros,
    decimal Beneficios,
    bool IncluirProvisoes);

public enum TipoContribuinteIndividual { ProLabore, Autonomo }

/// <param name="AliquotaIss">ISS retido do autônomo, quando a lei municipal exige; zero para não reter.</param>
public sealed record SimularProLaboreRequest(
    DateOnly Competencia,
    TipoContribuinteIndividual Tipo,
    decimal Valor,
    int Dependentes,
    decimal AliquotaIss,
    RegimeTributario Regime);

/// <param name="PlrAnterior">PLR já paga no mesmo ano; o imposto é recalculado sobre o total do ano (Lei 10.101/2000, art. 3º, § 7º).</param>
/// <param name="ImpostoRetidoAnterior">IRRF já retido sobre a PLR anterior, descontado do imposto recalculado.</param>
/// <param name="PensaoAlimenticia">Pensão alimentícia judicial descontada desta PLR, que reduz a base do imposto.</param>
public sealed record SimularPlrRequest(
    DateOnly Competencia,
    decimal Valor,
    decimal PlrAnterior,
    decimal ImpostoRetidoAnterior,
    decimal PensaoAlimenticia);

/// <param name="Remuneracao">Remuneração do mês, comparada ao limite do salário-família.</param>
/// <param name="Filhos">Filhos ou equiparados de até 14 anos, ou inválidos de qualquer idade.</param>
/// <param name="DiasTrabalhados">Dias do mês; nos meses de admissão e desligamento a cota é proporcional.</param>
public sealed record SimularSalarioFamiliaRequest(DateOnly Competencia, decimal Remuneracao, int Filhos, int DiasTrabalhados);

public enum GrauInsalubridade { Nenhum = 0, Minimo = 10, Medio = 20, Maximo = 40 }

public enum BaseInsalubridade { SalarioMinimo, Salario, ValorInformado }

/// <param name="ValorBaseInformado">Piso da categoria ou outra base prevista em convenção; usado com <see cref="BaseInsalubridade.ValorInformado"/>.</param>
public sealed record SimularAdicionaisRequest(
    DateOnly Competencia,
    decimal Salario,
    GrauInsalubridade Grau,
    BaseInsalubridade Base,
    decimal ValorBaseInformado,
    bool Periculosidade,
    int Dependentes);

/// <param name="HorasNoturnas">Horas de relógio trabalhadas entre 22h e 5h, convertidas para a hora noturna reduzida.</param>
public sealed record SimularHorasExtrasRequest(
    DateOnly Competencia,
    decimal Salario,
    decimal Divisor,
    decimal HorasFaixa1,
    decimal PercentualFaixa1,
    decimal HorasFaixa2,
    decimal PercentualFaixa2,
    decimal HorasNoturnas,
    decimal PercentualNoturno,
    int Feriados,
    int Dependentes);
