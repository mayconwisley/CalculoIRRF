using CalculoIRRF.DataBase;
using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository;

public class SimplificadoRepository : ISimplificadoRepository
{
    private readonly CalculoImpostoContext _calculoImpostoContext;

    public SimplificadoRepository()
    {
        _calculoImpostoContext = new();
    }

    public async Task<Simplificado> Create(Simplificado simplificado)
    {
        if (simplificado is null)
            throw new ArgumentException("Erro ao cadastrar dados");

        _calculoImpostoContext.Simplificado.Add(simplificado);
        await _calculoImpostoContext.SaveChangesAsync();
        return simplificado;
    }

    public async Task<Simplificado> Delete(int id)
    {
        var simplificado = await GetById(id) ?? throw new ArgumentException("Erro ao deletar dados");

        _calculoImpostoContext.Simplificado.Remove(simplificado);
        await _calculoImpostoContext.SaveChangesAsync();

        return simplificado;
    }

    public async Task<IEnumerable<Simplificado>> GetAll()
    {
        return await _calculoImpostoContext.Simplificado.ToListAsync();
    }

    public async Task<IEnumerable<Simplificado>> GetByCompetence(DateTime competence)
    {
        return await _calculoImpostoContext.Simplificado
                     .Where(w => w.Competencia == competence)
                     .ToListAsync();
    }

    public async Task<Simplificado> GetById(int id)
    {
        return await _calculoImpostoContext.Simplificado.FindAsync(id);
    }

    public async Task<Simplificado> Update(Simplificado simplificado)
    {
        if (simplificado is null || simplificado.Id <= 0)
        {
            throw new ArgumentException("Objeto inválido para atualização");
        }

        Simplificado simplificado1 = await GetById(simplificado.Id);
        _calculoImpostoContext.Simplificado.Entry(simplificado1)
        .CurrentValues.SetValues(simplificado);

        await _calculoImpostoContext.SaveChangesAsync();
        return simplificado;
    }

    public async Task<decimal> Value(DateTime competence)
    {
        return await _calculoImpostoContext.Simplificado
                     .Where(w => w.Competencia == _calculoImpostoContext.Simplificado
                                                .Where(w => w.Competencia == competence)
                                                .Max(m => m.Competencia))
                     .Select(s => s.Valor)
                     .DefaultIfEmpty(0)
                     .FirstOrDefaultAsync();
    }
}
