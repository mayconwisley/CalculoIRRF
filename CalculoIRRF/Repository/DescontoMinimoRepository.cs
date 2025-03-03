using CalculoIRRF.DataBase;
using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository;

public class DescontoMinimoRepository(CalculoImpostoContext _calculoImpostoContext) : IDescontoMinimoRepository
{
    public async Task<DescontoMinimo> Create(DescontoMinimo descontoMinimo)
    {

        if (descontoMinimo is null)
            throw new ArgumentException(null, nameof(descontoMinimo));

        _calculoImpostoContext.DescontoMinimo.Add(descontoMinimo);
        await _calculoImpostoContext.SaveChangesAsync();
        return descontoMinimo;

    }

    public async Task<DescontoMinimo> Delete(int id)
    {

        var descontoMinimo = await GetById(id);
        if (descontoMinimo is null)
            return null;

        _calculoImpostoContext.DescontoMinimo.Remove(descontoMinimo);
        await _calculoImpostoContext.SaveChangesAsync();
        return descontoMinimo;

    }

    public async Task<IEnumerable<DescontoMinimo>> GetAll()
    {
        var listDescontoMinimo = await _calculoImpostoContext.DescontoMinimo
                                       .ToListAsync();
        return listDescontoMinimo ?? [];
    }

    public async Task<IEnumerable<DescontoMinimo>> GetByCompetence(DateTime competence)
    {
        var listDescontoMinimo = await _calculoImpostoContext.DescontoMinimo
                                       .Where(w => w.Competencia == _calculoImpostoContext.DescontoMinimo
                                                                    .Where(w => w.Competencia <= competence)
                                                                    .Max(w => w.Competencia))
                                       .ToListAsync();
        return listDescontoMinimo ?? [];
    }

    public async Task<DescontoMinimo> GetById(int id)
    {
        var descontoMinimo = await _calculoImpostoContext.DescontoMinimo
                                  .FindAsync(id);
        return descontoMinimo ?? new();
    }

    public async Task<DescontoMinimo> Update(DescontoMinimo descontoMinimo)
    {

        if (descontoMinimo is null || descontoMinimo.Id <= 0)
        {
            throw new ArgumentException("Objeto inválido para atualização");
        }


        DescontoMinimo descontoMinimo1 = await GetById(descontoMinimo.Id);

        _calculoImpostoContext.DescontoMinimo.Entry(descontoMinimo1)
        .CurrentValues.SetValues(descontoMinimo);
        await _calculoImpostoContext.SaveChangesAsync();

        return descontoMinimo ?? new();
    }

    public async Task<decimal> Value(DateTime competence)
    {
        var value = await _calculoImpostoContext.DescontoMinimo
                          .Where(w => w.Competencia == _calculoImpostoContext.DescontoMinimo
                                                       .Where(w => w.Competencia == competence)
                                                       .Max(m => m.Competencia))
                          .Select(s => s.Valor)
                          .DefaultIfEmpty(0)
                          .FirstOrDefaultAsync();
        return value;
    }
}
