using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEmployeeLoanService
    {
        IEmployeeLoanValidator GetValidator();
        IQueryable<EmployeeLoan> GetQueryable();
        IList<EmployeeLoan> GetAll();
        EmployeeLoan GetObjectById(int Id);
        EmployeeLoan CreateObject(EmployeeLoan employeeLoan, IEmployeeService _employeeService, ISalaryItemService _salaryItemService);
        EmployeeLoan UpdateObject(EmployeeLoan employeeLoan, IEmployeeService _employeeService, ISalaryItemService _salaryItemService);
        EmployeeLoan SoftDeleteObject(EmployeeLoan employeeLoan);
        bool DeleteObject(int Id);
    }
}