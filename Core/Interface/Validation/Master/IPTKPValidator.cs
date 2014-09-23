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
        PTKP VCreateObject(PTKP ptkp, IPTKPService _ptkpService);
        PTKP VUpdateObject(PTKP ptkp, IPTKPService _ptkpService);
        PTKP VDeleteObject(PTKP ptkp);
        bool ValidCreateObject(PTKP ptkp, IPTKPService _ptkpService);
        bool ValidUpdateObject(PTKP ptkp, IPTKPService _ptkpService);
        bool ValidDeleteObject(PTKP ptkp);
        bool isValid(PTKP ptkp);
        string PrintError(PTKP ptkp);
    }
}