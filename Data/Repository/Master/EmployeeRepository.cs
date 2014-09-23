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
    public class EmployeeRepository : EfRepository<Employee>, IEmployeeRepository
    {
        private AttPayrollEntities entities;
        public EmployeeRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<Employee> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<Employee> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public Employee GetObjectById(int Id)
        {
            Employee employee = Find(x => x.Id == Id && !x.IsDeleted);
            if (employee != null) { employee.Errors = new Dictionary<string, string>(); }
            return employee;
        }

        public Employee GetObjectByNIK(string NIK)
        {
            return FindAll(x => x.NIK == NIK && !x.IsDeleted).FirstOrDefault();
        }

        public Employee CreateObject(Employee employee)
        {
            employee.IsDeleted = false;
            employee.CreatedAt = DateTime.Now;
            return Create(employee);
        }

        public Employee UpdateObject(Employee employee)
        {
            employee.UpdatedAt = DateTime.Now;
            Update(employee);
            return employee;
        }

        public Employee SoftDeleteObject(Employee employee)
        {
            employee.IsDeleted = true;
            employee.DeletedAt = DateTime.Now;
            Update(employee);
            return employee;
        }

        public bool DeleteObject(int Id)
        {
            Employee employee = Find(x => x.Id == Id);
            return (Delete(employee) == 1) ? true : false;
        }

        public bool IsNIKDuplicated(Employee employee)
        {
            IQueryable<Employee> employees = FindAll(x => x.NIK == employee.NIK && !x.IsDeleted && x.Id != employee.Id);
            return (employees.Count() > 0 ? true : false);
        }
    }
}