using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace Data.Repository
{
    public class OtherExpenseDetailRepository : EfRepository<OtherExpenseDetail>, IOtherExpenseDetailRepository
    {
        private AttPayrollEntities entities;
        public OtherExpenseDetailRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<OtherExpenseDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<OtherExpenseDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public OtherExpenseDetail GetObjectById(int Id)
        {
            OtherExpenseDetail otherExpenseDetail = FindAll(x => x.Id == Id && !x.IsDeleted).Include("Employee").Include("OtherExpense").FirstOrDefault();
            if (otherExpenseDetail != null) { otherExpenseDetail.Errors = new Dictionary<string, string>(); }
            return otherExpenseDetail;
        }

        public OtherExpenseDetail CreateObject(OtherExpenseDetail otherExpenseDetail)
        {
            otherExpenseDetail.IsDeleted = false;
            otherExpenseDetail.CreatedAt = DateTime.Now;
            return Create(otherExpenseDetail);
        }

        public OtherExpenseDetail UpdateObject(OtherExpenseDetail otherExpenseDetail)
        {
            otherExpenseDetail.UpdatedAt = DateTime.Now;
            Update(otherExpenseDetail);
            return otherExpenseDetail;
        }

        public OtherExpenseDetail SoftDeleteObject(OtherExpenseDetail otherExpenseDetail)
        {
            otherExpenseDetail.IsDeleted = true;
            otherExpenseDetail.DeletedAt = DateTime.Now;
            Update(otherExpenseDetail);
            return otherExpenseDetail;
        }

        public bool DeleteObject(int Id)
        {
            OtherExpenseDetail otherExpenseDetail = Find(x => x.Id == Id);
            return (Delete(otherExpenseDetail) == 1) ? true : false;
        }

        
    }
}