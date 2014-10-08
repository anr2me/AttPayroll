using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISPKLValidator
    {

        bool ValidCreateObject(SPKL spkl, IEmployeeService _employeeService);
        bool ValidUpdateObject(SPKL spkl, IEmployeeService _employeeService);
        bool ValidDeleteObject(SPKL spkl);
        bool isValid(SPKL spkl);
        string PrintError(SPKL spkl);
    }
}