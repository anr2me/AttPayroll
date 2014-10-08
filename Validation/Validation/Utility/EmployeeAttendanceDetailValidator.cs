using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class EmployeeAttendanceDetailValidator : IEmployeeAttendanceDetailValidator
    {
        public EmployeeAttendanceDetail VHasEmployeeAttendance(EmployeeAttendanceDetail employeeAttendanceDetail, IEmployeeAttendanceService _employeeAttendanceService)
        {
            EmployeeAttendance employeeAttendance = _employeeAttendanceService.GetObjectById(employeeAttendanceDetail.EmployeeAttendanceId);
            if (employeeAttendance == null)
            {
                employeeAttendanceDetail.Errors.Add("EmployeeAttendance", "Tidak ada");
            }
            return employeeAttendanceDetail;
        }

        public EmployeeAttendanceDetail VHasSalaryItem(EmployeeAttendanceDetail employeeAttendanceDetail, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectById(employeeAttendanceDetail.SalaryItemId);
            if (salaryItem == null)
            {
                employeeAttendanceDetail.Errors.Add("SalaryItem", "Tidak ada");
            }
            return employeeAttendanceDetail;
        }

        public EmployeeAttendanceDetail VHasAmount(EmployeeAttendanceDetail employeeAttendanceDetail)
        {
            if (employeeAttendanceDetail.Amount < 0)
            {
                employeeAttendanceDetail.Errors.Add("Amount", "Harus lebih besar atau sama dengan 0");
            }
            return employeeAttendanceDetail;
        }

        public bool ValidCreateObject(EmployeeAttendanceDetail employeeAttendanceDetail, IEmployeeAttendanceService _employeeAttendanceService, ISalaryItemService _salaryItemService)
        {
            VHasEmployeeAttendance(employeeAttendanceDetail, _employeeAttendanceService);
            if (!isValid(employeeAttendanceDetail)) { return false; }
            VHasSalaryItem(employeeAttendanceDetail, _salaryItemService);
            if (!isValid(employeeAttendanceDetail)) { return false; }
            VHasAmount(employeeAttendanceDetail);
            return isValid(employeeAttendanceDetail);
        }

        public bool ValidUpdateObject(EmployeeAttendanceDetail employeeAttendanceDetail, IEmployeeAttendanceService _employeeAttendanceService, ISalaryItemService _salaryItemService)
        {
            employeeAttendanceDetail.Errors.Clear();
            ValidCreateObject(employeeAttendanceDetail, _employeeAttendanceService, _salaryItemService);
            return isValid(employeeAttendanceDetail);
        }

        public bool ValidDeleteObject(EmployeeAttendanceDetail employeeAttendanceDetail)
        {
            employeeAttendanceDetail.Errors.Clear();
            return isValid(employeeAttendanceDetail);
        }

        public bool isValid(EmployeeAttendanceDetail obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(EmployeeAttendanceDetail obj)
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
