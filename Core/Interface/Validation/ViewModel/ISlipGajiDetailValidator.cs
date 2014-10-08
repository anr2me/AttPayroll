using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISlipGajiDetailValidator
    {

        bool ValidCreateObject(SlipGajiDetail slipGajiDetail, IEmployeeService _employeeService,
                                ISlipGajiDetail1Service _slipGajiDetail1Service, ISlipGajiDetail2AService _slipGajiDetail2AService);
        bool ValidUpdateObject(SlipGajiDetail slipGajiDetail, IEmployeeService _employeeService,
                                ISlipGajiDetail1Service _slipGajiDetail1Service, ISlipGajiDetail2AService _slipGajiDetail2AService);
        bool ValidDeleteObject(SlipGajiDetail slipGajiDetail);
        bool isValid(SlipGajiDetail slipGajiDetail);
        string PrintError(SlipGajiDetail slipGajiDetail);
    }

}