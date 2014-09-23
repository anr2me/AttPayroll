using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IManualAttendanceValidator
    {
        ManualAttendance VCreateObject(ManualAttendance manualAttendance, IManualAttendanceService _manualAttendanceService);
        ManualAttendance VUpdateObject(ManualAttendance manualAttendance, IManualAttendanceService _manualAttendanceService);
        ManualAttendance VDeleteObject(ManualAttendance manualAttendance);
        bool ValidCreateObject(ManualAttendance manualAttendance, IManualAttendanceService _manualAttendanceService);
        bool ValidUpdateObject(ManualAttendance manualAttendance, IManualAttendanceService _manualAttendanceService);
        bool ValidDeleteObject(ManualAttendance manualAttendance);
        bool isValid(ManualAttendance manualAttendance);
        string PrintError(ManualAttendance manualAttendance);
    }
}