using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IEmployeeAttendanceRepository : IRepository<EmployeeAttendance>
    {
        IQueryable<EmployeeAttendance> GetQueryable();
        IList<EmployeeAttendance> GetAll();
        EmployeeAttendance GetObjectById(int Id);
        EmployeeAttendance CreateObject(EmployeeAttendance employeeAttendance);
        EmployeeAttendance UpdateObject(EmployeeAttendance employeeAttendance);
        EmployeeAttendance SoftDeleteObject(EmployeeAttendance employeeAttendance);
        bool DeleteObject(int Id);
    }
}