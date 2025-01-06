using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelo.Inss
{
    public class Cadastro
    {
        public bool Gravar(Objetos.Inss inss)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("INSERT INTO INSS(Competencia, Faixa, Valor, Porcentagem) ");
            sqlBuilder.Append("VALUES(@Competencia, @Faixa, @Valor, @Porcentagem)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", inss.Competencia);
                crud.AdicionarParamentro("Faixa", inss.Faixa);
                crud.AdicionarParamentro("Valor", inss.Valor);
                crud.AdicionarParamentro("Porcentagem", inss.Porcentagem);
                crud.Executar(CommandType.Text, sqlBuilder.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool GravarInssOnline(Objetos.Tributacao.InssGov inssGov)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("INSERT INTO InssGovOnline(DataCriacao, DataAtualizacao, Sequencia, BaseCalculo, Aliquota) ");
            sqlBuilder.Append("VALUES(@DataCriacao, @DataAtualizacao, @Sequencia, @BaseCalculo, @Aliquota)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("DataCriacao", inssGov.DataCriacao);
                crud.AdicionarParamentro("DataAtualizacao", inssGov.DataAtualizacao);
                crud.AdicionarParamentro("Sequencia", inssGov.Sequencia);
                crud.AdicionarParamentro("BaseCalculo", inssGov.BaseCaculo);
                crud.AdicionarParamentro("Aliquota", inssGov.Aliquota);
                crud.Executar(CommandType.Text, sqlBuilder.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Alterar(Objetos.Inss inss)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("UPDATE INSS ");
            sqlBuilder.Append("SET Competencia = @Competencia, Faixa = @Faixa, Valor = @Valor, Porcentagem = @Porcentagem ");
            sqlBuilder.Append("WHERE Id = @Id");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", inss.Competencia);
                crud.AdicionarParamentro("Faixa", inss.Faixa);
                crud.AdicionarParamentro("Valor", inss.Valor);
                crud.AdicionarParamentro("Porcentagem", inss.Porcentagem);
                crud.AdicionarParamentro("Id", inss.Id);
                crud.Executar(CommandType.Text, sqlBuilder.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool Excluir(int id)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("DELETE FROM INSS ");
            sqlBuilder.Append("WHERE Id = @Id");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Id", id);
                crud.Executar(CommandType.Text, sqlBuilder.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public int FaixaInss(decimal baseInss, DateTime competencia)
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
        public int UltimaFaixaInss(DateTime competencia)
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
        public decimal TetoInss(DateTime competencia)
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
