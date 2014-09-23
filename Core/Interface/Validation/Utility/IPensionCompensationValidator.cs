using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IPensionCompensationValidator
    {
        PensionCompensation VCreateObject(PensionCompensation pensionCompensation, IPensionCompensationService _pensionCompensationService);
        PensionCompensation VUpdateObject(PensionCompensation pensionCompensation, IPensionCompensationService _pensionCompensationService);
        PensionCompensation VDeleteObject(PensionCompensation pensionCompensation);
        bool ValidCreateObject(PensionCompensation pensionCompensation, IPensionCompensationService _pensionCompensationService);
        bool ValidUpdateObject(PensionCompensation pensionCompensation, IPensionCompensationService _pensionCompensationService);
        bool ValidDeleteObject(PensionCompensation pensionCompensation);
        bool isValid(PensionCompensation pensionCompensation);
        string PrintError(PensionCompensation pensionCompensation);
    }
}