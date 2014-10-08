using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SalaryStandardDetailValidator : ISalaryStandardDetailValidator
    {
        public SalaryStandardDetail VHasSalaryStandard(SalaryStandardDetail salaryStandardDetail, ISalaryStandardService _salaryStandardService)
        {
            SalaryStandard salaryStandard = _salaryStandardService.GetObjectById(salaryStandardDetail.SalaryStandardId);
            if (salaryStandard == null)
            {
                salaryStandardDetail.Errors.Add("SalaryStandard", "Tidak ada");
            }
            return salaryStandardDetail;
        }

        public SalaryStandardDetail VHasSalaryItem(SalaryStandardDetail salaryStandardDetail, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(salaryStandardDetail.SalaryItemId);
            if (salaryItem == null)
            {
                salaryStandardDetail.Errors.Add("SalaryItem", "Tidak ada");
            }
            return salaryStandardDetail;
        }

        public SalaryStandardDetail VHasAmount(SalaryStandardDetail salaryStandardDetail)
        {
            if (salaryStandardDetail.Amount < 0)
            {
                salaryStandardDetail.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return salaryStandardDetail;
        }

        public bool ValidCreateObject(SalaryStandardDetail salaryStandardDetail, ISalaryStandardService _salaryStandardService, ISalaryItemService _salaryItemService)
        {
            VHasSalaryStandard(salaryStandardDetail, _salaryStandardService);
            if (!isValid(salaryStandardDetail)) { return false; }
            VHasSalaryItem(salaryStandardDetail, _salaryItemService);
            if (!isValid(salaryStandardDetail)) { return false; }
            VHasAmount(salaryStandardDetail);
            return isValid(salaryStandardDetail);
        }

        public bool ValidUpdateObject(SalaryStandardDetail salaryStandardDetail, ISalaryStandardService _salaryStandardService, ISalaryItemService _salaryItemService)
        {
            salaryStandardDetail.Errors.Clear();
            ValidCreateObject(salaryStandardDetail, _salaryStandardService, _salaryItemService);
            return isValid(salaryStandardDetail);
        }

        public bool ValidDeleteObject(SalaryStandardDetail salaryStandardDetail)
        {
            salaryStandardDetail.Errors.Clear();
            return isValid(salaryStandardDetail);
        }

        public bool isValid(SalaryStandardDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalaryStandardDetail obj)
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
