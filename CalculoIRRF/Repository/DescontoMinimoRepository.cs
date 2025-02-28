using CalculoIRRF.DataBase;
using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository;

public class DescontoMinimoRepository : IDescontoMinimoRepository
{
    private readonly CalculoImpostoContext _calculoImpostoContext;

    public DescontoMinimoRepository()
    {
        _calculoImpostoContext = new CalculoImpostoContext();
    }

    public async Task<DescontoMinimo> Create(DescontoMinimo descontoMinimo)
    {
        try
        {
            if (descontoMinimo is not null)
            {
                _calculoImpostoContext.DescontoMinimo.Add(descontoMinimo);
                await _calculoImpostoContext.SaveChangesAsync();
                return descontoMinimo;
            }
            return new();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<DescontoMinimo> Delete(int id)
    {
        try
        {
            if (id != 0)
            {
                var descontoMinimo = await GetById(id);
                if (descontoMinimo is not null)
                {
                    _calculoImpostoContext.DescontoMinimo.Remove(descontoMinimo);
                    await _calculoImpostoContext.SaveChangesAsync();
                    return descontoMinimo;
                }
            }
            return new();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<DescontoMinimo>> GetAll()
    {
        try
        {
            var listDescontoMinimo = await _calculoImpostoContext
                                           .DescontoMinimo
                                           .ToListAsync();
            return listDescontoMinimo ?? [];
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<IEnumerable<DescontoMinimo>> GetByCompetence(DateTime competence)
    {
        try
        {
            var maxCompetencia = await _calculoImpostoContext
                                       .DescontoMinimo
                                       .Where(w => w.Competencia <= competence)
                                       .MaxAsync(w => w.Competencia);

            var listDescontoMinimo = await _calculoImpostoContext
                                           .DescontoMinimo
                                           .Where(w => w.Competencia == maxCompetencia)
                                           .ToListAsync();
            return listDescontoMinimo ?? [];
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<DescontoMinimo> GetById(int id)
    {
        try
        {
            if (id != 0)
            {
                var descontoMinimo = await _calculoImpostoContext
                                           .DescontoMinimo
                                           .Where(w => w.Id == id)
                                           .FirstOrDefaultAsync();
                return descontoMinimo;
            }
            return new();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public async Task<DescontoMinimo> Update(DescontoMinimo descontoMinimo)
    {
        try
        {
            if (descontoMinimo is not null)
            {
                _calculoImpostoContext.DescontoMinimo.Add(descontoMinimo);
                await _calculoImpostoContext.SaveChangesAsync();
                return descontoMinimo;
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
            var competenceMax = await _calculoImpostoContext
                                      .DescontoMinimo
                                      .Where(w => w.Competencia == competence)
                                      .MaxAsync(m => m.Competencia);


            var value = await _calculoImpostoContext
                              .DescontoMinimo
                              .Where(w => w.Competencia == competenceMax)
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
