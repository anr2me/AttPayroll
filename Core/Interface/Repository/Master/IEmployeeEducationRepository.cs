using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IEmployeeEducationRepository : IRepository<EmployeeEducation>
    {
        IQueryable<EmployeeEducation> GetQueryable();
        IList<EmployeeEducation> GetAll();
        EmployeeEducation GetObjectById(int Id);
        EmployeeEducation CreateObject(EmployeeEducation employeeEducation);
        EmployeeEducation UpdateObject(EmployeeEducation employeeEducation);
        EmployeeEducation SoftDeleteObject(EmployeeEducation employeeEducation);
        bool DeleteObject(int Id);
    }
}