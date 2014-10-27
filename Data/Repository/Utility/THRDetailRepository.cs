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
    public class THRDetailRepository : EfRepository<THRDetail>, ITHRDetailRepository
    {
        private AttPayrollEntities entities;
        public THRDetailRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<THRDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<THRDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public THRDetail GetObjectById(int Id)
        {
            THRDetail thrDetail = FindAll(x => x.Id == Id && !x.IsDeleted).Include("Employee").Include("THR").FirstOrDefault();
            if (thrDetail != null) { thrDetail.Errors = new Dictionary<string, string>(); }
            return thrDetail;
        }

        public THRDetail CreateObject(THRDetail thrDetail)
        {
            thrDetail.IsDeleted = false;
            thrDetail.CreatedAt = DateTime.Now;
            return Create(thrDetail);
        }

        public THRDetail UpdateObject(THRDetail thrDetail)
        {
            thrDetail.UpdatedAt = DateTime.Now;
            Update(thrDetail);
            return thrDetail;
        }

        public THRDetail SoftDeleteObject(THRDetail thrDetail)
        {
            thrDetail.IsDeleted = true;
            thrDetail.DeletedAt = DateTime.Now;
            Update(thrDetail);
            return thrDetail;
        }

        public bool DeleteObject(int Id)
        {
            THRDetail thrDetail = Find(x => x.Id == Id);
            return (Delete(thrDetail) == 1) ? true : false;
        }

        
    }
}