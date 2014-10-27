using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITHRDetailRepository : IRepository<THRDetail>
    {
        IQueryable<THRDetail> GetQueryable();
        IList<THRDetail> GetAll();
        THRDetail GetObjectById(int Id);
        THRDetail CreateObject(THRDetail thrDetail);
        THRDetail UpdateObject(THRDetail thrDetail);
        THRDetail SoftDeleteObject(THRDetail thrDetail);
        bool DeleteObject(int Id);
    }
}