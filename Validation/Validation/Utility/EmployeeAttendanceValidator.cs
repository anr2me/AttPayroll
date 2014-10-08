using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class EmployeeAttendanceValidator : IEmployeeAttendanceValidator
    {
        public EmployeeAttendance VHasEmployee(EmployeeAttendance employeeAttendance, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(employeeAttendance.EmployeeId);
            if (employee == null)
            {
                employeeAttendance.Errors.Add("Employee", "Tidak ada");
            }
            return employeeAttendance;
        }

        public EmployeeAttendance VHasAttendanceDate(EmployeeAttendance employeeAttendance)
        {
            if (employeeAttendance.AttendanceDate == null || employeeAttendance.AttendanceDate.Equals(DateTime.FromBinary(0)))
            {
                employeeAttendance.Errors.Add("AttendanceDate", "Tidak valid");
            }
            return employeeAttendance;
        }

        public EmployeeAttendance VHasCheckInTime(EmployeeAttendance employeeAttendance)
        {
            if (employeeAttendance.CheckIn == null || employeeAttendance.CheckIn.Equals(DateTime.FromBinary(0)))
            {
                employeeAttendance.Errors.Add("CheckIn", "Tidak valid");
            }
            return employeeAttendance;
        }

        public EmployeeAttendance VHasCheckOutTime(EmployeeAttendance employeeAttendance)
        {
            if (employeeAttendance.CheckOut == null || employeeAttendance.CheckOut.Equals(DateTime.FromBinary(0)))
            {
                employeeAttendance.Errors.Add("CheckOut", "Tidak valid");
            }
            else if (employeeAttendance.CheckOut < employeeAttendance.CheckIn)
            {
                employeeAttendance.Errors.Add("CheckOut", "Harus lebih besar atau sama dengan CheckIn");
            }
            return employeeAttendance;
        }

        public bool ValidCreateObject(EmployeeAttendance employeeAttendance, IEmployeeService _employeeService)
        {
            VHasEmployee(employeeAttendance, _employeeService);
            if (!isValid(employeeAttendance)) { return false; }
            VHasAttendanceDate(employeeAttendance);
            if (!isValid(employeeAttendance)) { return false; }
            VHasCheckInTime(employeeAttendance);
            if (!isValid(employeeAttendance)) { return false; }
            VHasCheckOutTime(employeeAttendance);
            return isValid(employeeAttendance);
        }

        public bool ValidUpdateObject(EmployeeAttendance employeeAttendance, IEmployeeService _employeeService)
        {
            employeeAttendance.Errors.Clear();
            ValidCreateObject(employeeAttendance, _employeeService);
            return isValid(employeeAttendance);
        }

        public bool ValidDeleteObject(EmployeeAttendance employeeAttendance)
        {
            employeeAttendance.Errors.Clear();
            return isValid(employeeAttendance);
        }

        public bool isValid(EmployeeAttendance obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(EmployeeAttendance obj)
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
