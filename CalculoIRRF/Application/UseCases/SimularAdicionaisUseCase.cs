#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

/// <summary>
/// Adicionais de insalubridade (10%, 20% ou 40% do salário mínimo, CLT, art. 192) e de periculosidade (30% do salário,
/// CLT, art. 193, § 1º), com o INSS e o IRRF da remuneração. Os dois não se acumulam: aplica-se o maior (art. 193, § 2º).
/// </summary>
public sealed class SimularAdicionaisUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularDemonstrativoUseCase<SimularAdicionaisRequest>
{
    private const decimal PercentualPericulosidade = 30m;

    public async Task<DemonstrativoDto> ExecutarAsync(SimularAdicionaisRequest r, CancellationToken cancellationToken)
    {
        if (r.Salario < 0m || r.ValorBaseInformado < 0m || r.Dependentes < 0)
            throw new ArgumentException("O salário, a base informada e a quantidade de dependentes não podem ser negativos.");
        if (r.Grau == GrauInsalubridade.Nenhum && !r.Periculosidade)
            throw new ArgumentException("Escolha um grau de insalubridade ou marque a periculosidade.");

        var tabelas = await TabelasDaCompetencia.ObterAsync(tributacaoConsulta, r.Competencia, cancellationToken);
        var percentualInsalubridade = (decimal)(int)r.Grau;
        decimal baseInsalubridade = 0m;
        var descricaoBase = string.Empty;
        if (r.Grau != GrauInsalubridade.Nenhum)
        {
            (baseInsalubridade, descricaoBase) = r.Base switch
            {
                BaseInsalubridade.SalarioMinimo => (tabelas.SalarioMinimo ?? throw new InvalidOperationException($"Não há salário mínimo cadastrado para a competência {Formato.Competencia(r.Competencia)}."), "salário mínimo"),
                BaseInsalubridade.Salario => (r.Salario, "salário"),
                _ => (r.ValorBaseInformado, "base informada")
            };
            if (baseInsalubridade <= 0m)
                throw new ArgumentException("Informe uma base maior que zero para o adicional de insalubridade.");
        }

        var insalubridade = CalculadoraTributacao.Arredondar(baseInsalubridade * percentualInsalubridade / 100m);
        var periculosidade = r.Periculosidade ? CalculadoraTributacao.Arredondar(r.Salario * PercentualPericulosidade / 100m) : 0m;
        var aplicaPericulosidade = r.Periculosidade && periculosidade >= insalubridade;
        var adicional = aplicaPericulosidade ? periculosidade : insalubridade;
        var nomeGrau = r.Grau switch { GrauInsalubridade.Minimo => "grau mínimo", GrauInsalubridade.Medio => "grau médio", _ => "grau máximo" };
        var nomeAdicional = aplicaPericulosidade ? "Adicional de periculosidade" : $"Adicional de insalubridade ({nomeGrau})";
        var remuneracao = r.Salario + adicional;

        var inss = tabelas.CalcularInss(remuneracao);
        var irrf = tabelas.CalcularIrrf(remuneracao, inss.Valor, r.Dependentes);
        var fgts = CalculadoraTributacao.Arredondar(remuneracao * .08m);
        var liquido = remuneracao - inss.Valor - irrf.Imposto;
        var ambos = r.Periculosidade && r.Grau != GrauInsalubridade.Nenhum;

        var formulas = new List<FormulaDto>();
        if (r.Grau != GrauInsalubridade.Nenhum)
            formulas.Add(new($"Insalubridade ({nomeGrau})", $"{Formato.Moeda(baseInsalubridade)} ({descricaoBase}) x {Formato.PercentualCurto(percentualInsalubridade)} = {Formato.Moeda(insalubridade)}"));
        if (r.Periculosidade)
            formulas.Add(new("Periculosidade", $"{Formato.Moeda(r.Salario)} (salário) x {Formato.PercentualCurto(PercentualPericulosidade)} = {Formato.Moeda(periculosidade)}"));
        if (ambos)
            formulas.Add(new("Adicional aplicado", aplicaPericulosidade
                ? $"Os adicionais não se acumulam (CLT, art. 193, § 2º): aplicado o de periculosidade, maior ou igual ({Formato.Moeda(periculosidade)} contra {Formato.Moeda(insalubridade)})."
                : $"Os adicionais não se acumulam (CLT, art. 193, § 2º): aplicado o de insalubridade, maior ({Formato.Moeda(insalubridade)} contra {Formato.Moeda(periculosidade)})."));
        formulas.Add(new("Remuneração do mês", $"{Formato.Moeda(r.Salario)} (salário) + {Formato.Moeda(adicional)} (adicional) = {Formato.Moeda(remuneracao)}"));

        var informativos = new List<VerbaDto> { new("FGTS", "8%", fgts) };
        if (ambos)
            informativos.Add(aplicaPericulosidade
                ? new($"Adicional de insalubridade não aplicado ({nomeGrau})", Formato.PercentualCurto(percentualInsalubridade), insalubridade)
                : new("Adicional de periculosidade não aplicado", Formato.PercentualCurto(PercentualPericulosidade), periculosidade));

        var observacoes = new List<string>
        {
            "O grau de insalubridade (10%, 20% ou 40%) é definido por laudo técnico, conforme a NR-15. A base é o salário mínimo, salvo previsão diferente em convenção coletiva ou decisão judicial.",
            "A periculosidade é de 30% sobre o salário, sem gratificações, prêmios ou participação nos lucros (CLT, art. 193, § 1º).",
            "Os adicionais integram a remuneração para horas extras, férias, 13º, aviso prévio e FGTS: use a remuneração com o adicional nas outras calculadoras."
        };
        if (ambos)
            observacoes.Insert(0, "Insalubridade e periculosidade não se acumulam: o empregado recebe o adicional mais vantajoso (CLT, art. 193, § 2º).");

        return new DemonstrativoDto(
            "Insalubridade e periculosidade",
            $"Competência {Formato.Competencia(tabelas.Competencia)}",
            [
                new("Adicional", Formato.Moeda(adicional), aplicaPericulosidade ? "Periculosidade de 30% do salário" : $"Insalubridade em {nomeGrau} ({Formato.PercentualCurto(percentualInsalubridade)})"),
                new("Remuneração do mês", Formato.Moeda(remuneracao), $"Salário de {Formato.Moeda(r.Salario)}"),
                new("Salário líquido", Formato.Moeda(liquido), "Remuneração menos INSS e IRRF"),
                aplicaPericulosidade
                    ? new("Base da periculosidade", Formato.Moeda(r.Salario), "Salário, sem gratificações")
                    : new("Base da insalubridade", Formato.Moeda(baseInsalubridade), r.Base == BaseInsalubridade.SalarioMinimo ? $"Salário mínimo de {tabelas.Competencia.Year}" : descricaoBase == "salário" ? "Salário" : "Valor informado")
            ],
            [
                new("Salário", "", r.Salario),
                new(nomeAdicional, Formato.PercentualCurto(aplicaPericulosidade ? PercentualPericulosidade : percentualInsalubridade), adicional)
            ],
            [
                new("INSS", "", inss.Valor),
                new(MemoriaTributaria.DescricaoIrrf("IRRF", irrf), MemoriaTributaria.ReferenciaIrrf(irrf), irrf.Imposto)
            ],
            informativos,
            [
                new GrupoMemoriaDto("Adicionais", $"Adicional: {Formato.Moeda(adicional)}", formulas),
                MemoriaTributaria.Inss("INSS", inss, "remuneração do mês"),
                MemoriaTributaria.Irrf("IRRF", irrf, "remuneração do mês")
            ],
            observacoes);
    }
}
