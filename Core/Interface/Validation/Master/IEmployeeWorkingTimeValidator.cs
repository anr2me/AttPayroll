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

        bool ValidCreateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService);
        bool ValidUpdateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService);
        bool ValidDeleteObject(EmployeeWorkingTime employeeWorkingTime);
        bool isValid(EmployeeWorkingTime employeeWorkingTime);
        string PrintError(EmployeeWorkingTime employeeWorkingTime);
    }
}