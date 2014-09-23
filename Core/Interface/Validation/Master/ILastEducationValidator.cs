using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ILastEducationValidator
    {
        LastEducation VCreateObject(LastEducation lastEducation, ILastEducationService _lastEducationService);
        LastEducation VUpdateObject(LastEducation lastEducation, ILastEducationService _lastEducationService);
        LastEducation VDeleteObject(LastEducation lastEducation);
        bool ValidCreateObject(LastEducation lastEducation, ILastEducationService _lastEducationService);
        bool ValidUpdateObject(LastEducation lastEducation, ILastEducationService _lastEducationService);
        bool ValidDeleteObject(LastEducation lastEducation);
        bool isValid(LastEducation lastEducation);
        string PrintError(LastEducation lastEducation);
    }
}