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
    public class DivisionService : IDivisionService
    {
        private IDivisionRepository _repository;
        private IDivisionValidator _validator;
        public DivisionService(IDivisionRepository _divisionRepository, IDivisionValidator _divisionValidator)
        {
            _repository = _divisionRepository;
            _validator = _divisionValidator;
        }

        public IDivisionValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Division> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Division> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<Division> GetObjectsByDepartmentId(int DepartmentId)
        {
            return _repository.FindAll(x => x.DepartmentId == DepartmentId && !x.IsDeleted).ToList();
        }

        public Division GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public Division GetObjectByCode(string Code)
        {
            return _repository.FindAll(x => x.Code == Code && !x.IsDeleted).FirstOrDefault();
        }

        public Division GetObjectByName(string name)
        {
            return _repository.FindAll(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
        }

        public Division CreateObject(int departmentId, string Code, string Name, string Description, IDepartmentService _departmentService)
        {
            Division division = new Division
            {
                DepartmentId = departmentId,
                Code = Code,
                Name = Name,
                Description = Description,
            };
            return this.CreateObject(division, _departmentService);
        }

        public Division FindOrCreateObject(int departmentId, string Code, string Name, string Description, IDepartmentService _departmentService)
        {
            Division division = GetObjectByCode(Code);
            if (division != null)
            {
                division.Errors = new Dictionary<String, String>();
                return division;
            }
            division = new Division
            {
                DepartmentId = departmentId,
                Code = Code,
                Name = Name,
                Description = Description,
            };
            return this.CreateObject(division, _departmentService);
        }

        public Division CreateObject(Division division, IDepartmentService _departmentService)
        {
            division.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(division, this, _departmentService) ? _repository.CreateObject(division) : division);
        }

        public Division UpdateObject(Division division, IDepartmentService _departmentService)
        {
            return (division = _validator.ValidUpdateObject(division, this, _departmentService) ? _repository.UpdateObject(division) : division);
        }

        public Division SoftDeleteObject(Division division, IEmployeeService _employeeService)
        {
            return (division = _validator.ValidDeleteObject(division, _employeeService) ?
                    _repository.SoftDeleteObject(division) : division);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(Division division)
        {
            IQueryable<Division> divisions = _repository.FindAll(x => x.Code == division.Code && !x.IsDeleted && x.Id != division.Id);
            return (divisions.Count() > 0 ? true : false);
        }

        public bool IsNameDuplicated(Division division)
        {
            IQueryable<Division> divisions = _repository.FindAll(x => x.Name == division.Name && !x.IsDeleted && x.Id != division.Id);
            return (divisions.Count() > 0 ? true : false);
        }
    }
}