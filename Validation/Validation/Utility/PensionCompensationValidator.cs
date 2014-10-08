using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class PensionCompensationValidator : IPensionCompensationValidator
    {
        public PensionCompensation VHasEmployee(PensionCompensation pensionCompensation, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(pensionCompensation.EmployeeId);
            if (employee == null)
            {
                pensionCompensation.Errors.Add("Employee", "Tidak ada");
            }
            return pensionCompensation;
        }

        public PensionCompensation VHasPensionDate(PensionCompensation pensionCompensation)
        {
            if (pensionCompensation.PensionDate == null || pensionCompensation.PensionDate.Equals(DateTime.FromBinary(0)))
            {
                pensionCompensation.Errors.Add("PensionDate", "Tidak valid");
            }
            return pensionCompensation;
        }

        public PensionCompensation VIsValidPensionValue(PensionCompensation pensionCompensation)
        {
            if (pensionCompensation.PensionValue < 0)
            {
                pensionCompensation.Errors.Add("PensionValue", "Harus lebih besar atau sama dengan 0");
            }
            return pensionCompensation;
        }

        public PensionCompensation VIsValidPensionMultiply(PensionCompensation pensionCompensation)
        {
            if (pensionCompensation.PensionMultiply < 0)
            {
                pensionCompensation.Errors.Add("PensionMultiply", "Harus lebih besar atau sama dengan 0");
            }
            return pensionCompensation;
        }

        public PensionCompensation VIsValidMedAndHousing(PensionCompensation pensionCompensation)
        {
            if (pensionCompensation.MedAndHousing < 0)
            {
                pensionCompensation.Errors.Add("MedAndHousing", "Harus lebih besar atau sama dengan 0");
            }
            return pensionCompensation;
        }

        public bool ValidCreateObject(PensionCompensation pensionCompensation, IEmployeeService _employeeService)
        {
            VHasEmployee(pensionCompensation, _employeeService);
            if (!isValid(pensionCompensation)) { return false; }
            VHasPensionDate(pensionCompensation);
            if (!isValid(pensionCompensation)) { return false; }
            VIsValidPensionValue(pensionCompensation);
            if (!isValid(pensionCompensation)) { return false; }
            VIsValidPensionMultiply(pensionCompensation);
            if (!isValid(pensionCompensation)) { return false; }
            VIsValidMedAndHousing(pensionCompensation);
            return isValid(pensionCompensation);
        }

        public bool ValidUpdateObject(PensionCompensation pensionCompensation, IEmployeeService _employeeService)
        {
            pensionCompensation.Errors.Clear();
            ValidCreateObject(pensionCompensation, _employeeService);
            return isValid(pensionCompensation);
        }

        public bool ValidDeleteObject(PensionCompensation pensionCompensation)
        {
            pensionCompensation.Errors.Clear();
            return isValid(pensionCompensation);
        }

        public bool isValid(PensionCompensation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PensionCompensation obj)
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
