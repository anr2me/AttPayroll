using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISlipGajiDetail2ARepository : IRepository<SlipGajiDetail2A>
    {
        IQueryable<SlipGajiDetail2A> GetQueryable();
        IList<SlipGajiDetail2A> GetAll();
        SlipGajiDetail2A GetObjectById(int Id);
        SlipGajiDetail2A CreateObject(SlipGajiDetail2A slipGajiDetail2A);
        SlipGajiDetail2A UpdateObject(SlipGajiDetail2A slipGajiDetail2A);
        bool DeleteObject(int Id);
    }

}