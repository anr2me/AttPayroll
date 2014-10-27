using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IOtherIncomeDetailService
    {
        IOtherIncomeDetailValidator GetValidator();
        IQueryable<OtherIncomeDetail> GetQueryable();
        IList<OtherIncomeDetail> GetAll();
        OtherIncomeDetail GetObjectById(int Id);
        OtherIncomeDetail GetObjectByEmployeeIdAndSalaryItemId(int employeeId, int salaryItemId, DateTime date);
        OtherIncomeDetail CreateObject(OtherIncomeDetail otherIncomeDetail, IOtherIncomeService _otherIncomeService, IEmployeeService _employeeService);
        OtherIncomeDetail UpdateObject(OtherIncomeDetail otherIncomeDetail, IOtherIncomeService _otherIncomeService, IEmployeeService _employeeService);
        OtherIncomeDetail SoftDeleteObject(OtherIncomeDetail otherIncomeDetail);
        bool DeleteObject(int Id);
    }
}