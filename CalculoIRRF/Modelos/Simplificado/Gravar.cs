using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelos.Simplificado
{
    public class Gravar
    {
        public bool Item(Objetos.Simplificado simplificado)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("INSERT INTO Simplificado(Competencia, Valor) ");
            sqlBuilder.Append("VALUES(@Competencia, @Valor)");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", simplificado.Competencia);
                crud.AdicionarParamentro("Valor", simplificado.Valor);
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
