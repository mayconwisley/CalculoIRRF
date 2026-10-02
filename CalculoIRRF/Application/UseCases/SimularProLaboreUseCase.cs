#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

/// <summary>
/// Pró-labore do sócio ou pagamento a autônomo (RPA): INSS de 11% retido pela empresa até o teto, IRRF pela tabela mensal
/// e, para o autônomo, o ISS quando a lei municipal exige a retenção. A empresa paga também a contribuição patronal de 20%,
/// exceto no Simples Nacional fora do anexo IV.
/// </summary>
public sealed class SimularProLaboreUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularDemonstrativoUseCase<SimularProLaboreRequest>
{
    private const decimal AliquotaPatronal = 20m;

    public async Task<DemonstrativoDto> ExecutarAsync(SimularProLaboreRequest r, CancellationToken cancellationToken)
    {
        if (r.Valor < 0m || r.Dependentes < 0 || r.AliquotaIss < 0m)
            throw new ArgumentException("O valor, a alíquota do ISS e a quantidade de dependentes não podem ser negativos.");
        if (r.AliquotaIss > 5m)
            throw new ArgumentException("A alíquota máxima do ISS é de 5% (Lei Complementar 116/2003).");

        var autonomo = r.Tipo == TipoContribuinteIndividual.Autonomo;
        var tabelas = await TabelasDaCompetencia.ObterAsync(tributacaoConsulta, r.Competencia, cancellationToken);
        var inss = tabelas.CalcularInssContribuinteIndividual(r.Valor);
        var irrf = tabelas.CalcularIrrf(r.Valor, inss.Valor, r.Dependentes);
        var iss = autonomo ? CalculadoraTributacao.Arredondar(r.Valor * r.AliquotaIss / 100m) : 0m;
        var liquido = r.Valor - inss.Valor - irrf.Imposto - iss;
        var aliquotaPatronal = r.Regime == RegimeTributario.SimplesNacional ? 0m : AliquotaPatronal;
        var patronal = CalculadoraTributacao.Arredondar(r.Valor * aliquotaPatronal / 100m);
        var custo = r.Valor + patronal;
        var nome = autonomo ? "Serviço de autônomo (RPA)" : "Pró-labore";

        var descontos = new List<VerbaDto>
        {
            new("INSS retido", "11%", inss.Valor),
            new(MemoriaTributaria.DescricaoIrrf("IRRF retido", irrf), MemoriaTributaria.ReferenciaIrrf(irrf), irrf.Imposto)
        };
        if (iss > 0m) descontos.Add(new("ISS retido", Formato.PercentualCurto(r.AliquotaIss), iss));

        var informativos = new List<VerbaDto>();
        if (patronal > 0m) informativos.Add(new("INSS patronal pago pela empresa", Formato.PercentualCurto(aliquotaPatronal), patronal));
        informativos.Add(new("Custo total para a empresa", "", custo));

        var formulasInss = new List<FormulaDto>
        {
            new("Base de cálculo", inss.LimitadaAoTeto
                ? $"{Formato.Moeda(r.Valor)}, limitada ao teto de {Formato.Moeda(inss.BaseConsiderada)}"
                : Formato.Moeda(inss.BaseConsiderada)),
            new("INSS retido", $"{Formato.Moeda(inss.BaseConsiderada)} x 11% = {Formato.Moeda(inss.Valor)}")
        };
        var formulas = new List<FormulaDto>
        {
            new("Líquido", $"{Formato.Moeda(r.Valor)} - {Formato.Moeda(inss.Valor)} (INSS) - {Formato.Moeda(irrf.Imposto)} (IRRF){(iss > 0m ? $" - {Formato.Moeda(iss)} (ISS)" : "")} = {Formato.Moeda(liquido)}"),
            new("Custo para a empresa", patronal > 0m
                ? $"{Formato.Moeda(r.Valor)} + {Formato.Moeda(r.Valor)} x 20% (patronal) = {Formato.Moeda(custo)}"
                : $"{Formato.Moeda(r.Valor)}: no Simples Nacional, a contribuição patronal está incluída no DAS")
        };
        if (iss > 0m)
            formulas.Insert(0, new("ISS retido", $"{Formato.Moeda(r.Valor)} x {Formato.PercentualCurto(r.AliquotaIss)} = {Formato.Moeda(iss)}"));

        var observacoes = new List<string>
        {
            "O INSS de 11% vale quando a empresa retém a contribuição do contribuinte individual, com o mesmo teto do INSS dos empregados (Lei 10.666/2003).",
            "O IRRF usa a tabela mensal, com a dedução do INSS e dos dependentes ou o desconto simplificado, o que for mais vantajoso."
        };
        observacoes.Add(autonomo
            ? "O autônomo recebe por RPA, sem vínculo de emprego: não há FGTS, 13º nem férias. O ISS só é retido quando a lei do município exige."
            : "O pró-labore não tem FGTS, 13º nem férias.");

        return new DemonstrativoDto(
            nome,
            $"Competência {Formato.Competencia(tabelas.Competencia)}",
            [
                new("Líquido a receber", Formato.Moeda(liquido), $"Bruto de {Formato.Moeda(r.Valor)}"),
                new("INSS retido", Formato.Moeda(inss.Valor), inss.LimitadaAoTeto ? "11% limitado ao teto" : "11% do valor"),
                new("IRRF retido", Formato.Moeda(irrf.Imposto), irrf.SimplificadaAplicada ? "Com desconto simplificado" : "Com deduções legais"),
                new("Custo para a empresa", Formato.Moeda(custo), patronal > 0m ? "Com o INSS patronal de 20%" : "Sem INSS patronal no Simples")
            ],
            [new(autonomo ? "Valor do serviço" : "Pró-labore", "", r.Valor)],
            descontos,
            informativos,
            [
                new GrupoMemoriaDto(nome, $"Líquido: {Formato.Moeda(liquido)}", formulas),
                new GrupoMemoriaDto("INSS do contribuinte individual", $"INSS: {Formato.Moeda(inss.Valor)}", formulasInss),
                MemoriaTributaria.Irrf("IRRF", irrf, autonomo ? "valor do serviço" : "pró-labore")
            ],
            observacoes);
    }
}
