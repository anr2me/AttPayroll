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
    public class SPKLService : ISPKLService
    {
        private ISPKLRepository _repository;
        private ISPKLValidator _validator;
        public SPKLService(ISPKLRepository _spklRepository, ISPKLValidator _spklValidator)
        {
            _repository = _spklRepository;
            _validator = _spklValidator;
        }

        public ISPKLValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SPKL> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SPKL> GetAll()
        {
            return _repository.GetAll();
        }

        public SPKL GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SPKL CreateObject(SPKL spkl, IEmployeeService _employeeService)
        {
            spkl.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(spkl, _employeeService) ? _repository.CreateObject(spkl) : spkl);
        }

        public SPKL UpdateObject(SPKL spkl, IEmployeeService _employeeService)
        {
            return (spkl = _validator.ValidUpdateObject(spkl, _employeeService) ? _repository.UpdateObject(spkl) : spkl);
        }

        public SPKL SoftDeleteObject(SPKL spkl)
        {
            return (spkl = _validator.ValidDeleteObject(spkl) ?
                    _repository.SoftDeleteObject(spkl) : spkl);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}