using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalarySlipDetailService
    {
        ISalarySlipDetailValidator GetValidator();
        IQueryable<SalarySlipDetail> GetQueryable();
        IList<SalarySlipDetail> GetAll();
        SalarySlipDetail GetObjectById(int Id);
        SalarySlipDetail CreateObject(SalarySlipDetail salarySlipDetail);
        SalarySlipDetail UpdateObject(SalarySlipDetail salarySlipDetail);
        SalarySlipDetail SoftDeleteObject(SalarySlipDetail salarySlipDetail);
        bool DeleteObject(int Id);
    }
}