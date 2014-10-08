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
    public class EmployeeLoanDetailService : IEmployeeLoanDetailService
    {
        private IEmployeeLoanDetailRepository _repository;
        private IEmployeeLoanDetailValidator _validator;
        public EmployeeLoanDetailService(IEmployeeLoanDetailRepository _employeeLoanDetailRepository, IEmployeeLoanDetailValidator _employeeLoanDetailValidator)
        {
            _repository = _employeeLoanDetailRepository;
            _validator = _employeeLoanDetailValidator;
        }

        public IEmployeeLoanDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<EmployeeLoanDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<EmployeeLoanDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public EmployeeLoanDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public EmployeeLoanDetail CreateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanService _employeeLoanService)
        {
            employeeLoanDetail.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(employeeLoanDetail, _employeeLoanService) ? _repository.CreateObject(employeeLoanDetail) : employeeLoanDetail);
        }

        public EmployeeLoanDetail UpdateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanService _employeeLoanService)
        {
            return (employeeLoanDetail = _validator.ValidUpdateObject(employeeLoanDetail, _employeeLoanService) ? _repository.UpdateObject(employeeLoanDetail) : employeeLoanDetail);
        }

        public EmployeeLoanDetail SoftDeleteObject(EmployeeLoanDetail employeeLoanDetail)
        {
            return (employeeLoanDetail = _validator.ValidDeleteObject(employeeLoanDetail) ?
                    _repository.SoftDeleteObject(employeeLoanDetail) : employeeLoanDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}