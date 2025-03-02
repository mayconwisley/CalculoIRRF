using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository.Interface;

interface IIrrfRepository
{
    Task<IEnumerable<Irrf>> GetAll();
    Task<IEnumerable<Irrf>> GetByCompetence(DateTime competence);
    Task<Irrf> GetById(int id);
    Task<int> Range(decimal baseIrrf, DateTime competence);
    Task<int> LastRange(DateTime competence);
    Task<decimal> Percent(int range, DateTime competence);
    Task<decimal> Deduction(int range, DateTime competence);
    Task<decimal> Value(int range, DateTime competence);
    Task<Irrf> Create(Irrf irrf);
    Task<Irrf> Update(Irrf irrf);
    Task<Irrf> Delete(int id);
}
