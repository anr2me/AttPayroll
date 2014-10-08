using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class DepartmentValidator : IDepartmentValidator
    {
        public Department VHasCompanyInfo(Department department, ICompanyInfoService _companyInfoService)
        {
            CompanyInfo companyInfo = _companyInfoService.GetObjectById(department.CompanyInfoId);
            if (companyInfo == null)
            {
                department.Errors.Add("CompanyInfo", "Tidak valid");
            }
            return department;
        }

        public Department VHasUniqueName(Department department, IDepartmentService _departmentService)
        {
            if (String.IsNullOrEmpty(department.Name) || department.Name.Trim() == "")
            {
                department.Errors.Add("Name", "Tidak boleh kosong");
            }
            else if (_departmentService.IsNameDuplicated(department))
            {
                department.Errors.Add("Name", "Tidak boleh ada duplikasi");
            }
            return department;
        }

        public bool ValidCreateObject(Department department, IDepartmentService _departmentService, ICompanyInfoService _companyInfoService)
        {
            VHasCompanyInfo(department, _companyInfoService);
            if (!isValid(department)) { return false; }
            VHasUniqueName(department, _departmentService);
            return isValid(department);
        }

        public bool ValidUpdateObject(Department department, IDepartmentService _departmentService, ICompanyInfoService _companyInfoService)
        {
            department.Errors.Clear();
            ValidCreateObject(department, _departmentService, _companyInfoService);
            return isValid(department);
        }

        public bool ValidDeleteObject(Department department)
        {
            department.Errors.Clear();
            return isValid(department);
        }

        public bool isValid(Department obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Department obj)
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
