using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IEmployeeLoanValidator
    {
        EmployeeLoan VCreateObject(EmployeeLoan employeeLoan, IEmployeeLoanService _employeeLoanService);
        EmployeeLoan VUpdateObject(EmployeeLoan employeeLoan, IEmployeeLoanService _employeeLoanService);
        EmployeeLoan VDeleteObject(EmployeeLoan employeeLoan);
        bool ValidCreateObject(EmployeeLoan employeeLoan, IEmployeeLoanService _employeeLoanService);
        bool ValidUpdateObject(EmployeeLoan employeeLoan, IEmployeeLoanService _employeeLoanService);
        bool ValidDeleteObject(EmployeeLoan employeeLoan);
        bool isValid(EmployeeLoan employeeLoan);
        string PrintError(EmployeeLoan employeeLoan);
    }
}