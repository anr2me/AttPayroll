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
        Division VCreateObject(Division division, IDivisionService _divisionService);
        Division VUpdateObject(Division division, IDivisionService _divisionService);
        Division VDeleteObject(Division division);
        bool ValidCreateObject(Division division, IDivisionService _divisionService);
        bool ValidUpdateObject(Division division, IDivisionService _divisionService);
        bool ValidDeleteObject(Division division);
        bool isValid(Division division);
        string PrintError(Division division);
    }
}