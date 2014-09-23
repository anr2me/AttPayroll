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
        SalaryStandardDetail VCreateObject(SalaryStandardDetail salaryStandardDetail, ISalaryStandardDetailService _salaryStandardDetailService);
        SalaryStandardDetail VUpdateObject(SalaryStandardDetail salaryStandardDetail, ISalaryStandardDetailService _salaryStandardDetailService);
        SalaryStandardDetail VDeleteObject(SalaryStandardDetail salaryStandardDetail);
        bool ValidCreateObject(SalaryStandardDetail salaryStandardDetail, ISalaryStandardDetailService _salaryStandardDetailService);
        bool ValidUpdateObject(SalaryStandardDetail salaryStandardDetail, ISalaryStandardDetailService _salaryStandardDetailService);
        bool ValidDeleteObject(SalaryStandardDetail salaryStandardDetail);
        bool isValid(SalaryStandardDetail salaryStandardDetail);
        string PrintError(SalaryStandardDetail salaryStandardDetail);
    }
}