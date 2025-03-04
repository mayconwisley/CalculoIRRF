using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Interface;

public interface ISimplificadoServices
{
    Task<bool> Gravar(Simplificado simplificado);
    Task<bool> Alterar(Simplificado simplificado);
    Task<bool> Excluir(int id);
    Task<double> ValorSimplificado(DateTime competencia);
    Task<IEnumerable<Simplificado>> ListarTodos();
    Task<IEnumerable<Simplificado>> ListarTodosPorCompetencia(DateTime competencia);
    Task<Simplificado> ListarPorId(int id);
}
