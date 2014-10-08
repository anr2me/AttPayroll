using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISalaryEmployeeDetailValidator
    {

        bool ValidCreateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeService _salaryEmployeeService, ISalaryItemService _salaryItemService);
        bool ValidUpdateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeService _salaryEmployeeService, ISalaryItemService _salaryItemService);
        bool ValidDeleteObject(SalaryEmployeeDetail salaryEmployeeDetail);
        bool isValid(SalaryEmployeeDetail salaryEmployeeDetail);
        string PrintError(SalaryEmployeeDetail salaryEmployeeDetail);
    }
}