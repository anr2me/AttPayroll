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
    public class OtherExpenseRepository : EfRepository<OtherExpense>, IOtherExpenseRepository
    {
        private AttPayrollEntities entities;
        public OtherExpenseRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<OtherExpense> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<OtherExpense> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public OtherExpense GetObjectById(int Id)
        {
            OtherExpense otherExpense = Find(x => x.Id == Id && !x.IsDeleted);
            if (otherExpense != null) { otherExpense.Errors = new Dictionary<string, string>(); }
            return otherExpense;
        }

        public OtherExpense CreateObject(OtherExpense otherExpense)
        {
            otherExpense.IsDeleted = false;
            otherExpense.CreatedAt = DateTime.Now;
            return Create(otherExpense);
        }

        public OtherExpense UpdateObject(OtherExpense otherExpense)
        {
            otherExpense.UpdatedAt = DateTime.Now;
            Update(otherExpense);
            return otherExpense;
        }

        public OtherExpense SoftDeleteObject(OtherExpense otherExpense)
        {
            otherExpense.IsDeleted = true;
            otherExpense.DeletedAt = DateTime.Now;
            Update(otherExpense);
            return otherExpense;
        }

        public bool DeleteObject(int Id)
        {
            OtherExpense otherExpense = Find(x => x.Id == Id);
            return (Delete(otherExpense) == 1) ? true : false;
        }

        
    }
}