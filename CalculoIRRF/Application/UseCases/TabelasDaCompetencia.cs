#nullable enable

using CalculoIRRF.Application.Abstractions;
using CalculoIRRF.Application.DTOs;
using CalculoIRRF.Domain.Tributacao;

namespace CalculoIRRF.Application.UseCases;

/// <summary>
/// Tabelas de INSS e IRRF vigentes em uma competência e a apuração mensal comum a todas as calculadoras:
/// o INSS por faixas e o IRRF nas duas modalidades, deduções legais e desconto simplificado, com a redução mensal.
/// </summary>
internal sealed class TabelasDaCompetencia
{
    private static readonly DateOnly InicioSimplificado = new(2023, 5, 1);
    private const decimal AliquotaContribuinteIndividual = 11m;
    private readonly IReadOnlyList<FaixaTributaria> _faixasInss;
    private readonly IReadOnlyList<FaixaTributaria> _faixasIrrf;
    private readonly IReadOnlyList<RegraReducaoMensalIrrf> _regrasReducao;

    private TabelasDaCompetencia(DateOnly competencia, PerfilTributarioDto perfil)
    {
        Competencia = competencia;
        _faixasInss = perfil.FaixasInss.Select(item => new FaixaTributaria(item.Numero, item.Limite, item.Aliquota, item.Deducao)).ToArray();
        _faixasIrrf = perfil.FaixasIrrf.Select(item => new FaixaTributaria(item.Numero, item.Limite, item.Aliquota, item.Deducao)).ToArray();
        _regrasReducao = perfil.ReducoesMensaisIrrf.Select(item => new RegraReducaoMensalIrrf(item.Faixa, item.LimiteRendimentos, item.Multiplicador, item.ValorBase)).ToArray();
        TetoInss = _faixasInss.Max(item => item.Limite);
        DeducaoPorDependente = perfil.DeducaoPorDependente;
        DescontoSimplificado = competencia >= InicioSimplificado ? perfil.DeducaoSimplificada : null;
        DescontoMinimo = perfil.DescontoMinimo;
    }

    public DateOnly Competencia { get; }
    public decimal TetoInss { get; }
    public decimal DeducaoPorDependente { get; }

    /// <summary>Nulo antes de 05/2023, quando a modalidade simplificada não existia.</summary>
    public decimal? DescontoSimplificado { get; }

    public decimal DescontoMinimo { get; }

    public static async Task<TabelasDaCompetencia> ObterAsync(ITributacaoConsulta consulta, DateOnly competencia, CancellationToken cancellationToken) =>
        new(competencia, await consulta.ObterPerfilAsync(competencia, cancellationToken));

    /// <summary>INSS do segurado empregado, com a base limitada ao teto. Valores negativos devem ser barrados antes, na validação.</summary>
    public ApuracaoInss CalcularInss(decimal baseInss)
    {
        var baseConsiderada = Math.Min(baseInss, TetoInss);
        var detalhes = CalculadoraInss.CalcularDetalhes(Competencia, baseConsiderada, _faixasInss).Select(Mapear).ToArray();
        return new ApuracaoInss(baseInss, baseConsiderada, CalculadoraTributacao.Arredondar(detalhes.Sum(item => item.Imposto)), detalhes);
    }

    /// <summary>INSS de 11% do contribuinte individual (sócio ou autônomo) retido pela empresa, com a base limitada ao teto.</summary>
    public ApuracaoInss CalcularInssContribuinteIndividual(decimal remuneracao)
    {
        var baseConsiderada = Math.Min(Math.Max(0m, remuneracao), TetoInss);
        var valor = CalculadoraTributacao.Arredondar(baseConsiderada * AliquotaContribuinteIndividual / 100m);
        return new ApuracaoInss(remuneracao, baseConsiderada, valor, baseConsiderada > 0m ? [new DetalheFaixaDto(1, baseConsiderada, AliquotaContribuinteIndividual, valor)] : []);
    }

    /// <param name="rendimentos">Rendimentos tributáveis, que também definem a redução mensal.</param>
    /// <param name="inss">Contribuição previdenciária deduzida na modalidade de deduções legais.</param>
    public ApuracaoIrrf CalcularIrrf(decimal rendimentos, decimal inss, int dependentes)
    {
        var baseNormal = Math.Max(0m, rendimentos - inss - dependentes * DeducaoPorDependente);
        var normal = CalcularModalidade("Normal", rendimentos, baseNormal);
        var simplificada = DescontoSimplificado is { } desconto
            ? CalcularModalidade("Simplificado", rendimentos, Math.Max(0m, rendimentos - desconto))
            : new ModalidadeIrrfDto("Simplificado", 0m, 0m, 0m, 0m, 0m, 0m, 0m, []);
        return new ApuracaoIrrf(rendimentos, inss, dependentes, DeducaoPorDependente, DescontoSimplificado, normal, simplificada);
    }

    private ModalidadeIrrfDto CalcularModalidade(string nome, decimal rendimentosTributaveis, decimal baseCalculo)
    {
        var faixa = _faixasIrrf.OrderBy(item => item.Numero).FirstOrDefault(item => baseCalculo <= item.Limite) ?? _faixasIrrf.MaxBy(item => item.Limite)!;
        var impostoAntesReducao = CalculadoraTributacao.CalcularPorFaixa(baseCalculo, _faixasIrrf);
        var reducao = CalculadoraReducaoMensalIrrf.Calcular(rendimentosTributaveis, impostoAntesReducao, _regrasReducao);
        var imposto = CalculadoraTributacao.Arredondar(impostoAntesReducao - reducao);
        var aliquotaEfetiva = rendimentosTributaveis == 0m ? 0m : Math.Truncate(imposto / rendimentosTributaveis * 10_000m) / 100m;
        return new ModalidadeIrrfDto(nome, baseCalculo, faixa.Aliquota, faixa.Deducao, impostoAntesReducao, reducao, imposto, aliquotaEfetiva, CalculadoraTributacao.CalcularProgressivo(baseCalculo, _faixasIrrf).Select(Mapear).ToArray());
    }

    private static DetalheFaixaDto Mapear(ResultadoFaixaTributaria item) => new(item.Faixa, item.BaseCalculada, item.Aliquota, item.Imposto);
}

/// <param name="BaseInformada">Base antes da limitação ao teto.</param>
internal sealed record ApuracaoInss(decimal BaseInformada, decimal BaseConsiderada, decimal Valor, IReadOnlyList<DetalheFaixaDto> Detalhes)
{
    public bool LimitadaAoTeto => BaseConsiderada < BaseInformada;
}

internal sealed record ApuracaoIrrf(
    decimal Rendimentos,
    decimal Inss,
    int Dependentes,
    decimal DeducaoPorDependente,
    decimal? DescontoSimplificado,
    ModalidadeIrrfDto Normal,
    ModalidadeIrrfDto Simplificada)
{
    // A fonte pagadora aplica o desconto simplificado quando ele resulta em imposto menor que o das deduções legais.
    public bool SimplificadaAplicada => DescontoSimplificado is not null && Simplificada.Imposto < Normal.Imposto;
    public ModalidadeIrrfDto Aplicada => SimplificadaAplicada ? Simplificada : Normal;
    public decimal Imposto => Aplicada.Imposto;
}
