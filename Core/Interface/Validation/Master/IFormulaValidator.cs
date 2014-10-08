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
        bool ValidCreateObject(Formula formula, IFormulaService _formulaService, ISalaryItemService _salaryItemService);
        bool ValidUpdateObject(Formula formula, IFormulaService _formulaService, ISalaryItemService _salaryItemService);
        bool ValidDeleteObject(Formula formula);
        bool isValid(Formula formula);
        string PrintError(Formula formula);
    }
}