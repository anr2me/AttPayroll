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
    public class FPUserRepository : EfRepository<FPUser>, IFPUserRepository
    {
        private AttPayrollEntities entities;
        public FPUserRepository()
        {
            entities = new AttPayrollEntities();
            //entities.Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<FPUser> GetQueryable()
        {
            return FindAll(); //(x => !x.IsDeleted)
        }

        public IList<FPUser> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public FPUser GetObjectById(int Id)
        {
            FPUser fpUser = FindAll(x => x.Id == Id && !x.IsDeleted).Include(x => x.Employee).FirstOrDefault();
            if (fpUser != null) { fpUser.Errors = new Dictionary<string, string>(); }
            return fpUser;
        }

        public FPUser CreateObject(FPUser fpUser)
        {
            fpUser.IsDeleted = false;
            fpUser.CreatedAt = DateTime.Now;
            return Create(fpUser);
        }

        public FPUser UpdateObject(FPUser fpUser)
        {
            fpUser.UpdatedAt = DateTime.Now;
            Update(fpUser);
            return fpUser;
        }

        public FPUser SoftDeleteObject(FPUser fpUser)
        {
            fpUser.IsDeleted = true;
            fpUser.DeletedAt = DateTime.Now;
            Update(fpUser);
            return fpUser;
        }

        public bool DeleteObject(int Id)
        {
            FPUser fpUser = Find(x => x.Id == Id);
            return (Delete(fpUser) == 1) ? true : false;
        }

        
    }
}