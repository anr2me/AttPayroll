using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ITHRRepository : IRepository<THR>
    {
        IQueryable<THR> GetQueryable();
        IList<THR> GetAll();
        THR GetObjectById(int Id);
        THR CreateObject(THR thr);
        THR UpdateObject(THR thr);
        THR SoftDeleteObject(THR thr);
        bool DeleteObject(int Id);
    }
}