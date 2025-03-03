using CalculoIRRF.DataBase;
using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository;

public class IrrfRepository : IIrrfRepository
{
    private readonly CalculoImpostoContext _calculoImpostoContext;
    public IrrfRepository()
    {
        _calculoImpostoContext = new();
    }

    public async Task<Irrf> Create(Irrf irrf)
    {
        _calculoImpostoContext.Irrf.Add(irrf);
        await _calculoImpostoContext.SaveChangesAsync();
        return irrf;
    }

    public async Task<decimal> Deduction(int range, DateTime competence)
    {
        return await _calculoImpostoContext.Irrf
                      .Where(w => w.Faixa == range &&
                                  w.Competencia == _calculoImpostoContext.Irrf
                                                   .Where(w => w.Competencia == competence)
                                                   .Max(m => m.Competencia))
                      .Select(s => s.Deducao)
                      .DefaultIfEmpty(0)
                      .FirstOrDefaultAsync();
    }

    public async Task<Irrf> Delete(int id)
    {
        var irrf = await GetById(id) ?? throw new ArgumentException("Objeto inexistente");
        _calculoImpostoContext.Irrf.Remove(irrf);
        await _calculoImpostoContext.SaveChangesAsync();
        return irrf;


    }

    public async Task<IEnumerable<Irrf>> GetAll()
    {
        return await _calculoImpostoContext.Irrf
                     .ToListAsync();
    }

    public async Task<IEnumerable<Irrf>> GetByCompetence(DateTime competence)
    {
        return await _calculoImpostoContext.Irrf
                    .Where(w => w.Competencia == competence)
                    .ToListAsync();
    }

    public async Task<Irrf> GetById(int id)
    {
        return await _calculoImpostoContext.Irrf.FindAsync(id);
    }

    public async Task<int> LastRange(DateTime competence)
    {
        return await _calculoImpostoContext.Irrf
                      .Where(w => w.Competencia == competence)
                      .MaxAsync(m => m.Faixa);
    }

    public async Task<decimal> Percent(int range, DateTime competence)
    {
        return await _calculoImpostoContext.Irrf
                     .Where(w => w.Faixa == range &&
                                 w.Competencia == _calculoImpostoContext.Irrf
                                                  .Where(w => w.Competencia == competence)
                                                  .Max(m => m.Competencia))
                     .Select(s => s.Porcentagem)
                     .DefaultIfEmpty(0)
                     .FirstOrDefaultAsync();
    }

    public async Task<int> Range(decimal baseIrrf, DateTime competence)
    {
        return await _calculoImpostoContext.Irrf
                      .Where(w => baseIrrf >= w.Valor &&
                                  w.Competencia == _calculoImpostoContext.Irrf
                                                  .Where(w => w.Competencia == competence)
                                                  .Max(m => m.Competencia))
                      .MinAsync(m => m.Faixa);
    }

    public async Task<Irrf> Update(Irrf irrf)
    {
        if (irrf is null || irrf.Id <= 0)
        {
            throw new ArgumentException("Objeto inválido para atualização");
        }

        Irrf irrf1 = await GetById(irrf.Id);
        _calculoImpostoContext.Irrf.Entry(irrf1)
        .CurrentValues.SetValues(irrf);

        await _calculoImpostoContext.SaveChangesAsync();
        return irrf;
    }

    public async Task<decimal> Value(int range, DateTime competence)
    {
        return await _calculoImpostoContext.Irrf
                     .Where(w => w.Faixa == range &&
                                 w.Competencia == _calculoImpostoContext.Irrf
                                                  .Where(w => w.Competencia == competence)
                                                  .Max(m => m.Competencia))
                     .Select(s => s.Valor)
                     .DefaultIfEmpty(0)
                     .FirstOrDefaultAsync();
    }
}
