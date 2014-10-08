using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SalaryItemValidator : ISalaryItemValidator
    {
        public SalaryItem VHasUniqueCode(SalaryItem salaryItem, ISalaryItemService _salaryItemService)
        {
            if (String.IsNullOrEmpty(salaryItem.Code) || salaryItem.Code.Trim() == "")
            {
                salaryItem.Errors.Add("Code", "Tidak boleh kosong");
            }
            else if (_salaryItemService.IsCodeDuplicated(salaryItem))
            {
                salaryItem.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return salaryItem;
        }

        public SalaryItem VHasDefaultValue(SalaryItem salaryItem)
        {
            if (salaryItem.DefaultValue < 0)
            {
                salaryItem.Errors.Add("DefaultValue", "Harus lebih besar atau sama dengan 0");
            }
            return salaryItem;
        }

        public bool ValidCreateObject(SalaryItem salaryItem, ISalaryItemService _salaryItemService)
        {
            VHasUniqueCode(salaryItem, _salaryItemService);
            if (!isValid(salaryItem)) { return false; }
            VHasDefaultValue(salaryItem);
            return isValid(salaryItem);
        }

        public bool ValidUpdateObject(SalaryItem salaryItem, ISalaryItemService _salaryItemService)
        {
            salaryItem.Errors.Clear();
            ValidCreateObject(salaryItem, _salaryItemService);
            return isValid(salaryItem);
        }

        public bool ValidDeleteObject(SalaryItem salaryItem)
        {
            salaryItem.Errors.Clear();
            return isValid(salaryItem);
        }

        public bool isValid(SalaryItem obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalaryItem obj)
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
