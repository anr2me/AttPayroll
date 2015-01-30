using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IFPAttLogValidator
    {

        bool ValidCreateObject(FPAttLog fpAttLog, IFPAttLogService _fpAttLogService, IFPUserService _fpUserService);
        bool ValidUpdateObject(FPAttLog fpAttLog, IFPAttLogService _fpAttLogService, IFPUserService _fpUserService);
        bool ValidDeleteObject(FPAttLog fpAttLog);
        bool isValid(FPAttLog fpAttLog);
        string PrintError(FPAttLog fpAttLog);
    }
}