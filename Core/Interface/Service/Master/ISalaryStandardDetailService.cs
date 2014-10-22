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
        IList<SalaryStandardDetail> GetObjectsByTitleInfoId(int TitleInfoId);
        SalaryStandardDetail GetObjectBySalaryItemId(int SalaryItemId, int SalaryStandardId);
        SalaryStandardDetail GetObjectById(int Id);
        SalaryStandardDetail CreateObject(SalaryStandardDetail salaryStandardDetail, ISalaryStandardService _salaryStandardService, ISalaryItemService _salaryItemService);
        SalaryStandardDetail UpdateObject(SalaryStandardDetail salaryStandardDetail, ISalaryStandardService _salaryStandardService, ISalaryItemService _salaryItemService);
        SalaryStandardDetail SoftDeleteObject(SalaryStandardDetail salaryStandardDetail);
        bool DeleteObject(int Id);
    }
}