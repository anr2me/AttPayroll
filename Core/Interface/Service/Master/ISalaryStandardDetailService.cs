using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalaryStandardDetailService
    {
        ISalaryStandardDetailValidator GetValidator();
        IQueryable<SalaryStandardDetail> GetQueryable();
        IList<SalaryStandardDetail> GetAll();
        SalaryStandardDetail GetObjectById(int Id);
        SalaryStandardDetail CreateObject(SalaryStandardDetail salaryStandardDetail);
        SalaryStandardDetail UpdateObject(SalaryStandardDetail salaryStandardDetail);
        SalaryStandardDetail SoftDeleteObject(SalaryStandardDetail salaryStandardDetail);
        bool DeleteObject(int Id);
    }
}