#nullable enable

using CalculoIRRF.Application.DTOs;
using System.Globalization;

namespace CalculoIRRF.Presentation.ViewModels.Calculadoras;

public enum TipoCalculadora { SalarioPeloLiquido, DecimoTerceiro, Ferias, HorasExtras, Rescisao, CustoFuncionario, ProLaboreAutonomo }

/// <summary>Dados da tela principal aproveitados para preencher uma calculadora ao abri-la.</summary>
public sealed record ContextoCalculo(DateOnly? Competencia, decimal? Salario, int? Dependentes);

/// <summary>Uma calculadora da janela padrão: os campos do formulário e o cálculo que produz o demonstrativo.</summary>
public interface ICalculadora
{
    string Titulo { get; }
    string Descricao { get; }
    string InstrucaoInicial { get; }
    IReadOnlyList<CampoViewModel> Campos { get; }

    /// <summary>Nome sugerido para o PDF do último cálculo.</summary>
    string NomeArquivoPdf { get; }

    void Preencher(ContextoCalculo contexto);

    /// <summary>Lança <see cref="ArgumentException"/> quando um campo está vazio, em formato inválido ou fora das regras.</summary>
    Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken);
}

/// <summary>Leitura dos campos no padrão brasileiro, com mensagens que dizem qual campo corrigir e como.</summary>
public abstract class CalculadoraBase : ICalculadora
{
    protected static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");

    public abstract string Titulo { get; }
    public abstract string Descricao { get; }
    public abstract string InstrucaoInicial { get; }
    public abstract IReadOnlyList<CampoViewModel> Campos { get; }
    public abstract string NomeArquivoPdf { get; }

    public abstract void Preencher(ContextoCalculo contexto);
    public abstract Task<DemonstrativoDto> CalcularAsync(CancellationToken cancellationToken);

    protected static CampoTextoViewModel Competencia(string rotulo = "Competência", string? dica = null) =>
        new(rotulo, TipoCampo.Competencia, DateTime.Today.ToString("MM/yyyy", Cultura), dica ?? "Mês e ano das tabelas de INSS e IRRF (MM/AAAA).");

    protected static CampoTextoViewModel Moeda(string rotulo, string? dica = null) => new(rotulo, TipoCampo.Moeda, "0,00", dica);

    protected static CampoTextoViewModel Inteiro(string rotulo, int inicial, string? dica = null) => new(rotulo, TipoCampo.Inteiro, inicial.ToString(Cultura), dica);

    protected static void PreencherMoeda(CampoTextoViewModel campo, decimal? valor)
    {
        if (valor is { } informado && informado > 0m) campo.Valor = informado.ToString("N2", Cultura);
    }

    protected static decimal LerMoeda(CampoTextoViewModel campo) =>
        decimal.TryParse(campo.Valor, NumberStyles.Number, Cultura, out var valor) ? valor : throw Invalido(campo, "use somente números e vírgula nos centavos, por exemplo 3.500,00");

    protected static decimal LerNumero(CampoTextoViewModel campo) =>
        decimal.TryParse(campo.Valor, NumberStyles.Number, Cultura, out var valor) ? valor : throw Invalido(campo, "use somente números, com vírgula nas casas decimais");

    protected static int LerInteiro(CampoTextoViewModel campo) =>
        int.TryParse(campo.Valor.Trim(), NumberStyles.Integer, Cultura, out var valor) ? valor : throw Invalido(campo, "use um número inteiro");

    protected static DateOnly LerData(CampoTextoViewModel campo) =>
        DateOnly.TryParseExact(campo.Valor.Trim(), "dd/MM/yyyy", Cultura, DateTimeStyles.None, out var data) ? data : throw Invalido(campo, "use o formato dd/mm/aaaa");

    protected static DateOnly LerCompetencia(CampoTextoViewModel campo) =>
        DateTime.TryParseExact(campo.Valor.Trim(), "MM/yyyy", Cultura, DateTimeStyles.None, out var data) ? DateOnly.FromDateTime(data) : throw Invalido(campo, "use o formato mm/aaaa");

    /// <summary>Aceita horas de relógio ("10:30") ou em decimal ("10,5").</summary>
    protected static decimal LerHoras(CampoTextoViewModel campo)
    {
        var texto = campo.Valor.Trim();
        var partes = texto.Split(':');
        if (partes.Length == 2 && int.TryParse(partes[0], out var horas) && int.TryParse(partes[1], out var minutos) && horas >= 0 && minutos is >= 0 and < 60)
            return horas + minutos / 60m;
        return decimal.TryParse(texto, NumberStyles.Number, Cultura, out var decimais) ? decimais : throw Invalido(campo, "use horas e minutos, como 10:30, ou horas decimais, como 10,5");
    }

    private static ArgumentException Invalido(CampoViewModel campo, string orientacao) => new($"O campo \"{campo.Rotulo}\" está em formato inválido: {orientacao}.");
}
