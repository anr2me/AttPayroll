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

        bool ValidCreateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanService _employeeLoanService);
        bool ValidUpdateObject(EmployeeLoanDetail employeeLoanDetail, IEmployeeLoanService _employeeLoanService);
        bool ValidDeleteObject(EmployeeLoanDetail employeeLoanDetail);
        bool isValid(EmployeeLoanDetail employeeLoanDetail);
        string PrintError(EmployeeLoanDetail employeeLoanDetail);
    }
}