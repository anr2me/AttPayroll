using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalaryEmployeeDetailService
    {
        ISalaryEmployeeDetailValidator GetValidator();
        IQueryable<SalaryEmployeeDetail> GetQueryable();
        IList<SalaryEmployeeDetail> GetAll();
        SalaryEmployeeDetail GetObjectById(int Id);
        SalaryEmployeeDetail GetObjectByEmployeeIdAndSalaryItemId(int employeeId, int salaryItemId, DateTime date);
        SalaryEmployeeDetail CreateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeService _salaryEmployeeService, ISalaryItemService _salaryItemService);
        SalaryEmployeeDetail UpdateObject(SalaryEmployeeDetail salaryEmployeeDetail, ISalaryEmployeeService _salaryEmployeeService, ISalaryItemService _salaryItemService);
        SalaryEmployeeDetail SoftDeleteObject(SalaryEmployeeDetail salaryEmployeeDetail);
        bool DeleteObject(int Id);
    }
}