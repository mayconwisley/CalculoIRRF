﻿using CalculoIRRF.Model;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CalculoIRRF.Repository.Interface;

public interface IIrrfRepository
{
    Task<IEnumerable<Irrf>> GetAll();
    Task<IEnumerable<Irrf>> GetByCompetence(DateTime competence);
    Task<IEnumerable<IrrfRfb>> GetByDateUpdate(DateTime dateUpdate);
    Task<Irrf> GetById(int id);
    Task<bool> IsGov(DateTime competence);
    Task<int> Range(double baseIrrf, DateTime competence);
    Task<int> LastRange(DateTime competence);
    Task<double> Percent(int range, DateTime competence);
    Task<double> Deduction(int range, DateTime competence);
    Task<double> Value(int range, DateTime competence);
    Task<Irrf> Create(Irrf irrf);
    Task<IrrfRfb> Create(IrrfRfb irrfRfb);
    Task<Irrf> Update(Irrf irrf);
    Task<Irrf> Delete(int id);
}
