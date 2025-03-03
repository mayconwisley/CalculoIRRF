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
        try
        {
            await _simplificadoRepository.Create(simplificado);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> Alterar(Simplificado simplificado)
    {
        try
        {
            await _simplificadoRepository.Update(simplificado);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> Excluir(int id)
    {
        try
        {
            await _simplificadoRepository.Delete(id);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<decimal> ValorSimplificado(DateTime competencia)
    {
        try
        {
            return await _simplificadoRepository.Value(competencia);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<IEnumerable<Simplificado>> ListarTodos()
    {

        try
        {
            return await _simplificadoRepository.GetAll();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<IEnumerable<Simplificado>> ListarTodosPorCompetencia(DateTime competencia)
    {
        try
        {
            return await _simplificadoRepository.GetByCompetence(competencia);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<Simplificado> ListarPorId(int id)
    {
        try
        {
            var simplificado = await _simplificadoRepository.GetById(id);
            return simplificado;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
