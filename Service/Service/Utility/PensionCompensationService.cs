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
    public class PensionCompensationService : IPensionCompensationService
    {
        private IPensionCompensationRepository _repository;
        private IPensionCompensationValidator _validator;
        public PensionCompensationService(IPensionCompensationRepository _pensionCompensationRepository, IPensionCompensationValidator _pensionCompensationValidator)
        {
            _repository = _pensionCompensationRepository;
            _validator = _pensionCompensationValidator;
        }

        public IPensionCompensationValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PensionCompensation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PensionCompensation> GetAll()
        {
            return _repository.GetAll();
        }

        public PensionCompensation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PensionCompensation CreateObject(PensionCompensation pensionCompensation)
        {
            pensionCompensation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(pensionCompensation, this) ? _repository.CreateObject(pensionCompensation) : pensionCompensation);
        }

        public PensionCompensation UpdateObject(PensionCompensation pensionCompensation)
        {
            return (pensionCompensation = _validator.ValidUpdateObject(pensionCompensation, this) ? _repository.UpdateObject(pensionCompensation) : pensionCompensation);
        }

        public PensionCompensation SoftDeleteObject(PensionCompensation pensionCompensation)
        {
            return (pensionCompensation = _validator.ValidDeleteObject(pensionCompensation) ?
                    _repository.SoftDeleteObject(pensionCompensation) : pensionCompensation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}