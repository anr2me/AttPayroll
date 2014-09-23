using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IEmployeeValidator
    {
        Employee VHasUniqueNIK(Employee employee, IEmployeeService _employeeService);
        Employee VCreateObject(Employee employee, IEmployeeService _employeeService);
        Employee VUpdateObject(Employee employee, IEmployeeService _employeeService);
        Employee VDeleteObject(Employee employee);
        bool ValidCreateObject(Employee employee, IEmployeeService _employeeService);
        bool ValidUpdateObject(Employee employee, IEmployeeService _employeeService);
        bool ValidDeleteObject(Employee employee);
        bool isValid(Employee employee);
        string PrintError(Employee employee);
    }
}