using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Interface;

public interface IIrrfServices
{
    Task<bool> Gravar(Irrf irrf);
    Task<bool> Alterar(Irrf irrf);
    Task<bool> Excluir(int id);
    Task<int> FaixaIrrf(decimal baseIrrf, DateTime competencia);
    Task<int> UltimaFaixaIrrf(DateTime competencia);
    Task<decimal> PorcentagemIrrf(int faixa, DateTime competencia);
    Task<decimal> DeducaoIrrf(int faixa, DateTime competencia);
    Task<decimal> ValorIrrf(int faixa, DateTime competencia);
    Task<IEnumerable<Irrf>> ListarTodos();
    Task<IEnumerable<Irrf>> ListarTodosPorCompetencia(DateTime competencia);
    Task<Irrf> ListarPorId(int id);
}
