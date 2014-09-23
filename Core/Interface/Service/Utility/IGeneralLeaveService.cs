using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IGeneralLeaveService
    {
        IGeneralLeaveValidator GetValidator();
        IQueryable<GeneralLeave> GetQueryable();
        IList<GeneralLeave> GetAll();
        GeneralLeave GetObjectById(int Id);
        GeneralLeave CreateObject(GeneralLeave generalLeave);
        GeneralLeave UpdateObject(GeneralLeave generalLeave);
        GeneralLeave SoftDeleteObject(GeneralLeave generalLeave);
        bool DeleteObject(int Id);
    }
}