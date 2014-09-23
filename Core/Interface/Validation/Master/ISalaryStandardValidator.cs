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
        SalaryStandard VCreateObject(SalaryStandard salaryStandard, ISalaryStandardService _salaryStandardService);
        SalaryStandard VUpdateObject(SalaryStandard salaryStandard, ISalaryStandardService _salaryStandardService);
        SalaryStandard VDeleteObject(SalaryStandard salaryStandard);
        bool ValidCreateObject(SalaryStandard salaryStandard, ISalaryStandardService _salaryStandardService);
        bool ValidUpdateObject(SalaryStandard salaryStandard, ISalaryStandardService _salaryStandardService);
        bool ValidDeleteObject(SalaryStandard salaryStandard);
        bool isValid(SalaryStandard salaryStandard);
        string PrintError(SalaryStandard salaryStandard);
    }
}