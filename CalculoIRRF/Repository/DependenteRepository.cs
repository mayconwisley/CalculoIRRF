using CalculoIRRF.DataBase;
using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository;

public class DependenteRepository : IDependenteRepository
{
    CalculoImpostoContext _calculoImpostoContext;

    public DependenteRepository()
    {
        _calculoImpostoContext = new CalculoImpostoContext();
    }

    public async Task<Dependente> Create(Dependente dependente)
    {
        try
        {
            if (dependente is not null)
            {
                _calculoImpostoContext.Dependente.Add(dependente);
                await _calculoImpostoContext.SaveChangesAsync();
                return dependente;
            }

            return new();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Dependente> Delete(int id)
    {
        try
        {
            if (id != 0)
            {
                Dependente dependente = await GetById(id);
                _calculoImpostoContext.Dependente.Remove(dependente);
                await _calculoImpostoContext.SaveChangesAsync();
                return dependente;
            }
            return new();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<IEnumerable<Dependente>> GetAll()
    {
        try
        {
            var listDependente = await _calculoImpostoContext
                                       .Dependente
                                       .ToListAsync();
            return listDependente ?? [];
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<Dependente>> GetByCompetence(DateTime competence)
    {
        try
        {
            var maxCompetencia = await _calculoImpostoContext
                                      .Dependente
                                      .Where(w => w.Competencia <= competence)
                                      .MaxAsync(w => w.Competencia);

            var listDependente = await _calculoImpostoContext
                                       .Dependente
                                       .Where(w => w.Competencia == maxCompetencia)
                                       .ToListAsync();
            return listDependente ?? [];

        }
        catch (Exception)
        {

            throw;
        }
    }

    public async Task<Dependente> GetById(int id)
    {
        try
        {
            var dependente = await _calculoImpostoContext
                                   .Dependente
                                   .Where(w => w.Id == id)
                                   .FirstOrDefaultAsync();
            return dependente ?? new();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<Dependente> Update(Dependente dependente)
    {
        try
        {
            if (dependente is not null)
            {
                Dependente dependente1 = await GetById(dependente.Id);
                if (dependente1 is not null)
                {
                    _calculoImpostoContext.Dependente.Entry(dependente1).CurrentValues.SetValues(dependente);
                    await _calculoImpostoContext.SaveChangesAsync();
                    return dependente;
                }
            }
            return new();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<decimal> Value(DateTime competence)
    {
        try
        {
            var value = await _calculoImpostoContext
                              .Dependente
                              .Where(w => w.Competencia == competence)
                              .Select(s => s.Valor)
                              .FirstOrDefaultAsync();
            return value;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
