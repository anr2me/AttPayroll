using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IEmployeeAttendanceDetailValidator
    {

        bool ValidCreateObject(EmployeeAttendanceDetail employeeAttendanceDetail, IEmployeeAttendanceService _employeeAttendanceService, ISalaryItemService _salaryItemService);
        bool ValidUpdateObject(EmployeeAttendanceDetail employeeAttendanceDetail, IEmployeeAttendanceService _employeeAttendanceService, ISalaryItemService _salaryItemService);
        bool ValidDeleteObject(EmployeeAttendanceDetail employeeAttendanceDetail);
        bool isValid(EmployeeAttendanceDetail employeeAttendanceDetail);
        string PrintError(EmployeeAttendanceDetail employeeAttendanceDetail);
    }
}