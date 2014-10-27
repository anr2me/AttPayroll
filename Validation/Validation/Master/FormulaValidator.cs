using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Validation.Validation
{
    public class FormulaValidator : IFormulaValidator
    {
        //public Formula VHasParentSalaryItem(Formula formula, ISalaryItemService _salaryItemService)
        //{
        //    SalaryItem salaryItem = _salaryItemService.GetObjectById(formula.SalaryItemId);
        //    if (salaryItem == null)
        //    {
        //        formula.Errors.Add("SalaryItem", "Tidak valid");
        //    }
        //    return formula;
        //}

        public Formula VHasFirstSalaryItem(Formula formula, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(formula.FirstSalaryItemId.GetValueOrDefault());
            if (salaryItem == null)
            {
                formula.Errors.Add("FirstSalaryItem", "Tidak valid");
            }
            return formula;
        }

        public Formula VHasSecondSalaryItem(Formula formula, ISalaryItemService _salaryItemService)
        {
            if (!formula.IsSecondValue)
            {
                SalaryItem salaryItem = _salaryItemService.GetObjectById(formula.SecondSalaryItemId.GetValueOrDefault());
                if (salaryItem == null)
                {
                    formula.Errors.Add("SecondSalaryItem", "Tidak valid");
                }
            }
            return formula;
        }

        //public Formula VHasValidSign(Formula formula)
        //{
        //    if (formula.IsSecondValue && formula.ValueSign == 0)
        //    {
        //        formula.Errors.Add("ValueSign", "Tidak valid");
        //    }
        //    return formula;
        //}

        public bool ValidCreateObject(Formula formula, IFormulaService _formulaService, ISalaryItemService _salaryItemService)
        {
            //VHasParentSalaryItem(formula, _salaryItemService);
            //if (!isValid(formula)) { return false; }
            VHasFirstSalaryItem(formula, _salaryItemService);
            if (!isValid(formula)) { return false; }
            VHasSecondSalaryItem(formula, _salaryItemService);
            //if (!isValid(formula)) { return false; }
            //VHasValidSign(formula);
            return isValid(formula);
        }

        public bool ValidUpdateObject(Formula formula, IFormulaService _formulaService, ISalaryItemService _salaryItemService)
        {
            formula.Errors.Clear();
            ValidCreateObject(formula, _formulaService, _salaryItemService);
            return isValid(formula);
        }

        public bool ValidDeleteObject(Formula formula)
        {
            formula.Errors.Clear();
            return isValid(formula);
        }

        public bool isValid(Formula obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Formula obj)
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
