using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPTKPService
    {
        IPTKPValidator GetValidator();
        IQueryable<PTKP> GetQueryable();
        IList<PTKP> GetAll();
        PTKP GetObjectById(int Id);
        PTKP CreateObject(PTKP ptkp);
        PTKP UpdateObject(PTKP ptkp);
        PTKP SoftDeleteObject(PTKP ptkp);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(PTKP ptkp);
    }
}