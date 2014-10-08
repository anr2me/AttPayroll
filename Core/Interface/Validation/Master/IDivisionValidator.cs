using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IDivisionValidator
    {
        Division VHasUniqueName(Division division, IDivisionService _divisionService);

        bool ValidCreateObject(Division division, IDivisionService _divisionService, IDepartmentService _departmentService);
        bool ValidUpdateObject(Division division, IDivisionService _divisionService, IDepartmentService _departmentService);
        bool ValidDeleteObject(Division division);
        bool isValid(Division division);
        string PrintError(Division division);
    }
}