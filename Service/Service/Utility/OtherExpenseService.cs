using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;

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

        public OtherExpense CreateObject(OtherExpense otherExpense, ISalaryItemService _salaryItemService)
        {
            otherExpense.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(otherExpense, this))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectByCode(otherExpense.Code);
                if (salaryItem != null)
                {
                    otherExpense.Errors = new Dictionary<string, string>();
                    otherExpense.Errors.Add("Code", "SalaryItem dengan Code ini sudah ada");
                    return otherExpense;
                }
                salaryItem = _salaryItemService.CreateObject(otherExpense.Code, otherExpense.Name, (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemType.OtherExpense, otherExpense.SalaryStatus, otherExpense.IsMainSalary, otherExpense.IsDetailSalary, false);
                if (salaryItem == null)
                {
                    otherExpense.Errors = new Dictionary<string, string>();
                    otherExpense.Errors.Add("Code", "Tidak dapat membuat SalaryItem dengan Code ini");
                    return otherExpense;
                }
                otherExpense.SalaryItemId = salaryItem.Id;
                _repository.CreateObject(otherExpense);
            }
            return otherExpense;
        }

        public OtherExpense UpdateObject(OtherExpense otherExpense, ISalaryItemService _salaryItemService)
        {
            if(_validator.ValidUpdateObject(otherExpense, this))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectById(otherExpense.SalaryItemId.GetValueOrDefault());
                if (salaryItem == null)
                {
                    salaryItem = _salaryItemService.CreateObject(otherExpense.Code, otherExpense.Name, (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemType.SalarySlip, otherExpense.SalaryStatus, otherExpense.IsMainSalary, otherExpense.IsDetailSalary, false);
                    otherExpense.SalaryItemId = salaryItem.Id;
                }
                else
                {
                    salaryItem.Code = otherExpense.Code;
                    salaryItem.Name = otherExpense.Name;
                    salaryItem.SalaryItemStatus = otherExpense.SalaryStatus;
                    _salaryItemService.UpdateObject(salaryItem);
                    if (salaryItem.Errors.Any())
                    {
                        otherExpense.Errors.Clear();
                        otherExpense.Errors.Add("Code", "Tidak dapat mengubah SalaryItem dengan Code ini");
                    }
                }
                _repository.UpdateObject(otherExpense);
            }
            return otherExpense;
        }

        public OtherExpense SoftDeleteObject(OtherExpense otherExpense, ISalaryItemService _salaryItemService)
        {
            if(_validator.ValidDeleteObject(otherExpense))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectById(otherExpense.SalaryItemId.GetValueOrDefault());
                _repository.SoftDeleteObject(otherExpense);
                if (salaryItem != null)
                {
                    _salaryItemService.SoftDeleteObject(salaryItem);
                }
            }
            return otherExpense;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(OtherExpense otherExpense)
        {
            IQueryable<OtherExpense> otherExpenses = _repository.FindAll(x => x.Code == otherExpense.Code && !x.IsDeleted && x.Id != otherExpense.Id);
            return (otherExpenses.Count() > 0 ? true : false);
        }

    }
}