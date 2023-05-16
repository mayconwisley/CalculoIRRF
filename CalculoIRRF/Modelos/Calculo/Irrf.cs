using System;
using System.Text;

namespace CalculoIRRF.Modelos.Calculo
{
    public class Irrf
    {
        DateTime _competencia;
        int _qtdDependente = 0;
        decimal _valorInss = 0;
        decimal _valorBruto = 0;


        public Irrf(DateTime competencia, int qtdDependente, decimal valorInss, decimal valorBruto)
        {
            _competencia = competencia;
            _qtdDependente = qtdDependente;
            _valorInss = valorInss;
            _valorBruto = valorBruto;
        }
        public decimal Normal()
        {
            Modelos.Irrf.Listar listar = new Modelos.Irrf.Listar();
            Modelos.Dependente.Listar listarDep = new Dependente.Listar();
            decimal valorDependente = listarDep.Valor(_competencia);

            decimal baseIrrf = _valorBruto - _valorInss - (_qtdDependente * valorDependente);
            int faixaIrrf = listar.Faixa(baseIrrf, _competencia);
            decimal porcentagemIrrf = listar.Porcentagem(faixaIrrf, _competencia);
            decimal deducaoIrrf = listar.Deducao(faixaIrrf, _competencia);

            decimal desconto = (baseIrrf * (porcentagemIrrf / 100)) - deducaoIrrf;
            desconto = Math.Round(desconto, 2);

            return desconto;
        }
        public string DescricaoCalculoNormal()
        {
            Modelos.Irrf.Listar listar = new Modelos.Irrf.Listar();
            Modelos.Dependente.Listar listarDep = new Dependente.Listar();
            decimal valorDependente = listarDep.Valor(_competencia);

            decimal baseIrrf = _valorBruto - _valorInss - (_qtdDependente * valorDependente);
            int faixaIrrf = listar.Faixa(baseIrrf, _competencia);
            decimal porcentagemIrrf = listar.Porcentagem(faixaIrrf, _competencia);
            decimal deducaoIrrf = listar.Deducao(faixaIrrf, _competencia);

            decimal desconto = (baseIrrf * (porcentagemIrrf / 100)) - deducaoIrrf;
            desconto = Math.Round(desconto, 2);

            StringBuilder strMensagem = new StringBuilder();
            strMensagem.Append("Informações de Calculo do IR Normal\n\n");
            strMensagem.Append($"Valor Bruto: {_valorBruto:#,##0.00}\n");
            strMensagem.Append($"Valor INSS: {_valorInss:#,##0.00}\n");
            strMensagem.Append($"Quantidade Dependente: {_qtdDependente} Valor: {valorDependente:#,##0.00} Total: {(_qtdDependente * valorDependente):#,##0.00}\n");
            strMensagem.Append($"Valor Base do IR: {baseIrrf:#,##0.00}\n");
            strMensagem.Append($"Porcentagem: {porcentagemIrrf:#,##0.00}% - Dedução: {deducaoIrrf:#,##0.00}\n");
            strMensagem.Append($"Valor do Desconto: {desconto:#,##0.00}\n\n");

            return strMensagem.ToString();
        }

        public decimal NormalProgressivo()
        {
            Modelos.Irrf.Listar listar = new Modelos.Irrf.Listar();
            Modelos.Dependente.Listar listarDep = new Dependente.Listar();
            decimal valorDependente = listarDep.Valor(_competencia);

            decimal baseIrrf = _valorBruto - _valorInss - (_qtdDependente * valorDependente);
            int faixaIrrf = listar.Faixa(baseIrrf, _competencia);

            decimal desconto = 0;
            decimal valorIrrfAnterior = 0;
            for (int i = 1; i <= faixaIrrf; i++)
            {
                decimal porcentagemIrrf = listar.Porcentagem(i, _competencia);
                decimal valorIrrf = listar.Valor(i, _competencia);

                decimal baseIrrfCalculo = valorIrrf - valorIrrfAnterior;

                if (baseIrrfCalculo > _valorBruto)
                {
                    baseIrrfCalculo = _valorBruto - valorIrrfAnterior;
                }

                desconto += (baseIrrfCalculo * (porcentagemIrrf / 100));
                valorIrrfAnterior = valorIrrf;
            }
            desconto = Math.Round(desconto, 2);

            return desconto;
        }

