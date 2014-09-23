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
    public class PPH21SPTRepository : EfRepository<PPH21SPT>, IPPH21SPTRepository
    {
        private AttPayrollEntities entities;
        public PPH21SPTRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<PPH21SPT> GetQueryable()
        {
            return FindAll(x => !x.IsDeleted);
        }

        public IList<PPH21SPT> GetAll()
        {
            return FindAll(x => !x.IsDeleted).ToList();
        }

        public PPH21SPT GetObjectById(int Id)
        {
            PPH21SPT pph21spt = Find(x => x.Id == Id && !x.IsDeleted);
            if (pph21spt != null) { pph21spt.Errors = new Dictionary<string, string>(); }
            return pph21spt;
        }

        public PPH21SPT CreateObject(PPH21SPT pph21spt)
        {
            pph21spt.IsDeleted = false;
            pph21spt.CreatedAt = DateTime.Now;
            return Create(pph21spt);
        }

        public PPH21SPT UpdateObject(PPH21SPT pph21spt)
        {
            pph21spt.UpdatedAt = DateTime.Now;
            Update(pph21spt);
            return pph21spt;
        }

        public PPH21SPT SoftDeleteObject(PPH21SPT pph21spt)
        {
            pph21spt.IsDeleted = true;
            pph21spt.DeletedAt = DateTime.Now;
            Update(pph21spt);
            return pph21spt;
        }

        public bool DeleteObject(int Id)
        {
            PPH21SPT pph21spt = Find(x => x.Id == Id);
            return (Delete(pph21spt) == 1) ? true : false;
        }

        
    }
}