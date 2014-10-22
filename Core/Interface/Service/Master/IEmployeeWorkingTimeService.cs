using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEmployeeWorkingTimeService
    {
        IEmployeeWorkingTimeValidator GetValidator();
        IQueryable<EmployeeWorkingTime> GetQueryable();
        IList<EmployeeWorkingTime> GetAll();
        IList<EmployeeWorkingTime> GetObjectsByWorkingTimeId(int WorkingTimeId);
        EmployeeWorkingTime GetObjectById(int Id);
        EmployeeWorkingTime CreateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService, IEmployeeService _employeeService);
        EmployeeWorkingTime UpdateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService, IEmployeeService _employeeService);
        EmployeeWorkingTime SoftDeleteObject(EmployeeWorkingTime employeeWorkingTime, IEmployeeService _employeeService);
        bool DeleteObject(int Id);
    }
}