#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

/// <summary>
/// Participação nos lucros ou resultados (Lei 10.101/2000): IRRF exclusivo na fonte pela tabela anual, sobre o total pago
/// no ano, descontado o imposto já retido. Não há INSS nem FGTS, e só a pensão alimentícia é deduzida da base.
/// </summary>
public sealed class SimularPlrUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularDemonstrativoUseCase<SimularPlrRequest>
{
    public async Task<DemonstrativoDto> ExecutarAsync(SimularPlrRequest r, CancellationToken cancellationToken)
    {
        if (r.Valor < 0m || r.PlrAnterior < 0m || r.ImpostoRetidoAnterior < 0m || r.PensaoAlimenticia < 0m)
            throw new ArgumentException("Os valores da PLR, do imposto retido e da pensão não podem ser negativos.");
        if (r.PensaoAlimenticia > r.Valor)
            throw new ArgumentException("A pensão alimentícia não pode ser maior que a PLR paga.");

        var tabelas = await TabelasDaCompetencia.ObterAsync(tributacaoConsulta, r.Competencia, cancellationToken);
        var totalAno = r.PlrAnterior + r.Valor;
        var apuracao = tabelas.CalcularIrrfPlr(Math.Max(0m, totalAno - r.PensaoAlimenticia));
        var imposto = Math.Max(0m, apuracao.Imposto - r.ImpostoRetidoAnterior);
        var liquido = r.Valor - imposto - r.PensaoAlimenticia;
        var isencao = tabelas.FaixasPlr.OrderBy(faixa => faixa.Numero).First().Limite;
        var temAnterior = r.PlrAnterior > 0m || r.ImpostoRetidoAnterior > 0m;

        var descontos = new List<VerbaDto> { new("IRRF sobre a PLR", imposto > 0m ? Formato.PercentualCurto(apuracao.Aliquota) : "", imposto) };
        if (r.PensaoAlimenticia > 0m) descontos.Add(new("Pensão alimentícia sobre a PLR", "", r.PensaoAlimenticia));

        var informativos = new List<VerbaDto>();
        if (temAnterior)
        {
            informativos.Add(new("PLR acumulada no ano", "", totalAno));
            informativos.Add(new("IRRF do ano pela tabela da PLR", Formato.PercentualCurto(apuracao.Aliquota), apuracao.Imposto));
            informativos.Add(new("IRRF já retido na PLR anterior", "", r.ImpostoRetidoAnterior));
        }

        var formulas = new List<FormulaDto>();
        if (temAnterior)
            formulas.Add(new("PLR do ano", $"{Formato.Moeda(r.PlrAnterior)} (anterior) + {Formato.Moeda(r.Valor)} (atual) = {Formato.Moeda(totalAno)}"));
        formulas.Add(new("Base de cálculo", r.PensaoAlimenticia > 0m
            ? $"{Formato.Moeda(totalAno)} (PLR) - {Formato.Moeda(r.PensaoAlimenticia)} (pensão alimentícia) = {Formato.Moeda(apuracao.BaseCalculo)}"
            : $"{Formato.Moeda(apuracao.BaseCalculo)} (PLR, sem deduções)"));
        formulas.Add(new("Imposto pela tabela anual", apuracao.Aliquota == 0m
            ? $"{Formato.Moeda(apuracao.BaseCalculo)} está na faixa isenta, até {Formato.Moeda(isencao)}: {Formato.Moeda(0m)}"
            : $"{Formato.Moeda(apuracao.BaseCalculo)} x {Formato.Percentual(apuracao.Aliquota)} - {Formato.Moeda(apuracao.Deducao)} = {Formato.Moeda(apuracao.Imposto)}"));
        if (temAnterior)
            formulas.Add(new("Imposto desta parcela", $"{Formato.Moeda(apuracao.Imposto)} (ano) - {Formato.Moeda(r.ImpostoRetidoAnterior)} (já retido) = {Formato.Moeda(imposto)}{(apuracao.Imposto < r.ImpostoRetidoAnterior ? " (não fica negativo)" : "")}"));
        formulas.Add(new("Líquido", $"{Formato.Moeda(r.Valor)} - {Formato.Moeda(imposto)} (IRRF){(r.PensaoAlimenticia > 0m ? $" - {Formato.Moeda(r.PensaoAlimenticia)} (pensão)" : "")} = {Formato.Moeda(liquido)}"));

        var faixas = tabelas.FaixasPlr.OrderBy(faixa => faixa.Numero).ToArray();
        var tabelaVigente = faixas.Select((faixa, indice) =>
        {
            var inicio = indice == 0 ? 0m : faixas[indice - 1].Limite + .01m;
            var intervalo = indice == faixas.Length - 1 ? $"Acima de {Formato.Moeda(faixas[indice - 1].Limite)}" : $"De {Formato.Moeda(inicio)} a {Formato.Moeda(faixa.Limite)}";
            return new FormulaDto(intervalo, faixa.Aliquota == 0m ? "Isento" : $"{Formato.Percentual(faixa.Aliquota)}, menos {Formato.Moeda(faixa.Deducao)}");
        }).ToArray();

        return new DemonstrativoDto(
            "Participação nos lucros ou resultados (PLR)",
            $"Pagamento em {Formato.Competencia(tabelas.Competencia)}",
            [
                new("Líquido da PLR", Formato.Moeda(liquido), "Sem INSS e sem FGTS"),
                new("IRRF desta parcela", Formato.Moeda(imposto), imposto > 0m ? $"Faixa de {Formato.PercentualCurto(apuracao.Aliquota)} da tabela anual" : "Dentro da faixa isenta"),
                new("PLR no ano", Formato.Moeda(totalAno), temAnterior ? "Somando a PLR anterior" : "Primeiro pagamento do ano"),
                new("Isenção anual", Formato.Moeda(isencao), "Até esse total no ano, não há IRRF")
            ],
            [new("Participação nos lucros ou resultados", "", r.Valor)],
            descontos,
            informativos,
            [
                new GrupoMemoriaDto("IRRF sobre a PLR", $"IRRF: {Formato.Moeda(imposto)}", formulas),
                new GrupoMemoriaDto("Tabela anual da PLR vigente", $"Competência {Formato.Competencia(tabelas.Competencia)}", tabelaVigente)
            ],
            [
                "A PLR tem tributação exclusiva na fonte, pela tabela anual, separada do salário e sem a dedução de dependentes, o desconto simplificado ou a redução mensal do IRRF.",
                "Com mais de um pagamento no ano, o imposto é recalculado sobre o total e o valor já retido é descontado (Lei 10.101/2000, art. 3º, § 7º). Se houve pensão na PLR anterior, informe a PLR anterior já descontada dela.",
                "Paga conforme a Lei 10.101/2000, no máximo duas vezes por ano e com intervalo de pelo menos um trimestre, a PLR não tem INSS nem FGTS."
            ]);
    }
}
