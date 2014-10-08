using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class EmployeeLeaveValidator : IEmployeeLeaveValidator
    {
        public EmployeeLeave VHasEmployee(EmployeeLeave employeeLeave, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(employeeLeave.EmployeeId);
            if (employee == null)
            {
                employeeLeave.Errors.Add("Employee", "Tidak ada");
            }
            return employeeLeave;
        }

        public EmployeeLeave VHasRemark(EmployeeLeave employeeLeave)
        {
            if (String.IsNullOrEmpty(employeeLeave.Remark) || employeeLeave.Remark.Trim() == "")
            {
                employeeLeave.Errors.Add("Remark", "Tidak boleh kosong");
            }
            return employeeLeave;
        }

        public EmployeeLeave VHasStartDate(EmployeeLeave employeeLeave)
        {
            if (employeeLeave.StartDate == null || employeeLeave.StartDate.Equals(DateTime.FromBinary(0)))
            {
                employeeLeave.Errors.Add("StartDate", "Tidak valid");
            }
            return employeeLeave;
        }

        public EmployeeLeave VHasEndDate(EmployeeLeave employeeLeave)
        {
            if (employeeLeave.EndDate == null || employeeLeave.EndDate.Equals(DateTime.FromBinary(0)))
            {
                employeeLeave.Errors.Add("EndDate", "Tidak valid");
            }
            else if (employeeLeave.EndDate.Date < employeeLeave.StartDate.Date)
            {
                employeeLeave.Errors.Add("EndDate", "Harus lebih besar atau sama dengan Start Date");
            }
            return employeeLeave;
        }

        public bool ValidCreateObject(EmployeeLeave employeeLeave, IEmployeeService _employeeService)
        {
            VHasEmployee(employeeLeave, _employeeService);
            if (!isValid(employeeLeave)) { return false; }
            VHasRemark(employeeLeave);
            if (!isValid(employeeLeave)) { return false; }
            VHasStartDate(employeeLeave);
            if (!isValid(employeeLeave)) { return false; }
            VHasEndDate(employeeLeave);
            return isValid(employeeLeave);
        }

        public bool ValidUpdateObject(EmployeeLeave employeeLeave, IEmployeeService _employeeService)
        {
            employeeLeave.Errors.Clear();
            ValidCreateObject(employeeLeave, _employeeService);
            return isValid(employeeLeave);
        }

        public bool ValidDeleteObject(EmployeeLeave employeeLeave)
        {
            employeeLeave.Errors.Clear();
            return isValid(employeeLeave);
        }

        public bool isValid(EmployeeLeave obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(EmployeeLeave obj)
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
