using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class WorkingDayValidator : IWorkingDayValidator
    {

        public WorkingDay VHasWorkingTime(WorkingDay workingDay, IWorkingTimeService _workingTimeService)
        {
            WorkingTime workingTime = _workingTimeService.GetObjectById(workingDay.WorkingTimeId);
            if (workingTime == null)
            {
                workingDay.Errors.Add("WorkingTime", "Tidak valid");
            }
            return workingDay;
        }

        public WorkingDay VHasCheckIn(WorkingDay workingDay)
        {
            if (workingDay.CheckIn == null || workingDay.CheckIn.Equals(DateTime.FromBinary(0)))
            {
                workingDay.Errors.Add("CheckIn", "Tidak valid");
            }
            return workingDay;
        }

        public WorkingDay VHasCheckOut(WorkingDay workingDay)
        {
            if (workingDay.CheckOut == null || workingDay.CheckOut.Equals(DateTime.FromBinary(0)))
            {
                workingDay.Errors.Add("CheckOut", "Tidak valid");
            }
            return workingDay;
        }

        public bool ValidCreateObject(WorkingDay workingDay, IWorkingTimeService _workingTimeService)
        {
            VHasWorkingTime(workingDay, _workingTimeService);
            if (!isValid(workingDay)) { return false; }
            VHasCheckIn(workingDay);
            if (!isValid(workingDay)) { return false; }
            VHasCheckOut(workingDay);
            return isValid(workingDay);
        }

        public bool ValidUpdateObject(WorkingDay workingDay, IWorkingTimeService _workingTimeService)
        {
            workingDay.Errors.Clear();
            ValidCreateObject(workingDay, _workingTimeService);
            return isValid(workingDay);
        }

        public bool ValidDeleteObject(WorkingDay workingDay)
        {
            workingDay.Errors.Clear();
            return isValid(workingDay);
        }

        public bool isValid(WorkingDay obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(WorkingDay obj)
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