        public string DescricaoCalculoNormalProgrssivo()
        {
            StringBuilder strMensagem = new StringBuilder();
            Modelos.Irrf.Listar listar = new Modelos.Irrf.Listar();
            Modelos.Dependente.Listar listarDep = new Dependente.Listar();
            decimal valorDependente = listarDep.Valor(_competencia);

            decimal baseIrrf = _valorBruto - _valorInss - (_qtdDependente * valorDependente);
            int faixaIrrf = listar.Faixa(baseIrrf, _competencia);

            decimal desconto = 0;
            decimal totalDesconto = 0;
            decimal valorIrrfAnterior = 0;

            strMensagem.Append("Informações de Calculo do IR Normal Prograssivo\n\n");
            strMensagem.Append($"Valor Bruto: {_valorBruto:#,##0.00}\n");
            strMensagem.Append($"Valor INSS: {_valorInss:#,##0.00}\n");
            strMensagem.Append($"Quantidade Dependente: {_qtdDependente} Valor: {valorDependente:#,##0.00} Total: {(_qtdDependente * valorDependente):#,##0.00}\n");
            strMensagem.Append($"Base de IR: {baseIrrf:#,##0.00}\n");

            for (int i = 1; i <= faixaIrrf; i++)
            {
                decimal porcentagemIrrf = listar.Porcentagem(i, _competencia);
                decimal valorIrrf = listar.Valor(i, _competencia);

                decimal baseIrrfCalculo = valorIrrf - valorIrrfAnterior;

                if (valorIrrf > baseIrrf)
                {
                    baseIrrfCalculo = baseIrrf - valorIrrfAnterior;
                }

                if (baseIrrfCalculo > _valorBruto)
                {
                    baseIrrfCalculo = baseIrrf - valorIrrfAnterior;
                }

                desconto = (baseIrrfCalculo * (porcentagemIrrf / 100));
                totalDesconto += desconto;

                valorIrrfAnterior = valorIrrf;

                strMensagem.Append($"{i}º Faixa ");
                strMensagem.Append($"Base do IR: {baseIrrfCalculo:#,##0.00} ");
                strMensagem.Append($"Porcentagem: {porcentagemIrrf:#,##0.00}% ");
                strMensagem.Append($"Imposto: {desconto:#,##0.00}\n");
            }
            strMensagem.Append($"Valor do Desconto: {totalDesconto:#,##0.00}\n\n");

            return strMensagem.ToString();
        }

        public decimal Simplificado()
        {
            if (_competencia < DateTime.Parse("01/05/2023"))
            {
                return 0;
            }

            Modelos.Simplificado.Listar listar = new Simplificado.Listar();
            decimal valorDeducao = listar.Valor(_competencia);
            decimal baseIrrf = _valorBruto - valorDeducao;

            Modelos.Irrf.Listar listarIrrf = new Modelos.Irrf.Listar();
            int faixaIrrf = listarIrrf.Faixa(baseIrrf, _competencia);
            decimal porcentagemIrrf = listarIrrf.Porcentagem(faixaIrrf, _competencia);
            decimal deducaoIrrf = listarIrrf.Deducao(faixaIrrf, _competencia);

            decimal desconto = (baseIrrf * (porcentagemIrrf / 100)) - deducaoIrrf;
            desconto = Math.Round(desconto, 2);

            return desconto;
        }

        public string DescricaoCalculoSimplificado()
        {
            if (_competencia < DateTime.Parse("01/05/2023"))
            {
                return "Calculo Simplificado é a partir de 05/2023!\n\n\n";
            }

            Modelos.Simplificado.Listar listar = new Simplificado.Listar();
            decimal valorDeducao = listar.Valor(_competencia);
            decimal baseIrrf = _valorBruto - valorDeducao;

            Modelos.Irrf.Listar listarIrrf = new Modelos.Irrf.Listar();
            int faixaIrrf = listarIrrf.Faixa(baseIrrf, _competencia);
            decimal porcentagemIrrf = listarIrrf.Porcentagem(faixaIrrf, _competencia);
            decimal deducaoIrrf = listarIrrf.Deducao(faixaIrrf, _competencia);

            decimal desconto = (baseIrrf * (porcentagemIrrf / 100)) - deducaoIrrf;
            desconto = Math.Round(desconto, 2);

            StringBuilder strMensagem = new StringBuilder();
            strMensagem.Append("Informações de Calculo do IR Simplificado\n\n");
            strMensagem.Append($"Valor Bruto: {_valorBruto:#,##0.00}\n");
            strMensagem.Append($"Valor Dedução: {valorDeducao:#,##0.00}\n");
            strMensagem.Append($"Valor Base do IR: {baseIrrf:#,##0.00}\n");
            strMensagem.Append($"Porcentagem: {porcentagemIrrf:#,##0.00}% - Dedução: {deducaoIrrf:#,##0.00}\n");
            strMensagem.Append($"Valor do Desconto: {desconto:#,##0.00}\n\n\n");

            return strMensagem.ToString();

        }

        public string Vantagem()
        {
            if (_competencia < DateTime.Parse("01/05/2023"))
            {
                return "";
            }

            decimal valorNormal = Normal();
            decimal valorSimplificado = Simplificado();

            if (valorNormal > valorSimplificado)
            {
                decimal total = valorNormal - valorSimplificado;

                return $"Calculo Simplificado é mais vantajoso!\nDiferença: {total:#,##0.00}\n\n";
            }
            else
            {
                decimal total = valorSimplificado - valorNormal;
                return $"Calculo Normal é mais vantajoso!\nDiferença: {total:#,##0.00}\n\n";
            }
        }
    }
}
