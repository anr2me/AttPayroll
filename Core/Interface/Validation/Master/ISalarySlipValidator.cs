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
        SalarySlip VCreateObject(SalarySlip salarySlip, ISalarySlipService _salarySlipService);
        SalarySlip VUpdateObject(SalarySlip salarySlip, ISalarySlipService _salarySlipService);
        SalarySlip VDeleteObject(SalarySlip salarySlip);
        bool ValidCreateObject(SalarySlip salarySlip, ISalarySlipService _salarySlipService);
        bool ValidUpdateObject(SalarySlip salarySlip, ISalarySlipService _salarySlipService);
        bool ValidDeleteObject(SalarySlip salarySlip);
        bool isValid(SalarySlip salarySlip);
        string PrintError(SalarySlip salarySlip);
    }
}