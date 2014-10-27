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
    public class THRService : ITHRService
    {
        private ITHRRepository _repository;
        private ITHRValidator _validator;
        public THRService(ITHRRepository _thrRepository, ITHRValidator _thrValidator)
        {
            _repository = _thrRepository;
            _validator = _thrValidator;
        }

        public ITHRValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<THR> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<THR> GetAll()
        {
            return _repository.GetAll();
        }

        public THR GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public THR CreateObject(THR thr, ISalaryItemService _salaryItemService)
        {
            thr.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(thr, this))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectByCode(thr.Code);
                if (salaryItem != null)
                {
                    thr.Errors = new Dictionary<string, string>();
                    thr.Errors.Add("Code", "SalaryItem dengan Code ini sudah ada");
                    return thr;
                }
                salaryItem = _salaryItemService.CreateObject(thr.Code, thr.Name, (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.THR, (int)Constant.SalaryItemStatus.Monthly, thr.IsMainSalary, thr.IsDetailSalary, false);
                if (salaryItem == null)
                {
                    thr.Errors = new Dictionary<string, string>();
                    thr.Errors.Add("Code", "Tidak dapat membuat SalaryItem dengan Code ini");
                    return thr;
                }
                thr.SalaryItemId = salaryItem.Id;
                _repository.CreateObject(thr);
            }
            return thr;
        }

        public THR UpdateObject(THR thr, ISalaryItemService _salaryItemService)
        {
            if(_validator.ValidUpdateObject(thr, this))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectById(thr.SalaryItemId.GetValueOrDefault());
                if (salaryItem == null)
                {
                    salaryItem = _salaryItemService.CreateObject(thr.Code, thr.Name, (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.SalarySlip, (int)Constant.SalaryItemStatus.Monthly, thr.IsMainSalary, thr.IsDetailSalary, false);
                    thr.SalaryItemId = salaryItem.Id;
                }
                else
                {
                    salaryItem.Code = thr.Code;
                    salaryItem.Name = thr.Name;
                    _salaryItemService.UpdateObject(salaryItem);
                    if (salaryItem.Errors.Any())
                    {
                        thr.Errors.Clear();
                        thr.Errors.Add("Code", "Tidak dapat mengubah SalaryItem dengan Code ini");
                    }
                }
                _repository.UpdateObject(thr);
            }
            return thr;
        }

        public THR SoftDeleteObject(THR thr, ISalaryItemService _salaryItemService)
        {
            if(_validator.ValidDeleteObject(thr))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectById(thr.SalaryItemId.GetValueOrDefault());
                _repository.SoftDeleteObject(thr);
                if (salaryItem != null)
                {
                    _salaryItemService.SoftDeleteObject(salaryItem);
                }
            }
            return thr;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(THR thr)
        {
            IQueryable<THR> thrs = _repository.FindAll(x => x.Code == thr.Code && !x.IsDeleted && x.Id != thr.Id);
            return (thrs.Count() > 0 ? true : false);
        }

    }
}