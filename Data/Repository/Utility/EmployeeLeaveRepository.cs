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
    public class EmployeeLeaveRepository : EfRepository<EmployeeLeave>, IEmployeeLeaveRepository
    {
        private AttPayrollEntities entities;
        public EmployeeLeaveRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<EmployeeLeave> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<EmployeeLeave> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public EmployeeLeave GetObjectById(int Id)
        {
            EmployeeLeave employeeLeave = Find(x => x.Id == Id && !x.IsDeleted);
            if (employeeLeave != null) { employeeLeave.Errors = new Dictionary<string, string>(); }
            return employeeLeave;
        }

        public EmployeeLeave CreateObject(EmployeeLeave employeeLeave)
        {
            employeeLeave.IsDeleted = false;
            employeeLeave.CreatedAt = DateTime.Now;
            return Create(employeeLeave);
        }

        public EmployeeLeave UpdateObject(EmployeeLeave employeeLeave)
        {
            employeeLeave.UpdatedAt = DateTime.Now;
            Update(employeeLeave);
            return employeeLeave;
        }

        public EmployeeLeave SoftDeleteObject(EmployeeLeave employeeLeave)
        {
            employeeLeave.IsDeleted = true;
            employeeLeave.DeletedAt = DateTime.Now;
            Update(employeeLeave);
            return employeeLeave;
        }

        public bool DeleteObject(int Id)
        {
            EmployeeLeave employeeLeave = Find(x => x.Id == Id);
            return (Delete(employeeLeave) == 1) ? true : false;
        }


    }
}