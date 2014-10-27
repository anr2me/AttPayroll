using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class THRValidator : ITHRValidator
    {
        public THR VHasUniqueCode(THR thr, ITHRService _thrService)
        {
            if (String.IsNullOrEmpty(thr.Code) || thr.Code.Trim() == "")
            {
                thr.Errors.Add("Code", "Tidak boleh kosong");
            }
            else if (_thrService.IsCodeDuplicated(thr))
            {
                thr.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return thr;
        }

        public THR VHasSalaryItem(THR thr, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(thr.SalaryItemId.GetValueOrDefault());
            if (salaryItem == null)
            {
                thr.Errors.Add("SalaryItem", "Tidak valid");
            }
            return thr;
        }

        public THR VHasEffectiveDate(THR thr)
        {
            if (thr.EffectiveDate == null || thr.EffectiveDate.Equals(DateTime.FromBinary(0)))
            {
                thr.Errors.Add("EffectiveDate", "Tidak valid");
            }
            return thr;
        }

        public bool ValidCreateObject(THR thr, ITHRService _thrService)
        {
            VHasUniqueCode(thr, _thrService);
            if (!isValid(thr)) { return false; }
            VHasEffectiveDate(thr);
            return isValid(thr);
        }

        public bool ValidUpdateObject(THR thr, ITHRService _thrService)
        {
            thr.Errors.Clear();
            ValidCreateObject(thr, _thrService);
            return isValid(thr);
        }

        public bool ValidDeleteObject(THR thr)
        {
            thr.Errors.Clear();
            return isValid(thr);
        }

        public bool isValid(THR obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(THR obj)
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
