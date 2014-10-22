using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IEmployeeWorkingTimeValidator
    {

        bool ValidCreateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService, IEmployeeService _employeeService);
        bool ValidUpdateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService, IEmployeeService _employeeService);
        bool ValidDeleteObject(EmployeeWorkingTime employeeWorkingTime, IEmployeeService _employeeService);
        bool isValid(EmployeeWorkingTime employeeWorkingTime);
        string PrintError(EmployeeWorkingTime employeeWorkingTime);
    }
}