using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelo.Irrf
{
    public class Cadastro
    {

        public bool Gravar(Objetos.Irrf irrf)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("INSERT INTO Irrf(Competencia, Faixa, Valor, Porcentagem, Deducao) ");
            sqlBuilder.Append("VALUES(@Competencia, @Faixa, @Valor, @Porcentagem, @Deducao)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", irrf.Competencia);
                crud.AdicionarParamentro("Faixa", irrf.Faixa);
                crud.AdicionarParamentro("Valor", irrf.Valor);
                crud.AdicionarParamentro("Porcentagem", irrf.Porcentagem);
                crud.AdicionarParamentro("Deducao", irrf.Deducao);
                crud.Executar(CommandType.Text, sqlBuilder.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public bool GravarRfbOnline(Objetos.Tributacao.IrrfRfb irrfRfb)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("INSERT INTO IrrfRfbOnline(DataCriacao, DataAtualizacao, Sequencia, BaseCaculo, Aliquota, Deducao, Dependente, Simplificado) ");
            sqlBuilder.Append("VALUES(@DataCriacao, @DataAtualizacao, @Sequencia, @BaseCaculo, @Aliquota, @Deducao, @Dependente, @Simplificado)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("DataCriacao", irrfRfb.DataCriacao);
                crud.AdicionarParamentro("DataAtualizacao", irrfRfb.DataAtualizacao);
                crud.AdicionarParamentro("Sequencia", irrfRfb.Sequencia);
                crud.AdicionarParamentro("BaseCaculo", irrfRfb.BaseCaculo);
                crud.AdicionarParamentro("Aliquota", irrfRfb.Aliquota);
                crud.AdicionarParamentro("Deducao", irrfRfb.Deducao);
                crud.AdicionarParamentro("Dependente", irrfRfb.Dependente);
                crud.AdicionarParamentro("Simplificado", irrfRfb.Simplificado);
                crud.Executar(CommandType.Text, sqlBuilder.ToString());
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public bool Alterar(Objetos.Irrf irrf)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("UPDATE Irrf ");
            sqlBuilder.Append("SET Competencia = @Competencia, Faixa = @Faixa, Valor = @Valor, Porcentagem = @Porcentagem, Deducao = @Deducao ");
            sqlBuilder.Append("WHERE Id = @Id");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", irrf.Competencia);
                crud.AdicionarParamentro("Faixa", irrf.Faixa);
                crud.AdicionarParamentro("Valor", irrf.Valor);
                crud.AdicionarParamentro("Porcentagem", irrf.Porcentagem);
                crud.AdicionarParamentro("Deducao", irrf.Deducao);
                crud.AdicionarParamentro("Id", irrf.Id);
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

            sqlBuilder.Append("DELETE FROM Irrf ");
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
        public int FaixaIrrf(decimal baseIrrf, DateTime competencia)
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
                crud.AdicionarParamentro("Valor", baseIrrf);
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
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public int UltimaFaixaIrrf(DateTime competencia)
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
                    return 0;
                }
                else
                {
                    return int.Parse(faixa);
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
