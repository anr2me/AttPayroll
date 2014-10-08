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
    public class SlipGajiMiniService : ISlipGajiMiniService
    {
        private ISlipGajiMiniRepository _repository;
        private ISlipGajiMiniValidator _validator;
        public SlipGajiMiniService(ISlipGajiMiniRepository _slipGajiMiniRepository, ISlipGajiMiniValidator _slipGajiMiniValidator)
        {
            _repository = _slipGajiMiniRepository;
            _validator = _slipGajiMiniValidator;
        }

        public ISlipGajiMiniValidator GetValidator()
        {
            return _validator;
        }

        public ISlipGajiMiniRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<SlipGajiMini> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SlipGajiMini> GetAll()
        {
            return _repository.GetAll();
        }

        public SlipGajiMini GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SlipGajiMini GetObjectByEmployeeMonth(int EmployeeId, DateTime YearMonth)
        {
            return _repository.FindAll(x => x.EmployeeId == EmployeeId && x.MONTH.Year == YearMonth.Year && x.MONTH.Month == YearMonth.Month).FirstOrDefault();
        }

        public SlipGajiMini FindOrCreateObject(SlipGajiMini slipGajiMini, IEmployeeService _employeeService)
        {
            slipGajiMini.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(slipGajiMini, _employeeService))
            {
                SlipGajiMini slipGajiMini2 = GetObjectByEmployeeMonth(slipGajiMini.EmployeeId, slipGajiMini.MONTH);
                if (slipGajiMini2 == null)
                {
                    _repository.CreateObject(slipGajiMini);
                }
                else
                {
                    slipGajiMini.Id = slipGajiMini2.Id;
                    _repository.UpdateObject(slipGajiMini);
                }
            }
            return slipGajiMini;
        }

        public SlipGajiMini CreateObject(SlipGajiMini slipGajiMini, IEmployeeService _employeeService)
        {
            slipGajiMini.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(slipGajiMini, _employeeService) ? _repository.CreateObject(slipGajiMini) : slipGajiMini);
        }

        public SlipGajiMini UpdateObject(SlipGajiMini slipGajiMini, IEmployeeService _employeeService)
        {
            return (slipGajiMini = _validator.ValidUpdateObject(slipGajiMini, _employeeService) ? _repository.UpdateObject(slipGajiMini) : slipGajiMini);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}