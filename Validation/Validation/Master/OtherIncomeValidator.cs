using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class OtherIncomeValidator : IOtherIncomeValidator
    {

        public OtherIncome VHasEmployee(OtherIncome otherIncome, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(otherIncome.EmployeeId);
            if (employee == null)
            {
                otherIncome.Errors.Add("Employee", "Tidak valid");
            }
            return otherIncome;
        }

        public OtherIncome VHasSalaryItem(OtherIncome otherIncome, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(otherIncome.SalaryItemId);
            if (salaryItem == null)
            {
                otherIncome.Errors.Add("SalaryItem", "Tidak valid");
            }
            return otherIncome;
        }

        public bool ValidCreateObject(OtherIncome otherIncome, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            VHasEmployee(otherIncome, _employeeService);
            if (!isValid(otherIncome)) { return false; }
            VHasSalaryItem(otherIncome, _salaryItemService);
            return isValid(otherIncome);
        }

        public bool ValidUpdateObject(OtherIncome otherIncome, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            otherIncome.Errors.Clear();
            ValidCreateObject(otherIncome, _employeeService, _salaryItemService);
            return isValid(otherIncome);
        }

        public bool ValidDeleteObject(OtherIncome otherIncome)
        {
            otherIncome.Errors.Clear();
            return isValid(otherIncome);
        }

        public bool isValid(OtherIncome obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(OtherIncome obj)
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
