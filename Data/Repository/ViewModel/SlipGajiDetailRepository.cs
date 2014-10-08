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
    public class SlipGajiDetailRepository : EfRepository<SlipGajiDetail>, ISlipGajiDetailRepository
    {
        private AttPayrollEntities entities;
        public SlipGajiDetailRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<SlipGajiDetail> GetQueryable()
        {
            return FindAll();
        }

        public IList<SlipGajiDetail> GetAll()
        {
            return FindAll().ToList();
        }

        public SlipGajiDetail GetObjectById(int Id)
        {
            SlipGajiDetail slipGajiDetail = Find(x => x.Id == Id);
            if (slipGajiDetail != null) { slipGajiDetail.Errors = new Dictionary<string, string>(); }
            return slipGajiDetail;
        }

        public SlipGajiDetail CreateObject(SlipGajiDetail slipGajiDetail)
        {
            return Create(slipGajiDetail);
        }

        public SlipGajiDetail UpdateObject(SlipGajiDetail slipGajiDetail)
        {
            Update(slipGajiDetail);
            return slipGajiDetail;
        }

        public bool DeleteObject(int Id)
        {
            SlipGajiDetail slipGajiDetail = Find(x => x.Id == Id);
            return (Delete(slipGajiDetail) == 1) ? true : false;
        }
    }

    
}