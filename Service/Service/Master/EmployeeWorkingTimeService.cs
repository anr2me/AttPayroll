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
    public class EmployeeWorkingTimeService : IEmployeeWorkingTimeService
    {
        private IEmployeeWorkingTimeRepository _repository;
        private IEmployeeWorkingTimeValidator _validator;
        public EmployeeWorkingTimeService(IEmployeeWorkingTimeRepository _employeeWorkingTimeRepository, IEmployeeWorkingTimeValidator _employeeWorkingTimeValidator)
        {
            _repository = _employeeWorkingTimeRepository;
            _validator = _employeeWorkingTimeValidator;
        }

        public IEmployeeWorkingTimeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<EmployeeWorkingTime> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<EmployeeWorkingTime> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<EmployeeWorkingTime> GetObjectsByWorkingTimeId(int WorkingTimeId)
        {
            return _repository.FindAll(x => x.WorkingTimeId == WorkingTimeId && !x.IsDeleted).ToList();
        }

        public EmployeeWorkingTime GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public EmployeeWorkingTime CreateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService, IEmployeeService _employeeService)
        {
            employeeWorkingTime.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(employeeWorkingTime, _workingTimeService, _employeeService) ? _repository.CreateObject(employeeWorkingTime) : employeeWorkingTime);
        }

        public EmployeeWorkingTime UpdateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService, IEmployeeService _employeeService)
        {
            return (employeeWorkingTime = _validator.ValidUpdateObject(employeeWorkingTime, _workingTimeService, _employeeService) ? _repository.UpdateObject(employeeWorkingTime) : employeeWorkingTime);
        }

        public EmployeeWorkingTime SoftDeleteObject(EmployeeWorkingTime employeeWorkingTime, IEmployeeService _employeeService)
        {
            return (employeeWorkingTime = _validator.ValidDeleteObject(employeeWorkingTime, _employeeService) ?
                    _repository.SoftDeleteObject(employeeWorkingTime) : employeeWorkingTime);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}