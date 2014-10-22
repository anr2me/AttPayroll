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
    public class WorkingDayService : IWorkingDayService
    {
        private IWorkingDayRepository _repository;
        private IWorkingDayValidator _validator;
        public WorkingDayService(IWorkingDayRepository _workingDayRepository, IWorkingDayValidator _workingDayValidator)
        {
            _repository = _workingDayRepository;
            _validator = _workingDayValidator;
        }

        public IWorkingDayValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<WorkingDay> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<WorkingDay> GetAll()
        {
            return _repository.GetAll();
        }

        public IList<WorkingDay> GetObjectsByWorkingTimeId(int WorkingTimeId)
        {
            return _repository.GetQueryable().Where(x => x.WorkingTimeId == WorkingTimeId).ToList();
        }

        public WorkingDay GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public WorkingDay GetObjectByCode(string code)
        {
            return _repository.GetObjectByCode(code);
        }

        public WorkingDay CreateObject(WorkingDay workingDay, IWorkingTimeService _workingTimeService)
        {
            workingDay.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(workingDay, _workingTimeService) ? _repository.CreateObject(workingDay) : workingDay);
        }

        public WorkingDay UpdateObject(WorkingDay workingDay, IWorkingTimeService _workingTimeService)
        {
            return (workingDay = _validator.ValidUpdateObject(workingDay, _workingTimeService) ? _repository.UpdateObject(workingDay) : workingDay);
        }

        public WorkingDay CreateOrUpdateObject(WorkingDay workingDay, IWorkingTimeService _workingTimeService)
        {
            WorkingDay wd = GetObjectById(workingDay.Id);
            if (wd == null)
            {
                CreateObject(workingDay, _workingTimeService);
            }
            else
            {
                UpdateObject(workingDay, _workingTimeService);
            }
            return workingDay;
        }

        public WorkingDay SoftDeleteObject(WorkingDay workingDay)
        {
            return (workingDay = _validator.ValidDeleteObject(workingDay) ?
                    _repository.SoftDeleteObject(workingDay) : workingDay);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}