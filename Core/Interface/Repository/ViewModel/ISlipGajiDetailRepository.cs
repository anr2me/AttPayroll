using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISlipGajiDetailRepository : IRepository<SlipGajiDetail>
    {
        IQueryable<SlipGajiDetail> GetQueryable();
        IList<SlipGajiDetail> GetAll();
        SlipGajiDetail GetObjectById(int Id);
        SlipGajiDetail CreateObject(SlipGajiDetail slipGajiDetail);
        SlipGajiDetail UpdateObject(SlipGajiDetail slipGajiDetail);
        bool DeleteObject(int Id);
    }

}