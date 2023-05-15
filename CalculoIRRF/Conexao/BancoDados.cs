using System;
using System.Data;
using System.Data.SQLite;

namespace CalculoIRRF.Conexao
{
    public class BancoDados
    {
        protected SQLiteConnection liteConnection;
        private string strConexao = @"Data Source=|DataDirectory|BancoDados\calculoIrrf.db";

        protected bool Conectar()
        {
            liteConnection = new SQLiteConnection(strConexao);
            try
            {
                liteConnection.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
                throw new Exception(ex.Message);
            }
        }
        protected bool Desconectar()
        {
            try
            {
                if (liteConnection.State != ConnectionState.Closed)
                {
                    liteConnection.Close();
                    liteConnection.Dispose();
                    return true;
                }
                else
                {
                    liteConnection.Dispose();
                    return false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
