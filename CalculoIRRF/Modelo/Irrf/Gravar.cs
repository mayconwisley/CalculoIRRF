using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelo.Irrf
{
    public class Gravar
    {
        public bool Item(Objetos.Irrf irrf)
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
    }
}
