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
    public class OtherExpenseDetailService : IOtherExpenseDetailService
    {
        private IOtherExpenseDetailRepository _repository;
        private IOtherExpenseDetailValidator _validator;
        public OtherExpenseDetailService(IOtherExpenseDetailRepository _otherExpenseDetailRepository, IOtherExpenseDetailValidator _otherExpenseDetailValidator)
        {
            _repository = _otherExpenseDetailRepository;
            _validator = _otherExpenseDetailValidator;
        }

        public IOtherExpenseDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<OtherExpenseDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<OtherExpenseDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public OtherExpenseDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public OtherExpenseDetail GetObjectByEmployeeIdAndSalaryItemId(int employeeId, int salaryItemId, DateTime date)
        {
            return _repository.GetQueryable().Include("OtherExpense").Where(x => x.EmployeeId == employeeId && x.OtherExpense.SalaryItemId == salaryItemId && x.EffectiveDate <= date).OrderByDescending(x => x.EffectiveDate).FirstOrDefault();
        }

        public OtherExpenseDetail CreateObject(OtherExpenseDetail otherExpenseDetail, IOtherExpenseService _otherExpenseService, IEmployeeService _employeeService)
        {
            otherExpenseDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(otherExpenseDetail, _otherExpenseService, _employeeService) ? _repository.CreateObject(otherExpenseDetail) : otherExpenseDetail);
        }

        public OtherExpenseDetail UpdateObject(OtherExpenseDetail otherExpenseDetail, IOtherExpenseService _otherExpenseService, IEmployeeService _employeeService)
        {
            return (otherExpenseDetail = _validator.ValidUpdateObject(otherExpenseDetail, _otherExpenseService, _employeeService) ? _repository.UpdateObject(otherExpenseDetail) : otherExpenseDetail);
        }

        public OtherExpenseDetail SoftDeleteObject(OtherExpenseDetail otherExpenseDetail)
        {
            return (otherExpenseDetail = _validator.ValidDeleteObject(otherExpenseDetail) ?
                    _repository.SoftDeleteObject(otherExpenseDetail) : otherExpenseDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}