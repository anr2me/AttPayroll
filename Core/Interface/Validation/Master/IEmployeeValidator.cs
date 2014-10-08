using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IEmployeeValidator
    {
        Employee VHasUniqueNIK(Employee employee, IEmployeeService _employeeService);

        bool ValidCreateObject(Employee employee, IEmployeeService _employeeService, IDivisionService _divisionService, ITitleInfoService _titleInfoService);
        bool ValidUpdateObject(Employee employee, IEmployeeService _employeeService, IDivisionService _divisionService, ITitleInfoService _titleInfoService);
        bool ValidDeleteObject(Employee employee);
        bool isValid(Employee employee);
        string PrintError(Employee employee);
    }
}