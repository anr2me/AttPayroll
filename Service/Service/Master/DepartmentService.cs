using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using System.Data.Entity;

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

        public IList<Department> GetObjectsByBranchOfficeId(int BranchOfficeId)
        {
            return _repository.FindAll(x => x.BranchOfficeId == BranchOfficeId && !x.IsDeleted).ToList();
        }

        public Department GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Department GetObjectByCode(string Code)
        {
            return _repository.FindAll(x => x.Code == Code && !x.IsDeleted).FirstOrDefault();
        }

        public Department GetObjectByName(string name)
        {
            return _repository.FindAll(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
        }

        public Department CreateObject(int branchOfficeId, string Code, string Name, string Description, IBranchOfficeService _branchOfficeService)
        {
            Department department = new Department
            {
                BranchOfficeId = branchOfficeId,
                Code = Code,
                Name = Name,
                Description = Description,
            };
            return this.CreateObject(department, _branchOfficeService);
        }

        public Department FindOrCreateObject(int branchOfficeId, string Code, string Name, string Description, IBranchOfficeService _branchOfficeService)
        {

            Department department = GetObjectByCode(Code);
            if (department != null)
            {
                department.Errors = new Dictionary<String, String>();
                return department;
            };
            department = new Department
            {
                BranchOfficeId = branchOfficeId,
                Code = Code,
                Name = Name,
                Description = Description,
            };
            return this.CreateObject(department, _branchOfficeService);
        }

        public Department CreateObject(Department department, IBranchOfficeService _branchOfficeService)
        {
            department.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(department, this, _branchOfficeService) ? _repository.CreateObject(department) : department);
        }

        public Department UpdateObject(Department department, IBranchOfficeService _branchOfficeService)
        {
            return (department = _validator.ValidUpdateObject(department, this, _branchOfficeService) ? _repository.UpdateObject(department) : department);
        }

        public Department SoftDeleteObject(Department department, IDivisionService _divisionService)
        {
            return (department = _validator.ValidDeleteObject(department, _divisionService) ?
                    _repository.SoftDeleteObject(department) : department);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(Department department)
        {
            IQueryable<Department> departments = _repository.FindAll(x => x.Code == department.Code && !x.IsDeleted && x.Id != department.Id);
            return (departments.Count() > 0 ? true : false);
        }

        public bool IsNameDuplicated(Department department)
        {
            IQueryable<Department> departments = _repository.FindAll(x => x.Name == department.Name && !x.IsDeleted && x.Id != department.Id);
            return (departments.Count() > 0 ? true : false);
        }
    }
}