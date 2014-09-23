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
    public class EmployeeLoanRepository : EfRepository<EmployeeLoan>, IEmployeeLoanRepository
    {
        private AttPayrollEntities entities;
        public EmployeeLoanRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<EmployeeLoan> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<EmployeeLoan> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public EmployeeLoan GetObjectById(int Id)
        {
            EmployeeLoan employeeLoan = Find(x => x.Id == Id && !x.IsDeleted);
            if (employeeLoan != null) { employeeLoan.Errors = new Dictionary<string, string>(); }
            return employeeLoan;
        }

        public EmployeeLoan CreateObject(EmployeeLoan employeeLoan)
        {
            employeeLoan.IsDeleted = false;
            employeeLoan.CreatedAt = DateTime.Now;
            return Create(employeeLoan);
        }

        public EmployeeLoan UpdateObject(EmployeeLoan employeeLoan)
        {
            employeeLoan.UpdatedAt = DateTime.Now;
            Update(employeeLoan);
            return employeeLoan;
        }

        public EmployeeLoan SoftDeleteObject(EmployeeLoan employeeLoan)
        {
            employeeLoan.IsDeleted = true;
            employeeLoan.DeletedAt = DateTime.Now;
            Update(employeeLoan);
            return employeeLoan;
        }

        public bool DeleteObject(int Id)
        {
            EmployeeLoan employeeLoan = Find(x => x.Id == Id);
            return (Delete(employeeLoan) == 1) ? true : false;
        }


    }
}