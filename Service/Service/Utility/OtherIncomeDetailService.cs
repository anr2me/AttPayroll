using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace Service.Service
{
    public class OtherIncomeDetailService : IOtherIncomeDetailService
    {
        private IOtherIncomeDetailRepository _repository;
        private IOtherIncomeDetailValidator _validator;
        public OtherIncomeDetailService(IOtherIncomeDetailRepository _otherIncomeDetailRepository, IOtherIncomeDetailValidator _otherIncomeDetailValidator)
        {
            _repository = _otherIncomeDetailRepository;
            _validator = _otherIncomeDetailValidator;
        }

        public IOtherIncomeDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<OtherIncomeDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<OtherIncomeDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public OtherIncomeDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public OtherIncomeDetail GetObjectByEmployeeIdAndSalaryItemId(int employeeId, int salaryItemId, DateTime date)
        {
            return _repository.GetQueryable().Include("OtherIncome").Where(x => x.EmployeeId == employeeId && x.OtherIncome.SalaryItemId == salaryItemId && x.EffectiveDate <= date).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
        }

        public OtherIncomeDetail CreateObject(OtherIncomeDetail otherIncomeDetail, IOtherIncomeService _otherIncomeService, IEmployeeService _employeeService)
        {
            otherIncomeDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(otherIncomeDetail, _otherIncomeService, _employeeService) ? _repository.CreateObject(otherIncomeDetail) : otherIncomeDetail);
        }

        public OtherIncomeDetail UpdateObject(OtherIncomeDetail otherIncomeDetail, IOtherIncomeService _otherIncomeService, IEmployeeService _employeeService)
        {
            return (otherIncomeDetail = _validator.ValidUpdateObject(otherIncomeDetail, _otherIncomeService, _employeeService) ? _repository.UpdateObject(otherIncomeDetail) : otherIncomeDetail);
        }

        public OtherIncomeDetail SoftDeleteObject(OtherIncomeDetail otherIncomeDetail)
        {
            return (otherIncomeDetail = _validator.ValidDeleteObject(otherIncomeDetail) ?
                    _repository.SoftDeleteObject(otherIncomeDetail) : otherIncomeDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}