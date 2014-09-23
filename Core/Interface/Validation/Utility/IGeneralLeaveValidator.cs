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
        GeneralLeave VCreateObject(GeneralLeave generalLeave, IGeneralLeaveService _generalLeaveService);
        GeneralLeave VUpdateObject(GeneralLeave generalLeave, IGeneralLeaveService _generalLeaveService);
        GeneralLeave VDeleteObject(GeneralLeave generalLeave);
        bool ValidCreateObject(GeneralLeave generalLeave, IGeneralLeaveService _generalLeaveService);
        bool ValidUpdateObject(GeneralLeave generalLeave, IGeneralLeaveService _generalLeaveService);
        bool ValidDeleteObject(GeneralLeave generalLeave);
        bool isValid(GeneralLeave generalLeave);
        string PrintError(GeneralLeave generalLeave);
    }
}