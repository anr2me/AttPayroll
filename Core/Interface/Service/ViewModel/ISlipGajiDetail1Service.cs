using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Repository;

namespace Core.Interface.Service
{
    public interface ISlipGajiDetail1Service
    {
        ISlipGajiDetail1Validator GetValidator();
        ISlipGajiDetail1Repository GetRepository();
        IQueryable<SlipGajiDetail1> GetQueryable();
        IList<SlipGajiDetail1> GetAll();
        SlipGajiDetail1 GetObjectById(int Id);
        SlipGajiDetail1 CreateObject(SlipGajiDetail1 slipGajiDetail1, ISlipGajiDetailService _slipGajiDetailService);
        SlipGajiDetail1 UpdateObject(SlipGajiDetail1 slipGajiDetail1, ISlipGajiDetailService _slipGajiDetailService);
        bool DeleteObject(int Id);
    }

}