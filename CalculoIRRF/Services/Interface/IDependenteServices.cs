using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Interface;

public interface IDependenteServices
{
    Task<IEnumerable<Dependente>> ListarTodos();
    Task<IEnumerable<Dependente>> ListarTodosPorCompetencia(DateTime competencia);
    Task<Dependente> ListarPorId(int id);
    Task<bool> Gravar(Dependente dependente);
    Task<bool> Alterar(Dependente dependente);
    Task<bool> Excluir(int id);
    Task<double> VlrDependente(DateTime competencia);
}
