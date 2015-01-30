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
    public class FPTemplateRepository : EfRepository<FPTemplate>, IFPTemplateRepository
    {
        private AttPayrollEntities entities;
        public FPTemplateRepository()
        {
            entities = new AttPayrollEntities();
            //entities.Configuration.ProxyCreationEnabled = false;
        }

        public IQueryable<FPTemplate> GetQueryable()
        {
            return FindAll(); // (x => !x.IsDeleted)
        }

        public IList<FPTemplate> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public FPTemplate GetObjectById(int Id)
        {
            FPTemplate fpTemplate = FindAll(x => x.Id == Id && !x.IsDeleted).Include(x => x.FPUser).FirstOrDefault();
            if (fpTemplate != null) { fpTemplate.Errors = new Dictionary<string, string>(); }
            return fpTemplate;
        }

        public FPTemplate CreateObject(FPTemplate fpTemplate)
        {
            fpTemplate.IsDeleted = false;
            fpTemplate.CreatedAt = DateTime.Now;
            return Create(fpTemplate);
        }

        public FPTemplate UpdateObject(FPTemplate fpTemplate)
        {
            fpTemplate.UpdatedAt = DateTime.Now;
            Update(fpTemplate);
            return fpTemplate;
        }

        public FPTemplate SoftDeleteObject(FPTemplate fpTemplate)
        {
            fpTemplate.IsDeleted = true;
            fpTemplate.DeletedAt = DateTime.Now;
            Update(fpTemplate);
            return fpTemplate;
        }

        public bool DeleteObject(int Id)
        {
            FPTemplate fpTemplate = Find(x => x.Id == Id);
            return (Delete(fpTemplate) == 1) ? true : false;
        }

        
    }
}