using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalarySlipDetailService
    {
        ISalarySlipDetailValidator GetValidator();
        IQueryable<SalarySlipDetail> GetQueryable();
        IList<SalarySlipDetail> GetAll();
        SalarySlipDetail GetObjectById(int Id);
        SalarySlipDetail CreateObject(int SalarySlipId, int SalarySign, string FirstSalaryItemCode, string Operator, string SecondSalaryItemCode, decimal SecondValue,
                                bool HasMinValue, decimal MinValue, bool HasMaxValue, decimal MaxValue,                   
                                ISalarySlipService _salarySlipService, IFormulaService _formulaService, ISalaryItemService _salaryItemService);
        SalarySlipDetail CreateObject(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService, IFormulaService _formulaService);
        SalarySlipDetail UpdateObject(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService, IFormulaService _formulaService);
        SalarySlipDetail SoftDeleteObject(SalarySlipDetail salarySlipDetail);
        bool DeleteObject(int Id);
        decimal CalcSalarySlipDetail(SalarySlipDetail salarySlipDetail, IDictionary<string, decimal> salaryItemsValue, IEnumerable<SalaryItem> salaryItems, IFormulaService _formulaService);
    }
}