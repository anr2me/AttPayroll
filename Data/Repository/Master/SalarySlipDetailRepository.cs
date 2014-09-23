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
    public class SalarySlipDetailRepository : EfRepository<SalarySlipDetail>, ISalarySlipDetailRepository
    {
        private AttPayrollEntities entities;
        public SalarySlipDetailRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<SalarySlipDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalarySlipDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public SalarySlipDetail GetObjectById(int Id)
        {
            SalarySlipDetail salarySlipDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (salarySlipDetail != null) { salarySlipDetail.Errors = new Dictionary<string, string>(); }
            return salarySlipDetail;
        }

        public SalarySlipDetail CreateObject(SalarySlipDetail salarySlipDetail)
        {
            salarySlipDetail.IsDeleted = false;
            salarySlipDetail.CreatedAt = DateTime.Now;
            return Create(salarySlipDetail);
        }

        public SalarySlipDetail UpdateObject(SalarySlipDetail salarySlipDetail)
        {
            salarySlipDetail.UpdatedAt = DateTime.Now;
            Update(salarySlipDetail);
            return salarySlipDetail;
        }

        public SalarySlipDetail SoftDeleteObject(SalarySlipDetail salarySlipDetail)
        {
            salarySlipDetail.IsDeleted = true;
            salarySlipDetail.DeletedAt = DateTime.Now;
            Update(salarySlipDetail);
            return salarySlipDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalarySlipDetail salarySlipDetail = Find(x => x.Id == Id);
            return (Delete(salarySlipDetail) == 1) ? true : false;
        }

        
    }
}