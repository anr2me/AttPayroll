using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IManualAttendanceService
    {
        IManualAttendanceValidator GetValidator();
        IQueryable<ManualAttendance> GetQueryable();
        IList<ManualAttendance> GetAll();
        ManualAttendance GetObjectById(int Id);
        ManualAttendance CreateObject(ManualAttendance manualAttendance);
        ManualAttendance UpdateObject(ManualAttendance manualAttendance);
        ManualAttendance SoftDeleteObject(ManualAttendance manualAttendance);
        bool DeleteObject(int Id);
    }
}