using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelo.Irrf
{
    public class Alterar
    {
        public bool Item(Objetos.Irrf irrf)
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
    }
}
