using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SalarySlipDetailValidator : ISalarySlipDetailValidator
    {
        public SalarySlipDetail VHasSalarySlip(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService)
        {
            SalarySlip salarySlip = _salarySlipService.GetObjectById(salarySlipDetail.SalarySlipId);
            if (salarySlip == null)
            {
                salarySlipDetail.Errors.Add("SalarySlip", "Tidak ada");
            }
            return salarySlipDetail;
        }

        public SalarySlipDetail VHasSalaryItem(SalarySlipDetail salarySlipDetail, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(salarySlipDetail.SalaryItemId);
            if (salaryItem == null)
            {
                salarySlipDetail.Errors.Add("SalaryItem", "Tidak ada");
            }
            return salarySlipDetail;
        }

        public bool ValidCreateObject(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService)
        {
            VHasSalarySlip(salarySlipDetail, _salarySlipService);
            if (!isValid(salarySlipDetail)) { return false; }
            VHasSalaryItem(salarySlipDetail, _salaryItemService);
            return isValid(salarySlipDetail);
        }

        public bool ValidUpdateObject(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService)
        {
            salarySlipDetail.Errors.Clear();
            ValidCreateObject(salarySlipDetail, _salarySlipService, _salaryItemService);
            return isValid(salarySlipDetail);
        }

        public bool ValidDeleteObject(SalarySlipDetail salarySlipDetail)
        {
            salarySlipDetail.Errors.Clear();
            return isValid(salarySlipDetail);
        }

        public bool isValid(SalarySlipDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalarySlipDetail obj)
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
