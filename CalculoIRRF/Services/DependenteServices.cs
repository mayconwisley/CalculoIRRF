using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using CalculoIRRF.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services;

public class DependenteServices(IDependenteRepository _dependenteRepository) : IDependenteServices
{
    public async Task<bool> Gravar(Dependente dependente)
    {
        try
        {
            await _dependenteRepository.Create(dependente);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> Alterar(Dependente dependente)
    {

        try
        {
            await _dependenteRepository.Update(dependente);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<bool> Excluir(int id)
    {
        try
        {
            await _dependenteRepository.Delete(id);
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<double> VlrDependente(DateTime competencia)
    {
        try
        {
            var value = await _dependenteRepository.Value(competencia);
            return value;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<IEnumerable<Dependente>> ListarTodos()
    {
        try
        {
            var listDependente = await _dependenteRepository.GetAll();
            return listDependente;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<IEnumerable<Dependente>> ListarTodosPorCompetencia(DateTime competencia)
    {
        try
        {
            var listDependente = await _dependenteRepository.GetByCompetence(competencia);
            return listDependente;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
    public async Task<Dependente> ListarPorId(int id)
    {
        try
        {
            var dependente = await _dependenteRepository.GetById(id);
            return dependente;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
