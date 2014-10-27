using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IOtherExpenseDetailValidator
    {

        bool ValidCreateObject(OtherExpenseDetail otherExpenseDetail, IOtherExpenseService _otherExpenseService, IEmployeeService _employeeService);
        bool ValidUpdateObject(OtherExpenseDetail otherExpenseDetail, IOtherExpenseService _otherExpenseService, IEmployeeService _employeeService);
        bool ValidDeleteObject(OtherExpenseDetail otherExpenseDetail);
        bool isValid(OtherExpenseDetail otherExpenseDetail);
        string PrintError(OtherExpenseDetail otherExpenseDetail);
    }
}