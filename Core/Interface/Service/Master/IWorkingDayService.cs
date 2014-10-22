using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IWorkingDayService
    {
        IWorkingDayValidator GetValidator();
        IQueryable<WorkingDay> GetQueryable();
        IList<WorkingDay> GetAll();
        IList<WorkingDay> GetObjectsByWorkingTimeId(int WorkingTimeId);
        WorkingDay GetObjectById(int Id);
        WorkingDay GetObjectByCode(string code);
        WorkingDay CreateOrUpdateObject(WorkingDay workingDay, IWorkingTimeService _workingTimeService);
        WorkingDay CreateObject(WorkingDay workingDay, IWorkingTimeService _workingTimeService);
        WorkingDay UpdateObject(WorkingDay workingDay, IWorkingTimeService _workingTimeService);
        WorkingDay SoftDeleteObject(WorkingDay workingDay);
        bool DeleteObject(int Id);
    }
}