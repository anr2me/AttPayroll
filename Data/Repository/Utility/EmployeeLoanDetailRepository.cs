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
    public class EmployeeLoanDetailRepository : EfRepository<EmployeeLoanDetail>, IEmployeeLoanDetailRepository
    {
        private AttPayrollEntities entities;
        public EmployeeLoanDetailRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<EmployeeLoanDetail> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<EmployeeLoanDetail> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public EmployeeLoanDetail GetObjectById(int Id)
        {
            EmployeeLoanDetail employeeLoanDetail = Find(x => x.Id == Id && !x.IsDeleted);
            if (employeeLoanDetail != null) { employeeLoanDetail.Errors = new Dictionary<string, string>(); }
            return employeeLoanDetail;
        }

        public EmployeeLoanDetail CreateObject(EmployeeLoanDetail employeeLoanDetail)
        {
            employeeLoanDetail.IsDeleted = false;
            employeeLoanDetail.CreatedAt = DateTime.Now;
            return Create(employeeLoanDetail);
        }

        public EmployeeLoanDetail UpdateObject(EmployeeLoanDetail employeeLoanDetail)
        {
            employeeLoanDetail.UpdatedAt = DateTime.Now;
            Update(employeeLoanDetail);
            return employeeLoanDetail;
        }

        public EmployeeLoanDetail SoftDeleteObject(EmployeeLoanDetail employeeLoanDetail)
        {
            employeeLoanDetail.IsDeleted = true;
            employeeLoanDetail.DeletedAt = DateTime.Now;
            Update(employeeLoanDetail);
            return employeeLoanDetail;
        }

        public bool DeleteObject(int Id)
        {
            EmployeeLoanDetail employeeLoanDetail = Find(x => x.Id == Id);
            return (Delete(employeeLoanDetail) == 1) ? true : false;
        }


    }
}