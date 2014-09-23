using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISalarySlipDetailValidator
    {
        SalarySlipDetail VCreateObject(SalarySlipDetail salarySlipDetail, ISalarySlipDetailService _salarySlipDetailService);
        SalarySlipDetail VUpdateObject(SalarySlipDetail salarySlipDetail, ISalarySlipDetailService _salarySlipDetailService);
        SalarySlipDetail VDeleteObject(SalarySlipDetail salarySlipDetail);
        bool ValidCreateObject(SalarySlipDetail salarySlipDetail, ISalarySlipDetailService _salarySlipDetailService);
        bool ValidUpdateObject(SalarySlipDetail salarySlipDetail, ISalarySlipDetailService _salarySlipDetailService);
        bool ValidDeleteObject(SalarySlipDetail salarySlipDetail);
        bool isValid(SalarySlipDetail salarySlipDetail);
        string PrintError(SalarySlipDetail salarySlipDetail);
    }
}