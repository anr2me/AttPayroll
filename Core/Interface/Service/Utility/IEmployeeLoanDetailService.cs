using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IEmployeeLoanDetailService
    {
        IEmployeeLoanDetailValidator GetValidator();
        IQueryable<EmployeeLoanDetail> GetQueryable();
        IList<EmployeeLoanDetail> GetAll();
        EmployeeLoanDetail GetObjectById(int Id);
        EmployeeLoanDetail CreateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanService _employeeLoanService);
        EmployeeLoanDetail UpdateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanService _employeeLoanService);
        EmployeeLoanDetail SoftDeleteObject(EmployeeLoanDetail employeeLoanDetail);
        bool DeleteObject(int Id);
    }
}