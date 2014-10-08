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
    public class LastEducationService : ILastEducationService
    {
        private ILastEducationRepository _repository;
        private ILastEducationValidator _validator;
        public LastEducationService(ILastEducationRepository _lastEducationRepository, ILastEducationValidator _lastEducationValidator)
        {
            _repository = _lastEducationRepository;
            _validator = _lastEducationValidator;
        }

        public ILastEducationValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<LastEducation> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<LastEducation> GetAll()
        {
            return _repository.GetAll();
        }

        public LastEducation GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public LastEducation CreateObject(string Institute, string Major, string Level, DateTime EnrollmentDate, Nullable<DateTime> GraduationDate)
        {
            LastEducation lastEducation = new LastEducation
            {
                Institute = Institute,
                Major = Major,
                Level = Level,
                EnrollmentDate = EnrollmentDate,
                GraduationDate = GraduationDate,
            };
            return this.CreateObject(lastEducation);
        }

        public LastEducation CreateObject(LastEducation lastEducation)
        {
            lastEducation.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(lastEducation, this) ? _repository.CreateObject(lastEducation) : lastEducation);
        }

        public LastEducation UpdateObject(LastEducation lastEducation)
        {
            return (lastEducation = _validator.ValidUpdateObject(lastEducation, this) ? _repository.UpdateObject(lastEducation) : lastEducation);
        }

        public LastEducation SoftDeleteObject(LastEducation lastEducation)
        {
            return (lastEducation = _validator.ValidDeleteObject(lastEducation) ?
                    _repository.SoftDeleteObject(lastEducation) : lastEducation);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}