using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class OtherExpenseValidator : IOtherExpenseValidator
    {

        public OtherExpense VHasEmployee(OtherExpense otherExpense, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(otherExpense.EmployeeId);
            if (employee == null)
            {
                otherExpense.Errors.Add("Employee", "Tidak valid");
            }
            return otherExpense;
        }

        public OtherExpense VHasSalaryItem(OtherExpense otherExpense, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(otherExpense.SalaryItemId);
            if (salaryItem == null)
            {
                otherExpense.Errors.Add("SalaryItem", "Tidak valid");
            }
            return otherExpense;
        }

        public bool ValidCreateObject(OtherExpense otherExpense, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            VHasEmployee(otherExpense, _employeeService);
            if (!isValid(otherExpense)) { return false; }
            VHasSalaryItem(otherExpense, _salaryItemService);
            return isValid(otherExpense);
        }

        public bool ValidUpdateObject(OtherExpense otherExpense, IEmployeeService _employeeService, ISalaryItemService _salaryItemService)
        {
            otherExpense.Errors.Clear();
            ValidCreateObject(otherExpense, _employeeService, _salaryItemService);
            return isValid(otherExpense);
        }

        public bool ValidDeleteObject(OtherExpense otherExpense)
        {
            otherExpense.Errors.Clear();
            return isValid(otherExpense);
        }

        public bool isValid(OtherExpense obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(OtherExpense obj)
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
