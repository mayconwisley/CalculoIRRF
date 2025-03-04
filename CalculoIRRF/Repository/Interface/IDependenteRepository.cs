using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository.Interface;

public interface IDependenteRepository
{
    Task<IEnumerable<Dependente>> GetAll();
    Task<IEnumerable<Dependente>> GetByCompetence(DateTime competence);
    Task<Dependente> GetById(int id);
    Task<Dependente> Create(Dependente dependente);
    Task<Dependente> Update(Dependente dependente);
    Task<Dependente> Delete(int id);
    Task<double> Value(DateTime competence);
}
