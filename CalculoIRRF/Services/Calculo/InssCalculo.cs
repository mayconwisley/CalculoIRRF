using CalculoIRRF.Services.Interface;
using System;
using System.Text;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Calculo;

public class InssCalculo(DateTime _competencia, double _baseInss, IInssServices _inssServices)
{
    public async Task<double> NormalProgressivo()
    {
        double teto = await _inssServices.TetoInss(_competencia);

        if (_baseInss > teto)
        {
            _baseInss = teto;
        }

        int faixaInss = await _inssServices.FaixaInss(_baseInss, _competencia);

        double desconto = 0;
        double valorInssAnterior = 0;

        for (int i = 1; i <= faixaInss; i++)
        {
            double porcentagemInss = await _inssServices.PorcentagemInss(i, _competencia);
            double valorInss = await _inssServices.ValorInss(i, _competencia);

            double baseInssCalculo = valorInss - valorInssAnterior;

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

        int faixaInss = await _inssServices.FaixaInss(_baseInss, _competencia);
        double totalDesconto = 0;
        double valorInssAnterior = 0;

        strMensagem.Append("Informações de Calculo do INSS\n\n");
        strMensagem.Append($"Base INSS: {_baseInss:#,##0.00}\n");

        for (int i = 1; i <= faixaInss; i++)
        {
            double porcentagemInss = await _inssServices.PorcentagemInss(i, _competencia);
            double valorInss = await _inssServices.ValorInss(i, _competencia);

            double baseInssCalculo = valorInss - valorInssAnterior;

            if (valorInss > _baseInss)
            {
                baseInssCalculo = _baseInss - valorInssAnterior;
            }

            if (baseInssCalculo > _baseInss)
            {
                baseInssCalculo = _baseInss - valorInssAnterior;
            }

            double desconto = baseInssCalculo * (porcentagemInss / 100);
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
