using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelos.Dependente
{
    public class Alterar
    {
        public bool Item(Objetos.Dependente dependente)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("UPDATE Dependente ");
            sqlBuilder.Append("SET Competencia = @Competencia, Valor = @Valor ");
            sqlBuilder.Append("WHERE Id = @Id");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", dependente.Competencia);
                crud.AdicionarParamentro("Valor", dependente.Valor);
                crud.AdicionarParamentro("Id", dependente.Id);
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
