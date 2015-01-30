using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Repository;

namespace Core.Interface.Service
{
    public interface IFPUserService
    {
        IFPUserValidator GetValidator();
        IFPUserRepository GetRepository();
        IQueryable<FPUser> GetQueryable();
        IList<FPUser> GetAll();
        FPUser GetObjectById(int Id);
        FPUser GetObjectByPIN(int PIN);
        FPUser CreateObject(FPUser fpUser, IEmployeeService _employeeService);
        FPUser UpdateObject(FPUser fpUser, IEmployeeService _employeeService);
        FPUser UpdateOrCreateObject(FPUser fpUser, IEmployeeService _employeeService);
        FPUser SoftDeleteObject(FPUser fpUser);
        bool DeleteObject(int Id);
        bool IsPINDuplicated(FPUser fpUser);
    }
}