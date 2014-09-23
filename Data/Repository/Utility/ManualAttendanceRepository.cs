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
    public class ManualAttendanceRepository : EfRepository<ManualAttendance>, IManualAttendanceRepository
    {
        private AttPayrollEntities entities;
        public ManualAttendanceRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<ManualAttendance> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<ManualAttendance> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public ManualAttendance GetObjectById(int Id)
        {
            ManualAttendance manualAttendance = Find(x => x.Id == Id && !x.IsDeleted);
            if (manualAttendance != null) { manualAttendance.Errors = new Dictionary<string, string>(); }
            return manualAttendance;
        }

        public ManualAttendance CreateObject(ManualAttendance manualAttendance)
        {
            manualAttendance.IsDeleted = false;
            manualAttendance.CreatedAt = DateTime.Now;
            return Create(manualAttendance);
        }

        public ManualAttendance UpdateObject(ManualAttendance manualAttendance)
        {
            manualAttendance.UpdatedAt = DateTime.Now;
            Update(manualAttendance);
            return manualAttendance;
        }

        public ManualAttendance SoftDeleteObject(ManualAttendance manualAttendance)
        {
            manualAttendance.IsDeleted = true;
            manualAttendance.DeletedAt = DateTime.Now;
            Update(manualAttendance);
            return manualAttendance;
        }

        public bool DeleteObject(int Id)
        {
            ManualAttendance manualAttendance = Find(x => x.Id == Id);
            return (Delete(manualAttendance) == 1) ? true : false;
        }


    }
}