using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IFormulaService
    {
        IFormulaValidator GetValidator();
        IQueryable<Formula> GetQueryable();
        IList<Formula> GetAll();
        Formula GetObjectById(int Id);
        Formula CreateObject(Formula formula, ISalaryItemService _salaryItemService);
        Formula UpdateObject(Formula formula, ISalaryItemService _salaryItemService);
        Formula SoftDeleteObject(Formula formula);
        bool DeleteObject(int Id);
        decimal CalcFormula(Formula formula, IDictionary<string, decimal> salaryItemsValue, IEnumerable<SalaryItem> salaryItems);
        //decimal CalcFormula(int formulaId, int employeeId, DateTime date, ISalaryItemService _salaryItemService, 
        //            ISalaryEmployeeDetailService _salaryEmployeeDetailService, IEmployeeAttendanceDetailService _employeeAttendanceDetailService);
        //bool IsFormulaInfinite(int formulaId, ISalaryItemService _salaryItemService, IList<int> stack = null);
    }
}