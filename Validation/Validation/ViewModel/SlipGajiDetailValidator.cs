using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class SlipGajiDetailValidator : ISlipGajiDetailValidator
    {
        public SlipGajiDetail VHasEmployee(SlipGajiDetail slipGajiDetail, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(slipGajiDetail.EmployeeId); 
            if (employee == null)
            {
                slipGajiDetail.Errors.Add("Employee", "Tidak valid");
            }
            return slipGajiDetail;
        }

        public SlipGajiDetail VHasMonth(SlipGajiDetail slipGajiDetail)
        {
            if (slipGajiDetail.MONTH.Equals(DateTime.FromBinary(0)))
            {
                slipGajiDetail.Errors.Add("Month", "Tidak valid");
            }
            return slipGajiDetail;
        }

        public SlipGajiDetail VHasSlipGajiDetail1(SlipGajiDetail slipGajiDetail, ISlipGajiDetail1Service _slipGajiDetail1Service)
        {
            SlipGajiDetail1 slipGajiDetail1 = _slipGajiDetail1Service.GetObjectById(slipGajiDetail.SlipGajiDetail1Id.GetValueOrDefault());
            if (slipGajiDetail1 == null)
            {
                slipGajiDetail.Errors.Add("Generic", "SlipGajiDetail1 Tidak valid");
            }
            return slipGajiDetail;
        }

        public SlipGajiDetail VHasSlipGajiDetail2A(SlipGajiDetail slipGajiDetail, ISlipGajiDetail2AService _slipGajiDetail2AService)
        {
            SlipGajiDetail2A slipGajiDetail2A = _slipGajiDetail2AService.GetObjectById(slipGajiDetail.SlipGajiDetail2AId.GetValueOrDefault());
            if (slipGajiDetail2A == null)
            {
                slipGajiDetail.Errors.Add("Generic", "SlipGajiDetail2A Tidak valid");
            }
            return slipGajiDetail;
        }

        public bool ValidCreateObject(SlipGajiDetail slipGajiDetail, IEmployeeService _employeeService, 
                                ISlipGajiDetail1Service _slipGajiDetail1Service, ISlipGajiDetail2AService _slipGajiDetail2AService)
        {
            VHasEmployee(slipGajiDetail, _employeeService);
            if (!isValid(slipGajiDetail)) { return false; }
            VHasMonth(slipGajiDetail);
            if (!isValid(slipGajiDetail)) { return false; }
            VHasSlipGajiDetail1(slipGajiDetail, _slipGajiDetail1Service);
            if (!isValid(slipGajiDetail)) { return false; }
            VHasSlipGajiDetail2A(slipGajiDetail, _slipGajiDetail2AService);
            return isValid(slipGajiDetail);
        }

        public bool ValidUpdateObject(SlipGajiDetail slipGajiDetail, IEmployeeService _employeeService,
                                ISlipGajiDetail1Service _slipGajiDetail1Service, ISlipGajiDetail2AService _slipGajiDetail2AService)
        {
            slipGajiDetail.Errors.Clear();
            ValidCreateObject(slipGajiDetail, _employeeService, _slipGajiDetail1Service, _slipGajiDetail2AService);
            return isValid(slipGajiDetail);
        }

        public bool ValidDeleteObject(SlipGajiDetail slipGajiDetail)
        {
            slipGajiDetail.Errors.Clear();
            return isValid(slipGajiDetail);
        }

        public bool isValid(SlipGajiDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(SlipGajiDetail obj)
        {
            string erroroutput = "";
            KeyValuePair<string, string> first = obj.Errors.ElementAt(0);
            erroroutput += first.Key + "," + first.Value;
            foreach (KeyValuePair<string, string> pair in obj.Errors.Skip(1))
            {
                erroroutput += Environment.NewLine;
                erroroutput += pair.Key + "," + pair.Value;
            }
            return erroroutput;
        }

    }
}
