using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Objetos
{
    public class Irrf
    {
        public int Id { get; set; }
        public DateTime Competencia { get; set; }
        public int Faixa { get; set; }
        public int UltimaFaixa { get; private set; }
        public decimal Valor { get; set; }
        public decimal Porcentagem { get; set; }
        public decimal Deducao { get; set; }

        public Irrf() { }
        public Irrf(DateTime competencia)
        {
            UltimaFaixaIrrf(competencia);
        }
        public Irrf(decimal baseIrrf, DateTime competencia)
        {
            FaixaIrrf(baseIrrf, competencia);
        }
        public bool Gravar()
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("INSERT INTO Irrf(Competencia, Faixa, Valor, Porcentagem, Deducao) ");
            sqlBuilder.Append("VALUES(@Competencia, @Faixa, @Valor, @Porcentagem, @Deducao)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", Competencia);
                crud.AdicionarParamentro("Faixa", Faixa);
                crud.AdicionarParamentro("Valor", Valor);
                crud.AdicionarParamentro("Porcentagem", Porcentagem);
                crud.AdicionarParamentro("Deducao", Deducao);
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

            sqlBuilder.Append("UPDATE Irrf ");
            sqlBuilder.Append("SET Competencia = @Competencia, Faixa = @Faixa, Valor = @Valor, Porcentagem = @Porcentagem, Deducao = @Deducao ");
            sqlBuilder.Append("WHERE Id = @Id");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", Competencia);
                crud.AdicionarParamentro("Faixa", Faixa);
                crud.AdicionarParamentro("Valor", Valor);
                crud.AdicionarParamentro("Porcentagem", Porcentagem);
                crud.AdicionarParamentro("Deducao", Deducao);
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

            sqlBuilder.Append("DELETE FROM Irrf ");
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
        private void FaixaIrrf(decimal baseInss, DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT MIN(Faixa) AS Faixa ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("WHERE Valor >= @Valor ");
            sqlBuilder.Append("AND Competencia = (SELECT MAX(Competencia) FROM IRRF WHERE Competencia <= @Competencia)");

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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        private void UltimaFaixaIrrf(DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT MAX(Faixa) AS Faixa ");
            sqlBuilder.Append("FROM IRRF ");
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
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public decimal PorcentagemIrrf(int faixa, DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Porcentagem ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("WHERE Faixa = @Faixa ");
            sqlBuilder.Append("AND Competencia = (SELECT MAX(Competencia) FROM IRRF WHERE Competencia <= @Competencia)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Faixa", faixa);
                crud.AdicionarParamentro("Competencia", competencia);

                var strValor = crud.Executar(CommandType.Text, sqlBuilder.ToString());
                if (strValor is null)
                {
                    return 0;
                }
                else
                {
                    return decimal.Parse(strValor.ToString());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public decimal DeducaoIrrf(int faixa, DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Deducao ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("WHERE Faixa = @Faixa ");
            sqlBuilder.Append("AND Competencia = (SELECT MAX(Competencia) FROM IRRF WHERE Competencia <= @Competencia)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Faixa", faixa);
                crud.AdicionarParamentro("Competencia", competencia);
                var strValor = crud.Executar(CommandType.Text, sqlBuilder.ToString());

                if (strValor is null)
                {
                    return 0;
                }
                else
                {
                    return decimal.Parse(strValor.ToString());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public decimal ValorIrrf(int faixa, DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Valor ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("WHERE Faixa = @Faixa ");
            sqlBuilder.Append("AND Competencia = (SELECT MAX(Competencia) FROM IRRF WHERE Competencia <= @Competencia)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Faixa", faixa);
                crud.AdicionarParamentro("Competencia", competencia);
                var strValor = crud.Executar(CommandType.Text, sqlBuilder.ToString());
                if (strValor is null)
                {
                    return 0;
                }
                else
                {
                    return decimal.Parse(strValor.ToString());
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public DataTable ListarTodos()
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Id, Competencia, Faixa, Valor, Porcentagem, Deducao ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("ORDER BY Competencia DESC");

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
        public DataTable ListarTodosPorCompetencia(DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Id, Competencia, Faixa, Valor, Porcentagem, Deducao ");
            sqlBuilder.Append("FROM IRRF ");
            sqlBuilder.Append("WHERE Competencia = (SELECT MAX(Competencia) FROM IRRF WHERE Competencia <= @Competencia)");

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
