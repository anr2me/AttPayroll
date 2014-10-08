using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISlipGajiMiniValidator
    {

        bool ValidCreateObject(SlipGajiMini slipGajiMini, IEmployeeService _employeeService);
        bool ValidUpdateObject(SlipGajiMini slipGajiMini, IEmployeeService _employeeService);
        bool ValidDeleteObject(SlipGajiMini slipGajiMini);
        bool isValid(SlipGajiMini slipGajiMini);
        string PrintError(SlipGajiMini slipGajiMini);
    }
}