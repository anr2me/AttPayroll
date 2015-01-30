using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class FPAttLogValidator : IFPAttLogValidator
    {
        public FPAttLog VHasUser(FPAttLog fpAttLog, IFPUserService _fpUserService)
        {
            FPUser fpUser = _fpUserService.GetObjectById(fpAttLog.FPUserId);
            if (fpUser == null)
            {
                fpAttLog.Errors.Add("Generic", "FPUser Tidak ada");
            }
            return fpAttLog;
        }

        public FPAttLog VHasUniqueUserTime(FPAttLog fpAttLog, IFPAttLogService _fpAttLogService)
        {
            if (fpAttLog.Time_second == null || fpAttLog.Time_second.Equals(DateTime.FromBinary(0)))
            {
                fpAttLog.Errors.Add("Generic", "Time_second tidak valid");
            }
            else if (_fpAttLogService.IsUserTimeDuplicated(fpAttLog))
            {
                fpAttLog.Errors.Add("Generic", "Tidak boleh ada duplikasi waktu di user yang sama");
            }
            return fpAttLog;
        }

        public bool ValidCreateObject(FPAttLog fpAttLog, IFPAttLogService _fpAttLogService, IFPUserService _fpUserService)
        {
            VHasUser(fpAttLog, _fpUserService);
            if (!isValid(fpAttLog)) { return false; }
            VHasUniqueUserTime(fpAttLog, _fpAttLogService);
            return isValid(fpAttLog);
        }

        public bool ValidUpdateObject(FPAttLog fpAttLog, IFPAttLogService _fpAttLogService, IFPUserService _fpUserService)
        {
            fpAttLog.Errors.Clear();
            ValidCreateObject(fpAttLog, _fpAttLogService, _fpUserService);
            return isValid(fpAttLog);
        }

        public bool ValidDeleteObject(FPAttLog fpAttLog)
        {
            fpAttLog.Errors.Clear();
            return isValid(fpAttLog);
        }

        public bool isValid(FPAttLog obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(FPAttLog obj)
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
