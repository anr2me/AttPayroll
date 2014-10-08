using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEmployeeAttendanceService
    {
        IEmployeeAttendanceValidator GetValidator();
        IQueryable<EmployeeAttendance> GetQueryable();
        IList<EmployeeAttendance> GetAll();
        EmployeeAttendance GetObjectById(int Id);
        EmployeeAttendance CreateObject(EmployeeAttendance employeeAttendance, IEmployeeService _employeeService);
        EmployeeAttendance UpdateObject(EmployeeAttendance employeeAttendance, IEmployeeService _employeeService);
        EmployeeAttendance SoftDeleteObject(EmployeeAttendance employeeAttendance);
        bool DeleteObject(int Id);
    }
}