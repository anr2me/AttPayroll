using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class FPUserValidator : IFPUserValidator
    {
        public FPUser VHasEmployee(FPUser fpUser, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(fpUser.EmployeeId.GetValueOrDefault());
            if (employee == null)
            {
                fpUser.Errors.Add("Employee", "Tidak ada");
            }
            return fpUser;
        }

        public FPUser VHasUniquePIN(FPUser fpUser, IFPUserService _fpUserService)
        {
            if (fpUser.PIN < 0 || fpUser.PIN > 65535)
            {
                fpUser.Errors.Add("PIN", "Harus antara 0 sampai 65535");
            }
            else if (_fpUserService.IsPINDuplicated(fpUser))
            {
                fpUser.Errors.Add("PIN", "Tidak boleh ada duplikasi");
            }
            return fpUser;
        }

        public bool ValidCreateObject(FPUser fpUser, IFPUserService _fpUserService, IEmployeeService _employeeService)
        {
            //VHasEmployee(fpUser, _employeeService);
            //if (!isValid(fpUser)) { return false; }
            VHasUniquePIN(fpUser, _fpUserService);
            return isValid(fpUser);
        }

        public bool ValidUpdateObject(FPUser fpUser, IFPUserService _fpUserService, IEmployeeService _employeeService)
        {
            fpUser.Errors.Clear();
            ValidCreateObject(fpUser, _fpUserService, _employeeService);
            return isValid(fpUser);
        }

        public bool ValidDeleteObject(FPUser fpUser)
        {
            fpUser.Errors.Clear();
            return isValid(fpUser);
        }

        public bool isValid(FPUser obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(FPUser obj)
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
