using Core.DomainModel;
using Core.Interface.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data.Context;
using Data.Repository;
using System.Data;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace Data.Repository
{
    public class SalaryStandardDetailRepository : EfRepository<SalaryStandardDetail>, ISalaryStandardDetailRepository
    {
        private AttPayrollEntities entities;
        public SalaryStandardDetailRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<SalaryStandardDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalaryStandardDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public IList<SalaryStandardDetail> GetObjectsByTitleInfoId(int TitleInfoId)
        {
            return FindAll().Include("SalaryStandard").Where(x => !x.IsDeleted && x.SalaryStandard.TitleInfoId == TitleInfoId).ToList();
        }

        public SalaryStandardDetail GetObjectById(int Id)
        {
            SalaryStandardDetail salaryStandardDetail = FindAll(x => x.Id == Id && !x.IsDeleted).Include("SalaryStandard").Include("SalaryItem").FirstOrDefault();
            if (salaryStandardDetail != null) { salaryStandardDetail.Errors = new Dictionary<string, string>(); }
            return salaryStandardDetail;
        }

        public SalaryStandardDetail CreateObject(SalaryStandardDetail salaryStandardDetail)
        {
            salaryStandardDetail.IsDeleted = false;
            salaryStandardDetail.CreatedAt = DateTime.Now;
            return Create(salaryStandardDetail);
        }

        public SalaryStandardDetail UpdateObject(SalaryStandardDetail salaryStandardDetail)
        {
            salaryStandardDetail.UpdatedAt = DateTime.Now;
            Update(salaryStandardDetail);
            return salaryStandardDetail;
        }

        public SalaryStandardDetail SoftDeleteObject(SalaryStandardDetail salaryStandardDetail)
        {
            salaryStandardDetail.IsDeleted = true;
            salaryStandardDetail.DeletedAt = DateTime.Now;
            Update(salaryStandardDetail);
            return salaryStandardDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalaryStandardDetail salaryStandardDetail = Find(x => x.Id == Id);
            return (Delete(salaryStandardDetail) == 1) ? true : false;
        }

        
    }
}