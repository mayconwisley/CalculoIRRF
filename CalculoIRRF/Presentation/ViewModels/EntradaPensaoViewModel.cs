namespace CalculoIRRF.Presentation.ViewModels;

public sealed record EntradaPensaoViewModel(DateOnly Competencia, decimal ValorBruto, decimal BaseInss, int Dependentes);
