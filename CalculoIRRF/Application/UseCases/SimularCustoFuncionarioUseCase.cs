#nullable enable

using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

/// <summary>
/// Custo mensal de um empregado para a empresa: salário, encargos sobre a folha, provisões de 13º e férias com os seus
/// encargos e benefícios. No Simples Nacional (anexos I a III e V) a contribuição patronal está incluída no DAS;
/// no anexo IV, a empresa recolhe à parte a contribuição de 20% e o RAT, mas não as contribuições a terceiros.
/// </summary>
public sealed class SimularCustoFuncionarioUseCase : ISimularDemonstrativoUseCase<SimularCustoFuncionarioRequest>
{
    private const decimal AliquotaPatronal = 20m;
    private const decimal AliquotaFgts = 8m;
    private const decimal HorasMensais = 220m;

    public Task<DemonstrativoDto> ExecutarAsync(SimularCustoFuncionarioRequest r, CancellationToken cancellationToken)
    {
        if (r.Salario < 0m || r.Beneficios < 0m || r.Terceiros < 0m)
            throw new ArgumentException("O salário, os benefícios e as contribuições a terceiros não podem ser negativos.");
        if (r.Rat is < 0m or > 3m)
            throw new ArgumentException("O RAT deve estar entre 1% e 3%, conforme o grau de risco da atividade.");
        if (r.Fap is < 0.5m or > 2m)
            throw new ArgumentException("O FAP deve estar entre 0,5 e 2.");

        var pagaPatronal = r.Regime != RegimeTributario.SimplesNacional;
        var pagaTerceiros = r.Regime == RegimeTributario.LucroRealOuPresumido;
        var aliquotaPatronal = pagaPatronal ? AliquotaPatronal : 0m;
        var aliquotaRat = pagaPatronal ? r.Rat * r.Fap : 0m;
        var aliquotaTerceiros = pagaTerceiros ? r.Terceiros : 0m;
        var aliquotaEncargos = aliquotaPatronal + aliquotaRat + aliquotaTerceiros + AliquotaFgts;

        var patronal = Percentual(r.Salario, aliquotaPatronal);
        var rat = Percentual(r.Salario, aliquotaRat);
        var terceiros = Percentual(r.Salario, aliquotaTerceiros);
        var fgts = Percentual(r.Salario, AliquotaFgts);
        var provisao13 = r.IncluirProvisoes ? CalculadoraTributacao.Arredondar(r.Salario / 12m) : 0m;
        var provisaoFerias = r.IncluirProvisoes ? CalculadoraTributacao.Arredondar(r.Salario / 12m * 4m / 3m) : 0m;
        var encargosProvisoes = Percentual(provisao13 + provisaoFerias, aliquotaEncargos);

        var itens = new List<VerbaDto> { new("Salário", "", r.Salario) };
        if (patronal > 0m) itens.Add(new("INSS patronal", Formato.PercentualCurto(aliquotaPatronal), patronal));
        if (rat > 0m) itens.Add(new("RAT ajustado pelo FAP", Formato.Percentual(aliquotaRat), rat));
        if (terceiros > 0m) itens.Add(new("Contribuições a terceiros (Sistema S e outras)", Formato.PercentualCurto(aliquotaTerceiros), terceiros));
        itens.Add(new("FGTS", Formato.PercentualCurto(AliquotaFgts), fgts));
        if (r.IncluirProvisoes)
        {
            itens.Add(new("Provisão de 13º salário", "1/12", provisao13));
            itens.Add(new("Provisão de férias + 1/3", "1/12 + 1/3", provisaoFerias));
            itens.Add(new("Encargos sobre as provisões", Formato.Percentual(aliquotaEncargos), encargosProvisoes));
        }
        if (r.Beneficios > 0m) itens.Add(new("Benefícios", "", r.Beneficios));

        var custo = itens.Sum(item => item.Valor);
        var acrescimo = r.Salario == 0m ? 0m : (custo - r.Salario) / r.Salario * 100m;

        var formulas = new List<FormulaDto>
        {
            new("Regime", NomeRegime(r.Regime)),
            new("Alíquota de encargos sobre a folha", $"{Formato.PercentualCurto(aliquotaPatronal)} (patronal) + {Formato.Percentual(aliquotaRat)} (RAT {Formato.PercentualCurto(r.Rat)} x FAP {Formato.Numero(r.Fap)}) + {Formato.PercentualCurto(aliquotaTerceiros)} (terceiros) + {Formato.PercentualCurto(AliquotaFgts)} (FGTS) = {Formato.Percentual(aliquotaEncargos)}"),
            new("Encargos mensais", $"{Formato.Moeda(r.Salario)} x {Formato.Percentual(aliquotaEncargos)} = {Formato.Moeda(patronal + rat + terceiros + fgts)}")
        };
        if (r.IncluirProvisoes)
        {
            formulas.Add(new("Provisão de 13º", $"{Formato.Moeda(r.Salario)} ÷ 12 = {Formato.Moeda(provisao13)}"));
            formulas.Add(new("Provisão de férias + 1/3", $"{Formato.Moeda(r.Salario)} ÷ 12 x 4/3 = {Formato.Moeda(provisaoFerias)}"));
            formulas.Add(new("Encargos sobre as provisões", $"({Formato.Moeda(provisao13)} + {Formato.Moeda(provisaoFerias)}) x {Formato.Percentual(aliquotaEncargos)} = {Formato.Moeda(encargosProvisoes)}"));
        }
        formulas.Add(new("Custo mensal", $"{string.Join(" + ", itens.Select(item => Formato.Moeda(item.Valor)))} = {Formato.Moeda(custo)}"));

        var demonstrativo = new DemonstrativoDto(
            "Custo do funcionário",
            NomeRegime(r.Regime),
            [
                new("Custo mensal", Formato.Moeda(custo), $"Salário de {Formato.Moeda(r.Salario)}"),
                new("Custo anual", Formato.Moeda(custo * 12m), r.IncluirProvisoes ? "12 meses, com as provisões" : "12 meses, sem 13º e férias"),
                new("Acréscimo sobre o salário", Formato.Percentual(CalculadoraTributacao.Arredondar(acrescimo)), "Encargos, provisões e benefícios"),
                new("Custo por hora", Formato.Moeda(custo / HorasMensais), "Jornada de 220 horas mensais")
            ],
            itens,
            [],
            [],
            [new GrupoMemoriaDto("Custo do funcionário", $"Custo mensal: {Formato.Moeda(custo)}", formulas)],
            [
                "No Simples Nacional, anexos I a III e V, a contribuição patronal já está incluída no DAS; no anexo IV, a empresa recolhe à parte os 20% e o RAT, mas não as contribuições a terceiros.",
                "As provisões distribuem mês a mês o 13º e as férias com 1/3, pagos uma vez por ano. Não inclui a provisão da multa do FGTS em caso de dispensa.",
                "Confira o RAT e o FAP da empresa no eSocial ou com a contabilidade; as contribuições a terceiros podem variar conforme a atividade."
            ],
            RotuloProventos: "Custo",
            RotuloResultado: "Custo mensal total");
        return Task.FromResult(demonstrativo);
    }

    private static decimal Percentual(decimal valor, decimal aliquota) => CalculadoraTributacao.Arredondar(valor * aliquota / 100m);

    private static string NomeRegime(RegimeTributario regime) => regime switch
    {
        RegimeTributario.LucroRealOuPresumido => "Lucro Real ou Presumido",
        RegimeTributario.SimplesNacional => "Simples Nacional (anexos I, II, III e V)",
        _ => "Simples Nacional (anexo IV)"
    };
}
