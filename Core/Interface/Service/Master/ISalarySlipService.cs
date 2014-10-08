using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalarySlipService
    {
        ISalarySlipValidator GetValidator();
        IQueryable<SalarySlip> GetQueryable();
        IList<SalarySlip> GetAll();
        SalarySlip GetObjectById(int Id);
        SalarySlip CreateObject(string Code, string Name, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService);
        SalarySlip CreateObject(SalarySlip salarySlip, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService);
        SalarySlip UpdateObject(SalarySlip salarySlip, ISalarySlipService _salarySlipService, ISalaryItemService _salaryItemService);
        SalarySlip SoftDeleteObject(SalarySlip salarySlip);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(SalarySlip salarySlip);
    }
}