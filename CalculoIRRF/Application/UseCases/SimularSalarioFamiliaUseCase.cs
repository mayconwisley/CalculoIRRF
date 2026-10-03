#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

/// <summary>
/// Salário-família (Lei 8.213/1991, arts. 65 a 70): uma cota por filho de até 14 anos, ou inválido, para o segurado com
/// remuneração até o limite da tabela. Nos meses de admissão e desligamento, a cota é proporcional aos dias trabalhados.
/// </summary>
public sealed class SimularSalarioFamiliaUseCase(ITributacaoConsulta tributacaoConsulta) : ISimularDemonstrativoUseCase<SimularSalarioFamiliaRequest>
{
    public async Task<DemonstrativoDto> ExecutarAsync(SimularSalarioFamiliaRequest r, CancellationToken cancellationToken)
    {
        if (r.Remuneracao < 0m || r.Filhos < 0)
            throw new ArgumentException("A remuneração e a quantidade de filhos não podem ser negativas.");
        if (r.DiasTrabalhados is < 1 or > 30)
            throw new ArgumentException("Os dias trabalhados devem estar entre 1 e 30.");

        var tabelas = await TabelasDaCompetencia.ObterAsync(tributacaoConsulta, r.Competencia, cancellationToken);
        var faixas = tabelas.FaixasSalarioFamilia;
        if (faixas.Count == 0)
            throw new InvalidOperationException($"Não há tabela de salário-família cadastrada para a competência {Formato.Competencia(r.Competencia)}.");

        var faixa = faixas.OrderBy(item => item.Faixa).FirstOrDefault(item => r.Remuneracao <= item.LimiteRemuneracao);
        var limite = faixas.Max(item => item.LimiteRemuneracao);
        var cota = faixa?.Cota ?? 0m;
        var integral = cota * r.Filhos;
        var valor = r.DiasTrabalhados < 30 ? CalculadoraTributacao.Arredondar(integral / 30m * r.DiasTrabalhados) : integral;
        var filhos = r.Filhos == 1 ? "1 filho" : $"{r.Filhos} filhos";

        var formulas = new List<FormulaDto>
        {
            new("Direito", faixa is null
                ? $"{Formato.Moeda(r.Remuneracao)} passa do limite de {Formato.Moeda(limite)}: sem direito ao salário-família neste mês."
                : faixas.Count > 1
                    ? $"{Formato.Moeda(r.Remuneracao)} está na faixa {faixa.Faixa}, até {Formato.Moeda(faixa.LimiteRemuneracao)}: cota de {Formato.Moeda(cota)} por filho."
                    : $"{Formato.Moeda(r.Remuneracao)} não passa do limite de {Formato.Moeda(limite)}: cota de {Formato.Moeda(cota)} por filho.")
        };
        if (faixa is not null)
        {
            formulas.Add(new("Valor do mês", $"{Formato.Moeda(cota)} x {filhos} = {Formato.Moeda(integral)}"));
            if (r.DiasTrabalhados < 30)
                formulas.Add(new("Proporcional aos dias", $"{Formato.Moeda(integral)} ÷ 30 x {Formato.Dias(r.DiasTrabalhados)} = {Formato.Moeda(valor)}"));
        }

        return new DemonstrativoDto(
            "Salário-família",
            $"Competência {Formato.Competencia(tabelas.Competencia)}",
            [
                new("Salário-família", Formato.Moeda(valor), faixa is null ? "Remuneração acima do limite" : $"{filhos} x {Formato.Moeda(cota)}"),
                new("Cota por filho", faixa is null ? "Sem direito" : Formato.Moeda(cota), faixas.Count > 1 && faixa is not null ? $"Faixa {faixa.Faixa}" : "Valor da tabela"),
                new("Limite de remuneração", Formato.Moeda(limite), $"Remuneração informada: {Formato.Moeda(r.Remuneracao)}"),
                new("Dias considerados", Formato.Dias(r.DiasTrabalhados), r.DiasTrabalhados < 30 ? "Proporcional (admissão ou desligamento)" : "Mês completo")
            ],
            [new("Salário-família", filhos, valor)],
            [],
            valor > 0m ? [new("Deduzido pela empresa das contribuições ao INSS", "", valor)] : [],
            [new GrupoMemoriaDto("Salário-família", $"Valor: {Formato.Moeda(valor)}", formulas)],
            [
                "Têm direito o empregado, inclusive o doméstico, e o trabalhador avulso com remuneração até o limite, para cada filho ou equiparado de até 14 anos ou inválido de qualquer idade. Se pai e mãe tiverem direito, os dois recebem.",
                "O salário-família não tem INSS, IRRF nem FGTS: a empresa paga junto com o salário e deduz o valor das contribuições ao INSS.",
                "Exige a certidão de nascimento, o atestado de vacinação anual até os 6 anos e a comprovação de frequência escolar a partir dos 7 anos.",
                "Nos meses de admissão e de desligamento, a cota é proporcional aos dias trabalhados."
            ],
            RotuloResultado: "Salário-família a receber");
    }
}
