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
    public class DepartmentRepository : EfRepository<Department>, IDepartmentRepository
    {
        private AttPayrollEntities entities;
        public DepartmentRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<Department> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<Department> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public Department GetObjectById(int Id)
        {
            Department department = Find(x => x.Id == Id && !x.IsDeleted);
            if (department != null) { department.Errors = new Dictionary<string, string>(); }
            return department;
        }

        public Department GetObjectByName(string Name)
        {
            return FindAll(x => x.Name == Name && !x.IsDeleted).FirstOrDefault();
        }

        public Department CreateObject(Department department)
        {
            department.IsDeleted = false;
            department.CreatedAt = DateTime.Now;
            return Create(department);
        }

        public Department UpdateObject(Department department)
        {
            department.UpdatedAt = DateTime.Now;
            Update(department);
            return department;
        }

        public Department SoftDeleteObject(Department department)
        {
            department.IsDeleted = true;
            department.DeletedAt = DateTime.Now;
            Update(department);
            return department;
        }

        public bool DeleteObject(int Id)
        {
            Department department = Find(x => x.Id == Id);
            return (Delete(department) == 1) ? true : false;
        }

        public bool IsNameDuplicated(Department department)
        {
            IQueryable<Department> departments = FindAll(x => x.Name == department.Name && !x.IsDeleted && x.Id != department.Id);
            return (departments.Count() > 0 ? true : false);
        }
    }
}