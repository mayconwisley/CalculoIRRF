#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

/// <summary>
/// 13º salário (Leis 4.090/1962 e 4.749/1965): a 1ª parcela, paga até 30/11, não tem descontos; a 2ª, paga até 20/12,
/// desconta a 1ª, o INSS e o IRRF, ambos calculados sobre o valor integral e à parte do salário do mês.
/// </summary>
public sealed class SimularDecimoTerceiroUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularDemonstrativoUseCase<SimularDecimoTerceiroRequest>
{
    public async Task<DemonstrativoDto> ExecutarAsync(SimularDecimoTerceiroRequest request, CancellationToken cancellationToken)
    {
        if (request.Salario < 0m || request.Medias < 0m || request.Dependentes < 0 || request.ValorAdiantamento < 0m)
            throw new ArgumentException("Os valores e a quantidade de dependentes não podem ser negativos.");
        if (request.Avos is < 1 or > 12)
            throw new ArgumentException("Os avos devem estar entre 1 e 12: um para cada mês com 15 dias ou mais de trabalho no ano.");

        var tabelas = await TabelasDaCompetencia.ObterAsync(tributacaoConsulta, request.Competencia, cancellationToken);
        var remuneracao = request.Salario + request.Medias;
        var integral = CalculadoraTributacao.Arredondar(remuneracao / 12m * request.Avos);
        var adiantamento = request.Adiantamento switch
        {
            AdiantamentoDecimoTerceiro.CinquentaPorCento => CalculadoraTributacao.Arredondar(integral / 2m),
            AdiantamentoDecimoTerceiro.SemAdiantamento => 0m,
            _ => request.ValorAdiantamento
        };
        if (adiantamento > integral)
            throw new ArgumentException($"O adiantamento informado ({Formato.Moeda(adiantamento)}) é maior que o 13º integral ({Formato.Moeda(integral)}).");

        var inss = tabelas.CalcularInss(integral);
        var irrf = tabelas.CalcularIrrf(integral, inss.Valor, request.Dependentes);
        var segundaParcela = integral - adiantamento - inss.Valor - irrf.Imposto;
        var liquidoTotal = integral - inss.Valor - irrf.Imposto;
        var fgts = CalculadoraTributacao.Arredondar(integral * .08m);

        var formulaIntegral = request.Medias > 0m
            ? $"({Formato.Moeda(request.Salario)} (salário) + {Formato.Moeda(request.Medias)} (médias)) ÷ 12 x {request.Avos} avos = {Formato.Moeda(integral)}"
            : $"{Formato.Moeda(request.Salario)} (salário) ÷ 12 x {request.Avos} avos = {Formato.Moeda(integral)}";
        var formulaAdiantamento = request.Adiantamento switch
        {
            AdiantamentoDecimoTerceiro.CinquentaPorCento => $"{Formato.Moeda(integral)} x 50% = {Formato.Moeda(adiantamento)}",
            AdiantamentoDecimoTerceiro.SemAdiantamento => "Não houve adiantamento: o 13º é pago de uma vez, em dezembro.",
            _ => $"Valor informado: {Formato.Moeda(adiantamento)}"
        };
        var descricaoAdiantamento = request.Adiantamento == AdiantamentoDecimoTerceiro.SemAdiantamento ? "Pagamento único" : "Até 30/11, sem descontos";

        var descontos = new List<VerbaDto>();
        if (adiantamento > 0m)
            descontos.Add(new("Adiantamento do 13º (1ª parcela)", request.Adiantamento == AdiantamentoDecimoTerceiro.CinquentaPorCento ? "50%" : "", adiantamento));
        descontos.Add(new("INSS sobre o 13º", "", inss.Valor));
        descontos.Add(new(MemoriaTributaria.DescricaoIrrf("IRRF sobre o 13º", irrf), MemoriaTributaria.ReferenciaIrrf(irrf), irrf.Imposto));

        return new DemonstrativoDto(
            "13º salário",
            $"Competência {Formato.Competencia(tabelas.Competencia)} • {Formato.Avos(request.Avos)}",
            [
                new("1ª parcela", Formato.Moeda(adiantamento), descricaoAdiantamento),
                new("2ª parcela líquida", Formato.Moeda(segundaParcela), "Até 20/12, com INSS e IRRF"),
                new("13º líquido total", Formato.Moeda(liquidoTotal), $"Integral de {Formato.Moeda(integral)}"),
                new("FGTS (8%)", Formato.Moeda(fgts), "Depositado pelo empregador")
            ],
            [new("13º salário integral", Formato.Avos(request.Avos), integral)],
            descontos,
            [new("FGTS sobre o 13º", "8%", fgts)],
            [
                new GrupoMemoriaDto("13º salário", $"2ª parcela: {Formato.Moeda(segundaParcela)}",
                [
                    new("Valor integral", formulaIntegral),
                    new("1ª parcela", formulaAdiantamento),
                    new("2ª parcela", $"{Formato.Moeda(integral)} (integral) - {Formato.Moeda(adiantamento)} (1ª parcela) - {Formato.Moeda(inss.Valor)} (INSS) - {Formato.Moeda(irrf.Imposto)} (IRRF) = {Formato.Moeda(segundaParcela)}")
                ]),
                MemoriaTributaria.Inss("INSS sobre o 13º", inss, "13º integral"),
                MemoriaTributaria.Irrf("IRRF sobre o 13º", irrf, "13º integral")
            ],
            [
                "Cada mês do ano com 15 dias ou mais de trabalho dá direito a 1/12 do 13º.",
                "O INSS e o IRRF do 13º são calculados sobre o valor integral, à parte do salário de dezembro; o IRRF do 13º é de tributação exclusiva na fonte.",
                "O FGTS é depositado sobre cada parcela no mês em que ela é paga."
            ]);
    }
}
