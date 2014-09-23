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
        EmployeeLeave VCreateObject(EmployeeLeave employeeLeave, IEmployeeLeaveService _employeeLeaveService);
        EmployeeLeave VUpdateObject(EmployeeLeave employeeLeave, IEmployeeLeaveService _employeeLeaveService);
        EmployeeLeave VDeleteObject(EmployeeLeave employeeLeave);
        bool ValidCreateObject(EmployeeLeave employeeLeave, IEmployeeLeaveService _employeeLeaveService);
        bool ValidUpdateObject(EmployeeLeave employeeLeave, IEmployeeLeaveService _employeeLeaveService);
        bool ValidDeleteObject(EmployeeLeave employeeLeave);
        bool isValid(EmployeeLeave employeeLeave);
        string PrintError(EmployeeLeave employeeLeave);
    }
}