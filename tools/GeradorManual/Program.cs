using GeradorManual;
using QuestPDF.Infrastructure;

// Uso: dotnet run --project tools/GeradorManual [-- <manual.md> <saida.pdf> [--previa <pasta>]]
// Sem argumentos, lê docs/MANUAL.md e grava docs/ManualDoUsuario.pdf a partir da pasta atual (raiz do repositório).
QuestPDF.Settings.License = LicenseType.Community;

var posicionais = args.Where(argumento => !argumento.StartsWith("--", StringComparison.Ordinal)).ToArray();
var manual = Path.GetFullPath(posicionais.ElementAtOrDefault(0) ?? Path.Combine("docs", "MANUAL.md"));
var saida = Path.GetFullPath(posicionais.ElementAtOrDefault(1) ?? Path.Combine("docs", "ManualDoUsuario.pdf"));
var indicePrevia = Array.IndexOf(args, "--previa");
var pastaPrevia = indicePrevia >= 0 && indicePrevia + 1 < args.Length ? Path.GetFullPath(args[indicePrevia + 1]) : null;

if (!File.Exists(manual))
{
    Console.Error.WriteLine($"Manual não encontrado: {manual}");
    return 1;
}

var documento = new ManualDocument(manual);
documento.GerarPdf(saida);
Console.WriteLine($"PDF gerado: {saida} ({new FileInfo(saida).Length / 1024} KB)");

if (pastaPrevia is not null)
{
    Directory.CreateDirectory(pastaPrevia);
    var paginas = documento.GerarPrevia(pastaPrevia);
    Console.WriteLine($"Prévia: {paginas} página(s) em {pastaPrevia}");
}

return 0;
