using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class GeneralLeaveValidator : IGeneralLeaveValidator
    {
        
        public GeneralLeave VHasRemark(GeneralLeave generalLeave)
        {
            if (String.IsNullOrEmpty(generalLeave.Remark) || generalLeave.Remark.Trim() == "")
            {
                generalLeave.Errors.Add("Remark", "Tidak boleh kosong");
            }
            return generalLeave;
        }

        public GeneralLeave VHasStartDate(GeneralLeave generalLeave)
        {
            if (generalLeave.StartDate == null || generalLeave.StartDate.Equals(DateTime.FromBinary(0)))
            {
                generalLeave.Errors.Add("StartDate", "Tidak valid");
            }
            return generalLeave;
        }

        public GeneralLeave VHasEndDate(GeneralLeave generalLeave)
        {
            if (generalLeave.EndDate == null || generalLeave.EndDate.Equals(DateTime.FromBinary(0)))
            {
                generalLeave.Errors.Add("EndDate", "Tidak valid");
            }
            else if (generalLeave.EndDate.Date < generalLeave.StartDate.Date)
            {
                generalLeave.Errors.Add("EndDate", "Harus lebih besar atau sama dengan Start Date");
            }
            return generalLeave;
        }

        public bool ValidCreateObject(GeneralLeave generalLeave)
        {
            VHasRemark(generalLeave);
            if (!isValid(generalLeave)) { return false; }
            VHasStartDate(generalLeave);
            if (!isValid(generalLeave)) { return false; }
            VHasEndDate(generalLeave);
            return isValid(generalLeave);
        }

        public bool ValidUpdateObject(GeneralLeave generalLeave)
        {
            generalLeave.Errors.Clear();
            ValidCreateObject(generalLeave);
            return isValid(generalLeave);
        }

        public bool ValidDeleteObject(GeneralLeave generalLeave)
        {
            generalLeave.Errors.Clear();
            return isValid(generalLeave);
        }

        public bool isValid(GeneralLeave obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(GeneralLeave obj)
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
