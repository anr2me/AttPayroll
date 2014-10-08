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
    public class OtherExpenseService : IOtherExpenseService
    {
        private IOtherExpenseRepository _repository;
        private IOtherExpenseValidator _validator;
        public OtherExpenseService(IOtherExpenseRepository _otherExpenseRepository, IOtherExpenseValidator _otherExpenseValidator)
        {
            _repository = _otherExpenseRepository;
            _validator = _otherExpenseValidator;
        }

        public IOtherExpenseValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<OtherExpense> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<OtherExpense> GetAll()
        {
            return _repository.GetAll();
        }

        public OtherExpense GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public OtherExpense CreateObject(OtherExpense otherExpense, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            otherExpense.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(otherExpense, _employeeService, _salaryItemService) ? _repository.CreateObject(otherExpense) : otherExpense);
        }

        public OtherExpense UpdateObject(OtherExpense otherExpense, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            return (otherExpense = _validator.ValidUpdateObject(otherExpense, _employeeService, _salaryItemService) ? _repository.UpdateObject(otherExpense) : otherExpense);
        }

        public OtherExpense SoftDeleteObject(OtherExpense otherExpense)
        {
            return (otherExpense = _validator.ValidDeleteObject(otherExpense) ?
                    _repository.SoftDeleteObject(otherExpense) : otherExpense);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}