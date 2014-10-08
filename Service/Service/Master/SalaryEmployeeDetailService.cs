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

        public SalaryEmployeeDetail GetObjectByEmployeeIdAndSalaryItemId(int employeeId, int salaryItemId, DateTime date)
        {
            return _repository.GetQueryable().Include("SalaryEmployee").Where(x => x.SalaryEmployee.EmployeeId == employeeId && x.SalaryItemId == salaryItemId && x.SalaryEmployee.EffectiveDate <= date).OrderByDescending(x => x.SalaryEmployee.EffectiveDate).FirstOrDefault();
        }

        public SalaryEmployeeDetail CreateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeService _salaryEmployeeService, ISalaryItemService _salaryItemService)
        {
            salaryEmployeeDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salaryEmployeeDetail, _salaryEmployeeService, _salaryItemService) ? _repository.CreateObject(salaryEmployeeDetail) : salaryEmployeeDetail);
        }

        public SalaryEmployeeDetail UpdateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeService _salaryEmployeeService, ISalaryItemService _salaryItemService)
        {
            return (salaryEmployeeDetail = _validator.ValidUpdateObject(salaryEmployeeDetail, _salaryEmployeeService, _salaryItemService) ? _repository.UpdateObject(salaryEmployeeDetail) : salaryEmployeeDetail);
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