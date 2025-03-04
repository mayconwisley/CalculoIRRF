using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using CalculoIRRF.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services;

public class DescontoMinimoServices(IDescontoMinimoRepository _descontoMinimoRepository) : IDescontoMinimoServices
{
    public async Task<bool> Gravar(DescontoMinimo descontoMinimo)
    {
        try
        {
            await _descontoMinimoRepository.Create(descontoMinimo);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<bool> Alterar(DescontoMinimo descontoMinimo)
    {
        try
        {
            await _descontoMinimoRepository.Update(descontoMinimo);
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
            await _descontoMinimoRepository.Delete(id);
            return true;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<double> ValorDescontoMinimo(DateTime competencia)
    {
        try
        {
            var value = await _descontoMinimoRepository.Value(competencia);
            return value;

        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<IEnumerable<DescontoMinimo>> ListarTodos()
    {
        try
        {
            var listDescontoMinimo = await _descontoMinimoRepository.GetAll();
            return listDescontoMinimo;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<IEnumerable<DescontoMinimo>> ListarTodosPorCompetencia(DateTime competencia)
    {
        try
        {
            var listDescontoMinimo = await _descontoMinimoRepository.GetByCompetence(competencia);
            return listDescontoMinimo;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public async Task<DescontoMinimo> ListarPorId(int id)
    {
        try
        {
            var descontoMinimo = await _descontoMinimoRepository.GetById(id);
            return descontoMinimo;
        }
        catch (Exception)
        {
            throw;
        }
    }
}
