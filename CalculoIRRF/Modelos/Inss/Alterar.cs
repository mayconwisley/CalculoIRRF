using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelos.Inss
{
    public class Alterar
    {
        public bool Item(Objetos.Inss inss)
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
    }
}
