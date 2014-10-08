using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISalarySlipValidator
    {

        bool ValidCreateObject(SalarySlip salarySlip, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService);
        bool ValidUpdateObject(SalarySlip salarySlip, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService);
        bool ValidDeleteObject(SalarySlip salarySlip);
        bool isValid(SalarySlip salarySlip);
        string PrintError(SalarySlip salarySlip);
    }
}