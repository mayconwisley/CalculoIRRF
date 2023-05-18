using CalculoIRRF.Conexao;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalculoIRRF.Modelos.DescontoMinimo
{
    public class Alterar
    {
        public bool Item(Objetos.DescontoMinimo descontoMinimo)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("UPDATE DescontoMinimo ");
            sqlBuilder.Append("SET Competencia = @Competencia, Valor = @Valor ");
            sqlBuilder.Append("WHERE Id = @Id");

            try
            {
                crud.LimparParametro();
                crud.AdicionarParamentro("Competencia", descontoMinimo.Competencia);
                crud.AdicionarParamentro("Valor", descontoMinimo.Valor);
                crud.AdicionarParamentro("Id", descontoMinimo.Id);
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
