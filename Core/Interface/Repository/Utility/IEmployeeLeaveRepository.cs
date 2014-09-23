using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IEmployeeLeaveRepository : IRepository<EmployeeLeave>
    {
        IQueryable<EmployeeLeave> GetQueryable();
        IList<EmployeeLeave> GetAll();
        EmployeeLeave GetObjectById(int Id);
        EmployeeLeave CreateObject(EmployeeLeave employeeLeave);
        EmployeeLeave UpdateObject(EmployeeLeave employeeLeave);
        EmployeeLeave SoftDeleteObject(EmployeeLeave employeeLeave);
        bool DeleteObject(int Id);
    }
}