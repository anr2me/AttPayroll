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
    public class SlipGajiDetail1Repository : EfRepository<SlipGajiDetail1>, ISlipGajiDetail1Repository
    {
        private AttPayrollEntities entities;
        public SlipGajiDetail1Repository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<SlipGajiDetail1> GetQueryable()
        {
            return FindAll();
        }

        public IList<SlipGajiDetail1> GetAll()
        {
            return FindAll().ToList();
        }

        public SlipGajiDetail1 GetObjectById(int Id)
        {
            SlipGajiDetail1 slipGajiDetail1 = Find(x => x.Id == Id);
            if (slipGajiDetail1 != null) { slipGajiDetail1.Errors = new Dictionary<string, string>(); }
            return slipGajiDetail1;
        }

        public SlipGajiDetail1 CreateObject(SlipGajiDetail1 slipGajiDetail1)
        {
            return Create(slipGajiDetail1);
        }

        public SlipGajiDetail1 UpdateObject(SlipGajiDetail1 slipGajiDetail1)
        {
            Update(slipGajiDetail1);
            return slipGajiDetail1;
        }

        public bool DeleteObject(int Id)
        {
            SlipGajiDetail1 slipGajiDetail1 = Find(x => x.Id == Id);
            return (Delete(slipGajiDetail1) == 1) ? true : false;
        }
    }

    
}