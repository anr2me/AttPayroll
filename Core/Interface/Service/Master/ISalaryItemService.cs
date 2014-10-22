using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;

namespace Core.Interface.Service
{
    public interface ISalaryItemService
    {
        ISalaryItemValidator GetValidator();
        IQueryable<SalaryItem> GetQueryable();
        IList<SalaryItem> GetAll();
        SalaryItem GetObjectById(int Id);
        SalaryItem GetObjectByCode(string Code);
        SalaryItem GetObjectByCode(Constant.LegacyAttendanceItem Code);
        SalaryItem GetObjectByCode(Constant.LegacySalaryItem Code);
        SalaryItem GetObjectByCode(Constant.LegacyMonthlyItem Code);
        SalaryItem FindOrCreateObject(string Code, string Name, int SalarySign, int SalaryItemType, int SalaryStatus, bool IsMainSalary, bool IsDetailSalary, bool IsLegacy);
        SalaryItem CreateObject(string Code, string Name, int SalarySign, int SalaryItemType, int SalaryStatus, bool IsMainSalary, bool IsDetailSalary, bool IsLegacy);
        SalaryItem CreateObject(SalaryItem salaryItem);
        SalaryItem UpdateObject(SalaryItem salaryItem);
        SalaryItem SoftDeleteObject(SalaryItem salaryItem);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(SalaryItem salaryItem);
        decimal CalcSalaryItem(SalaryItem salaryItem, IDictionary<string, decimal> salaryItemsValue, IFormulaService _formulaService);
        //decimal CalcSalaryItem(int salaryItemId, int employeeId, DateTime date, IFormulaService _formulaService,
        //            ISalaryEmployeeDetailService _salaryEmployeeDetailService, IEmployeeAttendanceDetailService _employeeAttendanceDetailService);
    }
}