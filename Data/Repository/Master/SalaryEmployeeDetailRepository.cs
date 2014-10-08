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
    public class SalaryEmployeeDetailRepository : EfRepository<SalaryEmployeeDetail>, ISalaryEmployeeDetailRepository
    {
        private AttPayrollEntities entities;
        public SalaryEmployeeDetailRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<SalaryEmployeeDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalaryEmployeeDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public SalaryEmployeeDetail GetObjectById(int Id)
        {
            SalaryEmployeeDetail salaryEmployeeDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (salaryEmployeeDetail != null) { salaryEmployeeDetail.Errors = new Dictionary<string, string>(); }
            return salaryEmployeeDetail;
        }

        public SalaryEmployeeDetail CreateObject(SalaryEmployeeDetail salaryEmployeeDetail)
        {
            salaryEmployeeDetail.IsDeleted = false;
            salaryEmployeeDetail.CreatedAt = DateTime.Now;
            return Create(salaryEmployeeDetail);
        }

        public SalaryEmployeeDetail UpdateObject(SalaryEmployeeDetail salaryEmployeeDetail)
        {
            salaryEmployeeDetail.UpdatedAt = DateTime.Now;
            Update(salaryEmployeeDetail);
            return salaryEmployeeDetail;
        }

        public SalaryEmployeeDetail SoftDeleteObject(SalaryEmployeeDetail salaryEmployeeDetail)
        {
            salaryEmployeeDetail.IsDeleted = true;
            salaryEmployeeDetail.DeletedAt = DateTime.Now;
            Update(salaryEmployeeDetail);
            return salaryEmployeeDetail;
        }

        public bool DeleteObject(int Id)
        {
            SalaryEmployeeDetail salaryEmployeeDetail = Find(x => x.Id == Id);
            return (Delete(salaryEmployeeDetail) == 1) ? true : false;
        }

        
    }
}