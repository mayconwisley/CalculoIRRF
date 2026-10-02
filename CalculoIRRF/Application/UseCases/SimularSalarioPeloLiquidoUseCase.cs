#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

/// <summary>Encontra o salário bruto que, descontados INSS e IRRF, resulta no líquido desejado.</summary>
public sealed class SimularSalarioPeloLiquidoUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularDemonstrativoUseCase<SimularSalarioPeloLiquidoRequest>
{
    // O arredondamento em centavos do INSS e do IRRF pode fazer o líquido oscilar um centavo entre brutos vizinhos;
    // por isso a busca procura o valor exato ao redor do resultado aproximado.
    private const long VizinhancaEmCentavos = 300;

    public async Task<DemonstrativoDto> ExecutarAsync(SimularSalarioPeloLiquidoRequest request, CancellationToken cancellationToken)
    {
        if (request.LiquidoDesejado < 0m || request.Dependentes < 0)
            throw new ArgumentException("Informe um líquido e uma quantidade de dependentes maiores ou iguais a zero.");

        var tabelas = await TabelasDaCompetencia.ObterAsync(tributacaoConsulta, request.Competencia, cancellationToken);
        decimal Liquido(long centavos) => Apurar(tabelas, centavos / 100m, request.Dependentes).Liquido;

        var desejado = (long)Math.Round(request.LiquidoDesejado * 100m, MidpointRounding.AwayFromZero);
        var minimo = desejado;
        var maximo = Math.Max(desejado * 3, 100_000);
        while (Liquido(maximo) < request.LiquidoDesejado)
            maximo *= 2;

        // O líquido cresce com o bruto: a busca binária encontra o menor bruto cujo líquido alcança o desejado.
        while (minimo < maximo)
        {
            var meio = minimo + (maximo - minimo) / 2;
            if (Liquido(meio) >= request.LiquidoDesejado) maximo = meio; else minimo = meio + 1;
        }

        var candidatos = Enumerable.Range(0, (int)(2 * VizinhancaEmCentavos + 1))
            .Select(deslocamento => Math.Max(0, minimo - VizinhancaEmCentavos + deslocamento))
            .Distinct()
            .Select(centavos => (Centavos: centavos, Liquido: Liquido(centavos)))
            .ToArray();
        var escolhido = candidatos.Where(item => item.Liquido == request.LiquidoDesejado).Select(item => (long?)item.Centavos).FirstOrDefault()
            ?? candidatos.Where(item => item.Liquido >= request.LiquidoDesejado).OrderBy(item => item.Liquido).ThenBy(item => item.Centavos).First().Centavos;

        var apuracao = Apurar(tabelas, escolhido / 100m, request.Dependentes);
        return Montar(request, tabelas, apuracao);
    }

    private static Apuracao Apurar(TabelasDaCompetencia tabelas, decimal bruto, int dependentes)
    {
        var inss = tabelas.CalcularInss(bruto);
        var irrf = tabelas.CalcularIrrf(bruto, inss.Valor, dependentes);
        return new Apuracao(bruto, inss, irrf, bruto - inss.Valor - irrf.Imposto);
    }

    private static DemonstrativoDto Montar(SimularSalarioPeloLiquidoRequest request, TabelasDaCompetencia tabelas, Apuracao apuracao)
    {
        var diferenca = apuracao.Liquido - request.LiquidoDesejado;
        var fgts = CalculadoraTributacao.Arredondar(apuracao.Bruto * .08m);
        var observacoes = new List<string>
        {
            "Considera que todo o salário é base de INSS e de IRRF, sem outros descontos. Para incluir descontos fixos, como vale-transporte ou plano de saúde, some-os ao líquido desejado.",
            "O IRRF usa a modalidade mais vantajosa para o trabalhador: deduções legais ou desconto simplificado."
        };
        if (diferenca != 0m)
            observacoes.Insert(0, $"Por causa do arredondamento em centavos do INSS e do IRRF, nenhum salário resulta exatamente em {Formato.Moeda(request.LiquidoDesejado)}; o mais próximo resulta em {Formato.Moeda(apuracao.Liquido)}.");

        return new DemonstrativoDto(
            "Salário bruto a partir do líquido",
            $"Competência {Formato.Competencia(tabelas.Competencia)}",
            [
                new("Salário bruto necessário", Formato.Moeda(apuracao.Bruto), $"Para receber {Formato.Moeda(request.LiquidoDesejado)}"),
                new("Líquido obtido", Formato.Moeda(apuracao.Liquido), diferenca == 0m ? "Igual ao desejado" : $"Diferença de {Formato.Moeda(diferenca)}"),
                new("Descontos", Formato.Moeda(apuracao.Inss.Valor + apuracao.Irrf.Imposto), $"INSS {Formato.Moeda(apuracao.Inss.Valor)} + IRRF {Formato.Moeda(apuracao.Irrf.Imposto)}"),
                new("FGTS (8%)", Formato.Moeda(fgts), "Depositado pelo empregador")
            ],
            [new("Salário bruto", "", apuracao.Bruto)],
            [
                new("INSS", "", apuracao.Inss.Valor),
                new(MemoriaTributaria.DescricaoIrrf("IRRF", apuracao.Irrf), MemoriaTributaria.ReferenciaIrrf(apuracao.Irrf), apuracao.Irrf.Imposto)
            ],
            [new("FGTS", "8%", fgts)],
            [
                new GrupoMemoriaDto("Busca do salário bruto", $"Bruto: {Formato.Moeda(apuracao.Bruto)}",
                [
                    new("Método", "O salário bruto foi encontrado por aproximações sucessivas: a cada tentativa, INSS e IRRF são recalculados até o líquido chegar ao valor desejado."),
                    new("Conferência", $"{Formato.Moeda(apuracao.Bruto)} (bruto) - {Formato.Moeda(apuracao.Inss.Valor)} (INSS) - {Formato.Moeda(apuracao.Irrf.Imposto)} (IRRF) = {Formato.Moeda(apuracao.Liquido)}")
                ]),
                MemoriaTributaria.Inss("INSS", apuracao.Inss, "salário bruto"),
                MemoriaTributaria.Irrf("IRRF", apuracao.Irrf, "salário bruto")
            ],
            observacoes);
    }

    private sealed record Apuracao(decimal Bruto, ApuracaoInss Inss, ApuracaoIrrf Irrf, decimal Liquido);
}
