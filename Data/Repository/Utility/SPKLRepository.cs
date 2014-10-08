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
    public class SPKLRepository : EfRepository<SPKL>, ISPKLRepository
    {
        private AttPayrollEntities entities;
        public SPKLRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<SPKL> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SPKL> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public SPKL GetObjectById(int Id)
        {
            SPKL spkl = Find(x => x.Id == Id && !x.IsDeleted);
            if (spkl != null) { spkl.Errors = new Dictionary<string, string>(); }
            return spkl;
        }

        public SPKL CreateObject(SPKL spkl)
        {
            spkl.IsDeleted = false;
            spkl.CreatedAt = DateTime.Now;
            return Create(spkl);
        }

        public SPKL UpdateObject(SPKL spkl)
        {
            spkl.UpdatedAt = DateTime.Now;
            Update(spkl);
            return spkl;
        }

        public SPKL SoftDeleteObject(SPKL spkl)
        {
            spkl.IsDeleted = true;
            spkl.DeletedAt = DateTime.Now;
            Update(spkl);
            return spkl;
        }

        public bool DeleteObject(int Id)
        {
            SPKL spkl = Find(x => x.Id == Id);
            return (Delete(spkl) == 1) ? true : false;
        }


    }
}