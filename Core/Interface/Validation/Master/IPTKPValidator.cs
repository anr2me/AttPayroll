using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IPTKPValidator
    {
        
        bool ValidCreateObject(PTKP ptkp, IPTKPService _ptkpService);
        bool ValidUpdateObject(PTKP ptkp, IPTKPService _ptkpService);
        bool ValidDeleteObject(PTKP ptkp);
        bool isValid(PTKP ptkp);
        string PrintError(PTKP ptkp);
    }
}