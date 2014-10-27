using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace Service.Service
{
    public class SalarySlipDetailService : ISalarySlipDetailService
    {
        private ISalarySlipDetailRepository _repository;
        private ISalarySlipDetailValidator _validator;
        public SalarySlipDetailService(ISalarySlipDetailRepository _salarySlipDetailRepository, ISalarySlipDetailValidator _salarySlipDetailValidator)
        {
            _repository = _salarySlipDetailRepository;
            _validator = _salarySlipDetailValidator;
        }

        public ISalarySlipDetailValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalarySlipDetail> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalarySlipDetail> GetAll()
        {
            return _repository.GetAll();
        }

        public SalarySlipDetail GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalarySlipDetail CreateObject(int SalarySlipId, int SalarySign, string FirstSalaryItemCode, string Operator, string SecondSalaryItemCode, decimal SecondValue, 
                                    bool HasMinValue, decimal MinValue, bool HasMaxValue, decimal MaxValue,
                                    ISalarySlipService _salarySlipService, IFormulaService _formulaService, ISalaryItemService _salaryItemService)
        {
            SalaryItem salaryItem = _salaryItemService.GetObjectByCode(FirstSalaryItemCode);
            Formula formula = new Formula 
            {
                FirstSalaryItemId = salaryItem.Id,
                FormulaOp = Operator,
            };
            salaryItem = _salaryItemService.GetObjectByCode(SecondSalaryItemCode);
            if (salaryItem == null) 
            {
                formula.IsSecondValue = true;
                formula.SecondValue = SecondValue;
                //formula.ValueSign = (int)Constant.SalarySign.Income;
            } 
            else 
            {
                formula.SecondSalaryItemId = salaryItem.Id;
            }
            _formulaService.CreateObject(formula, _salaryItemService);

            SalarySlipDetail salarySlipDetail = new SalarySlipDetail
            {
                SalarySlipId = SalarySlipId,
                SalarySign = SalarySign,
                FormulaId = formula.Id,
                HasMinValue = HasMinValue,
                MinValue = MinValue,
                HasMaxValue = HasMaxValue,
                MaxValue = MaxValue,
            };
            CreateObject(salarySlipDetail, _salarySlipService, _formulaService);
            if (salarySlipDetail.Errors.Any())
            {
                _formulaService.DeleteObject(formula.Id);
            }
            else
            {
                formula.SalarySlipDetailId = salarySlipDetail.Id;
                _formulaService.UpdateObject(formula, _salaryItemService);
            }
            return salarySlipDetail;
        }

        public SalarySlipDetail CreateObject(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService, IFormulaService _formulaService)
        {
            salarySlipDetail.Errors = new Dictionary<String, String>();
            if(_validator.ValidCreateObject(salarySlipDetail, _salarySlipService, _formulaService))
            {
                salarySlipDetail.Index = GetQueryable().Count() + 1;
                _repository.CreateObject(salarySlipDetail);
            }
            return salarySlipDetail;
        }

        public SalarySlipDetail UpdateObject(SalarySlipDetail salarySlipDetail, ISalarySlipService _salarySlipService, IFormulaService _formulaService)
        {
            return (salarySlipDetail = _validator.ValidUpdateObject(salarySlipDetail, _salarySlipService, _formulaService) ? _repository.UpdateObject(salarySlipDetail) : salarySlipDetail);
        }

        public SalarySlipDetail SoftDeleteObject(SalarySlipDetail salarySlipDetail)
        {
            return (salarySlipDetail = _validator.ValidDeleteObject(salarySlipDetail) ?
                    _repository.SoftDeleteObject(salarySlipDetail) : salarySlipDetail);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public decimal CalcSalarySlipDetail(SalarySlipDetail salarySlipDetail, IDictionary<string, decimal> salaryItemsValue, IEnumerable<SalaryItem> salaryItems, IFormulaService _formulaService)
        {
            decimal val = 0;
            if (salarySlipDetail != null)
            {
                Formula formula = _formulaService.GetObjectById(salarySlipDetail.FormulaId);
                if (formula != null)
                {
                    val = /*Math.Abs*/(_formulaService.CalcFormula(formula, salaryItemsValue, salaryItems));
                    if (salarySlipDetail.HasMinValue)
                    {
                        val = Math.Max(val, salarySlipDetail.MinValue);
                    }
                    if (salarySlipDetail.HasMaxValue)
                    {
                        val = Math.Min(val, salarySlipDetail.MaxValue);
                    }
                }
                salarySlipDetail.Value = val;
            }
            return val;
        }
    }
}