using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SlipGajiDetail2AValidator : ISlipGajiDetail2AValidator
    {
        public SlipGajiDetail2A VHasSlipGajiDetail(SlipGajiDetail2A slipGajiDetail2A, ISlipGajiDetailService _slipGajiDetailService)
        {
            SlipGajiDetail slipGajiDetail = _slipGajiDetailService.GetObjectById(slipGajiDetail2A.SlipGajiDetailId); 
            if (slipGajiDetail == null)
            {
                slipGajiDetail2A.Errors.Add("Generic", "SlipGajiDetail Tidak valid");
            }
            return slipGajiDetail2A;
        }

        public bool ValidCreateObject(SlipGajiDetail2A slipGajiDetail2A, ISlipGajiDetailService _slipGajiDetailService)
        {
            VHasSlipGajiDetail(slipGajiDetail2A, _slipGajiDetailService);
            return isValid(slipGajiDetail2A);
        }

        public bool ValidUpdateObject(SlipGajiDetail2A slipGajiDetail2A, ISlipGajiDetailService _slipGajiDetailService)
        {
            slipGajiDetail2A.Errors.Clear();
            ValidCreateObject(slipGajiDetail2A, _slipGajiDetailService);
            return isValid(slipGajiDetail2A);
        }

        public bool ValidDeleteObject(SlipGajiDetail2A slipGajiDetail2A)
        {
            slipGajiDetail2A.Errors.Clear();
            return isValid(slipGajiDetail2A);
        }

        public bool isValid(SlipGajiDetail2A obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SlipGajiDetail2A obj)
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
