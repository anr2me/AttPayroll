using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IGeneralLeaveValidator
    {
        
        bool ValidCreateObject(GeneralLeave generalLeave);
        bool ValidUpdateObject(GeneralLeave generalLeave);
        bool ValidDeleteObject(GeneralLeave generalLeave);
        bool isValid(GeneralLeave generalLeave);
        string PrintError(GeneralLeave generalLeave);
    }
}