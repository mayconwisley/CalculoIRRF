using CalculoIRRF.DataBase;
using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository;

public class DependenteRepository(CalculoImpostoContext _calculoImpostoContext) : IDependenteRepository
{
    public async Task<Dependente> Create(Dependente dependente)
    {
        if (dependente is null)
            throw new ArgumentException(null, nameof(dependente));

        _calculoImpostoContext.Dependente.Add(dependente);
        await _calculoImpostoContext.SaveChangesAsync();
        return dependente;

    }

    public async Task<Dependente> Delete(int id)
    {
        Dependente dependente = await GetById(id);
        if (dependente is null)
            return null;

        _calculoImpostoContext.Dependente.Remove(dependente);
        await _calculoImpostoContext.SaveChangesAsync();
        return dependente;
    }

    public async Task<IEnumerable<Dependente>> GetAll()
    {
        var listDependente = await _calculoImpostoContext.Dependente
                                   .ToListAsync();
        return listDependente ?? [];

    }

    public async Task<IEnumerable<Dependente>> GetByCompetence(DateTime competence)
    {
        var listDependente = await _calculoImpostoContext.Dependente
                                   .Where(w => w.Competencia == _calculoImpostoContext.Dependente
                                                                .Where(w => w.Competencia == competence)
                                                                .Max(m => m.Competencia))
                                   .ToListAsync();
        return listDependente ?? [];
    }

    public async Task<Dependente> GetById(int id)
    {
        var dependente = await _calculoImpostoContext.Dependente.FindAsync(id);
        return dependente ?? new();
    }

    public async Task<Dependente> Update(Dependente dependente)
    {
        if (dependente is null || dependente.Id <= 0)
        {
            throw new ArgumentException("Objeto inválido para atualização");
        }
        Dependente dependente1 = await GetById(dependente.Id);
        _calculoImpostoContext.Dependente.Entry(dependente1)
        .CurrentValues.SetValues(dependente);
        await _calculoImpostoContext.SaveChangesAsync();
        return dependente;
    }

    public async Task<decimal> Value(DateTime competence)
    {
        var value = await _calculoImpostoContext.Dependente
                              .Where(w => w.Competencia == _calculoImpostoContext.Dependente
                                                          .Where(w => w.Competencia <= competence)
                                                          .Max(w => w.Competencia))
                              .Select(s => s.Valor)
                              .DefaultIfEmpty(0)
                              .FirstOrDefaultAsync();
        return value;
    }
}
