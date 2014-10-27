using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Validation.Validation
{
    public class EmployeeValidator : IEmployeeValidator
    {
        public Employee VHasDivision(Employee employee, IDivisionService _divisionService)
        {
            Division division = _divisionService.GetObjectById(employee.DivisionId);
            if (division == null)
            {
                employee.Errors.Add("Division", "Tidak valid");
            }
            return employee;
        }

        public Employee VHasTitleInfo(Employee employee, ITitleInfoService _titleInfoService)
        {
            TitleInfo titleInfo = _titleInfoService.GetObjectById(employee.TitleInfoId);
            if (titleInfo == null)
            {
                employee.Errors.Add("TitleInfo", "Tidak valid");
            }
            return employee;
        }

        public Employee VHasUniqueNIK(Employee employee, IEmployeeService _employeeService)
        {
            if (String.IsNullOrEmpty(employee.NIK) || employee.NIK.Trim() == "")
            {
                employee.Errors.Add("NIK", "Tidak boleh kosong");
            }
            else if (_employeeService.IsNIKDuplicated(employee))
            {
                employee.Errors.Add("NIK", "Tidak boleh ada duplikasi");
            }
            return employee;
        }

        public Employee VHasName(Employee employee)
        {
            if (String.IsNullOrEmpty(employee.Name) || employee.Name.Trim() == "")
            {
                employee.Errors.Add("Name", "Tidak boleh kosong");
            }
            return employee;
        }

        public Employee VHasBirthDate(Employee employee)
        {
            if (employee.BirthDate == null || employee.BirthDate.Equals(DateTime.FromBinary(0)))
            {
                employee.Errors.Add("BirthDate", "Tidak valid");
            }
            return employee;
        }

        public Employee VHasAddress(Employee employee)
        {
            if (String.IsNullOrEmpty(employee.Address) || employee.Address.Trim() == "")
            {
                employee.Errors.Add("Address", "Tidak boleh kosong");
            }
            return employee;
        }

        public Employee VHasPhoneNumber(Employee employee)
        {
            if (String.IsNullOrEmpty(employee.PhoneNumber) || employee.PhoneNumber.Trim() == "")
            {
                employee.Errors.Add("PhoneNumber", "Tidak boleh kosong");
            }
            return employee;
        }

        public Employee VHasStartWorkingDate(Employee employee)
        {
            if (employee.StartWorkingDate == null || employee.StartWorkingDate.Equals(DateTime.FromBinary(0)))
            {
                employee.Errors.Add("StartWorkingDate", "Tidak valid");
            }
            return employee;
        }

        public bool ValidCreateObject(Employee employee, IEmployeeService _employeeService, IDivisionService _divisionService, ITitleInfoService _titleInfoService)
        {
            VHasDivision(employee, _divisionService);
            if (!isValid(employee)) { return false; }
            VHasTitleInfo(employee, _titleInfoService);
            if (!isValid(employee)) { return false; }
            VHasUniqueNIK(employee, _employeeService);
            if (!isValid(employee)) { return false; }
            VHasName(employee);
            if (!isValid(employee)) { return false; }
            VHasBirthDate(employee);
            if (!isValid(employee)) { return false; }
            VHasAddress(employee);
            if (!isValid(employee)) { return false; }
            VHasPhoneNumber(employee);
            if (!isValid(employee)) { return false; }
            VHasStartWorkingDate(employee);
            return isValid(employee);
        }

        public bool ValidUpdateObject(Employee employee, IEmployeeService _employeeService, IDivisionService _divisionService, ITitleInfoService _titleInfoService)
        {
            employee.Errors.Clear();
            ValidCreateObject(employee, _employeeService, _divisionService, _titleInfoService);
            return isValid(employee);
        }

        public bool ValidDeleteObject(Employee employee)
        {
            employee.Errors.Clear();
            return isValid(employee);
        }

        public bool isValid(Employee obj)
        {
            bool isValid = !obj.Errors.Any();
            return isValid;
        }

        public string PrintError(Employee obj)
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
