using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IEmployeeLoanDetailValidator
    {
        EmployeeLoanDetail VCreateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanDetailService _employeeLoanDetailService);
        EmployeeLoanDetail VUpdateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanDetailService _employeeLoanDetailService);
        EmployeeLoanDetail VDeleteObject(EmployeeLoanDetail employeeLoanDetail);
        bool ValidCreateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanDetailService _employeeLoanDetailService);
        bool ValidUpdateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanDetailService _employeeLoanDetailService);
        bool ValidDeleteObject(EmployeeLoanDetail employeeLoanDetail);
        bool isValid(EmployeeLoanDetail employeeLoanDetail);
        string PrintError(EmployeeLoanDetail employeeLoanDetail);
    }
}