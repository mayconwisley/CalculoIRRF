using CalculoIRRF.DataBase;
using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository;

public class IrrfRepository(CalculoImpostoContext _calculoImpostoContext) : IIrrfRepository
{
    public async Task<Irrf> Create(Irrf irrf)
    {
        _calculoImpostoContext.Irrf.Add(irrf);
        await _calculoImpostoContext.SaveChangesAsync();
        return irrf;
    }
    public async Task<IrrfRfb> Create(IrrfRfb irrfRfb)
    {
        _calculoImpostoContext.IrrfRfb.Add(irrfRfb);
        await _calculoImpostoContext.SaveChangesAsync();
        return irrfRfb;
    }

    public async Task<double> Deduction(int range, DateTime competence)
    {
        return await _calculoImpostoContext.Irrf
                      .Where(w => w.Faixa == range &&
                                  w.Competencia == _calculoImpostoContext.Irrf
                                                   .Where(w => w.Competencia <= competence)
                                                   .Max(m => m.Competencia))
                      .Select(s => s.Deducao)
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
                     .OrderByDescending(o => o.Competencia)
                     .ToListAsync();
    }

    public async Task<IEnumerable<Irrf>> GetByCompetence(DateTime competence)
    {
        return await _calculoImpostoContext.Irrf
                    .Where(w => w.Competencia == competence)
                    .OrderBy(o => o.Faixa)
                    .ToListAsync();
    }
    public async Task<IEnumerable<IrrfRfb>> GetByDateUpdate(DateTime dateUpdate)
    {
        return await _calculoImpostoContext.IrrfRfb
                    .Where(w => w.DataAtualizacao == dateUpdate)
                    .ToListAsync();
    }

    public async Task<Irrf> GetById(int id)
    {
        return await _calculoImpostoContext.Irrf.FindAsync(id);
    }

    public async Task<bool> IsGov(DateTime competence)
    {
        var isIrrfGov = await _calculoImpostoContext
                          .IrrfRfb
                          .Where(w => w.DataAtualizacao == competence)
                          .AnyAsync();

        return isIrrfGov;
    }

    public async Task<int> LastRange(DateTime competence)
    {
        var query = _calculoImpostoContext.Irrf
                      .Where(w => w.Competencia == competence);

        return await query.AnyAsync() ? await query.MaxAsync(m => m.Faixa) : 0;
    }

    public async Task<double> Percent(int range, DateTime competence)
    {
        return await _calculoImpostoContext.Irrf
                     .Where(w => w.Faixa == range &&
                                 w.Competencia == _calculoImpostoContext.Irrf
                                                  .Where(w => w.Competencia <= competence)
                                                  .Max(m => m.Competencia))
                     .Select(s => s.Porcentagem)
                     .FirstOrDefaultAsync();
    }

    public async Task<int> Range(double baseIrrf, DateTime competence)
    {
        var query = _calculoImpostoContext.Irrf
                      .Where(w => baseIrrf <= w.Valor &&
                                  w.Competencia == _calculoImpostoContext.Irrf
                                                  .Where(w => w.Competencia <= competence)
                                                  .Max(m => m.Competencia));

        return await query.AnyAsync() ? await query.MinAsync(m => m.Faixa) : 0;
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

    public async Task<double> Value(int range, DateTime competence)
    {
        return await _calculoImpostoContext.Irrf
                     .Where(w => w.Faixa == range &&
                                 w.Competencia == _calculoImpostoContext.Irrf
                                                  .Where(w => w.Competencia <= competence)
                                                  .Max(m => m.Competencia))
                     .Select(s => s.Valor)
                     .FirstOrDefaultAsync();
    }
}
