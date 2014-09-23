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
    public class LastEducationRepository : EfRepository<LastEducation>, ILastEducationRepository
    {
        private AttPayrollEntities entities;
        public LastEducationRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<LastEducation> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<LastEducation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public LastEducation GetObjectById(int Id)
        {
            LastEducation lastEducation = Find(x => x.Id == Id && !x.IsDeleted);
            if (lastEducation != null) { lastEducation.Errors = new Dictionary<string, string>(); }
            return lastEducation;
        }

        public LastEducation CreateObject(LastEducation lastEducation)
        {
            lastEducation.IsDeleted = false;
            lastEducation.CreatedAt = DateTime.Now;
            return Create(lastEducation);
        }

        public LastEducation UpdateObject(LastEducation lastEducation)
        {
            lastEducation.UpdatedAt = DateTime.Now;
            Update(lastEducation);
            return lastEducation;
        }

        public LastEducation SoftDeleteObject(LastEducation lastEducation)
        {
            lastEducation.IsDeleted = true;
            lastEducation.DeletedAt = DateTime.Now;
            Update(lastEducation);
            return lastEducation;
        }

        public bool DeleteObject(int Id)
        {
            LastEducation lastEducation = Find(x => x.Id == Id);
            return (Delete(lastEducation) == 1) ? true : false;
        }

        
    }
}