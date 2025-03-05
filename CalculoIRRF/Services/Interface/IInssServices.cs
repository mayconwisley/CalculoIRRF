using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Services.Interface;

public interface IInssServices
{
    Task<bool> Gravar(Inss inss);
    Task<bool> Gravar(InssGov inssGov);
    Task<bool> Alterar(Inss inss);
    Task<bool> Excluir(int id);
    Task<bool> IsGov(DateTime competencia);
    Task<int> FaixaInss(double baseInss, DateTime competencia);
    Task<int> UltimaFaixaInss(DateTime competencia);
    Task<double> PorcentagemInss(int faixa, DateTime competencia);
    Task<double> ValorInss(int faixa, DateTime competencia);
    Task<double> TetoInss(DateTime competencia);
    Task<IEnumerable<Inss>> ListarTodos();
    Task<IEnumerable<Inss>> ListarTodosPorCompetencia(DateTime competencia);
    Task<Inss> ListarPorId(int id);
}
