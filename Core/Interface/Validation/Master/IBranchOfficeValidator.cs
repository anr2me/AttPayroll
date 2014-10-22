using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IBranchOfficeValidator
    {
        BranchOffice VHasUniqueName(BranchOffice branchOffice, IBranchOfficeService _branchOfficeService);
        BranchOffice VHasAddress(BranchOffice branchOffice);
        BranchOffice VHasPhoneNumber(BranchOffice branchOffice);
        BranchOffice VHasEmail(BranchOffice branchOffice);
        
        bool ValidCreateObject(BranchOffice branchOffice, IBranchOfficeService _branchOfficeService);
        bool ValidUpdateObject(BranchOffice branchOffice, IBranchOfficeService _branchOfficeService);
        bool ValidDeleteObject(BranchOffice branchOffice, IDepartmentService _departmentService);
        bool isValid(BranchOffice branchOffice);
        string PrintError(BranchOffice branchOffice);
    }
}