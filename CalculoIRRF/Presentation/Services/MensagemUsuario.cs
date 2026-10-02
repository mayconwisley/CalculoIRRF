namespace CalculoIRRF.Presentation.Services;

/// <summary>
/// Texto de uma exceção como o usuário deve lê-lo. As ArgumentException acrescentam " (Parameter 'nome')" à mensagem,
/// detalhe útil para o desenvolvedor, mas que não deve aparecer nas janelas de aviso.
/// </summary>
public static class MensagemUsuario
{
    public static string De(Exception exception)
    {
        if (exception is not ArgumentException { ParamName: { Length: > 0 } parametro } argumento)
            return exception.Message;

        // O próprio .NET gera o sufixo, no mesmo formato e idioma usados na mensagem original.
        var sufixo = new ArgumentException(string.Empty, parametro).Message;
        return argumento.Message.EndsWith(sufixo, StringComparison.Ordinal) ? argumento.Message[..^sufixo.Length] : argumento.Message;
    }
}
