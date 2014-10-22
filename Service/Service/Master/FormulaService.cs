using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Data;

namespace Service.Service
{
    public class FormulaService : IFormulaService
    {
        private IFormulaRepository _repository;
        private IFormulaValidator _validator;
        public FormulaService(IFormulaRepository _formulaRepository, IFormulaValidator _formulaValidator)
        {
            _repository = _formulaRepository;
            _validator = _formulaValidator;
        }

        public IFormulaValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<Formula> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<Formula> GetAll()
        {
            return _repository.GetAll();
        }

        public Formula GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        //public Formula GetObjectBySalaryItemId(int SalaryItemId)
        //{
        //    return _repository.FindAll(x => x.SalaryItemId == SalaryItemId && !x.IsDeleted).FirstOrDefault();
        //}

        public Formula GetObjectBySalarySlipDetailId(int SalarySlipDetailId)
        {
            return _repository.FindAll(x => x.SalarySlipDetailId == SalarySlipDetailId && !x.IsDeleted).FirstOrDefault();
        }

        public Formula CreateObject(Formula formula, ISalaryItemService _salaryItemService)
        {
            formula.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(formula, this, _salaryItemService) ? _repository.CreateObject(formula) : formula);
        }

        public Formula UpdateObject(Formula formula, ISalaryItemService _salaryItemService)
        {
            return (formula = _validator.ValidUpdateObject(formula, this, _salaryItemService) ? _repository.UpdateObject(formula) : formula);
        }

