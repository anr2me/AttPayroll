using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SlipGajiMiniValidator : ISlipGajiMiniValidator
    {
        public SlipGajiMini VHasEmployee(SlipGajiMini slipGajiMini, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(slipGajiMini.EmployeeId); 
            if (employee == null)
            {
                slipGajiMini.Errors.Add("Employee", "Tidak valid");
            }
            return slipGajiMini;
        }

        public SlipGajiMini VHasMonth(SlipGajiMini slipGajiMini)
        {
            if (slipGajiMini.MONTH.Equals(DateTime.FromBinary(0)))
            {
                slipGajiMini.Errors.Add("Month", "Tidak valid");
            }
            return slipGajiMini;
        }

        public bool ValidCreateObject(SlipGajiMini slipGajiMini, IEmployeeService _employeeService)
        {
            VHasEmployee(slipGajiMini, _employeeService);
            if (!isValid(slipGajiMini)) { return false; }
            VHasMonth(slipGajiMini);
            return isValid(slipGajiMini);
        }

        public bool ValidUpdateObject(SlipGajiMini slipGajiMini, IEmployeeService _employeeService)
        {
            slipGajiMini.Errors.Clear();
            ValidCreateObject(slipGajiMini, _employeeService);
            return isValid(slipGajiMini);
        }

        public bool ValidDeleteObject(SlipGajiMini slipGajiMini)
        {
            slipGajiMini.Errors.Clear();
            return isValid(slipGajiMini);
        }

        public bool isValid(SlipGajiMini obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SlipGajiMini obj)
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
