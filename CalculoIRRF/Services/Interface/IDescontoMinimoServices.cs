using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Interface;

public interface IDescontoMinimoServices
{
    Task<bool> Gravar(DescontoMinimo descontoMinimo);
    Task<bool> Alterar(DescontoMinimo descontoMinimo);
    Task<bool> Excluir(int id);
    Task<double> ValorDescontoMinimo(DateTime competencia);
    Task<IEnumerable<DescontoMinimo>> ListarTodos();
    Task<IEnumerable<DescontoMinimo>> ListarTodosPorCompetencia(DateTime competencia);
    Task<DescontoMinimo> ListarPorId(int id);
}
