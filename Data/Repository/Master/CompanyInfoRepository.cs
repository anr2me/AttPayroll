using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;

namespace Data.Repository
{
    public class CompanyInfoRepository : EfRepository<CompanyInfo>, ICompanyInfoRepository
    {
        private AttPayrollEntities entities;
        public CompanyInfoRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<CompanyInfo> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<CompanyInfo> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public CompanyInfo GetObjectById(int Id)
        {
            CompanyInfo companyInfo = Find(x => x.Id == Id && !x.IsDeleted);
            if (companyInfo != null) { companyInfo.Errors = new Dictionary<string, string>(); }
            return companyInfo;
        }

        public CompanyInfo GetObjectByName(string Name)
        {
            return FindAll(x => x.Name == Name && !x.IsDeleted).FirstOrDefault();
        }

        public CompanyInfo CreateObject(CompanyInfo companyInfo)
        {
            companyInfo.IsDeleted = false;
            companyInfo.CreatedAt = DateTime.Now;
            return Create(companyInfo);
        }

        public CompanyInfo UpdateObject(CompanyInfo companyInfo)
        {
            companyInfo.UpdatedAt = DateTime.Now;
            Update(companyInfo);
            return companyInfo;
        }

        public CompanyInfo SoftDeleteObject(CompanyInfo companyInfo)
        {
            companyInfo.IsDeleted = true;
            companyInfo.DeletedAt = DateTime.Now;
            Update(companyInfo);
            return companyInfo;
        }

        public bool DeleteObject(int Id)
        {
            CompanyInfo companyInfo = Find(x => x.Id == Id);
            return (Delete(companyInfo) == 1) ? true : false;
        }

        public bool IsNameDuplicated(CompanyInfo companyInfo)
        {
            IQueryable<CompanyInfo> companyInfos = FindAll(x => x.Name == companyInfo.Name && !x.IsDeleted && x.Id != companyInfo.Id);
            return (companyInfos.Count() > 0 ? true : false);
        }

    }
}