using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelos.Irrf
{
    public class Listar
    {
        public int Faixa(decimal baseIrrf)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT MIN(Faixa) AS Faixa ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("WHERE Valor >= @Valor");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Valor", baseIrrf);
                return int.Parse(crud.Executar(CommandType.Text, sqlBuilder.ToString()).ToString());
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        public decimal Porcentagem(int faixa)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Porcentagem ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("WHERE Faixa = @Faixa");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Faixa", faixa);
                return decimal.Parse(crud.Executar(CommandType.Text, sqlBuilder.ToString()).ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public decimal Deducao(int faixa)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Deducao ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("WHERE Faixa = @Faixa");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Faixa", faixa);
                return decimal.Parse(crud.Executar(CommandType.Text, sqlBuilder.ToString()).ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public decimal Valor(int faixa)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Valor ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("WHERE Faixa = @Faixa");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Faixa", faixa);
                return decimal.Parse(crud.Executar(CommandType.Text, sqlBuilder.ToString()).ToString());
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable TodosItens()
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Id, Competencia, Faixa, Valor, Porcentagem, Deducao ");
            sqlBuilder.Append("FROM IRRF");

            try
            {
                crud.LimparParametro();
                DataTable dataTable = crud.Consulta(CommandType.Text, sqlBuilder.ToString());
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public DataTable TodosItensPorCompetencia(DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Id, Competencia, Faixa, Valor, Porcentagem, Deducao ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("WHERE Competencia = @Competencia ");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", competencia);
                DataTable dataTable = crud.Consulta(CommandType.Text, sqlBuilder.ToString());
                return dataTable;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
