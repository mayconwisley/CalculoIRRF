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
        await _dependenteRepository.Create(dependente);
        return true;
    }
    public async Task<bool> Alterar(Dependente dependente)
    {
        await _dependenteRepository.Update(dependente);
        return true;
    }
    public async Task<bool> Excluir(int id)
    {
        await _dependenteRepository.Delete(id);
        return true;
    }
    public async Task<double> VlrDependente(DateTime competencia)
    {
        var value = await _dependenteRepository.Value(competencia);
        return value;
    }
    public async Task<IEnumerable<Dependente>> ListarTodos()
    {
        var listDependente = await _dependenteRepository.GetAll();
        return listDependente;
    }
    public async Task<IEnumerable<Dependente>> ListarTodosPorCompetencia(DateTime competencia)
    {
        var listDependente = await _dependenteRepository.GetByCompetence(competencia);
        return listDependente;
    }
    public async Task<Dependente> ListarPorId(int id)
    {
        var dependente = await _dependenteRepository.GetById(id);
        return dependente;
    }
}
