using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class CompanyInfoValidator : ICompanyInfoValidator
    {

        public CompanyInfo VHasUniqueName(CompanyInfo companyInfo, ICompanyInfoService _companyInfoService)
        {
            if (String.IsNullOrEmpty(companyInfo.Name) || companyInfo.Name.Trim() == "")
            {
                companyInfo.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_companyInfoService.IsNameDuplicated(companyInfo))
            {
                companyInfo.Errors.Add("Name", "Tidak boleh ada duplikasi");
            }
            return companyInfo;
        }

        public CompanyInfo VHasAddress(CompanyInfo companyInfo)
        {
            if (String.IsNullOrEmpty(companyInfo.Address) || companyInfo.Address.Trim() == "")
            {
                companyInfo.Errors.Add("Address", "Tidak boleh kosong");
            }
            return companyInfo;
        }

        public CompanyInfo VHasPhoneNumber(CompanyInfo companyInfo)
        {
            if (String.IsNullOrEmpty(companyInfo.PhoneNumber) || companyInfo.PhoneNumber.Trim() == "")
            {
                companyInfo.Errors.Add("PhoneNumber", "Tidak boleh kosong");
            }
            return companyInfo;
        }

        public CompanyInfo VHasEmail(CompanyInfo companyInfo)
        {
            if (String.IsNullOrEmpty(companyInfo.Email) || companyInfo.Email.Trim() == "")
            {
                companyInfo.Errors.Add("Email", "Tidak boleh kosong");
            }
            return companyInfo;
        }

        public bool ValidCreateObject(CompanyInfo companyInfo, ICompanyInfoService _companyInfoService)
        {
            VHasUniqueName(companyInfo, _companyInfoService);
            if (!isValid(companyInfo)) { return false; }
            VHasAddress(companyInfo);
            if (!isValid(companyInfo)) { return false; }
            VHasPhoneNumber(companyInfo);
            if (!isValid(companyInfo)) { return false; }
            VHasEmail(companyInfo);
            return isValid(companyInfo);
        }

        public bool ValidUpdateObject(CompanyInfo companyInfo, ICompanyInfoService _companyInfoService)
        {
            companyInfo.Errors.Clear();
            return ValidCreateObject(companyInfo, _companyInfoService);
        }

        public bool ValidDeleteObject(CompanyInfo companyInfo)
        {
            companyInfo.Errors.Clear();
            return isValid(companyInfo);
        }

        public bool isValid(CompanyInfo obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(CompanyInfo obj)
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
