using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SalaryEmployeeValidator : ISalaryEmployeeValidator
    {

        public SalaryEmployee VHasEmployee(SalaryEmployee salaryEmployee, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(salaryEmployee.EmployeeId);
            if (employee == null)
            {
                salaryEmployee.Errors.Add("Employee", "Tidak ada");
            }
            return salaryEmployee;
        }

        public SalaryEmployee VHasEffectiveDate(SalaryEmployee salaryEmployee, ISalaryEmployeeService _salaryEmployeeService)
        {
            if (salaryEmployee.EffectiveDate == null || salaryEmployee.EffectiveDate.Equals(DateTime.FromBinary(0)))
            {
                salaryEmployee.Errors.Add("EffectiveDate", "Tidak valid");
            }
            else
            {
                SalaryEmployee active = _salaryEmployeeService.GetActiveObject();
                if (active != null && salaryEmployee.EffectiveDate < active.EffectiveDate)
                {
                    salaryEmployee.Errors.Add("EffectiveDate", "Harus lebih besar atau sama dengan SalaryEmployee yang Aktif");
                }
            }
            return salaryEmployee;
        }

        public bool ValidCreateObject(SalaryEmployee salaryEmployee, IEmployeeService _employeeService, ISalaryEmployeeService _salaryEmployeeService)
        {
            VHasEmployee(salaryEmployee, _employeeService);
            if (!isValid(salaryEmployee)) { return false; }
            VHasEffectiveDate(salaryEmployee, _salaryEmployeeService);
            return isValid(salaryEmployee);
        }

        public bool ValidUpdateObject(SalaryEmployee salaryEmployee, IEmployeeService _employeeService, ISalaryEmployeeService _salaryEmployeeService)
        {
            salaryEmployee.Errors.Clear();
            ValidCreateObject(salaryEmployee, _employeeService, _salaryEmployeeService);
            return isValid(salaryEmployee);
        }

        public bool ValidDeleteObject(SalaryEmployee salaryEmployee)
        {
            salaryEmployee.Errors.Clear();
            return isValid(salaryEmployee);
        }

        public bool isValid(SalaryEmployee obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SalaryEmployee obj)
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
