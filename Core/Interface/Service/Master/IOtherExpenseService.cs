using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IOtherExpenseService
    {
        IOtherExpenseValidator GetValidator();
        IQueryable<OtherExpense> GetQueryable();
        IList<OtherExpense> GetAll();
        OtherExpense GetObjectById(int Id);
        OtherExpense CreateObject(OtherExpense otherExpense, ISalaryItemService _salaryItemService);
        OtherExpense UpdateObject(OtherExpense otherExpense, ISalaryItemService _salaryItemService);
        OtherExpense SoftDeleteObject(OtherExpense otherExpense, ISalaryItemService _salaryItemService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(OtherExpense otherExpense);
    }
}