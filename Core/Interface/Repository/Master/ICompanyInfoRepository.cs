using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ICompanyInfoRepository : IRepository<CompanyInfo>
    {
        IQueryable<CompanyInfo> GetQueryable();
        IList<CompanyInfo> GetAll();
        CompanyInfo GetObjectById(int Id);
        CompanyInfo GetObjectByName(string Name);
        CompanyInfo CreateObject(CompanyInfo company);
        CompanyInfo UpdateObject(CompanyInfo company);
        CompanyInfo SoftDeleteObject(CompanyInfo company);
        bool DeleteObject(int Id);
        bool IsNameDuplicated(CompanyInfo company);
    }
}