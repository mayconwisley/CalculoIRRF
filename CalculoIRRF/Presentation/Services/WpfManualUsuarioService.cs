#nullable enable

using System.Diagnostics;
using System.IO;

namespace CalculoIRRF.Presentation.Services;

/// <summary>
/// Abre o manual em PDF distribuído com a aplicação; sem o arquivo ou sem leitor de PDF, abre a versão publicada no GitHub.
/// </summary>
public sealed class WpfManualUsuarioService(IUserNotifier notificador) : IManualUsuarioService
{
    private const string ManualOnline = "https://github.com/mayconwisley/CalculoIRRF/blob/master/docs/MANUAL.md";
    private static readonly string ManualLocal = Path.Combine(AppContext.BaseDirectory, "Manual", "ManualDoUsuario.pdf");

    public void Abrir()
    {
        if (File.Exists(ManualLocal) && TentarAbrir(ManualLocal, out _))
            return;

        if (!TentarAbrir(ManualOnline, out var erro))
            notificador.MostrarErro("Não foi possível abrir o manual do usuário.", erro!);
    }

    private static bool TentarAbrir(string destino, out Exception? erro)
    {
        try
        {
            Process.Start(new ProcessStartInfo(destino) { UseShellExecute = true });
            erro = null;
            return true;
        }
        catch (Exception exception)
        {
            erro = exception;
            return false;
        }
    }
}
