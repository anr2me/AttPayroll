using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IFPAttLogService
    {
        IFPAttLogValidator GetValidator();
        IQueryable<FPAttLog> GetQueryable();
        IList<FPAttLog> GetAll();
        FPAttLog GetObjectById(int Id);
        FPAttLog GetObjectByUserTime(int FPUserId, DateTime time);
        FPAttLog CreateObject(FPAttLog fpAttLog, IFPUserService _fpUserService);
        FPAttLog UpdateObject(FPAttLog fpAttLog, IFPUserService _fpUserService);
        FPAttLog FindOrCreateObject(FPAttLog fpAttLog, IFPUserService _fpUserService);
        FPAttLog SoftDeleteObject(FPAttLog fpAttLog);
        bool DeleteObject(int Id);
        bool IsUserTimeDuplicated(FPAttLog fpAttLog);
    }
}