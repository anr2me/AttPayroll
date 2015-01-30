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
    public class FPAttLogRepository : EfRepository<FPAttLog>, IFPAttLogRepository
    {
        private AttPayrollEntities entities;
        public FPAttLogRepository()
        {
            entities = new AttPayrollEntities();
            //entities.Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<FPAttLog> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<FPAttLog> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public FPAttLog GetObjectById(int Id)
        {
            FPAttLog fpAttLog = FindAll(x => x.Id == Id && !x.IsDeleted).Include(x => x.FPUser).FirstOrDefault();
            if (fpAttLog != null) { fpAttLog.Errors = new Dictionary<string, string>(); }
            return fpAttLog;
        }

        public FPAttLog CreateObject(FPAttLog fpAttLog)
        {
            fpAttLog.IsDeleted = false;
            fpAttLog.CreatedAt = DateTime.Now;
            return Create(fpAttLog);
        }

        public FPAttLog UpdateObject(FPAttLog fpAttLog)
        {
            fpAttLog.UpdatedAt = DateTime.Now;
            Update(fpAttLog);
            return fpAttLog;
        }

        public FPAttLog SoftDeleteObject(FPAttLog fpAttLog)
        {
            fpAttLog.IsDeleted = true;
            fpAttLog.DeletedAt = DateTime.Now;
            Update(fpAttLog);
            return fpAttLog;
        }

        public bool DeleteObject(int Id)
        {
            FPAttLog fpAttLog = Find(x => x.Id == Id);
            return (Delete(fpAttLog) == 1) ? true : false;
        }

        
    }
}