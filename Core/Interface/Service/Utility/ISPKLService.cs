using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISPKLService
    {
        ISPKLValidator GetValidator();
        IQueryable<SPKL> GetQueryable();
        IList<SPKL> GetAll();
        SPKL GetObjectById(int Id);
        SPKL CreateObject(SPKL spkl, IEmployeeService _employeeService);
        SPKL UpdateObject(SPKL spkl, IEmployeeService _employeeService);
        SPKL SoftDeleteObject(SPKL spkl);
        bool DeleteObject(int Id);
    }
}