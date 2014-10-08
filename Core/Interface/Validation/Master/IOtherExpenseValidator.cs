using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IOtherExpenseValidator
    {

        bool ValidCreateObject(OtherExpense otherExpense, IEmployeeService _employeeService, ISalaryItemService _salaryItemService);
        bool ValidUpdateObject(OtherExpense otherExpense, IEmployeeService _employeeService, ISalaryItemService _salaryItemService);
        bool ValidDeleteObject(OtherExpense otherExpense);
        bool isValid(OtherExpense otherExpense);
        string PrintError(OtherExpense otherExpense);
    }
}