using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalaryEmployeeService
    {
        ISalaryEmployeeValidator GetValidator();
        IQueryable<SalaryEmployee> GetQueryable();
        IList<SalaryEmployee> GetAll();
        SalaryEmployee GetObjectById(int Id);
        SalaryEmployee CreateObject(SalaryEmployee salaryEmployee, IEmployeeService _employeeService,
                            ISalaryEmployeeDetailService _salaryEmployeeDetailService, ISalaryItemService _salaryItemService,
                            ISalaryStandardDetailService _salaryStandardDetailService);
        SalaryEmployee UpdateObject(SalaryEmployee salaryEmployee, IEmployeeService _employeeService);
        SalaryEmployee SoftDeleteObject(SalaryEmployee salaryEmployee);
        bool DeleteObject(int Id);
    }
}