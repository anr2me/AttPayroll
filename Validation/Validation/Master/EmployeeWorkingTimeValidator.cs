using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class EmployeeWorkingTimeValidator : IEmployeeWorkingTimeValidator
    {

        public EmployeeWorkingTime VHasWorkingTime(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService)
        {
            WorkingTime workingTime = _workingTimeService.GetObjectById(employeeWorkingTime.WorkingTimeId);
            if (workingTime == null)
            {
                employeeWorkingTime.Errors.Add("WorkingTime", "Tidak valid");
            }
            return employeeWorkingTime;
        }

        public EmployeeWorkingTime VHasEmployee(EmployeeWorkingTime employeeWorkingTime, IEmployeeService _employeeService)
        {
            Employee employee = _employeeService.GetObjectById(employeeWorkingTime.EmployeeId);
            if (employee == null)
            {
                employeeWorkingTime.Errors.Add("Employee", "Tidak valid");
            }
            return employeeWorkingTime;
        }

        public EmployeeWorkingTime VHasStartDate(EmployeeWorkingTime employeeWorkingTime)
        {
            if (employeeWorkingTime.StartDate == null || employeeWorkingTime.StartDate.Equals(DateTime.FromBinary(0)))
            {
                employeeWorkingTime.Errors.Add("StartDate", "Tidak valid");
            }
            return employeeWorkingTime;
        }

        public EmployeeWorkingTime VHasEndDate(EmployeeWorkingTime employeeWorkingTime)
        {
            if (employeeWorkingTime.EndDate == null || employeeWorkingTime.EndDate.Equals(DateTime.FromBinary(0)))
            {
                employeeWorkingTime.Errors.Add("EndDate", "Tidak valid");
            }
            return employeeWorkingTime;
        }

        //public EmployeeWorkingTime VDontHaveEmployees(EmployeeWorkingTime employeeWorkingTime, IEmployeeService _employeeService)
        //{
        //    IList<Employee> employees = _employeeService.GetObjectsByEmployeeWorkingTimeId(employeeWorkingTime.Id);
        //    if (employees.Any())
        //    {
        //        employeeWorkingTime.Errors.Add("Generic", "Tidak boleh masih terasosiasi dengan Employees");
        //    }
        //    return employeeWorkingTime;
        //}

        public bool ValidCreateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService, IEmployeeService _employeeService)
        {
            VHasWorkingTime(employeeWorkingTime, _workingTimeService);
            if (!isValid(employeeWorkingTime)) { return false; }
            VHasEmployee(employeeWorkingTime, _employeeService);
            if (!isValid(employeeWorkingTime)) { return false; }
            VHasStartDate(employeeWorkingTime);
            if (!isValid(employeeWorkingTime)) { return false; }
            VHasEndDate(employeeWorkingTime);
            return isValid(employeeWorkingTime);
        }

        public bool ValidUpdateObject(EmployeeWorkingTime employeeWorkingTime, IWorkingTimeService _workingTimeService, IEmployeeService _employeeService)
        {
            employeeWorkingTime.Errors.Clear();
            ValidCreateObject(employeeWorkingTime, _workingTimeService, _employeeService);
            return isValid(employeeWorkingTime);
        }

        public bool ValidDeleteObject(EmployeeWorkingTime employeeWorkingTime, IEmployeeService _employeeService)
        {
            employeeWorkingTime.Errors.Clear();
            //VDontHaveEmployees(employeeWorkingTime, _employeeService);
            return isValid(employeeWorkingTime);
        }

        public bool isValid(EmployeeWorkingTime obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(EmployeeWorkingTime obj)
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
