using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IEmployeeLoanValidator
    {

        bool ValidCreateObject(EmployeeLoan employeeLoan, IEmployeeService _employeeService, ISalaryItemService _salaryItemService);
        bool ValidUpdateObject(EmployeeLoan employeeLoan, IEmployeeService _employeeService, ISalaryItemService _salaryItemService);
        bool ValidDeleteObject(EmployeeLoan employeeLoan);
        bool isValid(EmployeeLoan employeeLoan);
        string PrintError(EmployeeLoan employeeLoan);
    }
}