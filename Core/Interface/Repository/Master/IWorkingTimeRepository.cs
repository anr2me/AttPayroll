using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IWorkingTimeRepository : IRepository<WorkingTime>
    {
        IQueryable<WorkingTime> GetQueryable();
        IList<WorkingTime> GetAll();
        WorkingTime GetObjectById(int Id);
        WorkingTime CreateObject(WorkingTime workingTime);
        WorkingTime UpdateObject(WorkingTime workingTime);
        WorkingTime SoftDeleteObject(WorkingTime workingTime);
        bool DeleteObject(int Id);
    }
}