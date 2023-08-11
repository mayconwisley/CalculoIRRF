using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Objetos
{
    public class Inss
    {
        public int Id { get; set; }
        public DateTime Competencia { get; set; }
        public int Faixa { get; set; }
        public int UltimaFaixa { get; private set; }
        public decimal Valor { get; set; }
        public decimal Porcentagem { get; set; }
        public decimal Teto { get; set; }

        public Inss() { }
        public Inss(DateTime competencia)
        {
            UltimaFaixaInss(competencia);
            TetoInss(competencia);
        }
        public Inss(decimal baseInss, DateTime competencia)
        {
            FaixaInss(baseInss, competencia);
        }
        public bool Gravar()
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("INSERT INTO INSS(Competencia, Faixa, Valor, Porcentagem) ");
            sqlBuilder.Append("VALUES(@Competencia, @Faixa, @Valor, @Porcentagem)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", Competencia);
                crud.AdicionarParamentro("Faixa", Faixa);
                crud.AdicionarParamentro("Valor", Valor);
                crud.AdicionarParamentro("Porcentagem", Porcentagem);
                crud.Executar(CommandType.Text, sqlBuilder.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Alterar()
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("UPDATE INSS ");
            sqlBuilder.Append("SET Competencia = @Competencia, Faixa = @Faixa, Valor = @Valor, Porcentagem = @Porcentagem ");
            sqlBuilder.Append("WHERE Id = @Id");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", Competencia);
                crud.AdicionarParamentro("Faixa", Faixa);
                crud.AdicionarParamentro("Valor", Valor);
                crud.AdicionarParamentro("Porcentagem", Porcentagem);
                crud.AdicionarParamentro("Id", Id);
                crud.Executar(CommandType.Text, sqlBuilder.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Excluir()
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("DELETE FROM INSS ");
            sqlBuilder.Append("WHERE Id = @Id");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Id", Id);
                crud.Executar(CommandType.Text, sqlBuilder.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void FaixaInss(decimal baseInss, DateTime competencia)
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
                    Faixa = 0;
                }
                else
                {
                    Faixa = int.Parse(strValor);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        private void UltimaFaixaInss(DateTime competencia)
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
                    UltimaFaixa = 0;
                }
                else
                {
                    UltimaFaixa = int.Parse(faixa);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public decimal PorcentagemInss(int faixa, DateTime competencia)
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
        public decimal ValorInss(int faixa, DateTime competencia)
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
        private void TetoInss(DateTime competencia)
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
                    Teto = 0;
                }
                else
                {
                    Teto = decimal.Parse(strValor);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public DataTable ListarTodos()
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
        public DataTable ListarTodosPorCompetencia(DateTime competencia)
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
