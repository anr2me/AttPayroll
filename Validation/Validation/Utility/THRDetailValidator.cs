using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class THRDetailValidator : ITHRDetailValidator
    {
        public THRDetail VHasTHR(THRDetail thrDetail, ITHRService _thrService)
        {
            THR thr = _thrService.GetObjectById(thrDetail.THRId);
            if (thr == null)
            {
                thrDetail.Errors.Add("THR", "Tidak ada");
            }
            return thrDetail;
        }

        public THRDetail VHasEmployee(THRDetail thrDetail, IEmployeeService _employeeService, ITHRDetailService _thrDetailService)
        {
            Employee employee = _employeeService.GetObjectById(thrDetail.EmployeeId);
            if (employee == null)
            {
                thrDetail.Errors.Add("Employee", "Tidak ada");
            }
            else
            {
                THRDetail thrDetail2 = _thrDetailService.GetObjectByEmployeeIdAndTHRId(thrDetail.EmployeeId, thrDetail.THRId);
                if (thrDetail2 != null)
                {
                    thrDetail.Errors.Add("Employee", "Sudah ada");
                }
            }
            return thrDetail;
        }

        //public THRDetail VHasAmount(THRDetail thrDetail)
        //{
        //    if (thrDetail.Amount < 0)
        //    {
        //        thrDetail.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
        //    }
        //    return thrDetail;
        //}

        public bool ValidCreateObject(THRDetail thrDetail, ITHRService _thrService, IEmployeeService _employeeService, ITHRDetailService _thrDetailService)
        {
            VHasTHR(thrDetail, _thrService);
            if (!isValid(thrDetail)) { return false; }
            VHasEmployee(thrDetail, _employeeService, _thrDetailService);
            //if (!isValid(thrDetail)) { return false; }
            //VHasAmount(thrDetail);
            return isValid(thrDetail);
        }

        public bool ValidUpdateObject(THRDetail thrDetail, ITHRService _thrService, IEmployeeService _employeeService, ITHRDetailService _thrDetailService)
        {
            thrDetail.Errors.Clear();
            ValidCreateObject(thrDetail, _thrService, _employeeService, _thrDetailService);
            return isValid(thrDetail);
        }

        public bool ValidDeleteObject(THRDetail thrDetail)
        {
            thrDetail.Errors.Clear();
            return isValid(thrDetail);
        }

        public bool isValid(THRDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(THRDetail obj)
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
