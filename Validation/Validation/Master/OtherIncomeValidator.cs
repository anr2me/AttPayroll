using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class OtherIncomeValidator : IOtherIncomeValidator
    {
        public OtherIncome VHasUniqueCode(OtherIncome otherIncome, IOtherIncomeService _otherIncomeService)
        {
            if (String.IsNullOrEmpty(otherIncome.Code) || otherIncome.Code.Trim() == "")
            {
                otherIncome.Errors.Add("Code", "Tidak boleh kosong");
            }
            else if (_otherIncomeService.IsCodeDuplicated(otherIncome))
            {
                otherIncome.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return otherIncome;
        }

        public OtherIncome VHasSalaryItem(OtherIncome otherIncome, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(otherIncome.SalaryItemId.GetValueOrDefault());
            if (salaryItem == null)
            {
                otherIncome.Errors.Add("SalaryItem", "Tidak valid");
            }
            return otherIncome;
        }

        public bool ValidCreateObject(OtherIncome otherIncome, IOtherIncomeService _otherIncomeService)
        {
            VHasUniqueCode(otherIncome, _otherIncomeService);
            return isValid(otherIncome);
        }

        public bool ValidUpdateObject(OtherIncome otherIncome, IOtherIncomeService _otherIncomeService)
        {
            otherIncome.Errors.Clear();
            ValidCreateObject(otherIncome, _otherIncomeService);
            return isValid(otherIncome);
        }

        public bool ValidDeleteObject(OtherIncome otherIncome)
        {
            otherIncome.Errors.Clear();
            return isValid(otherIncome);
        }

        public bool isValid(OtherIncome obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(OtherIncome obj)
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
