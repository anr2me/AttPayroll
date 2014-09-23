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
        OtherExpense VCreateObject(OtherExpense otherExpense, IOtherExpenseService _otherExpenseService);
        OtherExpense VUpdateObject(OtherExpense otherExpense, IOtherExpenseService _otherExpenseService);
        OtherExpense VDeleteObject(OtherExpense otherExpense);
        bool ValidCreateObject(OtherExpense otherExpense, IOtherExpenseService _otherExpenseService);
        bool ValidUpdateObject(OtherExpense otherExpense, IOtherExpenseService _otherExpenseService);
        bool ValidDeleteObject(OtherExpense otherExpense);
        bool isValid(OtherExpense otherExpense);
        string PrintError(OtherExpense otherExpense);
    }
}