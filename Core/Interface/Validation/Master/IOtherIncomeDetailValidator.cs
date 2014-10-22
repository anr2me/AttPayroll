using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IOtherIncomeDetailValidator
    {

        bool ValidCreateObject(OtherIncomeDetail otherIncomeDetail, IOtherIncomeService _otherIncomeService, IEmployeeService _employeeService);
        bool ValidUpdateObject(OtherIncomeDetail otherIncomeDetail, IOtherIncomeService _otherIncomeService, IEmployeeService _employeeService);
        bool ValidDeleteObject(OtherIncomeDetail otherIncomeDetail);
        bool isValid(OtherIncomeDetail otherIncomeDetail);
        string PrintError(OtherIncomeDetail otherIncomeDetail);
    }
}