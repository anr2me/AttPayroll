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
        EmployeeWorkingTime GetObjectById(int Id);
        EmployeeWorkingTime CreateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService);
        EmployeeWorkingTime UpdateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService);
        EmployeeWorkingTime SoftDeleteObject(EmployeeWorkingTime employeeWorkingTime);
        bool DeleteObject(int Id);
    }
}