using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class EmployeeLoanValidator : IEmployeeLoanValidator
    {
        public EmployeeLoan VHasEmployee(EmployeeLoan employeeLoan, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(employeeLoan.EmployeeId);
            if (employee == null)
            {
                employeeLoan.Errors.Add("Employee", "Tidak ada");
            }
            return employeeLoan;
        }

        public EmployeeLoan VHasSalaryItem(EmployeeLoan employeeLoan, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(employeeLoan.SalaryItemId);
            if (salaryItem == null)
            {
                employeeLoan.Errors.Add("SalaryItem", "Tidak ada");
            }
            return employeeLoan;
        }

        public EmployeeLoan VHasRemark(EmployeeLoan employeeLoan)
        {
            if (String.IsNullOrEmpty(employeeLoan.Remark) || employeeLoan.Remark.Trim() == "")
            {
                employeeLoan.Errors.Add("Remark", "Tidak boleh kosong");
            }
            return employeeLoan;
        }

        public EmployeeLoan VHasStartDate(EmployeeLoan employeeLoan)
        {
            if (employeeLoan.StartDate == null || employeeLoan.StartDate.Equals(DateTime.FromBinary(0)))
            {
                employeeLoan.Errors.Add("StartDate", "Tidak valid");
            }
            return employeeLoan;
        }

        public EmployeeLoan VHasEndDate(EmployeeLoan employeeLoan)
        {
            if (employeeLoan.EndDate == null || employeeLoan.EndDate.Equals(DateTime.FromBinary(0)))
            {
                employeeLoan.Errors.Add("EndDate", "Tidak valid");
            }
            else if (employeeLoan.EndDate.Date < employeeLoan.StartDate.Date)
            {
                employeeLoan.Errors.Add("EndDate", "Harus lebih besar atau sama dengan Start Date");
            }
            return employeeLoan;
        }

        public EmployeeLoan VIsValidAmount(EmployeeLoan employeeLoan)
        {
            if (employeeLoan.Amount < 0)
            {
                employeeLoan.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return employeeLoan;
        }

        public EmployeeLoan VIsValidInterest(EmployeeLoan employeeLoan)
        {
            if (employeeLoan.Interest < 0)
            {
                employeeLoan.Errors.Add("Interest", "Harus lebih besar atau sama dengan 0");
            }
            return employeeLoan;
        }

        public EmployeeLoan VIsValidInstallmentTimes(EmployeeLoan employeeLoan)
        {
            if (employeeLoan.InstallmentTimes < 0)
            {
                employeeLoan.Errors.Add("InstallmentTimes", "Harus lebih besar atau sama dengan 0");
            }
            return employeeLoan;
        }

        public bool ValidCreateObject(EmployeeLoan employeeLoan, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            VHasEmployee(employeeLoan, _employeeService);
            if (!isValid(employeeLoan)) { return false; }
            VHasRemark(employeeLoan);
            if (!isValid(employeeLoan)) { return false; }
            VHasSalaryItem(employeeLoan, _salaryItemService);
            if (!isValid(employeeLoan)) { return false; }
            VIsValidAmount(employeeLoan);
            if (!isValid(employeeLoan)) { return false; }
            VIsValidInterest(employeeLoan);
            if (!isValid(employeeLoan)) { return false; }
            VIsValidInstallmentTimes(employeeLoan);
            if (!isValid(employeeLoan)) { return false; }
            VHasStartDate(employeeLoan);
            if (!isValid(employeeLoan)) { return false; }
            VHasEndDate(employeeLoan);
            return isValid(employeeLoan);
        }

        public bool ValidUpdateObject(EmployeeLoan employeeLoan, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            employeeLoan.Errors.Clear();
            ValidCreateObject(employeeLoan, _employeeService, _salaryItemService);
            return isValid(employeeLoan);
        }

        public bool ValidDeleteObject(EmployeeLoan employeeLoan)
        {
            employeeLoan.Errors.Clear();
            return isValid(employeeLoan);
        }

        public bool isValid(EmployeeLoan obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(EmployeeLoan obj)
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
