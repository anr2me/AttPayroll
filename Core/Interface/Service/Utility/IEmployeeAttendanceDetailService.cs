using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEmployeeAttendanceDetailService
    {
        IEmployeeAttendanceDetailValidator GetValidator();
        IQueryable<EmployeeAttendanceDetail> GetQueryable();
        IList<EmployeeAttendanceDetail> GetAll();
        EmployeeAttendanceDetail GetObjectById(int Id);
        EmployeeAttendanceDetail GetObjectByEmployeeIdAndSalaryItemId(int employeeId, int salaryItemId, DateTime date);
        EmployeeAttendanceDetail CreateObject(EmployeeAttendanceDetail employeeAttendanceDetail, IEmployeeAttendanceService _employeeAttendanceService, ISalaryItemService _salaryItemService);
        EmployeeAttendanceDetail UpdateObject(EmployeeAttendanceDetail employeeAttendanceDetail, IEmployeeAttendanceService _employeeAttendanceService, ISalaryItemService _salaryItemService);
        EmployeeAttendanceDetail SoftDeleteObject(EmployeeAttendanceDetail employeeAttendanceDetail);
        bool DeleteObject(int Id);
    }
}