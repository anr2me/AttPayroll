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
    public class PTKPRepository : EfRepository<PTKP>, IPTKPRepository
    {
        private AttPayrollEntities entities;
        public PTKPRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<PTKP> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PTKP> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public PTKP GetObjectById(int Id)
        {
            PTKP ptkp = Find(x => x.Id == Id && !x.IsDeleted);
            if (ptkp != null) { ptkp.Errors = new Dictionary<string, string>(); }
            return ptkp;
        }

        public PTKP CreateObject(PTKP ptkp)
        {
            ptkp.IsDeleted = false;
            ptkp.CreatedAt = DateTime.Now;
            return Create(ptkp);
        }

        public PTKP UpdateObject(PTKP ptkp)
        {
            ptkp.UpdatedAt = DateTime.Now;
            Update(ptkp);
            return ptkp;
        }

        public PTKP SoftDeleteObject(PTKP ptkp)
        {
            ptkp.IsDeleted = true;
            ptkp.DeletedAt = DateTime.Now;
            Update(ptkp);
            return ptkp;
        }

        public bool DeleteObject(int Id)
        {
            PTKP ptkp = Find(x => x.Id == Id);
            return (Delete(ptkp) == 1) ? true : false;
        }

        
    }
}