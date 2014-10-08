using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISalaryStandardDetailValidator
    {

        bool ValidCreateObject(SalaryStandardDetail salaryStandardDetail, ISalaryStandardService _salaryStandardService, ISalaryItemService _salaryItemService);
        bool ValidUpdateObject(SalaryStandardDetail salaryStandardDetail, ISalaryStandardService _salaryStandardService, ISalaryItemService _salaryItemService);
        bool ValidDeleteObject(SalaryStandardDetail salaryStandardDetail);
        bool isValid(SalaryStandardDetail salaryStandardDetail);
        string PrintError(SalaryStandardDetail salaryStandardDetail);
    }
}