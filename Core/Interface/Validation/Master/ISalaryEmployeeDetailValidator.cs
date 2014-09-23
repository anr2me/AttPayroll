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
        SalaryEmployeeDetail VCreateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeDetailService _salaryEmployeeDetailService);
        SalaryEmployeeDetail VUpdateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeDetailService _salaryEmployeeDetailService);
        SalaryEmployeeDetail VDeleteObject(SalaryEmployeeDetail salaryEmployeeDetail);
        bool ValidCreateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeDetailService _salaryEmployeeDetailService);
        bool ValidUpdateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeDetailService _salaryEmployeeDetailService);
        bool ValidDeleteObject(SalaryEmployeeDetail salaryEmployeeDetail);
        bool isValid(SalaryEmployeeDetail salaryEmployeeDetail);
        string PrintError(SalaryEmployeeDetail salaryEmployeeDetail);
    }
}