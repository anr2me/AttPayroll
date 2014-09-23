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
    public class PensionCompensationRepository : EfRepository<PensionCompensation>, IPensionCompensationRepository
    {
        private AttPayrollEntities entities;
        public PensionCompensationRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<PensionCompensation> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PensionCompensation> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public PensionCompensation GetObjectById(int Id)
        {
            PensionCompensation pensionCompensation = Find(x => x.Id == Id && !x.IsDeleted);
            if (pensionCompensation != null) { pensionCompensation.Errors = new Dictionary<string, string>(); }
            return pensionCompensation;
        }

        public PensionCompensation CreateObject(PensionCompensation pensionCompensation)
        {
            pensionCompensation.IsDeleted = false;
            pensionCompensation.CreatedAt = DateTime.Now;
            return Create(pensionCompensation);
        }

        public PensionCompensation UpdateObject(PensionCompensation pensionCompensation)
        {
            pensionCompensation.UpdatedAt = DateTime.Now;
            Update(pensionCompensation);
            return pensionCompensation;
        }

        public PensionCompensation SoftDeleteObject(PensionCompensation pensionCompensation)
        {
            pensionCompensation.IsDeleted = true;
            pensionCompensation.DeletedAt = DateTime.Now;
            Update(pensionCompensation);
            return pensionCompensation;
        }

        public bool DeleteObject(int Id)
        {
            PensionCompensation pensionCompensation = Find(x => x.Id == Id);
            return (Delete(pensionCompensation) == 1) ? true : false;
        }


    }
}