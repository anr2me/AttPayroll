using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IDivisionRepository : IRepository<Division>
    {
        IQueryable<Division> GetQueryable();
        IList<Division> GetAll();
        Division GetObjectById(int Id);
        Division GetObjectByName(string Name);
        Division CreateObject(Division division);
        Division UpdateObject(Division division);
        Division SoftDeleteObject(Division division);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(Division division);
    }
}