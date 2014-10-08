using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class EmployeeLoanDetailValidator : IEmployeeLoanDetailValidator
    {
        public EmployeeLoanDetail VHasEmployeeLoan(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanService _employeeLoanService)
        {
            EmployeeLoan employeeLoan = _employeeLoanService.GetObjectById(employeeLoanDetail.EmployeeLoanId);
            if (employeeLoan == null)
            {
                employeeLoanDetail.Errors.Add("EmployeeLoan", "Tidak ada");
            }
            return employeeLoanDetail;
        }

        public EmployeeLoanDetail VIsValidAmount(EmployeeLoanDetail employeeLoanDetail)
        {
            if (employeeLoanDetail.Amount < 0)
            {
                employeeLoanDetail.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return employeeLoanDetail;
        }

        public EmployeeLoanDetail VIsValidAmountPaid(EmployeeLoanDetail employeeLoanDetail)
        {
            if (employeeLoanDetail.AmountPaid < 0)
            {
                employeeLoanDetail.Errors.Add("AmountPaid", "Harus lebih besar atau sama dengan 0");
            }
            return employeeLoanDetail;
        }

        public bool ValidCreateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanService _employeeLoanService)
        {
            VHasEmployeeLoan(employeeLoanDetail, _employeeLoanService);
            if (!isValid(employeeLoanDetail)) { return false; }
            VIsValidAmount(employeeLoanDetail);
            if (!isValid(employeeLoanDetail)) { return false; }
            VIsValidAmountPaid(employeeLoanDetail);
            return isValid(employeeLoanDetail);
        }

        public bool ValidUpdateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanService _employeeLoanService)
        {
            employeeLoanDetail.Errors.Clear();
            ValidCreateObject(employeeLoanDetail, _employeeLoanService);
            return isValid(employeeLoanDetail);
        }

        public bool ValidDeleteObject(EmployeeLoanDetail employeeLoanDetail)
        {
            employeeLoanDetail.Errors.Clear();
            return isValid(employeeLoanDetail);
        }

        public bool isValid(EmployeeLoanDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(EmployeeLoanDetail obj)
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
