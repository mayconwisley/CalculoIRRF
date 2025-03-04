using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using CalculoIRRF.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services;

public class IrrfServices(IIrrfRepository _irrfRepository) : IIrrfServices
{
    public async Task<bool> Gravar(Irrf irrf)
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
    public async Task<bool> Alterar(Irrf irrf)
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
    public async Task<int> FaixaIrrf(double baseIrrf, DateTime competencia)
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
    public async Task<double> PorcentagemIrrf(int faixa, DateTime competencia)
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
    public async Task<double> DeducaoIrrf(int faixa, DateTime competencia)
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
    public async Task<double> ValorIrrf(int faixa, DateTime competencia)
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
    public async Task<IEnumerable<Irrf>> ListarTodos()
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
    public async Task<IEnumerable<Irrf>> ListarTodosPorCompetencia(DateTime competencia)
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
    public async Task<Irrf> ListarPorId(int id)
    {
        try
        {
            var irrf = await _irrfRepository.GetById(id);
            return irrf;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
