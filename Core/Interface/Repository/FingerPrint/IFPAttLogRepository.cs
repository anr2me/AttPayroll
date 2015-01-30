using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IFPAttLogRepository : IRepository<FPAttLog>
    {
        IQueryable<FPAttLog> GetQueryable();
        IList<FPAttLog> GetAll();
        FPAttLog GetObjectById(int Id);
        FPAttLog CreateObject(FPAttLog fpAttLog);
        FPAttLog UpdateObject(FPAttLog fpAttLog);
        FPAttLog SoftDeleteObject(FPAttLog fpAttLog);
        bool DeleteObject(int Id);
    }
}