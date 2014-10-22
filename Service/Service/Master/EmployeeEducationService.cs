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
    public class EmployeeEducationService : IEmployeeEducationService
    {
        private IEmployeeEducationRepository _repository;
        private IEmployeeEducationValidator _validator;
        public EmployeeEducationService(IEmployeeEducationRepository _employeeEducationRepository, IEmployeeEducationValidator _employeeEducationValidator)
        {
            _repository = _employeeEducationRepository;
            _validator = _employeeEducationValidator;
        }

        public IEmployeeEducationValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<EmployeeEducation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<EmployeeEducation> GetAll()
        {
            return _repository.GetAll();
        }

        public EmployeeEducation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public IList<EmployeeEducation> GetObjectsByEmployeeId(int EmployeeId)
        {
            return _repository.FindAll(x => x.EmployeeId == EmployeeId).ToList();
        }

        public EmployeeEducation CreateObject(string Institute, string Major, string Level, DateTime EnrollmentDate, Nullable<DateTime> GraduationDate, IEmployeeService _employeeService)
        {
            EmployeeEducation employeeEducation = new EmployeeEducation
            {
                Institute = Institute,
                Major = Major,
                Level = Level,
                EnrollmentDate = EnrollmentDate,
                GraduationDate = GraduationDate,
            };
            return this.CreateObject(employeeEducation, _employeeService);
        }

        public EmployeeEducation CreateObject(EmployeeEducation employeeEducation, IEmployeeService _employeeService)
        {
            employeeEducation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(employeeEducation, _employeeService) ? _repository.CreateObject(employeeEducation) : employeeEducation);
        }

        public EmployeeEducation UpdateObject(EmployeeEducation employeeEducation, IEmployeeService _employeeService)
        {
            return (employeeEducation = _validator.ValidUpdateObject(employeeEducation, _employeeService) ? _repository.UpdateObject(employeeEducation) : employeeEducation);
        }

        public EmployeeEducation SoftDeleteObject(EmployeeEducation employeeEducation)
        {
            return (employeeEducation = _validator.ValidDeleteObject(employeeEducation) ?
                    _repository.SoftDeleteObject(employeeEducation) : employeeEducation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}