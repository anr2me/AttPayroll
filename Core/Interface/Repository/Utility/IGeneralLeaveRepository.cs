using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IGeneralLeaveRepository : IRepository<GeneralLeave>
    {
        IQueryable<GeneralLeave> GetQueryable();
        IList<GeneralLeave> GetAll();
        GeneralLeave GetObjectById(int Id);
        GeneralLeave CreateObject(GeneralLeave generalLeave);
        GeneralLeave UpdateObject(GeneralLeave generalLeave);
        GeneralLeave SoftDeleteObject(GeneralLeave generalLeave);
        bool DeleteObject(int Id);
    }
}