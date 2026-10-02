namespace CalculoIRRF.Domain.Trabalhista;

/// <summary>Regras da CLT e da legislação trabalhista usadas pelas calculadoras.</summary>
public static class RegrasTrabalhistas
{
    /// <summary>Dias de férias conforme as faltas injustificadas no período aquisitivo (CLT, art. 130).</summary>
    public static int DiasDeFeriasPorFaltas(int faltas) => faltas switch
    {
        < 0 => throw new ArgumentOutOfRangeException(nameof(faltas), "As faltas não podem ser negativas."),
        <= 5 => 30,
        <= 14 => 24,
        <= 23 => 18,
        <= 32 => 12,
        _ => 0
    };

    /// <summary>O abono pecuniário converte em dinheiro até 1/3 dos dias de férias (CLT, art. 143).</summary>
    public static int DiasDeAbonoPecuniario(int diasDeDireito) => diasDeDireito / 3;

    /// <summary>A hora noturna urbana tem 52 minutos e 30 segundos (CLT, art. 73, § 1º): 7 horas de relógio valem 8 horas noturnas.</summary>
    public static decimal HorasNoturnasReduzidas(decimal horasDeRelogio) => horasDeRelogio * 8m / 7m;

    /// <summary>Dias úteis e dias de descanso (domingos e feriados) do mês, usados no reflexo das horas extras no DSR.</summary>
    public static (int DiasUteis, int DiasDeDescanso) DiasParaDsr(DateOnly competencia, int feriados)
    {
        if (feriados < 0)
            throw new ArgumentOutOfRangeException(nameof(feriados), "Os feriados não podem ser negativos.");

        var diasNoMes = DateTime.DaysInMonth(competencia.Year, competencia.Month);
        var domingos = Enumerable.Range(1, diasNoMes).Count(dia => new DateOnly(competencia.Year, competencia.Month, dia).DayOfWeek == DayOfWeek.Sunday);
        var descanso = domingos + feriados;
        if (descanso >= diasNoMes)
            throw new ArgumentException("Os feriados informados deixam o mês sem dias úteis.", nameof(feriados));

        return (diasNoMes - descanso, descanso);
    }

    /// <summary>Aviso prévio proporcional: 30 dias mais 3 por ano completo de serviço, até 90 dias (Lei 12.506/2011).</summary>
    public static int DiasDeAvisoPrevio(DateOnly admissao, DateOnly desligamento) => Math.Min(90, 30 + 3 * AnosCompletos(admissao, desligamento));

    public static int AnosCompletos(DateOnly inicio, DateOnly fim)
    {
        var anos = fim.Year - inicio.Year;
        if (fim < inicio.AddYears(anos))
            anos--;
        return Math.Max(0, anos);
    }

    /// <summary>
    /// Avos de 13º no ano civil: cada mês com 15 dias ou mais de trabalho conta 1/12 (Lei 4.090/1962, art. 1º, § 2º).
    /// </summary>
    public static int AvosDecimoTerceiro(DateOnly inicioTrabalho, DateOnly fimTrabalho, int ano)
    {
        var inicio = inicioTrabalho > new DateOnly(ano, 1, 1) ? inicioTrabalho : new DateOnly(ano, 1, 1);
        var fim = fimTrabalho < new DateOnly(ano, 12, 31) ? fimTrabalho : new DateOnly(ano, 12, 31);
        if (fim < inicio)
            return 0;

        var avos = 0;
        for (var mes = inicio.Month; mes <= fim.Month; mes++)
        {
            var primeiroDia = new DateOnly(ano, mes, 1);
            var ultimoDia = primeiroDia.AddMonths(1).AddDays(-1);
            var diasTrabalhados = (fim < ultimoDia ? fim : ultimoDia).DayNumber - (inicio > primeiroDia ? inicio : primeiroDia).DayNumber + 1;
            if (diasTrabalhados >= 15)
                avos++;
        }
        return avos;
    }

    /// <summary>
    /// Avos de férias desde o início do período aquisitivo: cada mês completo conta 1/12, e a fração de 15 dias ou mais
    /// também (CLT, art. 146, parágrafo único). Passa de 12 quando o período termina antes da data final.
    /// </summary>
    public static int AvosFerias(DateOnly inicioPeriodo, DateOnly fim)
    {
        if (fim < inicioPeriodo)
            return 0;

        var meses = 0;
        while (inicioPeriodo.AddMonths(meses + 1).AddDays(-1) <= fim)
            meses++;
        var diasRestantes = fim.DayNumber - inicioPeriodo.AddMonths(meses).DayNumber + 1;
        return meses + (diasRestantes >= 15 ? 1 : 0);
    }

    /// <summary>Início do período aquisitivo em curso na data: o último aniversário da admissão.</summary>
    public static DateOnly InicioPeriodoAquisitivo(DateOnly admissao, DateOnly data) => admissao.AddYears(AnosCompletos(admissao, data));
}
