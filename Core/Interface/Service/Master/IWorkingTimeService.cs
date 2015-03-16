using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IWorkingTimeService
    {
        IWorkingTimeValidator GetValidator();
        IQueryable<WorkingTime> GetQueryable();
        IList<WorkingTime> GetAll();
        WorkingTime GetObjectById(int Id);
        WorkingTime GetObjectByCode(string code);
        WorkingTime CreateObject(WorkingTime workingTime, IWorkingDayService _workingDayService);
        WorkingTime UpdateObject(WorkingTime workingTime, IWorkingDayService _workingDayService);
        WorkingTime SoftDeleteObject(WorkingTime workingTime, IEmployeeWorkingTimeService _employeeWorkingTimeService);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(WorkingTime workingTime);
        DateTime SetTimeZone(DateTime dateTime, TimeZoneInfo timeZoneInfo, decimal additionalMinutesOffset = 0);
    }
}