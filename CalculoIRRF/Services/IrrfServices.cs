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
		await _irrfRepository.Create(irrf);
		return true;
	}
	public async Task<bool> Alterar(Irrf irrf)
	{
		await _irrfRepository.Update(irrf);
		return true;
	}
	public async Task<bool> Excluir(int id)
	{
		await _irrfRepository.Delete(id);
		return true;
	}
	public async Task<int> FaixaIrrf(double baseIrrf, DateTime competencia)
	{
		return await _irrfRepository.Range(baseIrrf, competencia);
	}
	public async Task<int> UltimaFaixaIrrf(DateTime competencia)
	{
		return await _irrfRepository.LastRange(competencia);
	}
	public async Task<double> PorcentagemIrrf(int faixa, DateTime competencia)
	{
		return await _irrfRepository.Percent(faixa, competencia);
	}
	public async Task<double> DeducaoIrrf(int faixa, DateTime competencia)
	{
		return await _irrfRepository.Deduction(faixa, competencia);
	}
	public async Task<double> ValorIrrf(int faixa, DateTime competencia)
	{
		return await _irrfRepository.Value(faixa, competencia);
	}
	public async Task<IEnumerable<Irrf>> ListarTodos()
	{
		return await _irrfRepository.GetAll();
	}
	public async Task<IEnumerable<Irrf>> ListarTodosPorCompetencia(DateTime competencia)
	{
		return await _irrfRepository.GetByCompetence(competencia);
	}
	public async Task<Irrf> ListarPorId(int id)
	{
		var irrf = await _irrfRepository.GetById(id);
		return irrf;
	}

	public async Task<bool> GravarRfb(IrrfRfb irrfRfb)
	{
		await _irrfRepository.Create(irrfRfb);
		return true;
	}

	public Task<IEnumerable<IrrfRfb>> ListarTodosDataVigencia(DateTime dataAtualizacao)
	{
		return _irrfRepository.GetByDateUpdate(dataAtualizacao);
	}

	public async Task<bool> IsGov(DateTime vigencia)
	{
		return await _irrfRepository.IsGov(vigencia);
	}
}
