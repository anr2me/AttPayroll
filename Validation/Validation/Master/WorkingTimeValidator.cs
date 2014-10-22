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

        public WorkingTime VHasName(WorkingTime workingTime)
        {
            if (String.IsNullOrEmpty(workingTime.Name) || workingTime.Name.Trim() == "")
            {
                workingTime.Errors.Add("Name", "Tidak boleh kosong");
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

        public WorkingTime VHasMinCheckIn(WorkingTime workingTime)
        {
            if (workingTime.MinCheckIn == null || workingTime.MinCheckIn.Equals(DateTime.FromBinary(0)))
            {
                workingTime.Errors.Add("MinCheckIn", "Tidak valid");
            }
            return workingTime;
        }

        public WorkingTime VHasMinCheckOut(WorkingTime workingTime)
        {
            if (workingTime.MinCheckOut == null || workingTime.MinCheckOut.Equals(DateTime.FromBinary(0)))
            {
                workingTime.Errors.Add("MinCheckOut", "Tidak valid");
            }
            return workingTime;
        }

        public WorkingTime VHasMaxCheckIn(WorkingTime workingTime)
        {
            if (workingTime.MaxCheckIn == null || workingTime.MaxCheckIn.Equals(DateTime.FromBinary(0)))
            {
                workingTime.Errors.Add("MaxCheckIn", "Tidak valid");
            }
            return workingTime;
        }

        public WorkingTime VHasMaxCheckOut(WorkingTime workingTime)
        {
            if (workingTime.MaxCheckOut == null || workingTime.MaxCheckOut.Equals(DateTime.FromBinary(0)))
            {
                workingTime.Errors.Add("MaxCheckOut", "Tidak valid");
            }
            return workingTime;
        }

        public WorkingTime VHasBreakOut(WorkingTime workingTime)
        {
            if (workingTime.BreakOut == null || workingTime.BreakOut.Equals(DateTime.FromBinary(0)))
            {
                workingTime.Errors.Add("BreakOut", "Tidak valid");
            }
            return workingTime;
        }

        public WorkingTime VHasBreakIn(WorkingTime workingTime)
        {
            if (workingTime.BreakIn == null || workingTime.BreakIn.Equals(DateTime.FromBinary(0)))
            {
                workingTime.Errors.Add("BreakIn", "Tidak valid");
            }
            return workingTime;
        }

        public WorkingTime VHasValidMinCheckIn(WorkingTime workingTime)
        {
            if (workingTime.MinCheckIn > workingTime.CheckIn)
            {
                workingTime.Errors.Add("MinCheckIn", "Harus lebih kecil atau sama dengan CheckIn");
            }
            return workingTime;
        }

        public WorkingTime VHasValidMaxCheckIn(WorkingTime workingTime)
        {
            if (workingTime.MaxCheckIn < workingTime.CheckIn)
            {
                workingTime.Errors.Add("MaxCheckIn", "Harus lebih besar atau sama dengan CheckIn");
            }
            return workingTime;
        }

        public WorkingTime VHasValidMinCheckOut(WorkingTime workingTime)
        {
            if (workingTime.MinCheckOut > workingTime.CheckOut || workingTime.MinCheckOut <= workingTime.MaxCheckIn)
            {
                workingTime.Errors.Add("MinCheckOut", "Harus lebih kecil atau sama dengan CheckOut dan setelah MaxCheckIn");
            }
            return workingTime;
        }

        public WorkingTime VHasValidMaxCheckOut(WorkingTime workingTime)
        {
            if (workingTime.MaxCheckOut < workingTime.CheckOut || workingTime.MaxCheckOut >= workingTime.MinCheckIn.AddDays(1))
            {
                workingTime.Errors.Add("MaxCheckOut", "Harus lebih besar atau sama dengan CheckOut dan sebelum MinCheckIn hari berikutnya");
            }
            return workingTime;
        }

        public WorkingTime VHasValidBreakIn(WorkingTime workingTime)
        {
            if (workingTime.BreakIn < workingTime.BreakOut || workingTime.BreakIn >= workingTime.MinCheckOut)
            {
                workingTime.Errors.Add("BreakIn", "Harus lebih besar atau sama dengan BreakOut dan sebelum MinCheckOut");
            }
            return workingTime;
        }

        public WorkingTime VHasValidBreakOut(WorkingTime workingTime)
        {
            if (workingTime.BreakOut > workingTime.BreakIn || workingTime.BreakOut <= workingTime.MaxCheckIn)
            {
                workingTime.Errors.Add("BreakOut", "Harus lebih kecil atau sama dengan BreakIn dan setelah MaxCheckIn");
            }
            return workingTime;
        }

        public WorkingTime VHasValidCheckInTolerance(WorkingTime workingTime)
        {
            if (workingTime.CheckInTolerance < 0)
            {
                workingTime.Errors.Add("CheckInTolerance", "Tidak boleh negatif");
            }
            return workingTime;
        }

        public WorkingTime VHasValidCheckOutTolerance(WorkingTime workingTime)
        {
            if (workingTime.CheckOutTolerance < 0)
            {
                workingTime.Errors.Add("CheckOutTolerance", "Tidak boleh negatif");
            }
            return workingTime;
        }

        public WorkingTime VDontHaveEmployeeWorkingTime(WorkingTime workingTime, IEmployeeWorkingTimeService _employeeWorkingTimeService)
        {
            IList<EmployeeWorkingTime> employeeWorkingTimes = _employeeWorkingTimeService.GetObjectsByWorkingTimeId(workingTime.Id);
            if (employeeWorkingTimes.Any())
            {
                workingTime.Errors.Add("Generic", "Tidak boleh masih terasosiasi dengan EmployeeWorkingTime");
            }
            return workingTime;
        }

        public bool ValidCreateObject(WorkingTime workingTime, IWorkingTimeService _workingTimeService)
        {
            VHasUniqueCode(workingTime, _workingTimeService);
            if (!isValid(workingTime)) { return false; }
            VHasName(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasMinCheckIn(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasCheckIn(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasMaxCheckIn(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasBreakOut(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasBreakIn(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasMinCheckOut(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasCheckOut(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasMaxCheckOut(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasValidCheckInTolerance(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasValidCheckOutTolerance(workingTime);
            if (!isValid(workingTime)) { return false; }
            // Need to fix the day before further validation for correct range checking
            FixWorkingTimeRange(workingTime);
            VHasValidMinCheckIn(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasValidMaxCheckIn(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasValidMinCheckOut(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasValidMaxCheckOut(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasValidBreakOut(workingTime);
            if (!isValid(workingTime)) { return false; }
            VHasValidBreakIn(workingTime);
            return isValid(workingTime);
        }

        public bool ValidUpdateObject(WorkingTime workingTime, IWorkingTimeService _workingTimeService)
        {
            workingTime.Errors.Clear();
            ValidCreateObject(workingTime, _workingTimeService);
            return isValid(workingTime);
        }

        public bool ValidDeleteObject(WorkingTime workingTime, IEmployeeWorkingTimeService _employeeWorkingTimeService)
        {
            workingTime.Errors.Clear();
            VDontHaveEmployeeWorkingTime(workingTime, _employeeWorkingTimeService);
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

        public WorkingTime FixWorkingTimeRange(WorkingTime workingTime)
        {
            // Need to fix the day for correct hours range checking, all dates must not be null
            while (workingTime.CheckIn < workingTime.MinCheckIn)
            {
                workingTime.CheckIn = workingTime.CheckIn.AddDays(1); // workingTime.MinCheckIn.Date + workingTime.CheckIn.TimeOfDay
            }
            while (workingTime.CheckOut < workingTime.MinCheckOut)
            {
                workingTime.CheckOut = workingTime.CheckOut.AddDays(1);
            }

            while (workingTime.CheckOut < workingTime.CheckIn)
            {
                workingTime.CheckOut = workingTime.CheckOut.AddDays(1);
            }
            while (workingTime.BreakIn < workingTime.BreakOut)
            {
                workingTime.BreakIn = workingTime.BreakIn.AddDays(1);
            }

            while (workingTime.MaxCheckIn < workingTime.CheckIn)
            {
                workingTime.MaxCheckIn = workingTime.MaxCheckIn.AddDays(1);
            }
            while (workingTime.MaxCheckOut < workingTime.CheckOut)
            {
                workingTime.MaxCheckOut = workingTime.MaxCheckOut.AddDays(1);
            }
            return workingTime;
        }

    }
}
