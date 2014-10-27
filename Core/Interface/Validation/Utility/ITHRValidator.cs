using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ITHRValidator
    {

        bool ValidCreateObject(THR thr, ITHRService _thrService);
        bool ValidUpdateObject(THR thr, ITHRService _thrService);
        bool ValidDeleteObject(THR thr);
        bool isValid(THR thr);
        string PrintError(THR thr);
    }
}