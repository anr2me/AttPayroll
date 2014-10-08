using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class PTKPValidator : IPTKPValidator
    {
        public PTKP VHasUniqueCode(PTKP ptkp, IPTKPService _ptkpService)
        {
            if (String.IsNullOrEmpty(ptkp.Code) || ptkp.Code.Trim() == "")
            {
                ptkp.Errors.Add("Code", "Tidak boleh kosong");
            }
            else if (_ptkpService.IsCodeDuplicated(ptkp))
            {
                ptkp.Errors.Add("Code", "Tidak boleh ada duplikasi");
            }
            return ptkp;
        }

        public PTKP VHasAmount(PTKP ptkp)
        {
            if (ptkp.Amount < 0)
            {
                ptkp.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return ptkp;
        }

        public bool ValidCreateObject(PTKP ptkp, IPTKPService _ptkpService)
        {
            VHasUniqueCode(ptkp, _ptkpService);
            if (!isValid(ptkp)) { return false; }
            VHasAmount(ptkp);
            return isValid(ptkp);
        }

        public bool ValidUpdateObject(PTKP ptkp, IPTKPService _ptkpService)
        {
            ptkp.Errors.Clear();
            ValidCreateObject(ptkp, _ptkpService);
            return isValid(ptkp);
        }

        public bool ValidDeleteObject(PTKP ptkp)
        {
            ptkp.Errors.Clear();
            return isValid(ptkp);
        }

        public bool isValid(PTKP obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(PTKP obj)
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
