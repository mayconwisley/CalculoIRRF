using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using CalculoIRRF.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services;

public class SimplificadoServices(ISimplificadoRepository _simplificadoRepository) : ISimplificadoServices
{
    public async Task<bool> Gravar(Simplificado simplificado)
    {
        await _simplificadoRepository.Create(simplificado);
        return true;
    }
    public async Task<bool> Alterar(Simplificado simplificado)
    {
        await _simplificadoRepository.Update(simplificado);
        return true;
    }
    public async Task<bool> Excluir(int id)
    {
        await _simplificadoRepository.Delete(id);
        return true;
    }
    public async Task<double> ValorSimplificado(DateTime competencia)
    {
        return await _simplificadoRepository.Value(competencia);
    }
    public async Task<IEnumerable<Simplificado>> ListarTodos()
    {

        return await _simplificadoRepository.GetAll();
    }
    public async Task<IEnumerable<Simplificado>> ListarTodosPorCompetencia(DateTime competencia)
    {
        return await _simplificadoRepository.GetByCompetence(competencia);
    }
    public async Task<Simplificado> ListarPorId(int id)
    {
        var simplificado = await _simplificadoRepository.GetById(id);
        return simplificado;
    }
}
