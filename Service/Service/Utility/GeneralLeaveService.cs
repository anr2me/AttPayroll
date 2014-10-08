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
    public class GeneralLeaveService : IGeneralLeaveService
    {
        private IGeneralLeaveRepository _repository;
        private IGeneralLeaveValidator _validator;
        public GeneralLeaveService(IGeneralLeaveRepository _generalLeaveRepository, IGeneralLeaveValidator _generalLeaveValidator)
        {
            _repository = _generalLeaveRepository;
            _validator = _generalLeaveValidator;
        }

        public IGeneralLeaveValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<GeneralLeave> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<GeneralLeave> GetAll()
        {
            return _repository.GetAll();
        }

        public GeneralLeave GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public GeneralLeave CreateObject(GeneralLeave generalLeave)
        {
            generalLeave.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(generalLeave) ? _repository.CreateObject(generalLeave) : generalLeave);
        }

        public GeneralLeave UpdateObject(GeneralLeave generalLeave)
        {
            return (generalLeave = _validator.ValidUpdateObject(generalLeave) ? _repository.UpdateObject(generalLeave) : generalLeave);
        }

        public GeneralLeave SoftDeleteObject(GeneralLeave generalLeave)
        {
            return (generalLeave = _validator.ValidDeleteObject(generalLeave) ?
                    _repository.SoftDeleteObject(generalLeave) : generalLeave);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}