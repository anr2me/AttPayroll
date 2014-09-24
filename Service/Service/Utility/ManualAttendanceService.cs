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
    public class ManualAttendanceService : IManualAttendanceService
    {
        private IManualAttendanceRepository _repository;
        private IManualAttendanceValidator _validator;
        public ManualAttendanceService(IManualAttendanceRepository _manualAttendanceRepository, IManualAttendanceValidator _manualAttendanceValidator)
        {
            _repository = _manualAttendanceRepository;
            _validator = _manualAttendanceValidator;
        }

        public IManualAttendanceValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<ManualAttendance> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<ManualAttendance> GetAll()
        {
            return _repository.GetAll();
        }

        public ManualAttendance GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public ManualAttendance CreateObject(ManualAttendance manualAttendance)
        {
            manualAttendance.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(manualAttendance, this) ? _repository.CreateObject(manualAttendance) : manualAttendance);
        }

        public ManualAttendance UpdateObject(ManualAttendance manualAttendance)
        {
            return (manualAttendance = _validator.ValidUpdateObject(manualAttendance, this) ? _repository.UpdateObject(manualAttendance) : manualAttendance);
        }

        public ManualAttendance SoftDeleteObject(ManualAttendance manualAttendance)
        {
            return (manualAttendance = _validator.ValidDeleteObject(manualAttendance) ?
                    _repository.SoftDeleteObject(manualAttendance) : manualAttendance);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        
    }
}