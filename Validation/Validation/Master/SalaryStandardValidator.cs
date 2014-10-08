using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SalaryStandardValidator : ISalaryStandardValidator
    {

        public SalaryStandard VHasTitleInfo(SalaryStandard salaryStandard, ITitleInfoService _titleInfoService)
        {
            TitleInfo titleInfo = _titleInfoService.GetObjectById(salaryStandard.TitleInfoId);
            if (titleInfo == null)
            {
                salaryStandard.Errors.Add("TitleInfo", "Tidak ada");
            }
            return salaryStandard;
        }

        public SalaryStandard VHasEffectiveDate(SalaryStandard salaryStandard)
        {
            if (salaryStandard.EffectiveDate == null || salaryStandard.EffectiveDate.Equals(DateTime.FromBinary(0)))
            {
                salaryStandard.Errors.Add("EffectiveDate", "Tidak valid");
            }
            return salaryStandard;
        }

        public bool ValidCreateObject(SalaryStandard salaryStandard, ITitleInfoService _titleInfoService)
        {
            VHasTitleInfo(salaryStandard, _titleInfoService);
            if (!isValid(salaryStandard)) { return false; }
            VHasEffectiveDate(salaryStandard);
            return isValid(salaryStandard);
        }

        public bool ValidUpdateObject(SalaryStandard salaryStandard, ITitleInfoService _titleInfoService)
        {
            salaryStandard.Errors.Clear();
            ValidCreateObject(salaryStandard, _titleInfoService);
            return isValid(salaryStandard);
        }

        public bool ValidDeleteObject(SalaryStandard salaryStandard)
        {
            salaryStandard.Errors.Clear();
            return isValid(salaryStandard);
        }

        public bool isValid(SalaryStandard obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalaryStandard obj)
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
