using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelo.Inss
{
    public class Listar
    {
        public int Faixa(decimal baseInss, DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT MIN(Faixa) AS Faixa ");
            sqlBuilder.Append("FROM INSS ");
            sqlBuilder.Append("WHERE Valor >= @Valor ");
            sqlBuilder.Append("AND Competencia = (SELECT MAX(Competencia) FROM INSS WHERE Competencia <= @Competencia)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Valor", baseInss);
                crud.AdicionarParamentro("Competencia", competencia);
                string strValor = crud.Executar(CommandType.Text, sqlBuilder.ToString()).ToString();

                if (strValor == "")
                {
                    return 0;
                }
                else
                {
                    return int.Parse(strValor);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int UltimaFaixa(DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT MAX(Faixa) AS Faixa ");
            sqlBuilder.Append("FROM INSS ");
            sqlBuilder.Append("WHERE Competencia = @Competencia");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", competencia);

                string faixa = crud.Executar(CommandType.Text, sqlBuilder.ToString()).ToString();

                if (faixa == "")
                {
                    return 0;
                }
                else
                {
                    return int.Parse(faixa);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public decimal Porcentagem(int faixa, DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Porcentagem ");
            sqlBuilder.Append("FROM INSS ");
            sqlBuilder.Append("WHERE Faixa = @Faixa ");
            sqlBuilder.Append("AND Competencia = (SELECT MAX(Competencia) FROM INSS WHERE Competencia <= @Competencia)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Faixa", faixa);
                crud.AdicionarParamentro("Competencia", competencia);
                string strValor = crud.Executar(CommandType.Text, sqlBuilder.ToString()).ToString();
                if (strValor == "")
                {
                    return 0;
                }
                else
                {
                    return decimal.Parse(strValor);
                }

            }
            catch (Exception)
            {
                throw;
            }
        }
        public decimal Valor(int faixa, DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Valor ");
            sqlBuilder.Append("FROM INSS ");
            sqlBuilder.Append("WHERE Faixa = @Faixa ");
            sqlBuilder.Append("AND Competencia = (SELECT MAX(Competencia) FROM INSS WHERE Competencia <= @Competencia)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Faixa", faixa);
                crud.AdicionarParamentro("Competencia", competencia);
                string strValor = crud.Executar(CommandType.Text, sqlBuilder.ToString()).ToString();
                if (strValor == "")
                {
                    return 0;
                }
                else
                {
                    return decimal.Parse(strValor);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public decimal Teto(DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT MAX(Valor) ");
            sqlBuilder.Append("FROM INSS ");
            sqlBuilder.Append("WHERE Competencia = (SELECT MAX(Competencia) FROM INSS WHERE Competencia <= @Competencia) ");
            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", competencia);
                string strValor = crud.Executar(CommandType.Text, sqlBuilder.ToString()).ToString();

                if (strValor == "")
                {
                    return 0;
                }
                else
                {
                    return decimal.Parse(strValor);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable TodosItens()
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Id, Competencia, Faixa, Valor, Porcentagem ");
            sqlBuilder.Append("FROM INSS ");
            sqlBuilder.Append("ORDER BY Competencia DESC");

            try
            {
                crud.LimparParametro();
                DataTable dataTable = crud.Consulta(CommandType.Text, sqlBuilder.ToString());
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable TodosItensPorCompetencia(DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Id, Competencia, Faixa, Valor, Porcentagem ");
            sqlBuilder.Append("FROM INSS ");
            sqlBuilder.Append("WHERE Competencia = (SELECT MAX(Competencia) FROM INSS WHERE Competencia <= @Competencia)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", competencia);
                DataTable dataTable = crud.Consulta(CommandType.Text, sqlBuilder.ToString());
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
