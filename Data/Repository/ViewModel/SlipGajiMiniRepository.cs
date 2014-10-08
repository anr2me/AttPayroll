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
    public class SlipGajiMiniRepository : EfRepository<SlipGajiMini>, ISlipGajiMiniRepository
    {
        private AttPayrollEntities entities;
        public SlipGajiMiniRepository()
        {
            entities = new AttPayrollEntities();
        }

        public IQueryable<SlipGajiMini> GetQueryable()
        {
            return FindAll();
        }

        public IList<SlipGajiMini> GetAll()
        {
            return FindAll().ToList();
        }

        public SlipGajiMini GetObjectById(int Id)
        {
            SlipGajiMini slipGajiMini = Find(x => x.Id == Id);
            if (slipGajiMini != null) { slipGajiMini.Errors = new Dictionary<string, string>(); }
            return slipGajiMini;
        }

        public SlipGajiMini CreateObject(SlipGajiMini slipGajiMini)
        {
            return Create(slipGajiMini);
        }

        public SlipGajiMini UpdateObject(SlipGajiMini slipGajiMini)
        {
            Update(slipGajiMini);
            return slipGajiMini;
        }

        public bool DeleteObject(int Id)
        {
            SlipGajiMini slipGajiMini = Find(x => x.Id == Id);
            return (Delete(slipGajiMini) == 1) ? true : false;
        }
    }
}