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
    public class EmployeeAttendanceDetailService : IEmployeeAttendanceDetailService
    {
        private IEmployeeAttendanceDetailRepository _repository;
        private IEmployeeAttendanceDetailValidator _validator;
        public EmployeeAttendanceDetailService(IEmployeeAttendanceDetailRepository _employeeAttendanceDetailRepository, IEmployeeAttendanceDetailValidator _employeeAttendanceDetailValidator)
        {
            _repository = _employeeAttendanceDetailRepository;
            _validator = _employeeAttendanceDetailValidator;
        }

        public IEmployeeAttendanceDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<EmployeeAttendanceDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<EmployeeAttendanceDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public EmployeeAttendanceDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public EmployeeAttendanceDetail GetObjectByEmployeeIdAndSalaryItemId(int employeeId, int salaryItemId, DateTime date)
        {
            return _repository.GetQueryable().Include("EmployeeAttendance").Where(x => x.EmployeeAttendance.EmployeeId == employeeId && x.SalaryItemId == salaryItemId && x.EmployeeAttendance.AttendanceDate == date).FirstOrDefault();
        }

        public EmployeeAttendanceDetail CreateObject(EmployeeAttendanceDetail employeeAttendanceDetail, IEmployeeAttendanceService _employeeAttendanceService, ISalaryItemService _salaryItemService)
        {
            employeeAttendanceDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(employeeAttendanceDetail, _employeeAttendanceService, _salaryItemService) ? _repository.CreateObject(employeeAttendanceDetail) : employeeAttendanceDetail);
        }

        public EmployeeAttendanceDetail UpdateObject(EmployeeAttendanceDetail employeeAttendanceDetail, IEmployeeAttendanceService _employeeAttendanceService, ISalaryItemService _salaryItemService)
        {
            return (employeeAttendanceDetail = _validator.ValidUpdateObject(employeeAttendanceDetail, _employeeAttendanceService, _salaryItemService) ? _repository.UpdateObject(employeeAttendanceDetail) : employeeAttendanceDetail);
        }

        public EmployeeAttendanceDetail SoftDeleteObject(EmployeeAttendanceDetail employeeAttendanceDetail)
        {
            return (employeeAttendanceDetail = _validator.ValidDeleteObject(employeeAttendanceDetail) ?
                    _repository.SoftDeleteObject(employeeAttendanceDetail) : employeeAttendanceDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}