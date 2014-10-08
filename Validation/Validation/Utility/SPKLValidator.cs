using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SPKLValidator : ISPKLValidator
    {
        public SPKL VHasEmployee(SPKL spkl, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(spkl.EmployeeId);
            if (employee == null)
            {
                spkl.Errors.Add("Employee", "Tidak ada");
            }
            return spkl;
        }

        public SPKL VHasRemark(SPKL spkl)
        {
            if (String.IsNullOrEmpty(spkl.Remark) || spkl.Remark.Trim() == "")
            {
                spkl.Errors.Add("Remark", "Tidak boleh kosong");
            }
            return spkl;
        }

        public SPKL VHasStartTime(SPKL spkl)
        {
            if (spkl.StartTime == null || spkl.StartTime.Equals(DateTime.FromBinary(0)))
            {
                spkl.Errors.Add("StartTime", "Tidak valid");
            }
            return spkl;
        }

        public SPKL VHasEndTime(SPKL spkl)
        {
            if (spkl.EndTime == null || spkl.EndTime.Equals(DateTime.FromBinary(0)))
            {
                spkl.Errors.Add("EndTime", "Tidak valid");
            }
            else if (spkl.EndTime.Date < spkl.StartTime.Date)
            {
                spkl.Errors.Add("EndTime", "Harus lebih besar atau sama dengan Start Time");
            }
            return spkl;
        }

        public bool ValidCreateObject(SPKL spkl, IEmployeeService _employeeService)
        {
            VHasEmployee(spkl, _employeeService);
            if (!isValid(spkl)) { return false; }
            VHasRemark(spkl);
            if (!isValid(spkl)) { return false; }
            VHasStartTime(spkl);
            if (!isValid(spkl)) { return false; }
            VHasEndTime(spkl);
            return isValid(spkl);
        }

        public bool ValidUpdateObject(SPKL spkl, IEmployeeService _employeeService)
        {
            spkl.Errors.Clear();
            ValidCreateObject(spkl, _employeeService);
            return isValid(spkl);
        }

        public bool ValidDeleteObject(SPKL spkl)
        {
            spkl.Errors.Clear();
            return isValid(spkl);
        }

        public bool isValid(SPKL obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SPKL obj)
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
