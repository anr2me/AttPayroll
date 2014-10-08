using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IWorkingTimeValidator
    {
        
        bool ValidCreateObject(WorkingTime workingTime, IWorkingTimeService _workingTimeService);
        bool ValidUpdateObject(WorkingTime workingTime, IWorkingTimeService _workingTimeService);
        bool ValidDeleteObject(WorkingTime workingTime);
        bool isValid(WorkingTime workingTime);
        string PrintError(WorkingTime workingTime);
    }
}