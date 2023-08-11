using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Objetos
{
    public class Simplificado
    {
        public int Id { get; set; }
        public DateTime Competencia { get; set; }
        public decimal Valor { get; set; }

        public Simplificado() { }
        public Simplificado(DateTime competencia)
        {
            ValorSimplificado(competencia);
        }

        public bool Gravar()
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("INSERT INTO Simplificado(Competencia, Valor) ");
            sqlBuilder.Append("VALUES(@Competencia, @Valor)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", Competencia);
                crud.AdicionarParamentro("Valor", Valor);
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

            sqlBuilder.Append("UPDATE Simplificado ");
            sqlBuilder.Append("SET Competencia = @Competencia, Valor = @Valor ");
            sqlBuilder.Append("WHERE Id = @Id");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", Competencia);
                crud.AdicionarParamentro("Valor", Valor);
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

            sqlBuilder.Append("DELETE FROM Simplificado ");
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
        private void ValorSimplificado(DateTime competencia)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("SELECT Valor ");
            sqlBuilder.Append("FROM Simplificado ");
            sqlBuilder.Append("WHERE Competencia = (SELECT MAX(Competencia) FROM Simplificado WHERE Competencia <= @Competencia)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", competencia);


                var strValor = crud.Executar(CommandType.Text, sqlBuilder.ToString());
                if (strValor == null)
                {
                    Valor = 0;
                }
                else
                {
                    Valor = decimal.Parse(strValor.ToString());
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

            sqlBuilder.Append("SELECT Id, Competencia, Valor ");
            sqlBuilder.Append("FROM Simplificado");

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

            sqlBuilder.Append("SELECT Id, Competencia, Valor ");
            sqlBuilder.Append("FROM Simplificado ");
            sqlBuilder.Append("WHERE Competencia = (SELECT MAX(Competencia) FROM Simplificado WHERE Competencia <= @Competencia)");

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
