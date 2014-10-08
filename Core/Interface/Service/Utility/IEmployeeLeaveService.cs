using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEmployeeLeaveService
    {
        IEmployeeLeaveValidator GetValidator();
        IQueryable<EmployeeLeave> GetQueryable();
        IList<EmployeeLeave> GetAll();
        EmployeeLeave GetObjectById(int Id);
        EmployeeLeave CreateObject(EmployeeLeave employeeLeave, IEmployeeService _employeeService);
        EmployeeLeave UpdateObject(EmployeeLeave employeeLeave, IEmployeeService _employeeService);
        EmployeeLeave SoftDeleteObject(EmployeeLeave employeeLeave);
        bool DeleteObject(int Id);
    }
}