using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelos.Inss
{
    public class Gravar
    {
        public bool Item(Objetos.Inss inss)
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
    }
}
