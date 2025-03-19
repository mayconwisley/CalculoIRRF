using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Interface;

public interface IIrrfServices
{
    Task<bool> Gravar(Irrf irrf);
    Task<bool> GravarRfb(IrrfRfb irrfRfb);
    Task<bool> Alterar(Irrf irrf);
    Task<bool> Excluir(int id);
    Task<bool> IsGov(DateTime competencia);
    Task<int> FaixaIrrf(double baseIrrf, DateTime competencia);
    Task<int> UltimaFaixaIrrf(DateTime competencia);
    Task<double> PorcentagemIrrf(int faixa, DateTime competencia);
    Task<double> DeducaoIrrf(int faixa, DateTime competencia);
    Task<double> ValorIrrf(int faixa, DateTime competencia);
    Task<IEnumerable<Irrf>> ListarTodos();
    Task<IEnumerable<Irrf>> ListarTodosPorCompetencia(DateTime competencia);
    Task<IEnumerable<IrrfRfb>> ListarTodosDataAtualizacao(DateTime dataAtualizacao);
    Task<Irrf> ListarPorId(int id);
}
