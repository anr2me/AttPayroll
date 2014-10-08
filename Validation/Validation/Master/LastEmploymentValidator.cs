using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class LastEmploymentValidator : ILastEmploymentValidator
    {
        public LastEmployment VHasCompany(LastEmployment lastEmployment)
        {
            if (String.IsNullOrEmpty(lastEmployment.Company) || lastEmployment.Company.Trim() == "")
            {
                lastEmployment.Errors.Add("Company", "Tidak boleh kosong");
            }
            return lastEmployment;
        }

        public LastEmployment VHasTitle(LastEmployment lastEmployment)
        {
            if (String.IsNullOrEmpty(lastEmployment.Title) || lastEmployment.Title.Trim() == "")
            {
                lastEmployment.Errors.Add("Title", "Tidak boleh kosong");
            }
            return lastEmployment;
        }

        public LastEmployment VHasReason(LastEmployment lastEmployment)
        {
            if (String.IsNullOrEmpty(lastEmployment.ResignReason) || lastEmployment.ResignReason.Trim() == "")
            {
                lastEmployment.Errors.Add("ResignReason", "Tidak boleh kosong");
            }
            return lastEmployment;
        }

        public LastEmployment VHasStartDate(LastEmployment lastEmployment)
        {
            if (lastEmployment.StartDate == null || lastEmployment.StartDate.Equals(DateTime.FromBinary(0)))
            {
                lastEmployment.Errors.Add("StartDate", "Tidak valid");
            }
            return lastEmployment;
        }

        public LastEmployment VHasEndDate(LastEmployment lastEmployment)
        {
            if (lastEmployment.EndDate == null || lastEmployment.EndDate.Equals(DateTime.FromBinary(0)))
            {
                lastEmployment.Errors.Add("EndDate", "Tidak valid");
            }
            else if (lastEmployment.EndDate.GetValueOrDefault().Date < lastEmployment.StartDate.Date)
            {
                lastEmployment.Errors.Add("EndDate", "Harus lebih besar atau sama dengan Start Date");
            }
            return lastEmployment;
        }

        public bool ValidCreateObject(LastEmployment lastEmployment, ILastEmploymentService _lastEmploymentService)
        {
            VHasCompany(lastEmployment);
            if (!isValid(lastEmployment)) { return false; }
            VHasTitle(lastEmployment);
            //if (!isValid(lastEmployment)) { return false; }
            //VHasReason(lastEmployment);
            if (!isValid(lastEmployment)) { return false; }
            VHasStartDate(lastEmployment);
            if (!isValid(lastEmployment)) { return false; }
            VHasEndDate(lastEmployment);
            return isValid(lastEmployment);
        }

        public bool ValidUpdateObject(LastEmployment lastEmployment, ILastEmploymentService _lastEmploymentService)
        {
            lastEmployment.Errors.Clear();
            ValidCreateObject(lastEmployment, _lastEmploymentService);
            return isValid(lastEmployment);
        }

        public bool ValidDeleteObject(LastEmployment lastEmployment)
        {
            lastEmployment.Errors.Clear();
            return isValid(lastEmployment);
        }

        public bool isValid(LastEmployment obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(LastEmployment obj)
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
