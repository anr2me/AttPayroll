using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IEmployeeAttendanceValidator
    {

        bool ValidCreateObject(EmployeeAttendance employeeAttendance, IEmployeeService _employeeService, IEmployeeAttendanceService _employeeAttendanceService);
        bool ValidUpdateObject(EmployeeAttendance employeeAttendance, IEmployeeService _employeeService, IEmployeeAttendanceService _employeeAttendanceService);
        bool ValidDeleteObject(EmployeeAttendance employeeAttendance);
        bool isValid(EmployeeAttendance employeeAttendance);
        string PrintError(EmployeeAttendance employeeAttendance);
    }
}