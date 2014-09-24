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
    public class LastEmploymentService : ILastEmploymentService
    {
        private ILastEmploymentRepository _repository;
        private ILastEmploymentValidator _validator;
        public LastEmploymentService(ILastEmploymentRepository _lastEmploymentRepository, ILastEmploymentValidator _lastEmploymentValidator)
        {
            _repository = _lastEmploymentRepository;
            _validator = _lastEmploymentValidator;
        }

        public ILastEmploymentValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<LastEmployment> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<LastEmployment> GetAll()
        {
            return _repository.GetAll();
        }

        public LastEmployment GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public LastEmployment CreateObject(string Company, string Title, DateTime StartDate, Nullable<DateTime> EndDate, string ResignReason)
        {
            LastEmployment lastEmployment = new LastEmployment
            {
                Company = Company,
                Title = Title,
                StartDate = StartDate,
                EndDate = EndDate,
                ResignReason = ResignReason,
            };
            return this.CreateObject(lastEmployment);
        }

        public LastEmployment CreateObject(LastEmployment lastEmployment)
        {
            lastEmployment.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(lastEmployment, this) ? _repository.CreateObject(lastEmployment) : lastEmployment);
        }

        public LastEmployment UpdateObject(LastEmployment lastEmployment)
        {
            return (lastEmployment = _validator.ValidUpdateObject(lastEmployment, this) ? _repository.UpdateObject(lastEmployment) : lastEmployment);
        }

        public LastEmployment SoftDeleteObject(LastEmployment lastEmployment)
        {
            return (lastEmployment = _validator.ValidDeleteObject(lastEmployment) ?
                    _repository.SoftDeleteObject(lastEmployment) : lastEmployment);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}