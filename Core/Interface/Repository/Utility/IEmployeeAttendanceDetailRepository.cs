using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IEmployeeAttendanceDetailRepository : IRepository<EmployeeAttendanceDetail>
    {
        IQueryable<EmployeeAttendanceDetail> GetQueryable();
        IList<EmployeeAttendanceDetail> GetAll();
        EmployeeAttendanceDetail GetObjectById(int Id);
        EmployeeAttendanceDetail CreateObject(EmployeeAttendanceDetail employeeAttendanceDetail);
        EmployeeAttendanceDetail UpdateObject(EmployeeAttendanceDetail employeeAttendanceDetail);
        EmployeeAttendanceDetail SoftDeleteObject(EmployeeAttendanceDetail employeeAttendanceDetail);
        bool DeleteObject(int Id);
    }
}