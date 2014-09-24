using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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

        public Formula GetObjectByCode(string Code)
        {
            return _repository.FindAll(x => x.Code == Code && !x.IsDeleted).FirstOrDefault();
        }

        public Formula CreateObject(string Code, string Name)
        {
            Formula formula = new Formula
            {
                Code = Code,
                Name = Name,
            };
            return this.CreateObject(formula);
        }

        public Formula CreateObject(Formula formula)
        {
            formula.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(formula, this) ? _repository.CreateObject(formula) : formula);
        }

        public Formula UpdateObject(Formula formula)
        {
            return (formula = _validator.ValidUpdateObject(formula, this) ? _repository.UpdateObject(formula) : formula);
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

        public bool IsCodeDuplicated(Formula formula)
        {
            IQueryable<Formula> formulas = _repository.FindAll(x => x.Code == formula.Code && !x.IsDeleted && x.Id != formula.Id);
            return (formulas.Count() > 0 ? true : false);
        }
    }
}