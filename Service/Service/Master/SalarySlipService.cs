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
    public class SalarySlipService : ISalarySlipService
    {
        private ISalarySlipRepository _repository;
        private ISalarySlipValidator _validator;
        public SalarySlipService(ISalarySlipRepository _salarySlipRepository, ISalarySlipValidator _salarySlipValidator)
        {
            _repository = _salarySlipRepository;
            _validator = _salarySlipValidator;
        }

        public ISalarySlipValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalarySlip> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalarySlip> GetAll()
        {
            return _repository.GetAll();
        }

        public SalarySlip GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalarySlip GetObjectByCode(string Code)
        {
            return _repository.FindAll(x => x.Code == Code && !x.IsDeleted).FirstOrDefault();
        }

        public SalarySlip CreateObject(string Code, string Name, int SalarySign, int SalaryStatus, bool IsMainSalary, bool IsDetailSalary,
                                bool IsEnabled, bool IsPTKP, bool IsPPH21, ISalaryItemService _salaryItemService)
        {
            SalarySlip salarySlip = new SalarySlip
            {
                Code = Code,
                Name = Name,
                SalarySign = SalarySign,
                IsMainSalary = IsMainSalary,
                IsDetailSalary = IsDetailSalary,
                IsEnabled = IsEnabled,
                IsPTKP = IsPTKP,
                IsPPH21 = IsPPH21,
            };
            SalaryItem salaryItem = _salaryItemService.GetObjectByCode(Code);
            if (salaryItem != null)
            {
                salarySlip.Errors = new Dictionary<string, string>();
                salarySlip.Errors.Add("Code", "SalaryItem dengan Code ini sudah ada");
                return salarySlip;
            }
            salaryItem = _salaryItemService.CreateObject(Code, Name, SalarySign, (int)Constant.SalaryItemType.SalarySlip, SalaryStatus, IsMainSalary, IsDetailSalary, false);
            if (salaryItem == null)
            {
                salarySlip.Errors = new Dictionary<string, string>();
                salarySlip.Errors.Add("Code", "Tidak dapat membuat SalaryItem dengan Code ini");
                return salarySlip;
            }
            salarySlip.SalaryItemId = salaryItem.Id;
            return this.CreateObject(salarySlip, _salaryItemService);
        }

        public SalarySlip CreateObject(SalarySlip salarySlip, ISalaryItemService _salaryItemService)
        {
            salarySlip.Errors = new Dictionary<String, String>();
            SalaryItem salaryItem = _salaryItemService.GetObjectById(salarySlip.SalaryItemId.GetValueOrDefault());
            if (salaryItem == null)
            {
                salaryItem = _salaryItemService.GetObjectByCode(salarySlip.Code);
                if (salaryItem != null)
                {
                    salarySlip.Errors = new Dictionary<string, string>();
                    salarySlip.Errors.Add("Code", "SalaryItem dengan Code ini sudah ada");
                    return salarySlip;
                }
                salaryItem = _salaryItemService.CreateObject(salarySlip.Code, salarySlip.Name, salarySlip.SalarySign, (int)Constant.SalaryItemType.SalarySlip, (int)Constant.SalaryItemStatus.Monthly, salarySlip.IsMainSalary, salarySlip.IsDetailSalary, false);
                if (salaryItem == null)
                {
                    salarySlip.Errors = new Dictionary<string, string>();
                    salarySlip.Errors.Add("Code", "Tidak dapat membuat SalaryItem dengan Code ini");
                    return salarySlip;
                }
                salarySlip.SalaryItemId = salaryItem.Id;
            }
            if(_validator.ValidCreateObject(salarySlip, this, _salaryItemService)) 
            {
                salarySlip.Index = GetQueryable().Count() + 1;
                _repository.CreateObject(salarySlip);
            };
            return salarySlip;
        }

        public SalarySlip UpdateObject(SalarySlip salarySlip, ISalaryItemService _salaryItemService)
        {
            if(_validator.ValidUpdateObject(salarySlip, this, _salaryItemService))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectById(salarySlip.SalaryItemId.GetValueOrDefault());
                if (salaryItem == null)
                {
                    salaryItem = _salaryItemService.CreateObject(salarySlip.Code, salarySlip.Name, salarySlip.SalarySign, (int)Constant.SalaryItemType.SalarySlip, (int)Constant.SalaryItemStatus.Monthly, salarySlip.IsMainSalary, salarySlip.IsDetailSalary, false);
                    salarySlip.SalaryItemId = salaryItem.Id;
                }
                else
                {
                    salaryItem.Code = salarySlip.Code;
                    salaryItem.Name = salarySlip.Name;
                    _salaryItemService.UpdateObject(salaryItem);
                    if (salaryItem.Errors.Any())
                    {
                        salarySlip.Errors.Clear();
                        salarySlip.Errors.Add("Code", "Tidak dapat mengubah SalaryItem dengan Code ini");
                    }
                }
                _repository.UpdateObject(salarySlip);
            }
            return salarySlip;
        }

        public SalarySlip SoftDeleteObject(SalarySlip salarySlip, ISalaryItemService _salaryItemService)
        {
            if(_validator.ValidDeleteObject(salarySlip))
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectById(salarySlip.SalaryItemId.GetValueOrDefault());
                _repository.SoftDeleteObject(salarySlip);
                if (salaryItem != null)
                {
                    _salaryItemService.SoftDeleteObject(salaryItem);
                }
            }
            return salarySlip;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(SalarySlip salarySlip)
        {
            IQueryable<SalarySlip> salarySlips = _repository.FindAll(x => x.Code == salarySlip.Code && !x.IsDeleted && x.Id != salarySlip.Id);
            return (salarySlips.Count() > 0 ? true : false);
        }
    }
}