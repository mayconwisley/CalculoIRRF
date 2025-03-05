using CalculoIRRF.DataBase;
using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository;

public class InssRepository(CalculoImpostoContext _calculoImpostoContext) : IInssRepository
{
    public async Task<Inss> Create(Inss inss)
    {
        if (inss is null)
        {
            throw new ArgumentException("Erro no objeto INSS");
        }

        _calculoImpostoContext.Inss.Add(inss);
        await _calculoImpostoContext.SaveChangesAsync();
        return inss;
    }

    public async Task<InssGov> Create(InssGov inssGov)
    {
        if (inssGov is null)
        {
            throw new ArgumentException("Erro no objeto INSS GOV");
        }

        _calculoImpostoContext.InssGov.Add(inssGov);
        await _calculoImpostoContext.SaveChangesAsync();
        return inssGov;
    }

    public async Task<Inss> Delete(int id)
    {
        var inss = await GetById(id);
        if (inss is not null)
        {
            _calculoImpostoContext.Inss.Remove(inss);
            await _calculoImpostoContext.SaveChangesAsync();
            return inss;
        }
        return new();
    }

    public async Task<IEnumerable<Inss>> GetAll()
    {
        var listInss = await _calculoImpostoContext.Inss
                            .OrderByDescending(o => o.Competencia)
                            .ToListAsync();
        return listInss ?? [];
    }

    public async Task<IEnumerable<Inss>> GetByCompetence(DateTime competence)
    {
        var listInss = await _calculoImpostoContext
                            .Inss
                            .Where(w => w.Competencia == _calculoImpostoContext
                                                        .Inss
                                                        .Where(w => w.Competencia == competence)
                                                        .Max(m => m.Competencia))
                            .ToListAsync();

        return listInss ?? [];
    }

    public async Task<bool> IsGov(DateTime competence)
    {
        var isInssGov = await _calculoImpostoContext
                            .InssGov
                            .Where(w => w.DataAtualizacao == competence)
                            .AnyAsync();

        return isInssGov;
    }

    public async Task<Inss> GetById(int id)
    {
        var inss = await _calculoImpostoContext.Inss
                         .Where(w => w.Id == id)
                         .FirstOrDefaultAsync();
        return inss ?? null;
    }

    public async Task<int> LastRange(DateTime competence)
    {
        var query = _calculoImpostoContext.Inss
                    .Where(w => w.Competencia == competence);

        return await query.AnyAsync() ? await query.MinAsync(m => m.Faixa) : 0;
    }

    public async Task<double> Percent(int range, DateTime competence)
    {
        var percent = await _calculoImpostoContext.Inss
                            .Where(w => w.Faixa == range &&
                                        w.Competencia == _calculoImpostoContext
                                                        .Inss
                                                        .Where(w => w.Competencia <= competence)
                                                        .Max(m => m.Competencia))
                            .Select(s => s.Porcentagem)
                            .FirstOrDefaultAsync();
        return percent;
    }

    public async Task<int> Range(double baseInss, DateTime competence)
    {
        var range = await _calculoImpostoContext.Inss
                          .Where(w => w.Valor >= baseInss &&
                                      w.Competencia == _calculoImpostoContext.Inss
                                                       .Where(w => w.Competencia <= competence)
                                                       .Max(m => m.Competencia))
                          .Select(s => s.Faixa)

                          .FirstOrDefaultAsync();
        return range;
    }

    public async Task<double> Roof(DateTime competence)
    {
        var query = _calculoImpostoContext.Inss
                      .Where(w => w.Competencia == _calculoImpostoContext.Inss
                                                   .Where(w => w.Competencia <= competence)
                                                   .Max(m => m.Competencia));

        return await query.AnyAsync() ? await query.MaxAsync(ma => ma.Valor) : 0d;
    }

    public async Task<Inss> Update(Inss inss)
    {
        if (inss is null || inss.Id <= 0)
        {
            throw new ArgumentException("Objeto inválido para atualização");
        }
        Inss inss1 = await GetById(inss.Id);
        _calculoImpostoContext.Inss.Entry(inss1)
        .CurrentValues.SetValues(inss);

        await _calculoImpostoContext.SaveChangesAsync();

        return inss;
    }

    public async Task<double> Value(int range, DateTime competence)
    {
        var value = await _calculoImpostoContext.Inss
                          .Where(w => w.Faixa == range &&
                                    w.Competencia == _calculoImpostoContext.Inss
                                                    .Where(w => w.Competencia <= competence)
                                                    .Max(m => m.Competencia))
                          .Select(s => s.Valor)
                          .FirstOrDefaultAsync();
        return value;
    }
}
