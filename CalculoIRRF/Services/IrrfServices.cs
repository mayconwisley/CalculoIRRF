using CalculoIRRF.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services;

public class IrrfServices
{
    private readonly IrrfRepository _irrfRepository;
    public IrrfServices()
    {
        _irrfRepository = new();
    }

    public async Task<bool> Gravar(Model.Irrf irrf)
    {
        try
        {
            await _irrfRepository.Create(irrf);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> Alterar(Model.Irrf irrf)
    {
        try
        {
            await _irrfRepository.Update(irrf);
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
            await _irrfRepository.Delete(id);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<int> FaixaIrrf(decimal baseIrrf, DateTime competencia)
    {
        try
        {
            return await _irrfRepository.Range(baseIrrf, competencia);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<int> UltimaFaixaIrrf(DateTime competencia)
    {
        try
        {
            return await _irrfRepository.LastRange(competencia);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<decimal> PorcentagemIrrf(int faixa, DateTime competencia)
    {
        try
        {
            return await _irrfRepository.Percent(faixa, competencia);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<decimal> DeducaoIrrf(int faixa, DateTime competencia)
    {
        try
        {
            return await _irrfRepository.Deduction(faixa, competencia);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<decimal> ValorIrrf(int faixa, DateTime competencia)
    {

        try
        {
            return await _irrfRepository.Value(faixa, competencia);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<IEnumerable<Model.Irrf>> ListarTodos()
    {
        try
        {
            return await _irrfRepository.GetAll();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<IEnumerable<Model.Irrf>> ListarTodosPorCompetencia(DateTime competencia)
    {
        try
        {
            return await _irrfRepository.GetByCompetence(competencia);
        }
        catch (Exception)
        {
            throw;
        }
    }
}
