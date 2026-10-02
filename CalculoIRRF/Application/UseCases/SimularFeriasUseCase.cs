#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;
using CalculoIRRF.Domain.Trabalhista;

namespace CalculoIRRF.Application.UseCases;

/// <summary>
/// Férias com o terço constitucional, o abono pecuniário (venda de até 1/3 dos dias) e o adiantamento opcional do 13º.
/// O abono e o seu terço não têm INSS nem IRRF; o IRRF das férias é calculado à parte dos demais rendimentos do mês.
/// </summary>
public sealed class SimularFeriasUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularDemonstrativoUseCase<SimularFeriasRequest>
{
    private const int DiasMinimosDeGozo = 5;

    public async Task<DemonstrativoDto> ExecutarAsync(SimularFeriasRequest request, CancellationToken cancellationToken)
    {
        if (request.Salario < 0m || request.Medias < 0m || request.Dependentes < 0 || request.Faltas < 0 || request.DiasGozo < 0)
            throw new ArgumentException("Os valores, as faltas, os dias e a quantidade de dependentes não podem ser negativos.");

        var direito = RegrasTrabalhistas.DiasDeFeriasPorFaltas(request.Faltas);
        if (direito == 0)
            throw new ArgumentException("Com mais de 32 faltas injustificadas no período aquisitivo, não há direito a férias (CLT, art. 130).");

        var diasAbono = request.VenderAbono ? RegrasTrabalhistas.DiasDeAbonoPecuniario(direito) : 0;
        var diasDisponiveis = direito - diasAbono;
        var diasGozo = request.DiasGozo == 0 ? diasDisponiveis : request.DiasGozo;
        if (diasGozo > diasDisponiveis)
            throw new ArgumentException($"Os dias de descanso ({diasGozo}) passam dos {diasDisponiveis} dias disponíveis: {direito} de direito{(diasAbono > 0 ? $", menos {diasAbono} vendidos" : "")}.");
        if (diasGozo < DiasMinimosDeGozo)
            throw new ArgumentException("Cada período de férias deve ter pelo menos 5 dias de descanso (CLT, art. 134, § 1º).");

        var tabelas = await TabelasDaCompetencia.ObterAsync(tributacaoConsulta, request.Competencia, cancellationToken);
        var remuneracao = request.Salario + request.Medias;
        var ferias = CalculadoraTributacao.Arredondar(remuneracao / 30m * diasGozo);
        var terco = CalculadoraTributacao.Arredondar(ferias / 3m);
        var abono = CalculadoraTributacao.Arredondar(remuneracao / 30m * diasAbono);
        var tercoAbono = CalculadoraTributacao.Arredondar(abono / 3m);
        var adiantamento13 = request.AdiantarDecimoTerceiro ? CalculadoraTributacao.Arredondar(remuneracao / 2m) : 0m;

        var tributavel = ferias + terco;
        var inss = tabelas.CalcularInss(tributavel);
        var irrf = tabelas.CalcularIrrf(tributavel, inss.Valor, request.Dependentes);
        var fgts = CalculadoraTributacao.Arredondar((tributavel + adiantamento13) * .08m);

        var proventos = new List<VerbaDto>
        {
            new("Férias", Formato.Dias(diasGozo), ferias),
            new("1/3 constitucional sobre as férias", "", terco)
        };
        if (diasAbono > 0)
        {
            proventos.Add(new("Abono pecuniário", Formato.Dias(diasAbono), abono));
            proventos.Add(new("1/3 sobre o abono pecuniário", "", tercoAbono));
        }
        if (adiantamento13 > 0m)
            proventos.Add(new("Adiantamento do 13º (1ª parcela)", "50%", adiantamento13));

        var descontos = new List<VerbaDto>
        {
            new("INSS sobre as férias", "", inss.Valor),
            new(MemoriaTributaria.DescricaoIrrf("IRRF sobre as férias", irrf), MemoriaTributaria.ReferenciaIrrf(irrf), irrf.Imposto)
        };
        var liquido = proventos.Sum(verba => verba.Valor) - descontos.Sum(verba => verba.Valor);

        var formulaRemuneracao = request.Medias > 0m
            ? $"({Formato.Moeda(request.Salario)} (salário) + {Formato.Moeda(request.Medias)} (médias))"
            : $"{Formato.Moeda(request.Salario)} (salário)";
        var restante = diasGozo < diasDisponiveis ? $"; restam {Formato.Dias(diasDisponiveis - diasGozo)} para outro período" : "";
        var formulas = new List<FormulaDto>
        {
            new("Dias de direito", $"{request.Faltas} falta(s) injustificada(s) no período aquisitivo: {Formato.Dias(direito)} de férias (CLT, art. 130)"),
            new("Divisão dos dias", diasAbono > 0
                ? $"{Formato.Dias(direito)} = {Formato.Dias(diasGozo)} de descanso + {Formato.Dias(diasAbono)} vendidos{restante}"
                : $"{Formato.Dias(diasGozo)} de descanso{restante}"),
            new("Férias", $"{formulaRemuneracao} ÷ 30 x {Formato.Dias(diasGozo)} = {Formato.Moeda(ferias)}"),
            new("1/3 constitucional", $"{Formato.Moeda(ferias)} ÷ 3 = {Formato.Moeda(terco)}")
        };
        if (diasAbono > 0)
        {
            formulas.Add(new("Abono pecuniário", $"{formulaRemuneracao} ÷ 30 x {Formato.Dias(diasAbono)} = {Formato.Moeda(abono)}"));
            formulas.Add(new("1/3 sobre o abono", $"{Formato.Moeda(abono)} ÷ 3 = {Formato.Moeda(tercoAbono)}"));
        }
        if (adiantamento13 > 0m)
            formulas.Add(new("Adiantamento do 13º", $"{formulaRemuneracao} x 50% = {Formato.Moeda(adiantamento13)}"));
        formulas.Add(new("Líquido", $"{Formato.Moeda(proventos.Sum(verba => verba.Valor))} (proventos) - {Formato.Moeda(inss.Valor)} (INSS) - {Formato.Moeda(irrf.Imposto)} (IRRF) = {Formato.Moeda(liquido)}"));

        var observacoes = new List<string>
        {
            "O abono pecuniário e o seu terço não têm INSS, IRRF nem FGTS.",
            "O IRRF das férias é calculado à parte dos demais rendimentos do mês. O INSS foi calculado só sobre as férias; na folha, ele é somado ao do salário do mês, respeitando o teto.",
            "As férias devem ser pagas até 2 dias antes do início do descanso (CLT, art. 145)."
        };
        if (adiantamento13 > 0m)
            observacoes.Add("O adiantamento do 13º é pago sem descontos; o INSS e o IRRF são descontados na 2ª parcela, em dezembro.");

        return new DemonstrativoDto(
            "Férias",
            $"Competência {Formato.Competencia(tabelas.Competencia)} • {Formato.Dias(diasGozo)} de descanso",
            [
                new("Líquido das férias", Formato.Moeda(liquido), "Proventos menos INSS e IRRF"),
                new("Férias + 1/3", Formato.Moeda(tributavel), $"{Formato.Dias(diasGozo)} de descanso"),
                new("Abono + 1/3", Formato.Moeda(abono + tercoAbono), diasAbono > 0 ? $"{Formato.Dias(diasAbono)} vendidos, sem impostos" : "Sem venda de dias"),
                new("FGTS (8%)", Formato.Moeda(fgts), "Depositado pelo empregador")
            ],
            proventos,
            descontos,
            [new("FGTS sobre férias + 1/3" + (adiantamento13 > 0m ? " e adiantamento do 13º" : ""), "8%", fgts)],
            [
                new GrupoMemoriaDto("Férias", $"Líquido: {Formato.Moeda(liquido)}", formulas),
                MemoriaTributaria.Inss("INSS sobre as férias", inss, "férias + 1/3"),
                MemoriaTributaria.Irrf("IRRF sobre as férias", irrf, "férias + 1/3")
            ],
            observacoes);
    }
}
