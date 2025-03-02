using System;
using System.Text;
using System.Text.RegularExpressions;

namespace CalculoIRRF.Services.Validacao;

public class Validar
{
    public static string ValidarValor(string valor)
    {
        var strValor1 = new StringBuilder();
        string strValor = string.Empty;
        try
        {
            foreach (char x in valor)
            {
                var posicao = "1234567890,.".IndexOf(x);
                if (posicao >= 0)
                {
                    if (x == Convert.ToChar(","))
                    {
                        if (strValor.IndexOf(",") < 0)
                        {
                            strValor1.Append(strValor);
                            strValor1.Append(x);
                        }
                    }
                    else
                    {
                        strValor1.Append(strValor);
                        strValor1.Append(x);
                    }
                }
            }
            return strValor1.ToString(); ;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static string Formatar(string valor)
    {
        decimal formatar = decimal.Parse(valor);
        var strValor = formatar.ToString("#,##0.00");
        if (strValor == "0,00")
        {
            strValor = "0,00";
        }
        return strValor;
    }

    public static string Zero(string valor)
    {
        var strValor = valor;
        if (valor == "")
        {
            strValor = "0,00";
        }
        return strValor;
    }

    public static string ValidarNumero(string valor)
    {
        var strValor1 = new StringBuilder();
        string strValor = string.Empty;
        try
        {
            foreach (char x in valor)
            {
                var posicao = "1234567890".IndexOf(x);
                if (posicao >= 0)
                {
                    strValor1.Append(strValor);
                    strValor1.Append(x);
                }
            }
            return strValor1.ToString();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public static string FormatarNumero(string valor)
    {
        decimal formatar = decimal.Parse(valor);
        var strValor = formatar.ToString("0");
        if (strValor == "0")
        {
            strValor = "0";
        }
        return strValor;
    }

    public string ZeroNumero(string valor)
    {
        var strValor = valor;
        if (valor == "")
        {
            strValor = "0";
        }
        return strValor;
    }

    public static decimal ExtrairValor(string dados)
    {
        var match = Regex.Match(dados, @"[\d,.]+");

        if (match.Success)
        {
            return decimal.Parse(match.Value);
        }
        else
        {
            return 0;
        }
    }
    public static decimal ExtrairMaiorValor(string dados)
    {
        var matches = Regex.Matches(dados, @"[\d,.]+");

        if (matches.Count == 1)
        {
            return decimal.Parse(matches[0].Value);
        }

        if (decimal.Parse(matches[0].Value) > decimal.Parse(matches[1].Value))
        {
            return decimal.Parse(matches[0].Value);
        }
        else
        {
            return decimal.Parse(matches[1].Value);
        }
    }
}
