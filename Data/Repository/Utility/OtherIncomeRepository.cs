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
    public class OtherIncomeRepository : EfRepository<OtherIncome>, IOtherIncomeRepository
    {
        private AttPayrollEntities entities;
        public OtherIncomeRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<OtherIncome> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<OtherIncome> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public OtherIncome GetObjectById(int Id)
        {
            OtherIncome otherIncome = Find(x => x.Id == Id && !x.IsDeleted);
            if (otherIncome != null) { otherIncome.Errors = new Dictionary<string, string>(); }
            return otherIncome;
        }

        public OtherIncome CreateObject(OtherIncome otherIncome)
        {
            otherIncome.IsDeleted = false;
            otherIncome.CreatedAt = DateTime.Now;
            return Create(otherIncome);
        }

        public OtherIncome UpdateObject(OtherIncome otherIncome)
        {
            otherIncome.UpdatedAt = DateTime.Now;
            Update(otherIncome);
            return otherIncome;
        }

        public OtherIncome SoftDeleteObject(OtherIncome otherIncome)
        {
            otherIncome.IsDeleted = true;
            otherIncome.DeletedAt = DateTime.Now;
            Update(otherIncome);
            return otherIncome;
        }

        public bool DeleteObject(int Id)
        {
            OtherIncome otherIncome = Find(x => x.Id == Id);
            return (Delete(otherIncome) == 1) ? true : false;
        }

        
    }
}