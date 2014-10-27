using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ITHRDetailValidator
    {

        bool ValidCreateObject(THRDetail thrDetail, ITHRService _thrService, IEmployeeService _employeeService, ITHRDetailService _thrDetailService);
        bool ValidUpdateObject(THRDetail thrDetail, ITHRService _thrService, IEmployeeService _employeeService, ITHRDetailService _thrDetailService);
        bool ValidDeleteObject(THRDetail thrDetail);
        bool isValid(THRDetail thrDetail);
        string PrintError(THRDetail thrDetail);
    }
}