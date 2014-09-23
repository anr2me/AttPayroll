using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IManualAttendanceRepository : IRepository<ManualAttendance>
    {
        IQueryable<ManualAttendance> GetQueryable();
        IList<ManualAttendance> GetAll();
        ManualAttendance GetObjectById(int Id);
        ManualAttendance CreateObject(ManualAttendance manualAttendance);
        ManualAttendance UpdateObject(ManualAttendance manualAttendance);
        ManualAttendance SoftDeleteObject(ManualAttendance manualAttendance);
        bool DeleteObject(int Id);
    }
}