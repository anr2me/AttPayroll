using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;
using System.Data.Entity;

namespace Data.Repository
{
    public class FormulaRepository : EfRepository<Formula>, IFormulaRepository
    {
        private AttPayrollEntities entities;
        public FormulaRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<Formula> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<Formula> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public Formula GetObjectById(int Id)
        {
            Formula formula = Find(x => x.Id == Id && !x.IsDeleted);
            if (formula != null) { formula.Errors = new Dictionary<string, string>(); }
            return formula;
        }

        public Formula GetObjectByCode(string Code)
        {
            return FindAll(x => x.Code == Code && !x.IsDeleted).FirstOrDefault();
        }

        public Formula CreateObject(Formula formula)
        {
            formula.IsDeleted = false;
            formula.CreatedAt = DateTime.Now;
            return Create(formula);
        }

        public Formula UpdateObject(Formula formula)
        {
            formula.UpdatedAt = DateTime.Now;
            Update(formula);
            return formula;
        }

        public Formula SoftDeleteObject(Formula formula)
        {
            formula.IsDeleted = true;
            formula.DeletedAt = DateTime.Now;
            Update(formula);
            return formula;
        }

        public bool DeleteObject(int Id)
        {
            Formula formula = Find(x => x.Id == Id);
            return (Delete(formula) == 1) ? true : false;
        }

        public bool IsCodeDuplicated(Formula formula)
        {
            IQueryable<Formula> formulas = FindAll(x => x.Code == formula.Code && !x.IsDeleted && x.Id != formula.Id);
            return (formulas.Count() > 0 ? true : false);
        }
    }
}