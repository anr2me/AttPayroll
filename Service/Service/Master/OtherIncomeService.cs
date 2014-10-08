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
    public class OtherIncomeService : IOtherIncomeService
    {
        private IOtherIncomeRepository _repository;
        private IOtherIncomeValidator _validator;
        public OtherIncomeService(IOtherIncomeRepository _otherIncomeRepository, IOtherIncomeValidator _otherIncomeValidator)
        {
            _repository = _otherIncomeRepository;
            _validator = _otherIncomeValidator;
        }

        public IOtherIncomeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<OtherIncome> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<OtherIncome> GetAll()
        {
            return _repository.GetAll();
        }

        public OtherIncome GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public OtherIncome CreateObject(OtherIncome otherIncome, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            otherIncome.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(otherIncome, _employeeService, _salaryItemService) ? _repository.CreateObject(otherIncome) : otherIncome);
        }

        public OtherIncome UpdateObject(OtherIncome otherIncome, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            return (otherIncome = _validator.ValidUpdateObject(otherIncome, _employeeService, _salaryItemService) ? _repository.UpdateObject(otherIncome) : otherIncome);
        }

        public OtherIncome SoftDeleteObject(OtherIncome otherIncome)
        {
            return (otherIncome = _validator.ValidDeleteObject(otherIncome) ?
                    _repository.SoftDeleteObject(otherIncome) : otherIncome);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}