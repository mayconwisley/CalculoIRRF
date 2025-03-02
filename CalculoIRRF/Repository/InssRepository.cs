using CalculoIRRF.DataBase;
using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository;

public class InssRepository : IInssRepository
{
    private readonly CalculoImpostoContext _calculoImpostoContext;

    public InssRepository()
    {
        _calculoImpostoContext = new();
    }

    public async Task<Inss> Create(Inss inss)
    {
        if (inss is not null)
        {
            _calculoImpostoContext.Inss.Add(inss);
            await _calculoImpostoContext.SaveChangesAsync();
            return inss;
        }
        return new();
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
        var listInss = await _calculoImpostoContext
                            .Inss
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

    public async Task<Inss> GetById(int id)
    {
        var inss = await _calculoImpostoContext
                         .Inss
                         .Where(w => w.Id == id)
                         .FirstOrDefaultAsync();
        return inss ?? null;
    }

    public async Task<int> LastRange(DateTime competence)
    {
        var lastRange = await _calculoImpostoContext
                              .Inss
                              .Where(w => w.Competencia == competence)
                              .MinAsync(m => m.Faixa);
        return lastRange;
    }

    public async Task<decimal> Percent(int range, DateTime competence)
    {
        var percent = await _calculoImpostoContext
                            .Inss
                            .Where(w => w.Faixa == range &&
                                        w.Competencia == _calculoImpostoContext
                                                        .Inss
                                                        .Where(w => w.Competencia == competence)
                                                        .Max(m => m.Competencia))
                            .Select(s => s.Porcentagem)
                            .DefaultIfEmpty(0)
                            .FirstOrDefaultAsync();
        return percent;
    }

    public async Task<int> Range(decimal baseInss, DateTime competence)
    {
        var range = await _calculoImpostoContext.Inss
                          .Where(w => w.Valor == baseInss &&
                                      w.Competencia == _calculoImpostoContext.Inss
                                                       .Where(w => w.Competencia == competence)
                                                       .Max(m => m.Competencia))
                          .Select(s => s.Faixa)
                          .DefaultIfEmpty(0)
                          .FirstOrDefaultAsync();
        return range;
    }

    public async Task<decimal> Roof(DateTime competence)
    {
        var roof = await _calculoImpostoContext.Inss
                         .Where(w => w.Competencia == _calculoImpostoContext.Inss
                                                      .Where(w => w.Competencia == competence)
                                                      .Max(m => m.Competencia))
                         .MaxAsync(m => m.Valor);
        return roof;
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

    public async Task<decimal> Value(int range, DateTime competence)
    {
        var value = await _calculoImpostoContext.Inss
                          .Where(w => w.Faixa == range &&
                                    w.Competencia == _calculoImpostoContext.Inss
                                                    .Where(w => w.Competencia == competence)
                                                    .Max(m => m.Competencia))
                          .Select(s => s.Valor)
                          .DefaultIfEmpty(0)
                          .FirstOrDefaultAsync();
        return value;
    }
}
