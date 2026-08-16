using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

public sealed class SimularPensaoUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularPensaoUseCase
{
    public async Task<SimulacaoPensaoDto> ExecutarAsync(SimularPensaoRequest request, CancellationToken cancellationToken)
    {
        if (request.ValorBruto < 0m || request.BaseInss < 0m || request.Dependentes < 0 || request.PercentualPensao is < 0m or > 100m || request.OutrosDescontos < 0m || request.OutrosDescontos > request.ValorBruto)
            throw new ArgumentException("Os parâmetros da pensão são inválidos.");

        var perfil = await tributacaoConsulta.ObterPerfilAsync(request.Competencia, cancellationToken);
        var inss = perfil.FaixasInss.Select(x => new FaixaTributaria(x.Numero, x.Limite, x.Aliquota)).ToArray();
        var irrf = perfil.FaixasIrrf.Select(x => new FaixaTributaria(x.Numero, x.Limite, x.Aliquota, x.Deducao)).ToArray();
        var regrasReducao = perfil.ReducoesMensaisIrrf.Select(x => new RegraReducaoMensalIrrf(x.Faixa, x.LimiteRendimentos, x.Multiplicador, x.ValorBase)).ToArray();
        var baseInss = Math.Min(request.BaseInss, inss.Max(x => x.Limite));
        var valorInss = CalculadoraTributacao.Arredondar(CalculadoraInss.CalcularDetalhes(request.Competencia, baseInss, inss).Sum(x => x.Imposto));
        var rendimentos = request.ValorBruto - request.OutrosDescontos;
        var basePensao = rendimentos - valorInss;
        var normal = Calcular("Normal", rendimentos, rendimentos - valorInss - request.Dependentes * perfil.DeducaoPorDependente, basePensao, request.PercentualPensao, irrf, regrasReducao);
        var simplificada = Calcular("Simplificado", rendimentos, rendimentos - perfil.DeducaoSimplificada, basePensao, request.PercentualPensao, irrf, regrasReducao);
        var vantagem = normal.Total == simplificada.Total ? "As modalidades possuem o mesmo custo total." : normal.Total < simplificada.Total ? "O cálculo normal é mais vantajoso." : "O cálculo simplificado é mais vantajoso.";
        return new SimulacaoPensaoDto(valorInss, normal, simplificada, vantagem);
    }

    private static ModalidadePensaoDto Calcular(string nome, decimal rendimentosTributaveis, decimal baseIrrfInicial, decimal basePensaoInicial, decimal percentual, IReadOnlyList<FaixaTributaria> faixas, IReadOnlyList<RegraReducaoMensalIrrf> regrasReducao)
    {
        var pensao = 0m; var imposto = 0m; var impostoAntesReducao = 0m; var reducaoMensal = 0m; var iteracoes = 0;
        var detalhes = new List<IteracaoPensaoDto>();
        for (; iteracoes < 100; iteracoes++)
        {
            var baseIrrf = Math.Max(0m, baseIrrfInicial - pensao);
            var faixa = faixas.OrderBy(item => item.Numero).FirstOrDefault(item => baseIrrf <= item.Limite) ?? faixas.MaxBy(item => item.Limite)!;
            impostoAntesReducao = CalculadoraTributacao.CalcularPorFaixa(baseIrrf, faixas);
            reducaoMensal = CalculadoraReducaoMensalIrrf.Calcular(rendimentosTributaveis, impostoAntesReducao, regrasReducao);
            imposto = CalculadoraTributacao.Arredondar(impostoAntesReducao - reducaoMensal);
            var basePensao = Math.Max(0m, basePensaoInicial - imposto);
            var novaPensao = CalculadoraTributacao.Arredondar(basePensao * percentual / 100m);
            detalhes.Add(new IteracaoPensaoDto(iteracoes + 1, baseIrrf, faixa.Aliquota, faixa.Deducao, impostoAntesReducao, reducaoMensal, imposto, basePensao, novaPensao));
            if (Math.Abs(novaPensao - pensao) <= .01m) { pensao = novaPensao; break; }
            pensao = novaPensao;
        }
        return new ModalidadePensaoDto(nome, impostoAntesReducao, reducaoMensal, imposto, pensao, CalculadoraTributacao.Arredondar(imposto + pensao), iteracoes + 1, detalhes);
    }
}
