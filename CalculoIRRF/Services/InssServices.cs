using CalculoIRRF.Model;
using CalculoIRRF.Repository.Interface;
using CalculoIRRF.Services.Interface;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services;

public class InssServices(IInssRepository _inssRepository) : IInssServices
{
    public async Task<bool> Gravar(Inss inss)
    {
        await _inssRepository.Create(inss);
        return true;
    }

    public async Task<bool> Gravar(InssGov inssGov)
    {
        await _inssRepository.Create(inssGov);
        return true;
    }

    public async Task<bool> Alterar(Inss inss)
    {
        await _inssRepository.Update(inss);
        return true;
    }
    public async Task<bool> Excluir(int id)
    {
        await _inssRepository.Delete(id);
        return true;
    }
    public async Task<int> FaixaInss(double baseInss, DateTime competencia)
    {
        var range = await _inssRepository.Range(baseInss, competencia);
        return range;
    }
    public async Task<int> UltimaFaixaInss(DateTime competencia)
    {
        var lastRange = await _inssRepository.LastRange(competencia);
        return lastRange;

    }
    public async Task<double> PorcentagemInss(int faixa, DateTime competencia)
    {
        var percent = await _inssRepository.Percent(faixa, competencia);
        return percent;
    }
    public async Task<double> ValorInss(int faixa, DateTime competencia)
    {
        var value = await _inssRepository.Value(faixa, competencia);
        return value;
    }
    public async Task<double> TetoInss(DateTime competencia)
    {
        var roof = await _inssRepository.Roof(competencia);
        return roof;
    }
    public async Task<IEnumerable<Inss>> ListarTodos()
    {
        var listInss = await _inssRepository.GetAll();
        return listInss;
    }
    public async Task<IEnumerable<Inss>> ListarTodosPorCompetencia(DateTime competencia)
    {
        var listInss = await _inssRepository.GetByCompetence(competencia);
        return listInss;
    }

    public async Task<Inss> ListarPorId(int id)
    {
        var inss = await _inssRepository.GetById(id);
        return inss;
    }

    public async Task<bool> IsGov(DateTime competencia)
    {
        return await _inssRepository.IsGov(competencia);
    }
}
