﻿using CalculoIRRF.Conexao;
using System;
using System.Data;
using System.Text;

namespace CalculoIRRF.Modelo.DescontoMinimo
{
    public class Excluir
    {
        public bool Item(int id)
        {
            Crud crud = new Crud();
            StringBuilder sqlBuilder = new StringBuilder();

            sqlBuilder.Append("DELETE FROM DescontoMinimo ");
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
    }
}
