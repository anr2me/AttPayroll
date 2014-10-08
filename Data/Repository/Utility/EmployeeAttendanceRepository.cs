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
    public class EmployeeAttendanceRepository : EfRepository<EmployeeAttendance>, IEmployeeAttendanceRepository
    {
        private AttPayrollEntities entities;
        public EmployeeAttendanceRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<EmployeeAttendance> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<EmployeeAttendance> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public EmployeeAttendance GetObjectById(int Id)
        {
            EmployeeAttendance employeeAttendance = Find(x => x.Id == Id && !x.IsDeleted);
            if (employeeAttendance != null) { employeeAttendance.Errors = new Dictionary<string, string>(); }
            return employeeAttendance;
        }

        public EmployeeAttendance CreateObject(EmployeeAttendance employeeAttendance)
        {
            employeeAttendance.IsDeleted = false;
            employeeAttendance.CreatedAt = DateTime.Now;
            return Create(employeeAttendance);
        }

        public EmployeeAttendance UpdateObject(EmployeeAttendance employeeAttendance)
        {
            employeeAttendance.UpdatedAt = DateTime.Now;
            Update(employeeAttendance);
            return employeeAttendance;
        }

        public EmployeeAttendance SoftDeleteObject(EmployeeAttendance employeeAttendance)
        {
            employeeAttendance.IsDeleted = true;
            employeeAttendance.DeletedAt = DateTime.Now;
            Update(employeeAttendance);
            return employeeAttendance;
        }

        public bool DeleteObject(int Id)
        {
            EmployeeAttendance employeeAttendance = Find(x => x.Id == Id);
            return (Delete(employeeAttendance) == 1) ? true : false;
        }


    }
}