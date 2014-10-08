using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SalaryEmployeeDetailValidator : ISalaryEmployeeDetailValidator
    {
        public SalaryEmployeeDetail VHasSalaryEmployee(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeService _salaryEmployeeService)
        {
            SalaryEmployee salaryEmployee = _salaryEmployeeService.GetObjectById(salaryEmployeeDetail.SalaryEmployeeId);
            if (salaryEmployee == null)
            {
                salaryEmployeeDetail.Errors.Add("SalaryEmployee", "Tidak ada");
            }
            return salaryEmployeeDetail;
        }

        public SalaryEmployeeDetail VHasSalaryItem(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(salaryEmployeeDetail.SalaryItemId);
            if (salaryItem == null)
            {
                salaryEmployeeDetail.Errors.Add("SalaryItem", "Tidak ada");
            }
            return salaryEmployeeDetail;
        }

        public SalaryEmployeeDetail VHasAmount(SalaryEmployeeDetail salaryEmployeeDetail)
        {
            if (salaryEmployeeDetail.Amount < 0)
            {
                salaryEmployeeDetail.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return salaryEmployeeDetail;
        }

        public bool ValidCreateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeService _salaryEmployeeService, ISalaryItemService _salaryItemService)
        {
            VHasSalaryEmployee(salaryEmployeeDetail, _salaryEmployeeService);
            if (!isValid(salaryEmployeeDetail)) { return false; }
            VHasSalaryItem(salaryEmployeeDetail, _salaryItemService);
            if (!isValid(salaryEmployeeDetail)) { return false; }
            VHasAmount(salaryEmployeeDetail);
            return isValid(salaryEmployeeDetail);
        }

        public bool ValidUpdateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeService _salaryEmployeeService, ISalaryItemService _salaryItemService)
        {
            salaryEmployeeDetail.Errors.Clear();
            ValidCreateObject(salaryEmployeeDetail, _salaryEmployeeService, _salaryItemService);
            return isValid(salaryEmployeeDetail);
        }

        public bool ValidDeleteObject(SalaryEmployeeDetail salaryEmployeeDetail)
        {
            salaryEmployeeDetail.Errors.Clear();
            return isValid(salaryEmployeeDetail);
        }

        public bool isValid(SalaryEmployeeDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalaryEmployeeDetail obj)
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
