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
    public class SalaryEmployeeDetailService : ISalaryEmployeeDetailService
    {
        private ISalaryEmployeeDetailRepository _repository;
        private ISalaryEmployeeDetailValidator _validator;
        public SalaryEmployeeDetailService(ISalaryEmployeeDetailRepository _salaryEmployeeDetailRepository, ISalaryEmployeeDetailValidator _salaryEmployeeDetailValidator)
        {
            _repository = _salaryEmployeeDetailRepository;
            _validator = _salaryEmployeeDetailValidator;
        }

        public ISalaryEmployeeDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalaryEmployeeDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalaryEmployeeDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public SalaryEmployeeDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalaryEmployeeDetail CreateObject(SalaryEmployeeDetail salaryEmployeeDetail)
        {
            salaryEmployeeDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salaryEmployeeDetail, this) ? _repository.CreateObject(salaryEmployeeDetail) : salaryEmployeeDetail);
        }

        public SalaryEmployeeDetail UpdateObject(SalaryEmployeeDetail salaryEmployeeDetail)
        {
            return (salaryEmployeeDetail = _validator.ValidUpdateObject(salaryEmployeeDetail, this) ? _repository.UpdateObject(salaryEmployeeDetail) : salaryEmployeeDetail);
        }

        public SalaryEmployeeDetail SoftDeleteObject(SalaryEmployeeDetail salaryEmployeeDetail)
        {
            return (salaryEmployeeDetail = _validator.ValidDeleteObject(salaryEmployeeDetail) ?
                    _repository.SoftDeleteObject(salaryEmployeeDetail) : salaryEmployeeDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}