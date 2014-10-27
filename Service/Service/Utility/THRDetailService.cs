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
    public class THRDetailService : ITHRDetailService
    {
        private ITHRDetailRepository _repository;
        private ITHRDetailValidator _validator;
        public THRDetailService(ITHRDetailRepository _thrDetailRepository, ITHRDetailValidator _thrDetailValidator)
        {
            _repository = _thrDetailRepository;
            _validator = _thrDetailValidator;
        }

        public ITHRDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<THRDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<THRDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public THRDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public THRDetail GetObjectByEmployeeIdAndSalaryItemId(int employeeId, int salaryItemId, DateTime date)
        {
            return _repository.GetQueryable().Include("THR").Where(x => x.EmployeeId == employeeId && x.THR.SalaryItemId == salaryItemId && x.THR.EffectiveDate <= date).OrderByDescending(x => x.THR.EffectiveDate).FirstOrDefault();
        }

        public THRDetail GetObjectByEmployeeIdAndTHRId(int employeeId, int THRId)
        {
            return _repository.GetQueryable().Include("THR").Where(x => x.EmployeeId == employeeId && x.THRId == THRId).FirstOrDefault();
        }

        public THRDetail CreateObject(THRDetail thrDetail, ITHRService _thrService, IEmployeeService _employeeService)
        {
            thrDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(thrDetail, _thrService, _employeeService, this) ? _repository.CreateObject(thrDetail) : thrDetail);
        }

        public THRDetail UpdateObject(THRDetail thrDetail, ITHRService _thrService, IEmployeeService _employeeService)
        {
            return (thrDetail = _validator.ValidUpdateObject(thrDetail, _thrService, _employeeService, this) ? _repository.UpdateObject(thrDetail) : thrDetail);
        }

        public THRDetail SoftDeleteObject(THRDetail thrDetail)
        {
            return (thrDetail = _validator.ValidDeleteObject(thrDetail) ?
                    _repository.SoftDeleteObject(thrDetail) : thrDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}