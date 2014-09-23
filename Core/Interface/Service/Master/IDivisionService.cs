using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IDivisionService
    {
        IDivisionValidator GetValidator();
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