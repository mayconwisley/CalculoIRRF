using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository.Interface;

public interface ISimplificadoRepository
{
    Task<IEnumerable<Simplificado>> GetAll();
    Task<IEnumerable<Simplificado>> GetByCompetence(DateTime competence);
    Task<Simplificado> GetById(int id);
    Task<Simplificado> Create(Simplificado simplificado);
    Task<Simplificado> Update(Simplificado simplificado);
    Task<Simplificado> Delete(int id);
    Task<double> Value(DateTime competence);
}
