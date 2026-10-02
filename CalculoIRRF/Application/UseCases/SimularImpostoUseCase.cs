#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

public sealed class SimularImpostoUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularImpostoUseCase
{
    private static readonly DateOnly InicioSimplificado = new(2023, 5, 1);

    public async Task<SimulacaoImpostoDto> ExecutarAsync(SimularImpostoRequest request, CancellationToken cancellationToken)
    {
        Validar(request);
        var perfil = await tributacaoConsulta.ObterPerfilAsync(request.Competencia, cancellationToken);
        var faixasInss = Converter(perfil.FaixasInss);
        var faixasIrrf = Converter(perfil.FaixasIrrf);
        var regrasReducao = Converter(perfil.ReducoesMensaisIrrf);
        var tetoInss = faixasInss.Max(item => item.Limite);
        var baseInss = Math.Min(request.BaseInss, tetoInss);
        var detalhesInss = CalculadoraInss.CalcularDetalhes(request.Competencia, baseInss, faixasInss);
        var valorInss = CalculadoraTributacao.Arredondar(detalhesInss.Sum(item => item.Imposto));

        var baseNormal = Math.Max(0m, request.ValorBruto - valorInss - request.QuantidadeDependentes * perfil.DeducaoPorDependente);
        var normal = CalcularModalidade("Normal", request.ValorBruto, baseNormal, faixasIrrf, regrasReducao);
        var simplificadoDisponivel = request.Competencia >= InicioSimplificado;
        var baseSimplificada = simplificadoDisponivel ? Math.Max(0m, request.ValorBruto - perfil.DeducaoSimplificada) : 0m;
        var simplificada = simplificadoDisponivel
            ? CalcularModalidade("Simplificado", request.ValorBruto, baseSimplificada, faixasIrrf, regrasReducao)
            : new ModalidadeIrrfDto("Simplificado", 0m, 0m, 0m, 0m, 0m, 0m, 0m, []);

        var vantagem = CriarMensagemVantagem(normal.Imposto, simplificada.Imposto, simplificadoDisponivel);
        var modalidadeMaisVantajosa = ObterModalidadeMaisVantajosa(normal, simplificada, simplificadoDisponivel);
        return new SimulacaoImpostoDto(
            request,
            baseInss,
            valorInss,
            normal,
            simplificada,
            perfil.DescontoMinimo,
            // O FGTS não tem teto: incide sobre toda a remuneração informada, e não sobre a base limitada do INSS.
            CalculadoraTributacao.Arredondar(request.BaseInss * .08m),
            CalculadoraTributacao.Arredondar(request.BaseInss * .02m),
            vantagem,
            modalidadeMaisVantajosa,
            detalhesInss.Select(Mapear).ToArray(),
            perfil.DeducaoPorDependente,
            simplificadoDisponivel ? perfil.DeducaoSimplificada : null);
    }

    private static ModalidadeIrrfDto CalcularModalidade(
        string nome,
        decimal rendimentosTributaveis,
        decimal baseCalculo,
        IReadOnlyList<FaixaTributaria> faixas,
        IReadOnlyList<RegraReducaoMensalIrrf> regrasReducao)
    {
        var faixa = faixas.OrderBy(item => item.Numero).FirstOrDefault(item => baseCalculo <= item.Limite) ?? faixas.MaxBy(item => item.Limite)!;
        var impostoAntesReducao = CalculadoraTributacao.CalcularPorFaixa(baseCalculo, faixas);
        var reducao = CalculadoraReducaoMensalIrrf.Calcular(rendimentosTributaveis, impostoAntesReducao, regrasReducao);
        var imposto = CalculadoraTributacao.Arredondar(impostoAntesReducao - reducao);
        var aliquotaEfetiva = rendimentosTributaveis == 0m ? 0m : Math.Truncate(imposto / rendimentosTributaveis * 10_000m) / 100m;
        return new ModalidadeIrrfDto(nome, baseCalculo, faixa.Aliquota, faixa.Deducao, impostoAntesReducao, reducao, imposto, aliquotaEfetiva, CalculadoraTributacao.CalcularProgressivo(baseCalculo, faixas).Select(Mapear).ToArray());
    }

    private static IReadOnlyList<FaixaTributaria> Converter(IReadOnlyList<FaixaTributariaDto> faixas) => faixas.Select(item => new FaixaTributaria(item.Numero, item.Limite, item.Aliquota, item.Deducao)).ToArray();
    private static IReadOnlyList<RegraReducaoMensalIrrf> Converter(IReadOnlyList<RegraReducaoMensalIrrfDto> regras) => regras.Select(item => new RegraReducaoMensalIrrf(item.Faixa, item.LimiteRendimentos, item.Multiplicador, item.ValorBase)).ToArray();
    private static DetalheFaixaDto Mapear(ResultadoFaixaTributaria item) => new(item.Faixa, item.BaseCalculada, item.Aliquota, item.Imposto);

    private static void Validar(SimularImpostoRequest request)
    {
        if (request.ValorBruto < 0m || request.BaseInss < 0m || request.QuantidadeDependentes < 0)
            throw new ArgumentException("Os valores monetários e a quantidade de dependentes não podem ser negativos.");
    }

    private static string CriarMensagemVantagem(decimal normal, decimal simplificado, bool simplificadoDisponivel)
    {
        if (!simplificadoDisponivel)
            return "O desconto simplificado está disponível a partir de 05/2023.";
        if (normal == 0m && simplificado == 0m)
            return "Não há IRRF a recolher nas modalidades calculadas.";
        if (normal == simplificado)
            return "As modalidades normal e simplificada possuem o mesmo resultado.";
        var diferenca = Math.Abs(normal - simplificado);
        return normal > simplificado
            ? $"O cálculo simplificado é mais vantajoso. Diferença: {diferenca:N2}."
            : $"O cálculo normal é mais vantajoso. Diferença: {diferenca:N2}.";
    }

    private static string? ObterModalidadeMaisVantajosa(ModalidadeIrrfDto normal, ModalidadeIrrfDto simplificada, bool simplificadoDisponivel)
    {
        if (!simplificadoDisponivel || normal.Imposto == simplificada.Imposto)
            return null;

        return normal.Imposto < simplificada.Imposto ? normal.Nome : simplificada.Nome;
    }
}
