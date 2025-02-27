using CalculoIRRF.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Modelo.Dependente;

public class Cadastro
{
    DependenteRepository _dependenteRepository;
    public Cadastro()
    {
        _dependenteRepository = new();
    }

    public async Task<bool> Gravar(Model.Dependente dependente)
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
    public async Task<bool> Alterar(Model.Dependente dependente)
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
    public async Task<decimal> VlrDependente(DateTime competencia)
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
    public async Task<IEnumerable<Model.Dependente>> ListarTodos()
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
    public async Task<IEnumerable<Model.Dependente>> ListarTodosPorCompetencia(DateTime competencia)
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
}
