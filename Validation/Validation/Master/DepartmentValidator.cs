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
        public Department VHasBranchOffice(Department department, IBranchOfficeService _branchOfficeService)
        {
            BranchOffice branchOffice = _branchOfficeService.GetObjectById(department.BranchOfficeId);
            if (branchOffice == null)
            {
                department.Errors.Add("BranchOffice", "Tidak valid");
            }
            return department;
        }

        public Department VHasUniqueCode(Department department, IDepartmentService _departmentService)
        {
            if (String.IsNullOrEmpty(department.Code) || department.Code.Trim() == "")
            {
                department.Errors.Add("Code", "Tidak boleh kosong");
            }
            else if (_departmentService.IsCodeDuplicated(department))
            {
                department.Errors.Add("Code", "Tidak boleh ada duplikasi");
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

        public Department VDontHaveDivisions(Department department, IDivisionService _divisionService)
        {
            IList<Division> divisions = _divisionService.GetObjectsByDepartmentId(department.Id);
            if (divisions.Any())
            {
                department.Errors.Add("Generic", "Tidak boleh masih memiliki Divisions");
            }
            return department;
        }

        public bool ValidCreateObject(Department department, IDepartmentService _departmentService, IBranchOfficeService _branchOfficeService)
        {
            VHasBranchOffice(department, _branchOfficeService);
            if (!isValid(department)) { return false; }
            VHasUniqueCode(department, _departmentService);
            if (!isValid(department)) { return false; }
            VHasUniqueName(department, _departmentService);
            return isValid(department);
        }

        public bool ValidUpdateObject(Department department, IDepartmentService _departmentService, IBranchOfficeService _branchOfficeService)
        {
            department.Errors.Clear();
            ValidCreateObject(department, _departmentService, _branchOfficeService);
            return isValid(department);
        }

        public bool ValidDeleteObject(Department department, IDivisionService _divisionService)
        {
            department.Errors.Clear();
            VDontHaveDivisions(department, _divisionService);
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
