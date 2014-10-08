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
    public class SlipGajiDetail1Service : ISlipGajiDetail1Service
    {
        private ISlipGajiDetail1Repository _repository;
        private ISlipGajiDetail1Validator _validator;
        public SlipGajiDetail1Service(ISlipGajiDetail1Repository _slipGajiDetail1Repository, ISlipGajiDetail1Validator _slipGajiDetail1Validator)
        {
            _repository = _slipGajiDetail1Repository;
            _validator = _slipGajiDetail1Validator;
        }

        public ISlipGajiDetail1Validator GetValidator()
        {
            return _validator;
        }

        public ISlipGajiDetail1Repository GetRepository()
        {
            return _repository;
        }

        public IQueryable<SlipGajiDetail1> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SlipGajiDetail1> GetAll()
        {
            return _repository.GetAll();
        }

        public SlipGajiDetail1 GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SlipGajiDetail1 CreateObject(SlipGajiDetail1 slipGajiDetail1, ISlipGajiDetailService _slipGajiDetailService)
        {
            slipGajiDetail1.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(slipGajiDetail1, _slipGajiDetailService) ? _repository.CreateObject(slipGajiDetail1) : slipGajiDetail1);
        }

        public SlipGajiDetail1 UpdateObject(SlipGajiDetail1 slipGajiDetail1, ISlipGajiDetailService _slipGajiDetailService)
        {
            return (slipGajiDetail1 = _validator.ValidUpdateObject(slipGajiDetail1, _slipGajiDetailService) ? _repository.UpdateObject(slipGajiDetail1) : slipGajiDetail1);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }
    }
}