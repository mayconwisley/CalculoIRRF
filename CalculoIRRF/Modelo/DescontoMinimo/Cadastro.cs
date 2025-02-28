using CalculoIRRF.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Modelo.DescontoMinimo;

public class Cadastro
{
    private readonly DescontoMinimoRepository _descontoMinimoRepository;
    public Cadastro()
    {
        _descontoMinimoRepository = new();
    }

    public async Task<bool> Gravar(Model.DescontoMinimo descontoMinimo)
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
    public async Task<bool> Alterar(Model.DescontoMinimo descontoMinimo)
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
    public async Task<decimal> ValorDescontoMinimo(DateTime competencia)
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
    public async Task<IEnumerable<Model.DescontoMinimo>> ListarTodos()
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
    public async Task<IEnumerable<Model.DescontoMinimo>> ListarTodosPorCompetencia(DateTime competencia)
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
}
