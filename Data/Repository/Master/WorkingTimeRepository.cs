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
    public class WorkingTimeRepository : EfRepository<WorkingTime>, IWorkingTimeRepository
    {
        private AttPayrollEntities entities;
        public WorkingTimeRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<WorkingTime> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<WorkingTime> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public WorkingTime GetObjectById(int Id)
        {
            WorkingTime workingTime = Find(x => x.Id == Id && !x.IsDeleted);
            if (workingTime != null) { workingTime.Errors = new Dictionary<string, string>(); }
            return workingTime;
        }

        public WorkingTime CreateObject(WorkingTime workingTime)
        {
            workingTime.IsDeleted = false;
            workingTime.CreatedAt = DateTime.Now;
            return Create(workingTime);
        }

        public WorkingTime UpdateObject(WorkingTime workingTime)
        {
            workingTime.UpdatedAt = DateTime.Now;
            Update(workingTime);
            return workingTime;
        }

        public WorkingTime SoftDeleteObject(WorkingTime workingTime)
        {
            workingTime.IsDeleted = true;
            workingTime.DeletedAt = DateTime.Now;
            Update(workingTime);
            return workingTime;
        }

        public bool DeleteObject(int Id)
        {
            WorkingTime workingTime = Find(x => x.Id == Id);
            return (Delete(workingTime) == 1) ? true : false;
        }

        
    }
}