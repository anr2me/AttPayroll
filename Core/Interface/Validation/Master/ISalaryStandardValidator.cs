using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISalaryStandardValidator
    {

        bool ValidCreateObject(SalaryStandard salaryStandard, ITitleInfoService _titleInfoService);
        bool ValidUpdateObject(SalaryStandard salaryStandard, ITitleInfoService _titleInfoService);
        bool ValidDeleteObject(SalaryStandard salaryStandard);
        bool isValid(SalaryStandard salaryStandard);
        string PrintError(SalaryStandard salaryStandard);
    }
}