using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IPensionCompensationValidator
    {
        bool ValidCreateObject(PensionCompensation pensionCompensation, IEmployeeService _employeeService);
        bool ValidUpdateObject(PensionCompensation pensionCompensation, IEmployeeService _employeeService);
        bool ValidDeleteObject(PensionCompensation pensionCompensation);
        bool isValid(PensionCompensation pensionCompensation);
        string PrintError(PensionCompensation pensionCompensation);
    }
}