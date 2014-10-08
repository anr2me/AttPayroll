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
    public class SlipGajiDetail2ARepository : EfRepository<SlipGajiDetail2A>, ISlipGajiDetail2ARepository
    {
        private AttPayrollEntities entities;
        public SlipGajiDetail2ARepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<SlipGajiDetail2A> GetQueryable()
        {
            return FindAll();
        }

        public IList<SlipGajiDetail2A> GetAll()
        {
            return FindAll().ToList();
        }

        public SlipGajiDetail2A GetObjectById(int Id)
        {
            SlipGajiDetail2A slipGajiDetail2A = Find(x => x.Id == Id);
            if (slipGajiDetail2A != null) { slipGajiDetail2A.Errors = new Dictionary<string, string>(); }
            return slipGajiDetail2A;
        }

        public SlipGajiDetail2A CreateObject(SlipGajiDetail2A slipGajiDetail2A)
        {
            return Create(slipGajiDetail2A);
        }

        public SlipGajiDetail2A UpdateObject(SlipGajiDetail2A slipGajiDetail2A)
        {
            Update(slipGajiDetail2A);
            return slipGajiDetail2A;
        }

        public bool DeleteObject(int Id)
        {
            SlipGajiDetail2A slipGajiDetail2A = Find(x => x.Id == Id);
            return (Delete(slipGajiDetail2A) == 1) ? true : false;
        }
    }

    
}