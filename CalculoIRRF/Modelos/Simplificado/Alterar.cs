using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelos.Simplificado
{
    public class Alterar
    {
        public bool Item(Objetos.Simplificado simplificado)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("UPDATE Simplificado ");
            sqlBuilder.Append("SET Competencia = @Competencia, Valor = @Valor) ");
            sqlBuilder.Append("WHERE Id = @Id");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", simplificado.Competencia);
                crud.AdicionarParamentro("Valor", simplificado.Valor);
                crud.AdicionarParamentro("Id", simplificado.Id);
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
