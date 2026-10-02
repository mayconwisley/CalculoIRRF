#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

public sealed class SimularImpostoUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularImpostoUseCase
{
    public async Task<SimulacaoImpostoDto> ExecutarAsync(SimularImpostoRequest request, CancellationToken cancellationToken)
    {
        Validar(request);
        var tabelas = await TabelasDaCompetencia.ObterAsync(tributacaoConsulta, request.Competencia, cancellationToken);
        var inss = tabelas.CalcularInss(request.BaseInss);
        var irrf = tabelas.CalcularIrrf(request.ValorBruto, inss.Valor, request.QuantidadeDependentes);
        var simplificadoDisponivel = tabelas.DescontoSimplificado is not null;

        var vantagem = CriarMensagemVantagem(irrf.Normal.Imposto, irrf.Simplificada.Imposto, simplificadoDisponivel);
        var modalidadeMaisVantajosa = ObterModalidadeMaisVantajosa(irrf.Normal, irrf.Simplificada, simplificadoDisponivel);
        return new SimulacaoImpostoDto(
            request,
            inss.BaseConsiderada,
            inss.Valor,
            irrf.Normal,
            irrf.Simplificada,
            tabelas.DescontoMinimo,
            // O FGTS não tem teto: incide sobre toda a remuneração informada, e não sobre a base limitada do INSS.
            CalculadoraTributacao.Arredondar(request.BaseInss * .08m),
            CalculadoraTributacao.Arredondar(request.BaseInss * .02m),
            vantagem,
            modalidadeMaisVantajosa,
            inss.Detalhes,
            tabelas.DeducaoPorDependente,
            tabelas.DescontoSimplificado);
    }

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
