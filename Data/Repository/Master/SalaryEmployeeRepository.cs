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
    public class SalaryEmployeeRepository : EfRepository<SalaryEmployee>, ISalaryEmployeeRepository
    {
        private AttPayrollEntities entities;
        public SalaryEmployeeRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<SalaryEmployee> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalaryEmployee> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public SalaryEmployee GetObjectById(int Id)
        {
            SalaryEmployee salaryEmployee = Find(x => x.Id == Id && !x.IsDeleted);
            if (salaryEmployee != null) { salaryEmployee.Errors = new Dictionary<string, string>(); }
            return salaryEmployee;
        }

        public SalaryEmployee CreateObject(SalaryEmployee salaryEmployee)
        {
            salaryEmployee.IsDeleted = false;
            salaryEmployee.CreatedAt = DateTime.Now;
            return Create(salaryEmployee);
        }

        public SalaryEmployee UpdateObject(SalaryEmployee salaryEmployee)
        {
            salaryEmployee.UpdatedAt = DateTime.Now;
            Update(salaryEmployee);
            return salaryEmployee;
        }

        public SalaryEmployee SoftDeleteObject(SalaryEmployee salaryEmployee)
        {
            salaryEmployee.IsDeleted = true;
            salaryEmployee.DeletedAt = DateTime.Now;
            Update(salaryEmployee);
            return salaryEmployee;
        }

        public bool DeleteObject(int Id)
        {
            SalaryEmployee salaryEmployee = Find(x => x.Id == Id);
            return (Delete(salaryEmployee) == 1) ? true : false;
        }

        
    }
}