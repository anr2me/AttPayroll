using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;
using System.Data.Entity;

namespace Data.Repository
{
    public class TitleInfoRepository : EfRepository<TitleInfo>, ITitleInfoRepository
    {
        private AttPayrollEntities entities;
        public TitleInfoRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<TitleInfo> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<TitleInfo> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public TitleInfo GetObjectById(int Id)
        {
            TitleInfo titleInfo = Find(x => x.Id == Id && !x.IsDeleted);
            if (titleInfo != null) { titleInfo.Errors = new Dictionary<string, string>(); }
            return titleInfo;
        }

        public TitleInfo GetObjectByCode(string Code)
        {
            return FindAll(x => x.Code == Code && !x.IsDeleted).FirstOrDefault();
        }

        public TitleInfo CreateObject(TitleInfo titleInfo)
        {
            titleInfo.IsDeleted = false;
            titleInfo.CreatedAt = DateTime.Now;
            return Create(titleInfo);
        }

        public TitleInfo UpdateObject(TitleInfo titleInfo)
        {
            titleInfo.UpdatedAt = DateTime.Now;
            Update(titleInfo);
            return titleInfo;
        }

        public TitleInfo SoftDeleteObject(TitleInfo titleInfo)
        {
            titleInfo.IsDeleted = true;
            titleInfo.DeletedAt = DateTime.Now;
            Update(titleInfo);
            return titleInfo;
        }

        public bool DeleteObject(int Id)
        {
            TitleInfo titleInfo = Find(x => x.Id == Id);
            return (Delete(titleInfo) == 1) ? true : false;
        }

        public bool IsCodeDuplicated(TitleInfo titleInfo)
        {
            IQueryable<TitleInfo> titleInfos = FindAll(x => x.Code == titleInfo.Code && !x.IsDeleted && x.Id != titleInfo.Id);
            return (titleInfos.Count() > 0 ? true : false);
        }
    }
}