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
        WorkingTime CreateObject(WorkingTime workingTime);
        WorkingTime UpdateObject(WorkingTime workingTime);
        WorkingTime SoftDeleteObject(WorkingTime workingTime);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(WorkingTime workingTime);
    }
}