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
        await _descontoMinimoRepository.Create(descontoMinimo);
        return true;
    }
    public async Task<bool> Alterar(DescontoMinimo descontoMinimo)
    {
        await _descontoMinimoRepository.Update(descontoMinimo);
        return true;
    }
    public async Task<bool> Excluir(int id)
    {
        await _descontoMinimoRepository.Delete(id);
        return true;
    }
    public async Task<double> ValorDescontoMinimo(DateTime competencia)
    {
        var value = await _descontoMinimoRepository.Value(competencia);
        return value;
    }
    public async Task<IEnumerable<DescontoMinimo>> ListarTodos()
    {
        var listDescontoMinimo = await _descontoMinimoRepository.GetAll();
        return listDescontoMinimo;
    }
    public async Task<IEnumerable<DescontoMinimo>> ListarTodosPorCompetencia(DateTime competencia)
    {
        var listDescontoMinimo = await _descontoMinimoRepository.GetByCompetence(competencia);
        return listDescontoMinimo;
    }
    public async Task<DescontoMinimo> ListarPorId(int id)
    {
        var descontoMinimo = await _descontoMinimoRepository.GetById(id);
        return descontoMinimo;
    }
}
