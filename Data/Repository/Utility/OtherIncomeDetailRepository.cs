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
    public class OtherIncomeDetailRepository : EfRepository<OtherIncomeDetail>, IOtherIncomeDetailRepository
    {
        private AttPayrollEntities entities;
        public OtherIncomeDetailRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<OtherIncomeDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<OtherIncomeDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public OtherIncomeDetail GetObjectById(int Id)
        {
            OtherIncomeDetail otherIncomeDetail = FindAll(x => x.Id == Id && !x.IsDeleted).Include("Employee").Include("OtherIncome").FirstOrDefault();
            if (otherIncomeDetail != null) { otherIncomeDetail.Errors = new Dictionary<string, string>(); }
            return otherIncomeDetail;
        }

        public OtherIncomeDetail CreateObject(OtherIncomeDetail otherIncomeDetail)
        {
            otherIncomeDetail.IsDeleted = false;
            otherIncomeDetail.CreatedAt = DateTime.Now;
            return Create(otherIncomeDetail);
        }

        public OtherIncomeDetail UpdateObject(OtherIncomeDetail otherIncomeDetail)
        {
            otherIncomeDetail.UpdatedAt = DateTime.Now;
            Update(otherIncomeDetail);
            return otherIncomeDetail;
        }

        public OtherIncomeDetail SoftDeleteObject(OtherIncomeDetail otherIncomeDetail)
        {
            otherIncomeDetail.IsDeleted = true;
            otherIncomeDetail.DeletedAt = DateTime.Now;
            Update(otherIncomeDetail);
            return otherIncomeDetail;
        }

        public bool DeleteObject(int Id)
        {
            OtherIncomeDetail otherIncomeDetail = Find(x => x.Id == Id);
            return (Delete(otherIncomeDetail) == 1) ? true : false;
        }

        
    }
}