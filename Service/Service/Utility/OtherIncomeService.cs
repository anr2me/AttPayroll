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

        public OtherIncome GetObjectByName(string Name)
        {
            return GetQueryable().Where(x => x.Name == Name).FirstOrDefault();
        }

        public OtherIncome CreateObject(string Code, string Name, string Description, int SalaryStatus, ISalaryItemService _salaryItemService)
        {
            OtherIncome otherIncome = new OtherIncome()
            {
                Code = Code,
                Name = Name,
                Description = Description,
                SalaryStatus = SalaryStatus,
                IsDetailSalary = true,
            };
            CreateObject(otherIncome, _salaryItemService);
            return otherIncome;
        }

        public OtherIncome CreateObject(OtherIncome otherIncome, ISalaryItemService _salaryItemService)
        {
            otherIncome.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(otherIncome, this))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectByCode(otherIncome.Code);
                if (salaryItem != null)
                {
                    otherIncome.Errors = new Dictionary<string, string>();
                    otherIncome.Errors.Add("Code", "SalaryItem dengan Code ini sudah ada");
                    return otherIncome;
                }
                salaryItem = _salaryItemService.CreateObject(otherIncome.Code, otherIncome.Name, (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.OtherIncome, otherIncome.SalaryStatus, otherIncome.IsMainSalary, otherIncome.IsDetailSalary, false);
                if (salaryItem == null)
                {
                    otherIncome.Errors = new Dictionary<string, string>();
                    otherIncome.Errors.Add("Code", "Tidak dapat membuat SalaryItem dengan Code ini");
                    return otherIncome;
                }
                otherIncome.SalaryItemId = salaryItem.Id;
                _repository.CreateObject(otherIncome);
            }
            return otherIncome;
        }

        public OtherIncome UpdateObject(OtherIncome otherIncome, ISalaryItemService _salaryItemService)
        {
            if(_validator.ValidUpdateObject(otherIncome, this))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectById(otherIncome.SalaryItemId.GetValueOrDefault());
                if (salaryItem == null)
                {
                    salaryItem = _salaryItemService.CreateObject(otherIncome.Code, otherIncome.Name, (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.SalarySlip, otherIncome.SalaryStatus, otherIncome.IsMainSalary, otherIncome.IsDetailSalary, false);
                    otherIncome.SalaryItemId = salaryItem.Id;
                }
                else
                {
                    salaryItem.Code = otherIncome.Code;
                    salaryItem.Name = otherIncome.Name;
                    salaryItem.SalaryItemStatus = otherIncome.SalaryStatus;
                    _salaryItemService.UpdateObject(salaryItem);
                    if (salaryItem.Errors.Any())
                    {
                        otherIncome.Errors.Clear();
                        otherIncome.Errors.Add("Code", "Tidak dapat mengubah SalaryItem dengan Code ini");
                    }
                }
                _repository.UpdateObject(otherIncome);
            }
            return otherIncome;
        }

        public OtherIncome SoftDeleteObject(OtherIncome otherIncome, ISalaryItemService _salaryItemService)
        {
            if(_validator.ValidDeleteObject(otherIncome))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectById(otherIncome.SalaryItemId.GetValueOrDefault());
                _repository.SoftDeleteObject(otherIncome);
                if (salaryItem != null)
                {
                    _salaryItemService.SoftDeleteObject(salaryItem);
                }
            }
            return otherIncome;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(OtherIncome otherIncome)
        {
            IQueryable<OtherIncome> otherIncomes = _repository.FindAll(x => x.Code == otherIncome.Code && !x.IsDeleted && x.Id != otherIncome.Id);
            return (otherIncomes.Count() > 0 ? true : false);
        }

    }
}