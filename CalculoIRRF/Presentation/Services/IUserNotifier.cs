namespace CalculoIRRF.Presentation.Services;

public interface IUserNotifier
{
    void MostrarAviso(string mensagem, string titulo = "Dados inválidos");
    void MostrarErro(string mensagem, Exception exception);
}
