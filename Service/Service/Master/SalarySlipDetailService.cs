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
    public class SalarySlipDetailService : ISalarySlipDetailService
    {
        private ISalarySlipDetailRepository _repository;
        private ISalarySlipDetailValidator _validator;
        public SalarySlipDetailService(ISalarySlipDetailRepository _salarySlipDetailRepository, ISalarySlipDetailValidator _salarySlipDetailValidator)
        {
            _repository = _salarySlipDetailRepository;
            _validator = _salarySlipDetailValidator;
        }

        public ISalarySlipDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalarySlipDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalarySlipDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public SalarySlipDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalarySlipDetail CreateObject(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService)
        {
            salarySlipDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salarySlipDetail, _salarySlipService, _salaryItemService) ? _repository.CreateObject(salarySlipDetail) : salarySlipDetail);
        }

        public SalarySlipDetail UpdateObject(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService)
        {
            return (salarySlipDetail = _validator.ValidUpdateObject(salarySlipDetail, _salarySlipService, _salaryItemService) ? _repository.UpdateObject(salarySlipDetail) : salarySlipDetail);
        }

        public SalarySlipDetail SoftDeleteObject(SalarySlipDetail salarySlipDetail)
        {
            return (salarySlipDetail = _validator.ValidDeleteObject(salarySlipDetail) ?
                    _repository.SoftDeleteObject(salarySlipDetail) : salarySlipDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}