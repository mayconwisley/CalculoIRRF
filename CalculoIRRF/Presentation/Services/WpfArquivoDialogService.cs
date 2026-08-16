#nullable enable

using Microsoft.Win32;

namespace CalculoIRRF.Presentation.Services;

public sealed class WpfArquivoDialogService : IArquivoDialogService
{
    public string? SolicitarDestinoPdf(string nomeArquivoSugerido)
    {
        var dialogo = new SaveFileDialog
        {
            Title = "Salvar relatório em PDF",
            Filter = "Arquivo PDF (*.pdf)|*.pdf",
            DefaultExt = ".pdf",
            AddExtension = true,
            FileName = nomeArquivoSugerido
        };

        return dialogo.ShowDialog() == true ? dialogo.FileName : null;
    }
}
