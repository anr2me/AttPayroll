using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Repository;

namespace Core.Interface.Service
{
    public interface ISlipGajiDetail2AService
    {
        ISlipGajiDetail2AValidator GetValidator();
        ISlipGajiDetail2ARepository GetRepository();
        IQueryable<SlipGajiDetail2A> GetQueryable();
        IList<SlipGajiDetail2A> GetAll();
        IList<SlipGajiDetail2A> GetObjectsBySlipGajiDetailId(int SlipGajiDetailId);
        SlipGajiDetail2A GetObjectById(int Id);
        SlipGajiDetail2A CreateOrUpdateObject(SlipGajiDetail2A slipGajiDetail2A, ISlipGajiDetailService _slipGajiDetailService);
        SlipGajiDetail2A CreateObject(SlipGajiDetail2A slipGajiDetail2A, ISlipGajiDetailService _slipGajiDetailService);
        SlipGajiDetail2A UpdateObject(SlipGajiDetail2A slipGajiDetail2A, ISlipGajiDetailService _slipGajiDetailService);
        bool DeleteObject(int Id);
    }

}