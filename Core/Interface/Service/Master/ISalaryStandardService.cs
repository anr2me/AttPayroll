using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalaryStandardService
    {
        ISalaryStandardValidator GetValidator();
        IQueryable<SalaryStandard> GetQueryable();
        IList<SalaryStandard> GetAll();
        SalaryStandard GetObjectById(int Id);
        SalaryStandard CreateObject(SalaryStandard salaryStandard);
        SalaryStandard UpdateObject(SalaryStandard salaryStandard);
        SalaryStandard SoftDeleteObject(SalaryStandard salaryStandard);
        bool DeleteObject(int Id);
    }
}