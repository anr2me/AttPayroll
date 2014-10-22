using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class PPH21SPTValidator : IPPH21SPTValidator
    {
        public PPH21SPT VHasUniqueCode(PPH21SPT pph21spt, IPPH21SPTService _pph21sptService)
        {
            if (String.IsNullOrEmpty(pph21spt.Code) || pph21spt.Code.Trim() == "")
            {
                pph21spt.Errors.Add("Code", "Tidak boleh kosong");
            }
            else if (_pph21sptService.IsCodeDuplicated(pph21spt))
            {
                pph21spt.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return pph21spt;
        }

        public PPH21SPT VHasPercent(PPH21SPT pph21spt)
        {
            if (pph21spt.Percent < 0)
            {
                pph21spt.Errors.Add("Percent", "Harus lebih besar atau sama dengan 0");
            }
            return pph21spt;
        }

        public PPH21SPT VHasMinAmount(PPH21SPT pph21spt)
        {
            if (pph21spt.MinAmount < 0)
            {
                pph21spt.Errors.Add("MinAmount", "Harus lebih besar atau sama dengan 0");
            }
            return pph21spt;
        }

        public PPH21SPT VHasMaxAmount(PPH21SPT pph21spt)
        {
            if (pph21spt.MaxAmount < pph21spt.MinAmount && !pph21spt.IsInfiniteMaxAmount)
            {
                pph21spt.Errors.Add("MaxAmount", "Harus lebih besar atau sama dengan MinAmount");
            }
            return pph21spt;
        }

        public bool ValidCreateObject(PPH21SPT pph21spt, IPPH21SPTService _pph21sptService)
        {
            VHasUniqueCode(pph21spt, _pph21sptService);
            if (!isValid(pph21spt)) { return false; }
            VHasPercent(pph21spt);
            if (!isValid(pph21spt)) { return false; }
            VHasMinAmount(pph21spt);
            if (!isValid(pph21spt)) { return false; }
            VHasMaxAmount(pph21spt);
            return isValid(pph21spt);
        }

        public bool ValidUpdateObject(PPH21SPT pph21spt, IPPH21SPTService _pph21sptService)
        {
            pph21spt.Errors.Clear();
            ValidCreateObject(pph21spt, _pph21sptService);
            return isValid(pph21spt);
        }

        public bool ValidDeleteObject(PPH21SPT pph21spt)
        {
            pph21spt.Errors.Clear();
            return isValid(pph21spt);
        }

        public bool isValid(PPH21SPT obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PPH21SPT obj)
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