        public Formula SoftDeleteObject(Formula formula)
        {
            return (formula = _validator.ValidDeleteObject(formula) ?
                    _repository.SoftDeleteObject(formula) : formula);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        //decimal ExecuteDecimalCode(string statements)
        //{
        //    string source =
        //    "namespace Foo" +
        //    "{" +
        //        "public class Bar" +
        //        "{"+
        //            "public void Calc()" +
        //            "{" +
        //                statements +
        //            "}"+
        //        "}"+
        //    "}";

        //    decimal val = 0;
        //    return val;
        //}

        /// <summary>
        /// Menghitung nilai Formula
        /// </summary>
        /// <param name="formula">Object formula</param>
        /// <param name="salaryItemsValue">Pair antara salaryitem code dengan nilainya</param>
        /// <param name="salaryItems">List salary items</param>
        /// <returns></returns>
        public decimal CalcFormula(Formula formula, IDictionary<string, decimal> salaryItemsValue, IEnumerable<SalaryItem> salaryItems)
        {
            decimal val = 0;
            //Formula formula = GetObjectById(formulaId);
            if (formula != null)
            {
                decimal Op1 = 0;
                SalaryItem salaryItem = salaryItems.Where(x => x.Id == formula.FirstSalaryItemId.GetValueOrDefault()).FirstOrDefault(); // _salaryItemService.GetObjectById(formula.FirstSalaryItemId.GetValueOrDefault());
                try
                {
                    Op1 = salaryItemsValue[salaryItem.Code];
                }
                catch
                {
                    if (salaryItem != null)
                    {
                        Op1 = salaryItem.DefaultValue; // CurrentValue
                    }
                }
                //if (salaryItem.FormulaId.GetValueOrDefault() > 0)
                //{
                //    Op1 = CalcFormula(salaryItem.Formula, salaryItemsValue, salaryItems);
                //}
                //else
                //{
                //    if (Enum.IsDefined(typeof(Constant.LegacySalaryItem), salaryItem.Code))
                //    {
                //        Op1 = salaryItemsValue[salaryItem.Code]; // _salaryEmployeeDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
                //    }
                //    else if (Enum.IsDefined(typeof(Constant.LegacyAttendanceItem), salaryItem.Code))
                //    {
                //        Op1 = salaryItemsValue[salaryItem.Code]; // _employeeAttendanceDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
                //    }
                //    else if (Enum.IsDefined(typeof(Constant.LegacyMonthlyItem), salaryItem.Code))
                //    {
                //        Op1 = salaryItemsValue[salaryItem.Code]; // _employeeAttendanceDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
                //    }
                //    else
                //    {
                //        Op1 = salaryItem.DefaultValue; // CurrentValue
                //    }

                //}

                decimal Op2 = 0;
                if (formula.IsSecondValue)
                {
                    Op2 = formula.SecondValue;
                }
                else
                {
                    salaryItem = salaryItems.Where(x => x.Id == formula.SecondSalaryItemId.GetValueOrDefault()).FirstOrDefault(); // _salaryItemService.GetObjectById(formula.SecondSalaryItemId.GetValueOrDefault());
                    try
                    {
                        Op2 = salaryItemsValue[salaryItem.Code];
                    }
                    catch
                    {
                        if (salaryItem != null)
                        {
                            Op2 = salaryItem.DefaultValue; // CurrentValue
                        }
                    }
                    //if (salaryItem.FormulaId.GetValueOrDefault() > 0)
                    //{
                    //    Op2 = CalcFormula(salaryItem.Formula, salaryItemsValue, salaryItems);
                    //}
                    //else
                    //{
                    //    if (Enum.IsDefined(typeof(Constant.LegacySalaryItem), salaryItem.Code))
                    //    {
                    //        Op2 = salaryItemsValue[salaryItem.Code]; // _salaryEmployeeDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
                    //    }
                    //    else if (Enum.IsDefined(typeof(Constant.LegacyAttendanceItem), salaryItem.Code))
                    //    {
                    //        Op2 = salaryItemsValue[salaryItem.Code]; // _employeeAttendanceDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
                    //    }
                    //    else if (Enum.IsDefined(typeof(Constant.LegacyMonthlyItem), salaryItem.Code))
                    //    {
                    //        Op2 = salaryItemsValue[salaryItem.Code]; // _employeeAttendanceDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
                    //    }
                    //    else
                    //    {
                    //        Op2 = salaryItem.DefaultValue; // CurrentValue
                    //    }

                    //}
                }

                //string vs = Op1.ToString() + formula.FormulaOp + Op2.ToString();

                //DataTable dt = new DataTable();
                //try
                //{
                //    val = decimal.Parse(dt.Compute(vs, string.Empty).ToString()); // modulo/% only works for integer (without fractional value)
                //}
                //catch
                //{
                //    //val = 0;
                //}

                switch (formula.FormulaOp)
                {
                    case "+": val = Op1 + Op2; break;
                    case "-": val = Op1 - Op2; break;
                    case "*": val = Op1 * Op2; break;
                    case "/": { val = (Op2 == 0)? 0: (Op1 / Op2); break; }
                    case "%": { val = (Op2 == 0) ? 0 : (Op1 % Op2); break; }
                    case "\\": { val = (Op2 == 0) ? 0 : (int)(Op1 / Op2); break; }
                }

                //val = Math.Abs(val);
                //if (formula.HasMinValue)
                //{
                //    val = Math.Max(val, formula.MinValue);
                //}
                //if (formula.HasMaxValue)
                //{
                //    val = Math.Min(val, formula.MaxValue);
                //}
            }
            return val;
        }

        //public decimal CalcFormula(int formulaId, int employeeId, DateTime date, ISalaryItemService _salaryItemService, 
        //                    ISalaryEmployeeDetailService _salaryEmployeeDetailService, IEmployeeAttendanceDetailService _employeeAttendanceDetailService)
        //{
        //    decimal val = 0;
        //    Formula formula = GetObjectById(formulaId);
        //    if (formula != null)
        //    {
        //        SalaryItem salaryItem = _salaryItemService.GetObjectById(formula.FirstSalaryItemId.GetValueOrDefault());
        //        decimal Op1 = 0;
        //        if (salaryItem.FormulaId.GetValueOrDefault() > 0)
        //        {
        //            Op1 = CalcFormula(salaryItem.FormulaId.GetValueOrDefault(), employeeId, date, _salaryItemService, _salaryEmployeeDetailService, _employeeAttendanceDetailService);
        //        }
        //        else
        //        {
        //            if (Enum.IsDefined(typeof(Constant.LegacySalaryItem), salaryItem.Code))
        //            {
        //                Op1 = _salaryEmployeeDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
        //            }
        //            else if (Enum.IsDefined(typeof(Constant.LegacyAttendanceItem), salaryItem.Code))
        //            {
        //                Op1 = _employeeAttendanceDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
        //            }
        //            else if (Enum.IsDefined(typeof(Constant.LegacyMonthlyItem), salaryItem.Code))
        //            {
        //                Op1 = _employeeAttendanceDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
        //            }
        //            else
        //            {
        //                Op1 = salaryItem.DefaultValue;
        //            }
                    
        //        }

        //        decimal Op2 = 0;
        //        if (formula.IsValue)
        //        {
        //            Op2 = formula.Value;
        //        }
        //        else
        //        {
        //            salaryItem = _salaryItemService.GetObjectById(formula.SecondSalaryItemId.GetValueOrDefault());
        //            if (salaryItem.FormulaId.GetValueOrDefault() > 0)
        //            {
        //                Op2 = CalcFormula(salaryItem.FormulaId.GetValueOrDefault(), employeeId, date, _salaryItemService, _salaryEmployeeDetailService, _employeeAttendanceDetailService);
        //            }
        //            else
        //            {
        //                if (Enum.IsDefined(typeof(Constant.LegacySalaryItem), salaryItem.Code))
        //                {
        //                    Op2 = _salaryEmployeeDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
        //                }
        //                else if (Enum.IsDefined(typeof(Constant.LegacyAttendanceItem), salaryItem.Code))
        //                {
        //                    Op2 = _employeeAttendanceDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
        //                }
        //                else if (Enum.IsDefined(typeof(Constant.LegacyMonthlyItem), salaryItem.Code))
        //                {
        //                    Op2 = _employeeAttendanceDetailService.GetObjectByEmployeeIdAndSalaryItemId(employeeId, salaryItem.Id, date).Amount;
        //                }
        //                else
        //                {
        //                    Op2 = salaryItem.DefaultValue;
        //                }
                        
        //            }
        //        }

        //        string vs = Op1.ToString() + formula.FormulaOp + Op2.ToString();

        //        DataTable dt = new DataTable();
        //        val = decimal.Parse(dt.Compute(vs, string.Empty).ToString());
        //    }
        //    return val;
        //}

        //public bool IsFormulaInfinite(int formulaId, ISalaryItemService _salaryItemService, IList<int> stack = null)
        //{
        //    bool result = false;
        //    IList<int> mystack = stack;
        //    if (mystack == null)
        //    {
        //        mystack = new List<int>();
        //    }

        //    Formula formula = GetObjectById(formulaId);
        //    if (formula != null)
        //    {
        //        if (mystack.Contains(formula.SalaryItemId))
        //        {
        //            return true;
        //        }
        //        mystack.Add(formula.SalaryItemId);
        //        SalaryItem salaryItem = _salaryItemService.GetObjectById(formula.FirstSalaryItemId.GetValueOrDefault());
        //        if (salaryItem.FormulaId.GetValueOrDefault() > 0)
        //        {
        //            result = IsFormulaInfinite(salaryItem.FormulaId.GetValueOrDefault(), _salaryItemService, mystack);
        //            if (result) return true;
        //        }

        //        salaryItem = _salaryItemService.GetObjectById(formula.SecondSalaryItemId.GetValueOrDefault());
        //        if (salaryItem.FormulaId.GetValueOrDefault() > 0)
        //        {
        //            result = IsFormulaInfinite(salaryItem.FormulaId.GetValueOrDefault(), _salaryItemService, mystack);
        //            if (result) return true;
        //        }

        //    }
        //    return result;
        //}
    }
}