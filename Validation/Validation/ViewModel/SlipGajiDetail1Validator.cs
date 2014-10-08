using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SlipGajiDetail1Validator : ISlipGajiDetail1Validator
    {
        public SlipGajiDetail1 VHasSlipGajiDetail(SlipGajiDetail1 slipGajiDetail1, ISlipGajiDetailService _slipGajiDetailService)
        {
            SlipGajiDetail slipGajiDetail = _slipGajiDetailService.GetObjectById(slipGajiDetail1.SlipGajiDetailId); 
            if (slipGajiDetail == null)
            {
                slipGajiDetail1.Errors.Add("Generic", "SlipGajiDetail Tidak valid");
            }
            return slipGajiDetail1;
        }

        public bool ValidCreateObject(SlipGajiDetail1 slipGajiDetail1, ISlipGajiDetailService _slipGajiDetailService)
        {
            VHasSlipGajiDetail(slipGajiDetail1, _slipGajiDetailService);
            return isValid(slipGajiDetail1);
        }

        public bool ValidUpdateObject(SlipGajiDetail1 slipGajiDetail1, ISlipGajiDetailService _slipGajiDetailService)
        {
            slipGajiDetail1.Errors.Clear();
            ValidCreateObject(slipGajiDetail1, _slipGajiDetailService);
            return isValid(slipGajiDetail1);
        }

        public bool ValidDeleteObject(SlipGajiDetail1 slipGajiDetail1)
        {
            slipGajiDetail1.Errors.Clear();
            return isValid(slipGajiDetail1);
        }

        public bool isValid(SlipGajiDetail1 obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SlipGajiDetail1 obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
