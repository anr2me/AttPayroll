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
    public class SalaryItemRepository : EfRepository<SalaryItem>, ISalaryItemRepository
    {
        private AttPayrollEntities entities;
        public SalaryItemRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<SalaryItem> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<SalaryItem> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public SalaryItem GetObjectById(int Id)
        {
            SalaryItem salaryItem = Find(x => x.Id == Id && !x.IsDeleted);
            if (salaryItem != null) { salaryItem.Errors = new Dictionary<string, string>(); }
            return salaryItem;
        }

        public SalaryItem GetObjectByCode(string Code)
        {
            return FindAll(x => x.Code == Code && !x.IsDeleted).FirstOrDefault();
        }

        public SalaryItem CreateObject(SalaryItem salaryItem)
        {
            salaryItem.IsDeleted = false;
            salaryItem.CreatedAt = DateTime.Now;
            return Create(salaryItem);
        }

        public SalaryItem UpdateObject(SalaryItem salaryItem)
        {
            salaryItem.UpdatedAt = DateTime.Now;
            Update(salaryItem);
            return salaryItem;
        }

        public SalaryItem SoftDeleteObject(SalaryItem salaryItem)
        {
            salaryItem.IsDeleted = true;
            salaryItem.DeletedAt = DateTime.Now;
            Update(salaryItem);
            return salaryItem;
        }

        public bool DeleteObject(int Id)
        {
            SalaryItem salaryItem = Find(x => x.Id == Id);
            return (Delete(salaryItem) == 1) ? true : false;
        }

        public bool IsCodeDuplicated(SalaryItem salaryItem)
        {
            IQueryable<SalaryItem> salaryItems = FindAll(x => x.Code == salaryItem.Code && !x.IsDeleted && x.Id != salaryItem.Id);
            return (salaryItems.Count() > 0 ? true : false);
        }
    }
}