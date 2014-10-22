using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IDivisionService
    {
        IDivisionValidator GetValidator();
        IQueryable<Division> GetQueryable();
        IList<Division> GetAll();
        IList<Division> GetObjectsByDepartmentId(int DepartmentId);
        Division GetObjectById(int Id);
        Division GetObjectByName(string Name);
        Division CreateObject(Division division, IDepartmentService _departmentService);
        Division CreateObject(int departmentId, string Code, string Name, string Description, IDepartmentService _departmentService);
        Division UpdateObject(Division division, IDepartmentService _departmentService);
        Division SoftDeleteObject(Division division, IEmployeeService _employeeService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(Division division);
        bool IsNameDuplicated(Division division);
    }
}