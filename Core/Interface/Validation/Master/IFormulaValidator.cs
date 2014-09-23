using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface IFormulaValidator
    {
        Formula VCreateObject(Formula formula, IFormulaService _formulaService);
        Formula VUpdateObject(Formula formula, IFormulaService _formulaService);
        Formula VDeleteObject(Formula formula);
        bool ValidCreateObject(Formula formula, IFormulaService _formulaService);
        bool ValidUpdateObject(Formula formula, IFormulaService _formulaService);
        bool ValidDeleteObject(Formula formula);
        bool isValid(Formula formula);
        string PrintError(Formula formula);
    }
}