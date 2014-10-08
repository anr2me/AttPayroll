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
        Employee CreateObject(Employee employee, IDivisionService _divisionService, ITitleInfoService _titleInfoService);
        Employee CreateObject(string NIK, string Name, string Address, string PhoneNumber, string Email, string NPWP,
                              string PlaceOfBirth, DateTime BirthDate, int Sex, int MaritalStatus, int Children, int Religion,
                              IDivisionService _divisionService, ITitleInfoService _titleInfoService);
        Employee UpdateObject(Employee employee, IDivisionService _divisionService, ITitleInfoService _titleInfoService);
        Employee SoftDeleteObject(Employee employee);
        bool DeleteObject(int Id);
        bool IsNIKDuplicated(Employee employee);
    }
}