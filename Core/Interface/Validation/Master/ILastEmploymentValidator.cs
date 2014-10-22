using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ILastEmploymentValidator
    {
        bool ValidCreateObject(LastEmployment lastEmployment, IEmployeeService _employeeService);
        bool ValidUpdateObject(LastEmployment lastEmployment, IEmployeeService _employeeService);
        bool ValidDeleteObject(LastEmployment lastEmployment);
        bool isValid(LastEmployment lastEmployment);
        string PrintError(LastEmployment lastEmployment);
    }
}