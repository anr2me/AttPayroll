using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEmployeeEducationService
    {
        IEmployeeEducationValidator GetValidator();
        IQueryable<EmployeeEducation> GetQueryable();
        IList<EmployeeEducation> GetAll();
        IList<EmployeeEducation> GetObjectsByEmployeeId(int EmployeeId);
        EmployeeEducation GetObjectById(int Id);
        EmployeeEducation CreateObject(EmployeeEducation employeeEducation, IEmployeeService _employeeService);
        EmployeeEducation CreateObject(string Institute, string Major, string Level, DateTime EnrollmentDate, Nullable<DateTime> GraduationDate, IEmployeeService _employeeService);
        EmployeeEducation UpdateObject(EmployeeEducation employeeEducation, IEmployeeService _employeeService);
        EmployeeEducation SoftDeleteObject(EmployeeEducation employeeEducation);
        bool DeleteObject(int Id);
    }
}