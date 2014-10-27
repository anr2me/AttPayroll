using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class OtherExpenseValidator : IOtherExpenseValidator
    {
        public OtherExpense VHasUniqueCode(OtherExpense otherExpense, IOtherExpenseService _otherExpenseService)
        {
            if (String.IsNullOrEmpty(otherExpense.Code) || otherExpense.Code.Trim() == "")
            {
                otherExpense.Errors.Add("Code", "Tidak boleh kosong");
            }
            else if (_otherExpenseService.IsCodeDuplicated(otherExpense))
            {
                otherExpense.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return otherExpense;
        }

        public OtherExpense VHasSalaryItem(OtherExpense otherExpense, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(otherExpense.SalaryItemId.GetValueOrDefault());
            if (salaryItem == null)
            {
                otherExpense.Errors.Add("SalaryItem", "Tidak valid");
            }
            return otherExpense;
        }

        public bool ValidCreateObject(OtherExpense otherExpense, IOtherExpenseService _otherExpenseService)
        {
            VHasUniqueCode(otherExpense, _otherExpenseService);
            return isValid(otherExpense);
        }

        public bool ValidUpdateObject(OtherExpense otherExpense, IOtherExpenseService _otherExpenseService)
        {
            otherExpense.Errors.Clear();
            ValidCreateObject(otherExpense, _otherExpenseService);
            return isValid(otherExpense);
        }

        public bool ValidDeleteObject(OtherExpense otherExpense)
        {
            otherExpense.Errors.Clear();
            return isValid(otherExpense);
        }

        public bool isValid(OtherExpense obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(OtherExpense obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
