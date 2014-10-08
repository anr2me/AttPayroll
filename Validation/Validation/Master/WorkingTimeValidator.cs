using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class WorkingTimeValidator : IWorkingTimeValidator
    {
        public WorkingTime VHasUniqueCode(WorkingTime workingTime, IWorkingTimeService _workingTimeService)
        {
            if (String.IsNullOrEmpty(workingTime.Code) || workingTime.Code.Trim() == "")
            {
                workingTime.Errors.Add("Code", "Tidak boleh kosong");
            }
            else if (_workingTimeService.IsCodeDuplicated(workingTime))
            {
                workingTime.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return workingTime;
        }

        public WorkingTime VHasCheckIn(WorkingTime workingTime)
        {
            if (workingTime.CheckIn == null || workingTime.CheckIn.Equals(DateTime.FromBinary(0)))
            {
                workingTime.Errors.Add("CheckIn", "Tidak valid");
            }
            return workingTime;
        }

        public WorkingTime VHasCheckOut(WorkingTime workingTime)
        {
            if (workingTime.CheckOut == null || workingTime.CheckOut.Equals(DateTime.FromBinary(0)))
            {
                workingTime.Errors.Add("CheckOut", "Tidak valid");
            }
            return workingTime;
        }

        public bool ValidCreateObject(WorkingTime workingTime, IWorkingTimeService _workingTimeService)
        {
            VHasUniqueCode(workingTime, _workingTimeService);
            if (!isValid(workingTime)) { return false; }
            VHasCheckIn(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasCheckOut(workingTime);
            return isValid(workingTime);
        }

        public bool ValidUpdateObject(WorkingTime workingTime, IWorkingTimeService _workingTimeService)
        {
            workingTime.Errors.Clear();
            ValidCreateObject(workingTime, _workingTimeService);
            return isValid(workingTime);
        }

        public bool ValidDeleteObject(WorkingTime workingTime)
        {
            workingTime.Errors.Clear();
            return isValid(workingTime);
        }

        public bool isValid(WorkingTime obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(WorkingTime obj)
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
