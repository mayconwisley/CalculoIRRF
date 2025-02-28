using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository.Interface;

interface IDescontoMinimoRepository
{
    Task<IEnumerable<DescontoMinimo>> GetAll();
    Task<IEnumerable<DescontoMinimo>> GetByCompetence(DateTime competence);
    Task<DescontoMinimo> GetById(int id);
    Task<DescontoMinimo> Create(DescontoMinimo descontoMinimo);
    Task<DescontoMinimo> Update(DescontoMinimo descontoMinimo);
    Task<DescontoMinimo> Delete(int id);
    Task<decimal> Value(DateTime competence);
}
