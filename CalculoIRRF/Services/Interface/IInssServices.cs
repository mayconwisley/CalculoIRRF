using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Interface;

public interface IInssServices
{
    Task<bool> Gravar(Inss inss);
    Task<bool> Alterar(Inss inss);
    Task<bool> Excluir(int id);
    Task<int> FaixaInss(decimal baseInss, DateTime competencia);
    Task<int> UltimaFaixaInss(DateTime competencia);
    Task<decimal> PorcentagemInss(int faixa, DateTime competencia);
    Task<decimal> ValorInss(int faixa, DateTime competencia);
    Task<decimal> TetoInss(DateTime competencia);
    Task<IEnumerable<Inss>> ListarTodos();
    Task<IEnumerable<Inss>> ListarTodosPorCompetencia(DateTime competencia);
    Task<Inss> ListarPorId(int id);
}
