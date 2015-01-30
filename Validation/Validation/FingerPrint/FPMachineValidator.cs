using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FPDevice;

namespace Validation.Validation
{
    public class FPMachineValidator : IFPMachineValidator
    {
        public FPMachine VHasCompany(FPMachine fpMachine, ICompanyInfoService _companyInfoService)
        {
            CompanyInfo companyInfo = _companyInfoService.GetObjectById(fpMachine.CompanyInfoId);
            if (companyInfo == null)
            {
                fpMachine.Errors.Add("Generic", "Company Tidak valid");
            }
            return fpMachine;
        }

        public FPMachine VIsValidId(FPMachine fpMachine, IFPMachineService _fpMachineService)
        {
            var obj = _fpMachineService.GetQueryable().Where(x => x.Id == fpMachine.Id && !x.IsDeleted).FirstOrDefault();
            if (obj == null)
            {
                fpMachine.Errors.Add("Generic", "Id Tidak valid");
            }
            return fpMachine;
        }

        //public FPMachine VIsValidMachineNumber(FPMachine fpMachine, IFPMachineService _fpMachineService)
        //{
        //    var obj = _fpMachineService.GetQueryable().Where(x => x.MachineNumber == fpMachine.MachineNumber && !x.IsDeleted && x.Id != fpMachine.Id).FirstOrDefault();
        //    if (obj != null)
        //    {
        //        fpMachine.Errors.Add("MachineNumber", "Tidak boleh ada duplikasi");
        //    }
        //    return fpMachine;
        //}

        public FPMachine VHasUniqueName(FPMachine fpMachine, IFPMachineService _fpMachineService)
        {
            if (String.IsNullOrEmpty(fpMachine.MachineName) || fpMachine.MachineName.Trim() == "")
            {
                fpMachine.Errors.Add("MachineName", "Tidak boleh kosong");
            }
            else if (_fpMachineService.IsNameDuplicated(fpMachine))
            {
                fpMachine.Errors.Add("MachineName", "Tidak boleh ada duplikasi");
            }
            return fpMachine;
        }

        public FPMachine VIsConnected(FPMachine fpMachine)
        {
            fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id] != null ? FPMachines.fpDevices[fpMachine.Id].bIsConnected : false;
            if (!fpMachine.IsConnected)
            {
                fpMachine.Errors.Add("Generic", "Tidak tersambung");
            }
            return fpMachine;
        }

        public FPMachine VIsNotConnected(FPMachine fpMachine)
        {
            fpMachine.IsConnected = FPMachines.fpDevices[fpMachine.Id] != null ? FPMachines.fpDevices[fpMachine.Id].bIsConnected : false;
            if (fpMachine.IsConnected)
            {
                fpMachine.Errors.Add("Generic", "Sudah tersambung");
            }
            return fpMachine;
        }

        public bool ValidCreateObject(FPMachine fpMachine, IFPMachineService _fpMachineService, ICompanyInfoService _companyInfoService)
        {
            VHasCompany(fpMachine, _companyInfoService);
            if (!isValid(fpMachine)) { return false; }
            VHasUniqueName(fpMachine, _fpMachineService);
            return isValid(fpMachine);
        }

        public bool ValidUpdateObject(FPMachine fpMachine, IFPMachineService _fpMachineService, ICompanyInfoService _companyInfoService)
        {
            fpMachine.Errors.Clear();
            VIsValidId(fpMachine, _fpMachineService);
            if (!isValid(fpMachine)) { return false; }
            ValidCreateObject(fpMachine, _fpMachineService, _companyInfoService);
            return isValid(fpMachine);
        }

        public bool ValidDeleteObject(FPMachine fpMachine, IFPMachineService _fpMachineService)
        {
            fpMachine.Errors.Clear();
            VIsValidId(fpMachine, _fpMachineService);
            return isValid(fpMachine);
        }

        public bool ValidConnectObject(FPMachine fpMachine, IFPMachineService _fpMachineService)
        {
            fpMachine.Errors.Clear();
            VIsValidId(fpMachine, _fpMachineService);
            if (!isValid(fpMachine)) { return false; }
            VIsNotConnected(fpMachine);
            return isValid(fpMachine);
        }

        public bool ValidDisconnectObject(FPMachine fpMachine, IFPMachineService _fpMachineService)
        {
            fpMachine.Errors.Clear();
            VIsValidId(fpMachine, _fpMachineService);
            if (!isValid(fpMachine)) { return false; }
            VIsConnected(fpMachine);
            return isValid(fpMachine);
        }

        public bool ValidRefreshObject(FPMachine fpMachine, IFPMachineService _fpMachineService)
        {
            fpMachine.Errors.Clear();
            VIsValidId(fpMachine, _fpMachineService);
            if (!isValid(fpMachine)) { return false; }
            VIsConnected(fpMachine);
            return isValid(fpMachine);
        }

        public bool ValidSyncObject(FPMachine fpMachine, IFPMachineService _fpMachineService, IFPUserService _fpUserService, IFPTemplateService _fpTemplateService)
        {
            fpMachine.Errors.Clear();
            VIsValidId(fpMachine, _fpMachineService);
            //if (!isValid(fpMachine)) { return false; }

            return isValid(fpMachine);
        }

        public bool isValid(FPMachine obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(FPMachine obj)
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
