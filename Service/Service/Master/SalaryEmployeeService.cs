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
    public class SalaryEmployeeService : ISalaryEmployeeService
    {
        private ISalaryEmployeeRepository _repository;
        private ISalaryEmployeeValidator _validator;
        public SalaryEmployeeService(ISalaryEmployeeRepository _salaryEmployeeRepository, ISalaryEmployeeValidator _salaryEmployeeValidator)
        {
            _repository = _salaryEmployeeRepository;
            _validator = _salaryEmployeeValidator;
        }

        public ISalaryEmployeeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalaryEmployee> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalaryEmployee> GetAll()
        {
            return _repository.GetAll();
        }

        public SalaryEmployee GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalaryEmployee CreateObject(SalaryEmployee salaryEmployee, IEmployeeService _employeeService, 
                                    ISalaryEmployeeDetailService _salaryEmployeeDetailService, ISalaryItemService _salaryItemService, 
                                    ISalaryStandardDetailService _salaryStandardDetailService)
        {
            salaryEmployee.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(salaryEmployee, _employeeService)) 
            {
                _repository.CreateObject(salaryEmployee);
                if (!salaryEmployee.Errors.Any())
                {
                    Employee employee = _employeeService.GetObjectById(salaryEmployee.EmployeeId);
                    IList<SalaryStandardDetail> details = _salaryStandardDetailService.GetObjectsByTitleInfoId(employee.TitleInfoId);
                    foreach (var detail in details)
                    {
                        SalaryEmployeeDetail sed = new SalaryEmployeeDetail
                        {
                            SalaryEmployeeId = salaryEmployee.Id,
                            SalaryItemId = detail.SalaryItemId,
                            Amount = detail.Amount,
                        };
                        _salaryEmployeeDetailService.CreateObject(sed, this, _salaryItemService);
                    };
                }
            }
            return salaryEmployee;
        }

        public SalaryEmployee UpdateObject(SalaryEmployee salaryEmployee, IEmployeeService _employeeService)
        {
            if(_validator.ValidUpdateObject(salaryEmployee, _employeeService))
            {

                _repository.UpdateObject(salaryEmployee);
            }
            return salaryEmployee;
        }

        public SalaryEmployee SoftDeleteObject(SalaryEmployee salaryEmployee)
        {
            return (salaryEmployee = _validator.ValidDeleteObject(salaryEmployee) ?
                    _repository.SoftDeleteObject(salaryEmployee) : salaryEmployee);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}