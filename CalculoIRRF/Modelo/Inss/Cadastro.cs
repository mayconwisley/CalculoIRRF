using CalculoIRRF.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Modelo.Inss;

public class Cadastro
{
    private readonly InssRepository _inssRepository;
    public Cadastro()
    {
        _inssRepository = new();
    }

    public async Task<bool> Gravar(Model.Inss inss)
    {
        try
        {
            await _inssRepository.Create(inss);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> Alterar(Model.Inss inss)
    {
        try
        {
            await _inssRepository.Update(inss);
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
            await _inssRepository.Delete(id);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<int> FaixaInss(decimal baseInss, DateTime competencia)
    {
        try
        {
            var range = await _inssRepository.Range(baseInss, competencia);
            return range;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<int> UltimaFaixaInss(DateTime competencia)
    {
        try
        {
            var lastRange = await _inssRepository.LastRange(competencia);
            return lastRange;

        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<decimal> PorcentagemInss(int faixa, DateTime competencia)
    {
        try
        {
            var percent = await _inssRepository.Percent(faixa, competencia);
            return percent;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<decimal> ValorInss(int faixa, DateTime competencia)
    {
        try
        {
            var value = await _inssRepository.Value(faixa, competencia);
            return value;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<decimal> TetoInss(DateTime competencia)
    {
        try
        {
            var roof = await _inssRepository.Roof(competencia);
            return roof;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<IEnumerable<Model.Inss>> ListarTodos()
    {
        try
        {
            var listInss = await _inssRepository.GetAll();
            return listInss;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<IEnumerable<Model.Inss>> ListarTodosPorCompetencia(DateTime competencia)
    {
        try
        {
            var listInss = await _inssRepository.GetByCompetence(competencia);
            return listInss;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
