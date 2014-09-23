using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEmployeeService
    {
        IEmployeeValidator GetValidator();
        IQueryable<Employee> GetQueryable();
        IList<Employee> GetAll();
        Employee GetObjectById(int Id);
        Employee GetObjectByNIK(string NIK);
        Employee CreateObject(Employee employee);
        Employee UpdateObject(Employee employee);
        Employee SoftDeleteObject(Employee employee);
        bool DeleteObject(int Id);
        bool IsNIKDuplicated(Employee employee);
    }
}