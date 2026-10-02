#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;
using CalculoIRRF.Domain.Trabalhista;

namespace CalculoIRRF.Application.UseCases;

/// <summary>
/// Salário do mês com horas extras, adicional noturno e o reflexo das duas no descanso semanal remunerado (DSR),
/// com o INSS e o IRRF da remuneração total.
/// </summary>
public sealed class SimularHorasExtrasUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularDemonstrativoUseCase<SimularHorasExtrasRequest>
{
    // CF, art. 7º, XVI: a hora extra vale pelo menos 50% a mais que a normal.
    private const decimal PercentualMinimoHoraExtra = 50m;

    public async Task<DemonstrativoDto> ExecutarAsync(SimularHorasExtrasRequest request, CancellationToken cancellationToken)
    {
        if (request.Salario < 0m || request.HorasFaixa1 < 0m || request.HorasFaixa2 < 0m || request.HorasNoturnas < 0m || request.PercentualNoturno < 0m || request.Dependentes < 0)
            throw new ArgumentException("Os valores, as horas e a quantidade de dependentes não podem ser negativos.");
        if (request.Divisor <= 0m)
            throw new ArgumentException("O divisor de horas deve ser maior que zero, por exemplo 220 para 44 horas semanais.");
        if (request.PercentualFaixa1 < PercentualMinimoHoraExtra || request.PercentualFaixa2 < PercentualMinimoHoraExtra)
            throw new ArgumentException("O adicional de horas extras deve ser de pelo menos 50% (Constituição Federal, art. 7º, XVI).");

        var (diasUteis, diasDescanso) = RegrasTrabalhistas.DiasParaDsr(request.Competencia, request.Feriados);
        var tabelas = await TabelasDaCompetencia.ObterAsync(tributacaoConsulta, request.Competencia, cancellationToken);

        var valorHora = request.Salario / request.Divisor;
        var faixa1 = CalculadoraTributacao.Arredondar(valorHora * (1m + request.PercentualFaixa1 / 100m) * request.HorasFaixa1);
        var faixa2 = CalculadoraTributacao.Arredondar(valorHora * (1m + request.PercentualFaixa2 / 100m) * request.HorasFaixa2);
        var horasNoturnasReduzidas = RegrasTrabalhistas.HorasNoturnasReduzidas(request.HorasNoturnas);
        var adicionalNoturno = CalculadoraTributacao.Arredondar(valorHora * request.PercentualNoturno / 100m * horasNoturnasReduzidas);
        var variaveis = faixa1 + faixa2 + adicionalNoturno;
        var dsr = CalculadoraTributacao.Arredondar(variaveis / diasUteis * diasDescanso);
        var remuneracao = request.Salario + variaveis + dsr;

        var inss = tabelas.CalcularInss(remuneracao);
        var irrf = tabelas.CalcularIrrf(remuneracao, inss.Valor, request.Dependentes);
        var fgts = CalculadoraTributacao.Arredondar(remuneracao * .08m);
        var liquido = remuneracao - inss.Valor - irrf.Imposto;

        var proventos = new List<VerbaDto> { new("Salário", "", request.Salario) };
        if (faixa1 > 0m) proventos.Add(new($"Horas extras {Formato.PercentualCurto(request.PercentualFaixa1)}", Formato.Horas(request.HorasFaixa1), faixa1));
        if (faixa2 > 0m) proventos.Add(new($"Horas extras {Formato.PercentualCurto(request.PercentualFaixa2)}", Formato.Horas(request.HorasFaixa2), faixa2));
        if (adicionalNoturno > 0m) proventos.Add(new($"Adicional noturno {Formato.PercentualCurto(request.PercentualNoturno)}", Formato.Horas(horasNoturnasReduzidas), adicionalNoturno));
        if (dsr > 0m) proventos.Add(new("DSR sobre horas extras e adicional noturno", Formato.Dias(diasDescanso), dsr));

        var valorHoraTexto = $"{Formato.Moeda(request.Salario)} ÷ {Formato.Numero(request.Divisor)} horas = {valorHora.ToString("C4", Formato.Cultura)} por hora";
        var formulas = new List<FormulaDto> { new("Valor da hora normal", valorHoraTexto) };
        if (request.HorasFaixa1 > 0m)
            formulas.Add(new($"Horas extras {Formato.PercentualCurto(request.PercentualFaixa1)}", $"{valorHora.ToString("C4", Formato.Cultura)} x {Formato.Numero(1m + request.PercentualFaixa1 / 100m)} x {Formato.Horas(request.HorasFaixa1)} = {Formato.Moeda(faixa1)}"));
        if (request.HorasFaixa2 > 0m)
            formulas.Add(new($"Horas extras {Formato.PercentualCurto(request.PercentualFaixa2)}", $"{valorHora.ToString("C4", Formato.Cultura)} x {Formato.Numero(1m + request.PercentualFaixa2 / 100m)} x {Formato.Horas(request.HorasFaixa2)} = {Formato.Moeda(faixa2)}"));
        if (request.HorasNoturnas > 0m)
        {
            formulas.Add(new("Hora noturna reduzida", $"{Formato.Horas(request.HorasNoturnas)} de relógio x 60 ÷ 52,5 = {Formato.Horas(horasNoturnasReduzidas)} noturnas"));
            formulas.Add(new("Adicional noturno", $"{valorHora.ToString("C4", Formato.Cultura)} x {Formato.Percentual(request.PercentualNoturno)} x {Formato.Horas(horasNoturnasReduzidas)} = {Formato.Moeda(adicionalNoturno)}"));
        }
        formulas.Add(new("Dias do mês", $"{diasUteis} dias úteis e {diasDescanso} de descanso (domingos{(request.Feriados > 0 ? $" e {request.Feriados} feriado(s)" : "")})"));
        if (variaveis > 0m)
            formulas.Add(new("DSR", $"{Formato.Moeda(variaveis)} (horas extras e adicional) ÷ {diasUteis} dias úteis x {diasDescanso} dias de descanso = {Formato.Moeda(dsr)}"));
        formulas.Add(new("Remuneração do mês", $"{string.Join(" + ", proventos.Select(verba => Formato.Moeda(verba.Valor)))} = {Formato.Moeda(remuneracao)}"));

        return new DemonstrativoDto(
            "Horas extras e adicionais",
            $"Competência {Formato.Competencia(tabelas.Competencia)}",
            [
                new("Horas extras e adicionais", Formato.Moeda(variaveis + dsr), "Incluindo o reflexo no DSR"),
                new("Remuneração do mês", Formato.Moeda(remuneracao), $"Salário de {Formato.Moeda(request.Salario)}"),
                new("Salário líquido", Formato.Moeda(liquido), "Remuneração menos INSS e IRRF"),
                new("Valor da hora", Formato.Moeda(valorHora), $"Divisor {Formato.Numero(request.Divisor)}")
            ],
            proventos,
            [
                new("INSS", "", inss.Valor),
                new(MemoriaTributaria.DescricaoIrrf("IRRF", irrf), MemoriaTributaria.ReferenciaIrrf(irrf), irrf.Imposto)
            ],
            [new("FGTS", "8%", fgts)],
            [
                new GrupoMemoriaDto("Horas extras, adicional noturno e DSR", $"Remuneração: {Formato.Moeda(remuneracao)}", formulas),
                MemoriaTributaria.Inss("INSS", inss, "remuneração do mês"),
                MemoriaTributaria.Irrf("IRRF", irrf, "remuneração do mês")
            ],
            [
                "O divisor 220 corresponde a 44 horas semanais; use 200 para 40 horas, 180 para 36 e 150 para 30.",
                "A hora noturna urbana, das 22h às 5h, tem 52 minutos e 30 segundos (CLT, art. 73); as horas de relógio informadas foram convertidas.",
                "O DSR considera os domingos do mês e os feriados informados. Convenções coletivas podem prever adicionais maiores."
            ]);
    }
}
