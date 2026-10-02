using System.Globalization;

namespace CalculoIRRF.Application.UseCases;

/// <summary>Formatação brasileira dos valores exibidos nos demonstrativos e na memória de cálculo.</summary>
internal static class Formato
{
    public static readonly CultureInfo Cultura = CultureInfo.GetCultureInfo("pt-BR");

    public static string Moeda(decimal valor) => valor.ToString("C2", Cultura);
    public static string Percentual(decimal valor) => valor.ToString("#,##0.00##", Cultura) + "%";
    public static string Numero(decimal valor) => valor.ToString("#,##0.##", Cultura);

    /// <summary>Percentual sem casas decimais desnecessárias, para nomes de verbas: "50%", "7,5%".</summary>
    public static string PercentualCurto(decimal valor) => Numero(valor) + "%";
    public static string Data(DateOnly data) => data.ToString("dd/MM/yyyy", Cultura);
    public static string Competencia(DateOnly competencia) => competencia.ToString("MM/yyyy", Cultura);
    public static string Dias(int dias) => dias == 1 ? "1 dia" : $"{dias} dias";
    public static string Avos(int avos) => $"{avos}/12";

    /// <summary>Horas em decimal no formato de relógio, como 10,5 em "10:30 h".</summary>
    public static string Horas(decimal horas)
    {
        var minutos = (long)Math.Round(horas * 60m, MidpointRounding.AwayFromZero);
        return $"{minutos / 60}:{minutos % 60:00} h";
    }
}
