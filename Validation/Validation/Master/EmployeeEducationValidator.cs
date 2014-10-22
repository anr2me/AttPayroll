using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class EmployeeEducationValidator : IEmployeeEducationValidator
    {
        public EmployeeEducation VHasEmployee(EmployeeEducation employeeEducation, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(employeeEducation.EmployeeId);
            if (employee == null)
            {
                employeeEducation.Errors.Add("Employee", "Tidak valid");
            }
            return employeeEducation;
        }

        public EmployeeEducation VHasInstitute(EmployeeEducation employeeEducation)
        {
            if (String.IsNullOrEmpty(employeeEducation.Institute) || employeeEducation.Institute.Trim() == "")
            {
                employeeEducation.Errors.Add("Institute", "Tidak boleh kosong");
            }
            return employeeEducation;
        }

        public EmployeeEducation VHasMajor(EmployeeEducation employeeEducation)
        {
            if (String.IsNullOrEmpty(employeeEducation.Major) || employeeEducation.Major.Trim() == "")
            {
                employeeEducation.Errors.Add("Major", "Tidak boleh kosong");
            }
            return employeeEducation;
        }

        public EmployeeEducation VHasLevel(EmployeeEducation employeeEducation)
        {
            if (String.IsNullOrEmpty(employeeEducation.Level) || employeeEducation.Level.Trim() == "")
            {
                employeeEducation.Errors.Add("Level", "Tidak boleh kosong");
            }
            return employeeEducation;
        }

        public EmployeeEducation VHasEnrollmentDate(EmployeeEducation employeeEducation)
        {
            if (employeeEducation.EnrollmentDate == null || employeeEducation.EnrollmentDate.Equals(DateTime.FromBinary(0)))
            {
                employeeEducation.Errors.Add("EnrollmentDate", "Tidak valid");
            }
            return employeeEducation;
        }

        public EmployeeEducation VHasGraduationDate(EmployeeEducation employeeEducation)
        {
            if (employeeEducation.GraduationDate == null || employeeEducation.GraduationDate.Equals(DateTime.FromBinary(0)))
            {
                employeeEducation.Errors.Add("GraduationDate", "Tidak valid");
            }
            else if (employeeEducation.GraduationDate.GetValueOrDefault().Date < employeeEducation.EnrollmentDate.Date)
            {
                employeeEducation.Errors.Add("GraduationDate", "Harus lebih besar atau sama dengan Enrollment Date");
            }
            return employeeEducation;
        }

        public bool ValidCreateObject(EmployeeEducation employeeEducation, IEmployeeService _employeeService)
        {
            VHasEmployee(employeeEducation, _employeeService);
            if (!isValid(employeeEducation)) { return false; }
            VHasInstitute(employeeEducation);
            if (!isValid(employeeEducation)) { return false; }
            VHasMajor(employeeEducation);
            if (!isValid(employeeEducation)) { return false; }
            VHasLevel(employeeEducation);
            if (!isValid(employeeEducation)) { return false; }
            VHasEnrollmentDate(employeeEducation);
            return isValid(employeeEducation);
        }

        public bool ValidUpdateObject(EmployeeEducation employeeEducation, IEmployeeService _employeeService)
        {
            employeeEducation.Errors.Clear();
            ValidCreateObject(employeeEducation, _employeeService);
            return isValid(employeeEducation);
        }

        public bool ValidDeleteObject(EmployeeEducation employeeEducation)
        {
            employeeEducation.Errors.Clear();
            return isValid(employeeEducation);
        }

        public bool isValid(EmployeeEducation obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(EmployeeEducation obj)
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
