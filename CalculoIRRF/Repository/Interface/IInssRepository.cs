using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository.Interface;

interface IInssRepository
{
    Task<IEnumerable<Inss>> GetAll();
    Task<IEnumerable<Inss>> GetByCompetence(DateTime competence);
    Task<Inss> GetById(int id);
    Task<int> Range(decimal baseInss, DateTime competence);
    Task<int> LastRange(DateTime competence);
    Task<decimal> Percent(int range, DateTime competence);
    Task<decimal> Value(int range, DateTime competence);
    Task<decimal> Roof(DateTime competence);
    Task<Inss> Create(Inss inss);
    Task<Inss> Update(Inss inss);
    Task<Inss> Delete(int id);
}
