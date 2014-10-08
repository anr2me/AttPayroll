using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IEmployeeLeaveValidator
    {

        bool ValidCreateObject(EmployeeLeave employeeLeave, IEmployeeService _employeeService);
        bool ValidUpdateObject(EmployeeLeave employeeLeave, IEmployeeService _employeeService);
        bool ValidDeleteObject(EmployeeLeave employeeLeave);
        bool isValid(EmployeeLeave employeeLeave);
        string PrintError(EmployeeLeave employeeLeave);
    }
}