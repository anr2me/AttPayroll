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

        //public SalarySlipDetail VHasSalaryItem(SalarySlipDetail salarySlipDetail, ISalaryItemService _salaryItemService)
        //{
        //    SalaryItem salaryItem = _salaryItemService.GetObjectById(salarySlipDetail.SalaryItemId);
        //    if (salaryItem == null)
        //    {
        //        salarySlipDetail.Errors.Add("SalaryItem", "Tidak ada");
        //    }
        //    return salarySlipDetail;
        //}

        public SalarySlipDetail VHasFormula(SalarySlipDetail salarySlipDetail, IFormulaService _formulaService)
        {
            Formula formula = _formulaService.GetObjectById(salarySlipDetail.FormulaId);
            if (formula == null)
            {
                salarySlipDetail.Errors.Add("Formula", "Tidak ada");
            }
            return salarySlipDetail;
        }

        public SalarySlipDetail VIsValidMaxValue(SalarySlipDetail salarySlipDetail)
        {
            if (salarySlipDetail.HasMinValue && salarySlipDetail.HasMaxValue && salarySlipDetail.MaxValue < salarySlipDetail.MinValue)
            {
                salarySlipDetail.Errors.Add("MaxValue", "Harus lebih besar atau sama dengan MinValue");
            }
            return salarySlipDetail;
        }

        public bool ValidCreateObject(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService, IFormulaService _formulaService)
        {
            VHasSalarySlip(salarySlipDetail, _salarySlipService);
            if (!isValid(salarySlipDetail)) { return false; }
            VHasFormula(salarySlipDetail, _formulaService); // VHasSalaryItem
            if (!isValid(salarySlipDetail)) { return false; }
            VIsValidMaxValue(salarySlipDetail);
            return isValid(salarySlipDetail);
        }

        public bool ValidUpdateObject(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService, IFormulaService _formulaService)
        {
            salarySlipDetail.Errors.Clear();
            ValidCreateObject(salarySlipDetail, _salarySlipService, _formulaService);
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
