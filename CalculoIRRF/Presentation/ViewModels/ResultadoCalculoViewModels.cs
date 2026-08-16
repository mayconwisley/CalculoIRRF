#nullable enable

namespace CalculoIRRF.Presentation.ViewModels;

public sealed record IndicadorResumoViewModel(string Rotulo, string Valor, string? Complemento = null);

public sealed record ComparativoIrrfViewModel(
    string Modalidade,
    string BaseCalculo,
    string ReducaoMensal,
    string ImpostoFinal,
    bool EhMaisVantajosa);

public sealed record ComparativoPensaoViewModel(string Modalidade, string IrrfFinal, string Pensao, string Total);

public sealed record LinhaFaixaTributariaViewModel(string Faixa, string BaseCalculada, string Aliquota, string Imposto);

public sealed record SecaoMemoriaTributariaViewModel(string Titulo, string Total, IReadOnlyList<LinhaFaixaTributariaViewModel> Linhas);

public sealed record FormulaCalculoViewModel(string Titulo, string Formula);

public sealed record SecaoMemoriaIrrfViewModel(string Titulo, string Destaque, IReadOnlyList<FormulaCalculoViewModel> Formulas);

public sealed record IteracaoMemoriaPensaoViewModel(string Titulo, IReadOnlyList<FormulaCalculoViewModel> Formulas);

public sealed record SecaoMemoriaPensaoViewModel(string Titulo, string Destaque, IReadOnlyList<IteracaoMemoriaPensaoViewModel> Iteracoes);
