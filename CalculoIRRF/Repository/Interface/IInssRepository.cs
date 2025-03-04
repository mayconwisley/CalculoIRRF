using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository.Interface;

public interface IInssRepository
{
    Task<IEnumerable<Inss>> GetAll();
    Task<IEnumerable<Inss>> GetByCompetence(DateTime competence);
    Task<Inss> GetById(int id);
    Task<int> Range(double baseInss, DateTime competence);
    Task<int> LastRange(DateTime competence);
    Task<double> Percent(int range, DateTime competence);
    Task<double> Value(int range, DateTime competence);
    Task<double> Roof(DateTime competence);
    Task<Inss> Create(Inss inss);
    Task<Inss> Update(Inss inss);
    Task<Inss> Delete(int id);
}
