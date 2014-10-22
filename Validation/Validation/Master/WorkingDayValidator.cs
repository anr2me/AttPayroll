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
            WorkingTime workingTime = _workingTimeService.GetObjectById(workingDay.WorkingTimeId.GetValueOrDefault());
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

        public WorkingDay VHasMinCheckIn(WorkingDay workingDay)
        {
            if (workingDay.MinCheckIn == null || workingDay.MinCheckIn.Equals(DateTime.FromBinary(0)))
            {
                workingDay.Errors.Add("MinCheckIn", "Tidak valid");
            }
            return workingDay;
        }

        public WorkingDay VHasMinCheckOut(WorkingDay workingDay)
        {
            if (workingDay.MinCheckOut == null || workingDay.MinCheckOut.Equals(DateTime.FromBinary(0)))
            {
                workingDay.Errors.Add("MinCheckOut", "Tidak valid");
            }
            return workingDay;
        }

        public WorkingDay VHasMaxCheckIn(WorkingDay workingDay)
        {
            if (workingDay.MaxCheckIn == null || workingDay.MaxCheckIn.Equals(DateTime.FromBinary(0)))
            {
                workingDay.Errors.Add("MaxCheckIn", "Tidak valid");
            }
            return workingDay;
        }

        public WorkingDay VHasMaxCheckOut(WorkingDay workingDay)
        {
            if (workingDay.MaxCheckOut == null || workingDay.MaxCheckOut.Equals(DateTime.FromBinary(0)))
            {
                workingDay.Errors.Add("MaxCheckOut", "Tidak valid");
            }
            return workingDay;
        }

        public WorkingDay VHasValidMinCheckIn(WorkingDay workingDay)
        {
            if (workingDay.MinCheckIn > workingDay.CheckIn)
            {
                workingDay.Errors.Add("MinCheckIn", "Harus lebih kecil atau sama dengan CheckIn");
            }
            return workingDay;
        }

        public WorkingDay VHasValidMaxCheckIn(WorkingDay workingDay)
        {
            if (workingDay.MaxCheckIn < workingDay.CheckIn)
            {
                workingDay.Errors.Add("MaxCheckIn", "Harus lebih besar atau sama dengan CheckIn");
            }
            return workingDay;
        }

        public WorkingDay VHasValidMinCheckOut(WorkingDay workingDay)
        {
            if (workingDay.MinCheckOut > workingDay.CheckOut || workingDay.MinCheckOut <= workingDay.MaxCheckIn)
            {
                workingDay.Errors.Add("MinCheckOut", "Harus lebih kecil atau sama dengan CheckOut dan setelah MaxCheckIn");
            }
            return workingDay;
        }

        public WorkingDay VHasValidMaxCheckOut(WorkingDay workingDay)
        {
            if (workingDay.MaxCheckOut < workingDay.CheckOut || workingDay.MaxCheckOut >= workingDay.MinCheckIn.AddDays(1))
            {
                workingDay.Errors.Add("MaxCheckOut", "Harus lebih besar atau sama dengan CheckOut dan sebelum MinCheckIn hari berikutnya");
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
            if (!isValid(workingDay)) { return false; }
            VHasMinCheckIn(workingDay);
            if (!isValid(workingDay)) { return false; }
            VHasMinCheckOut(workingDay);
            if (!isValid(workingDay)) { return false; }
            VHasMaxCheckIn(workingDay);
            if (!isValid(workingDay)) { return false; }
            VHasMaxCheckOut(workingDay);
            if (!isValid(workingDay)) { return false; }
            // Need to fix the day before further validation for correct range checking
            FixWorkingDayRange(workingDay);
            VHasValidMinCheckIn(workingDay);
            if (!isValid(workingDay)) { return false; }
            VHasValidMaxCheckIn(workingDay);
            if (!isValid(workingDay)) { return false; }
            VHasValidMinCheckOut(workingDay);
            if (!isValid(workingDay)) { return false; }
            VHasValidMaxCheckOut(workingDay);
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

        public WorkingDay FixWorkingDayRange(WorkingDay workingDay)
        {
            // Need to fix the day for correct hours range checking, all dates must not be null
            while (workingDay.CheckIn < workingDay.MinCheckIn)
            {
                workingDay.CheckIn = workingDay.CheckIn.AddDays(1); // workingDay.MinCheckIn.Date + workingDay.CheckIn.TimeOfDay
            }
            while (workingDay.CheckOut < workingDay.MinCheckOut)
            {
                workingDay.CheckOut = workingDay.CheckOut.AddDays(1);
            }

            while (workingDay.CheckOut < workingDay.CheckIn)
            {
                workingDay.CheckOut = workingDay.CheckOut.AddDays(1);
            }
            while (workingDay.BreakIn < workingDay.BreakOut)
            {
                workingDay.BreakIn = workingDay.BreakIn.AddDays(1);
            }

            while (workingDay.MaxCheckIn < workingDay.CheckIn)
            {
                workingDay.MaxCheckIn = workingDay.MaxCheckIn.AddDays(1);
            }
            while (workingDay.MaxCheckOut < workingDay.CheckOut)
            {
                workingDay.MaxCheckOut = workingDay.MaxCheckOut.AddDays(1);
            }
            return workingDay;
        }

    }
}
