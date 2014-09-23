using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISalaryEmployeeValidator
    {
        SalaryEmployee VCreateObject(SalaryEmployee salaryEmployee, ISalaryEmployeeService _salaryEmployeeService);
        SalaryEmployee VUpdateObject(SalaryEmployee salaryEmployee, ISalaryEmployeeService _salaryEmployeeService);
        SalaryEmployee VDeleteObject(SalaryEmployee salaryEmployee);
        bool ValidCreateObject(SalaryEmployee salaryEmployee, ISalaryEmployeeService _salaryEmployeeService);
        bool ValidUpdateObject(SalaryEmployee salaryEmployee, ISalaryEmployeeService _salaryEmployeeService);
        bool ValidDeleteObject(SalaryEmployee salaryEmployee);
        bool isValid(SalaryEmployee salaryEmployee);
        string PrintError(SalaryEmployee salaryEmployee);
    }
}