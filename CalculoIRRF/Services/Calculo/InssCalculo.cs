using System;
using System.Text;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Calculo;

public class InssCalculo(DateTime _competencia, decimal _baseInss)
{
    public async Task<decimal> NormalProgressivo()
    {
        InssServices inss = new();

        decimal teto = await inss.TetoInss(_competencia);

        if (_baseInss > teto)
        {
            _baseInss = teto;
        }

        int faixaInss = await inss.FaixaInss(_baseInss, _competencia);

        decimal desconto = 0;
        decimal valorInssAnterior = 0;

        for (int i = 1; i <= faixaInss; i++)
        {
            decimal porcentagemInss = await inss.PorcentagemInss(i, _competencia);
            decimal valorInss = await inss.ValorInss(i, _competencia);

            decimal baseInssCalculo = valorInss - valorInssAnterior;

            if (valorInss > _baseInss)
            {
                baseInssCalculo = _baseInss - valorInssAnterior;
            }

            if (baseInssCalculo > _baseInss)
            {
                baseInssCalculo = _baseInss - valorInssAnterior;
            }

            desconto += baseInssCalculo * (porcentagemInss / 100);
            valorInssAnterior = valorInss;
        }
        desconto = Math.Round(desconto, 2);

        return desconto;
    }
    public async Task<string> DescricaoCalculoNormalProgressivo()
    {
        StringBuilder strMensagem = new();
        InssServices inss = new();

        int faixaInss = await inss.FaixaInss(_baseInss, _competencia);
        decimal totalDesconto = 0;
        decimal valorInssAnterior = 0;

        strMensagem.Append("Informações de Calculo do INSS\n\n");
        strMensagem.Append($"Base INSS: {_baseInss:#,##0.00}\n");

        for (int i = 1; i <= faixaInss; i++)
        {
            decimal porcentagemInss = await inss.PorcentagemInss(i, _competencia);
            decimal valorInss = await inss.ValorInss(i, _competencia);

            decimal baseInssCalculo = valorInss - valorInssAnterior;

            if (valorInss > _baseInss)
            {
                baseInssCalculo = _baseInss - valorInssAnterior;
            }

            if (baseInssCalculo > _baseInss)
            {
                baseInssCalculo = _baseInss - valorInssAnterior;
            }

            decimal desconto = baseInssCalculo * (porcentagemInss / 100);
            totalDesconto += desconto;

            valorInssAnterior = valorInss;

            strMensagem.Append($"{i}º Faixa ");
            strMensagem.Append($"Base do INSS: {baseInssCalculo:#,##0.00} ");
            strMensagem.Append($"Porcentagem: {porcentagemInss:#,##0.00}% ");
            strMensagem.Append($"Imposto: {desconto:#,##0.00}\n");
        }
        strMensagem.Append($"Valor do Desconto: {totalDesconto:#,##0.00}\n\n");

        return strMensagem.ToString();
    }
}
