using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IEmployeeLoanRepository : IRepository<EmployeeLoan>
    {
        IQueryable<EmployeeLoan> GetQueryable();
        IList<EmployeeLoan> GetAll();
        EmployeeLoan GetObjectById(int Id);
        EmployeeLoan CreateObject(EmployeeLoan employeeLoan);
        EmployeeLoan UpdateObject(EmployeeLoan employeeLoan);
        EmployeeLoan SoftDeleteObject(EmployeeLoan employeeLoan);
        bool DeleteObject(int Id);
    }
}