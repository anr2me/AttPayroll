using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISlipGajiDetail1Repository : IRepository<SlipGajiDetail1>
    {
        IQueryable<SlipGajiDetail1> GetQueryable();
        IList<SlipGajiDetail1> GetAll();
        SlipGajiDetail1 GetObjectById(int Id);
        SlipGajiDetail1 CreateObject(SlipGajiDetail1 slipGajiDetail1);
        SlipGajiDetail1 UpdateObject(SlipGajiDetail1 slipGajiDetail1);
        bool DeleteObject(int Id);
    }

}