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
    public class EmployeeLoanService : IEmployeeLoanService
    {
        private IEmployeeLoanRepository _repository;
        private IEmployeeLoanValidator _validator;
        public EmployeeLoanService(IEmployeeLoanRepository _employeeLoanRepository, IEmployeeLoanValidator _employeeLoanValidator)
        {
            _repository = _employeeLoanRepository;
            _validator = _employeeLoanValidator;
        }

        public IEmployeeLoanValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<EmployeeLoan> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<EmployeeLoan> GetAll()
        {
            return _repository.GetAll();
        }

        public EmployeeLoan GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public EmployeeLoan CreateObject(EmployeeLoan employeeLoan, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            employeeLoan.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(employeeLoan, _employeeService, _salaryItemService) ? _repository.CreateObject(employeeLoan) : employeeLoan);
        }

        public EmployeeLoan UpdateObject(EmployeeLoan employeeLoan, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            return (employeeLoan = _validator.ValidUpdateObject(employeeLoan, _employeeService, _salaryItemService) ? _repository.UpdateObject(employeeLoan) : employeeLoan);
        }

        public EmployeeLoan SoftDeleteObject(EmployeeLoan employeeLoan)
        {
            return (employeeLoan = _validator.ValidDeleteObject(employeeLoan) ?
                    _repository.SoftDeleteObject(employeeLoan) : employeeLoan);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}