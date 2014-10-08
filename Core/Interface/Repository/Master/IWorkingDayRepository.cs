using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IWorkingDayRepository : IRepository<WorkingDay>
    {
        IQueryable<WorkingDay> GetQueryable();
        IList<WorkingDay> GetAll();
        WorkingDay GetObjectById(int Id);
        WorkingDay CreateObject(WorkingDay workingDay);
        WorkingDay UpdateObject(WorkingDay workingDay);
        WorkingDay SoftDeleteObject(WorkingDay workingDay);
        bool DeleteObject(int Id);
    }
}