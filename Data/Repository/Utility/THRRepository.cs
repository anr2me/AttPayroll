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
    public class THRRepository : EfRepository<THR>, ITHRRepository
    {
        private AttPayrollEntities entities;
        public THRRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<THR> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<THR> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public THR GetObjectById(int Id)
        {
            THR thr = Find(x => x.Id == Id && !x.IsDeleted);
            if (thr != null) { thr.Errors = new Dictionary<string, string>(); }
            return thr;
        }

        public THR CreateObject(THR thr)
        {
            thr.IsDeleted = false;
            thr.CreatedAt = DateTime.Now;
            return Create(thr);
        }

        public THR UpdateObject(THR thr)
        {
            thr.UpdatedAt = DateTime.Now;
            Update(thr);
            return thr;
        }

        public THR SoftDeleteObject(THR thr)
        {
            thr.IsDeleted = true;
            thr.DeletedAt = DateTime.Now;
            Update(thr);
            return thr;
        }

        public bool DeleteObject(int Id)
        {
            THR thr = Find(x => x.Id == Id);
            return (Delete(thr) == 1) ? true : false;
        }

        
    }
}