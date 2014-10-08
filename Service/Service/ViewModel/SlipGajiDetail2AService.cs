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
    public class SlipGajiDetail2AService : ISlipGajiDetail2AService
    {
        private ISlipGajiDetail2ARepository _repository;
        private ISlipGajiDetail2AValidator _validator;
        public SlipGajiDetail2AService(ISlipGajiDetail2ARepository _slipGajiDetail2ARepository, ISlipGajiDetail2AValidator _slipGajiDetail2AValidator)
        {
            _repository = _slipGajiDetail2ARepository;
            _validator = _slipGajiDetail2AValidator;
        }

        public ISlipGajiDetail2AValidator GetValidator()
        {
            return _validator;
        }

        public ISlipGajiDetail2ARepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<SlipGajiDetail2A> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SlipGajiDetail2A> GetAll()
        {
            return _repository.GetAll();
        }

        public SlipGajiDetail2A GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SlipGajiDetail2A CreateObject(SlipGajiDetail2A slipGajiDetail2A, ISlipGajiDetailService _slipGajiDetailService)
        {
            slipGajiDetail2A.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(slipGajiDetail2A, _slipGajiDetailService) ? _repository.CreateObject(slipGajiDetail2A) : slipGajiDetail2A);
        }

        public SlipGajiDetail2A UpdateObject(SlipGajiDetail2A slipGajiDetail2A, ISlipGajiDetailService _slipGajiDetailService)
        {
            return (slipGajiDetail2A = _validator.ValidUpdateObject(slipGajiDetail2A, _slipGajiDetailService) ? _repository.UpdateObject(slipGajiDetail2A) : slipGajiDetail2A);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}