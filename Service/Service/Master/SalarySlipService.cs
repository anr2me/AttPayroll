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

        public SalarySlip CreateObject(string Code, string Name, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService)
        {
            SalarySlip salarySlip = new SalarySlip
            {
                Code = Code,
                Name = Name,
            };
            return this.CreateObject(salarySlip, _salarySlipService, _salaryItemService);
        }

        public SalarySlip CreateObject(SalarySlip salarySlip, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService)
        {
            salarySlip.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salarySlip, _salarySlipService, _salaryItemService) ? _repository.CreateObject(salarySlip) : salarySlip);
        }

        public SalarySlip UpdateObject(SalarySlip salarySlip, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService)
        {
            return (salarySlip = _validator.ValidUpdateObject(salarySlip, _salarySlipService, _salaryItemService) ? _repository.UpdateObject(salarySlip) : salarySlip);
        }

        public SalarySlip SoftDeleteObject(SalarySlip salarySlip)
        {
            return (salarySlip = _validator.ValidDeleteObject(salarySlip) ?
                    _repository.SoftDeleteObject(salarySlip) : salarySlip);
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