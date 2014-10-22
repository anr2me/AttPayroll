using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalaryStandardDetailRepository : IRepository<SalaryStandardDetail>
    {
        IQueryable<SalaryStandardDetail> GetQueryable();
        IList<SalaryStandardDetail> GetAll();
        IList<SalaryStandardDetail> GetObjectsByTitleInfoId(int TitleInfoId);
        SalaryStandardDetail GetObjectById(int Id);
        SalaryStandardDetail CreateObject(SalaryStandardDetail salaryStandardDetail);
        SalaryStandardDetail UpdateObject(SalaryStandardDetail salaryStandardDetail);
        SalaryStandardDetail SoftDeleteObject(SalaryStandardDetail salaryStandardDetail);
        bool DeleteObject(int Id);
    }
}