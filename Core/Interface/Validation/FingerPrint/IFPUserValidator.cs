using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IFPUserValidator
    {

        bool ValidCreateObject(FPUser fpUser, IFPUserService _fpUserService, IEmployeeService _employeeService);
        bool ValidUpdateObject(FPUser fpUser, IFPUserService _fpUserService, IEmployeeService _employeeService);
        bool ValidDeleteObject(FPUser fpUser);
        bool isValid(FPUser fpUser);
        string PrintError(FPUser fpUser);
    }
}