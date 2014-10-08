using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISlipGajiDetail1Validator
    {
        
        bool ValidCreateObject(SlipGajiDetail1 slipGajiDetail1, ISlipGajiDetailService _slipGajiDetailService);
        bool ValidUpdateObject(SlipGajiDetail1 slipGajiDetail1, ISlipGajiDetailService _slipGajiDetailService);
        bool ValidDeleteObject(SlipGajiDetail1 slipGajiDetail1);
        bool isValid(SlipGajiDetail1 slipGajiDetail1);
        string PrintError(SlipGajiDetail1 slipGajiDetail1);
    }

}