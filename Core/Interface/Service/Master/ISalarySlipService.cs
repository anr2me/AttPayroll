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
        SalarySlip CreateObject(SalarySlip salarySlip);
        SalarySlip UpdateObject(SalarySlip salarySlip);
        SalarySlip SoftDeleteObject(SalarySlip salarySlip);
        bool DeleteObject(int Id);
    }
}