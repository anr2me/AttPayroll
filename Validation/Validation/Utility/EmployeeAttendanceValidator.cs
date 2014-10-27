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
            FixDateTime(employeeAttendance);
            VHasCheckInTime(employeeAttendance);
            //if (!isValid(employeeAttendance)) { return false; }
            //VHasCheckOutTime(employeeAttendance);
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

        public EmployeeAttendance FixDateTime(EmployeeAttendance employeeAttendance)
        {
            employeeAttendance.AttendanceDate = employeeAttendance.AttendanceDate.Date;
            employeeAttendance.CheckIn = employeeAttendance.AttendanceDate.Add(employeeAttendance.CheckIn.Subtract(employeeAttendance.CheckIn.Date));
            employeeAttendance.CheckOut = employeeAttendance.AttendanceDate.Add(employeeAttendance.CheckOut.GetValueOrDefault().Subtract(employeeAttendance.CheckOut.GetValueOrDefault().Date));
            employeeAttendance.BreakOut = employeeAttendance.AttendanceDate.Add(employeeAttendance.BreakOut.GetValueOrDefault().Subtract(employeeAttendance.BreakOut.GetValueOrDefault().Date));
            employeeAttendance.BreakIn = employeeAttendance.AttendanceDate.Add(employeeAttendance.BreakIn.GetValueOrDefault().Subtract(employeeAttendance.BreakIn.GetValueOrDefault().Date));
            if (employeeAttendance.BreakOut != null && employeeAttendance.BreakOut.GetValueOrDefault() < employeeAttendance.CheckIn)
            {
                employeeAttendance.BreakOut = employeeAttendance.BreakOut.GetValueOrDefault().AddDays(1);
            }
            if (employeeAttendance.BreakIn != null && employeeAttendance.BreakIn.GetValueOrDefault() < employeeAttendance.CheckIn)
            {
                employeeAttendance.BreakIn = employeeAttendance.BreakIn.GetValueOrDefault().AddDays(1);
            }
            if (employeeAttendance.CheckOut != null && employeeAttendance.CheckOut.GetValueOrDefault() < employeeAttendance.CheckIn)
            {
                employeeAttendance.CheckOut = employeeAttendance.CheckOut.GetValueOrDefault().AddDays(1);
            }
            return employeeAttendance;
        }


    }
}
