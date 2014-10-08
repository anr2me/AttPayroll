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
    public class WorkingTimeService : IWorkingTimeService
    {
        private IWorkingTimeRepository _repository;
        private IWorkingTimeValidator _validator;
        public WorkingTimeService(IWorkingTimeRepository _workingTimeRepository, IWorkingTimeValidator _workingTimeValidator)
        {
            _repository = _workingTimeRepository;
            _validator = _workingTimeValidator;
        }

        public IWorkingTimeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<WorkingTime> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<WorkingTime> GetAll()
        {
            return _repository.GetAll();
        }

        public WorkingTime GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public WorkingTime CreateObject(WorkingTime workingTime)
        {
            workingTime.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(workingTime, this) ? _repository.CreateObject(workingTime) : workingTime);
        }

        public WorkingTime UpdateObject(WorkingTime workingTime)
        {
            return (workingTime = _validator.ValidUpdateObject(workingTime, this) ? _repository.UpdateObject(workingTime) : workingTime);
        }

        public WorkingTime SoftDeleteObject(WorkingTime workingTime)
        {
            return (workingTime = _validator.ValidDeleteObject(workingTime) ?
                    _repository.SoftDeleteObject(workingTime) : workingTime);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(WorkingTime workingTime)
        {
            IQueryable<WorkingTime> workingTimes = _repository.FindAll(x => x.Code == workingTime.Code && !x.IsDeleted && x.Id != workingTime.Id);
            return (workingTimes.Count() > 0 ? true : false);
        }
    }
}