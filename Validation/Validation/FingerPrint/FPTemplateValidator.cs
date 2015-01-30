using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class FPTemplateValidator : IFPTemplateValidator
    {
        public FPTemplate VHasUser(FPTemplate fpTemplate, IFPUserService _fpUserService)
        {
            FPUser fpUser = _fpUserService.GetObjectById(fpTemplate.FPUserId);
            if (fpUser == null)
            {
                fpTemplate.Errors.Add("FPUser", "Tidak ada");
            }
            return fpTemplate;
        }

        public FPTemplate VHasUniqueFingerID(FPTemplate fpTemplate, IFPTemplateService _fpTemplateService)
        {
            if (fpTemplate.FingerID < 0 || fpTemplate.FingerID > 9)
            {
                fpTemplate.Errors.Add("FingerID", "Harus antara 0 sampai 9");
            }
            else if (_fpTemplateService.IsFingerIDDuplicated(fpTemplate))
            {
                fpTemplate.Errors.Add("FingerID", "Tidak boleh ada duplikasi");
            }
            return fpTemplate;
        }

        public bool ValidCreateObject(FPTemplate fpTemplate, IFPTemplateService _fpTemplateService, IFPUserService _fpUserService)
        {
            VHasUser(fpTemplate, _fpUserService);
            if (!isValid(fpTemplate)) { return false; }
            VHasUniqueFingerID(fpTemplate, _fpTemplateService);
            return isValid(fpTemplate);
        }

        public bool ValidUpdateObject(FPTemplate fpTemplate, IFPTemplateService _fpTemplateService, IFPUserService _fpUserService)
        {
            fpTemplate.Errors.Clear();
            ValidCreateObject(fpTemplate, _fpTemplateService, _fpUserService);
            return isValid(fpTemplate);
        }

        public bool ValidDeleteObject(FPTemplate fpTemplate)
        {
            fpTemplate.Errors.Clear();
            return isValid(fpTemplate);
        }

        public bool isValid(FPTemplate obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(FPTemplate obj)
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
