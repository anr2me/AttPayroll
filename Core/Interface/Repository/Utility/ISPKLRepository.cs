using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISPKLRepository : IRepository<SPKL>
    {
        IQueryable<SPKL> GetQueryable();
        IList<SPKL> GetAll();
        SPKL GetObjectById(int Id);
        SPKL CreateObject(SPKL spkl);
        SPKL UpdateObject(SPKL spkl);
        SPKL SoftDeleteObject(SPKL spkl);
        bool DeleteObject(int Id);
    }
}