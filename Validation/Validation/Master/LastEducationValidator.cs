using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class LastEducationValidator : ILastEducationValidator
    {
        public LastEducation VHasInstitute(LastEducation lastEducation)
        {
            if (String.IsNullOrEmpty(lastEducation.Institute) || lastEducation.Institute.Trim() == "")
            {
                lastEducation.Errors.Add("Institute", "Tidak boleh kosong");
            }
            return lastEducation;
        }

        public LastEducation VHasMajor(LastEducation lastEducation)
        {
            if (String.IsNullOrEmpty(lastEducation.Major) || lastEducation.Major.Trim() == "")
            {
                lastEducation.Errors.Add("Major", "Tidak boleh kosong");
            }
            return lastEducation;
        }

        public LastEducation VHasLevel(LastEducation lastEducation)
        {
            if (String.IsNullOrEmpty(lastEducation.Level) || lastEducation.Level.Trim() == "")
            {
                lastEducation.Errors.Add("Level", "Tidak boleh kosong");
            }
            return lastEducation;
        }

        public LastEducation VHasEnrollmentDate(LastEducation lastEducation)
        {
            if (lastEducation.EnrollmentDate == null || lastEducation.EnrollmentDate.Equals(DateTime.FromBinary(0)))
            {
                lastEducation.Errors.Add("EnrollmentDate", "Tidak valid");
            }
            return lastEducation;
        }

        public LastEducation VHasGraduationDate(LastEducation lastEducation)
        {
            if (lastEducation.GraduationDate == null || lastEducation.GraduationDate.Equals(DateTime.FromBinary(0)))
            {
                lastEducation.Errors.Add("GraduationDate", "Tidak valid");
            }
            else if (lastEducation.GraduationDate.GetValueOrDefault().Date < lastEducation.EnrollmentDate.Date)
            {
                lastEducation.Errors.Add("GraduationDate", "Harus lebih besar atau sama dengan Enrollment Date");
            }
            return lastEducation;
        }

        public bool ValidCreateObject(LastEducation lastEducation, ILastEducationService _lastEducationService)
        {
            VHasInstitute(lastEducation);
            if (!isValid(lastEducation)) { return false; }
            VHasMajor(lastEducation);
            if (!isValid(lastEducation)) { return false; }
            VHasLevel(lastEducation);
            if (!isValid(lastEducation)) { return false; }
            VHasEnrollmentDate(lastEducation);
            return isValid(lastEducation);
        }

        public bool ValidUpdateObject(LastEducation lastEducation, ILastEducationService _lastEducationService)
        {
            lastEducation.Errors.Clear();
            ValidCreateObject(lastEducation, _lastEducationService);
            return isValid(lastEducation);
        }

        public bool ValidDeleteObject(LastEducation lastEducation)
        {
            lastEducation.Errors.Clear();
            return isValid(lastEducation);
        }

        public bool isValid(LastEducation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(LastEducation obj)
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
