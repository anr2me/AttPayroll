using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class OtherExpenseDetailValidator : IOtherExpenseDetailValidator
    {
        public OtherExpenseDetail VHasOtherExpense(OtherExpenseDetail otherExpenseDetail, IOtherExpenseService _otherExpenseService)
        {
            OtherExpense otherExpense = _otherExpenseService.GetObjectById(otherExpenseDetail.OtherExpenseId);
            if (otherExpense == null)
            {
                otherExpenseDetail.Errors.Add("OtherExpense", "Tidak ada");
            }
            return otherExpenseDetail;
        }

        public OtherExpenseDetail VHasEmployee(OtherExpenseDetail otherExpenseDetail, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(otherExpenseDetail.EmployeeId);
            if (employee == null)
            {
                otherExpenseDetail.Errors.Add("Employee", "Tidak ada");
            }
            return otherExpenseDetail;
        }

        public OtherExpenseDetail VHasAmount(OtherExpenseDetail otherExpenseDetail)
        {
            if (otherExpenseDetail.Amount < 0)
            {
                otherExpenseDetail.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return otherExpenseDetail;
        }

        public bool ValidCreateObject(OtherExpenseDetail otherExpenseDetail, IOtherExpenseService _otherExpenseService, IEmployeeService _employeeService)
        {
            VHasOtherExpense(otherExpenseDetail, _otherExpenseService);
            if (!isValid(otherExpenseDetail)) { return false; }
            VHasEmployee(otherExpenseDetail, _employeeService);
            if (!isValid(otherExpenseDetail)) { return false; }
            VHasAmount(otherExpenseDetail);
            return isValid(otherExpenseDetail);
        }

        public bool ValidUpdateObject(OtherExpenseDetail otherExpenseDetail, IOtherExpenseService _otherExpenseService, IEmployeeService _employeeService)
        {
            otherExpenseDetail.Errors.Clear();
            ValidCreateObject(otherExpenseDetail, _otherExpenseService, _employeeService);
            return isValid(otherExpenseDetail);
        }

        public bool ValidDeleteObject(OtherExpenseDetail otherExpenseDetail)
        {
            otherExpenseDetail.Errors.Clear();
            return isValid(otherExpenseDetail);
        }

        public bool isValid(OtherExpenseDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(OtherExpenseDetail obj)
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
