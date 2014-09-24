using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Service.Service
{
    public class DepartmentService : IDepartmentService
    {
        private IDepartmentRepository _repository;
        private IDepartmentValidator _validator;
        public DepartmentService(IDepartmentRepository _departmentRepository, IDepartmentValidator _departmentValidator)
        {
            _repository = _departmentRepository;
            _validator = _departmentValidator;
        }

        public IDepartmentValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Department> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Department> GetAll()
        {
            return _repository.GetAll();
        }

        public Department GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Department GetObjectByName(string name)
        {
            return _repository.FindAll(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
        }

        public Department CreateObject(string Code, string Name, string Description)
        {
            Department department = new Department
            {
                Code = Code,
                Name = Name,
                Description = Description,
            };
            return this.CreateObject(department);
        }

        public Department CreateObject(Department department)
        {
            department.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(department, this) ? _repository.CreateObject(department) : department);
        }

        public Department UpdateObject(Department department)
        {
            return (department = _validator.ValidUpdateObject(department, this) ? _repository.UpdateObject(department) : department);
        }

        public Department SoftDeleteObject(Department department)
        {
            return (department = _validator.ValidDeleteObject(department) ?
                    _repository.SoftDeleteObject(department) : department);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(Department department)
        {
            IQueryable<Department> departments = _repository.FindAll(x => x.Name == department.Name && !x.IsDeleted && x.Id != department.Id);
            return (departments.Count() > 0 ? true : false);
        }
    }
}