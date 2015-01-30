using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Entity;

namespace Service.Service
{
    public class FPAttLogService : IFPAttLogService
    {
        private IFPAttLogRepository _repository;
        private IFPAttLogValidator _validator;
        public FPAttLogService(IFPAttLogRepository _fpAttLogRepository, IFPAttLogValidator _fpAttLogValidator)
        {
            _repository = _fpAttLogRepository;
            _validator = _fpAttLogValidator;
        }

        public IFPAttLogValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<FPAttLog> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<FPAttLog> GetAll()
        {
            return _repository.GetAll();
        }

        public FPAttLog GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public FPAttLog GetObjectByUserTime(int FPUserId, DateTime time)
        {
            var obj = GetQueryable().Where(x => !x.IsDeleted && x.FPUserId == FPUserId && x.Time_second == time).Include(x => x.FPUser).FirstOrDefault();
            if (obj != null) obj.Errors = new Dictionary<string, string>();
            return obj;
        }

        public FPAttLog CreateObject(FPAttLog fpAttLog, IFPUserService _fpUserService)
        {
            fpAttLog.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(fpAttLog, this, _fpUserService) ? _repository.CreateObject(fpAttLog) : fpAttLog);
        }

        public FPAttLog UpdateObject(FPAttLog fpAttLog, IFPUserService _fpUserService)
        {
            return (fpAttLog = _validator.ValidUpdateObject(fpAttLog, this, _fpUserService) ? _repository.UpdateObject(fpAttLog) : fpAttLog);
        }

        public FPAttLog FindOrCreateObject(FPAttLog fpAttLog, IFPUserService _fpUserService)
        {
            var obj = GetObjectByUserTime(fpAttLog.FPUserId, fpAttLog.Time_second);
            if (obj == null)
            {
                obj = CreateObject(fpAttLog, _fpUserService);
                obj = GetObjectById(obj.Id); // so virtual can be accessed/included
            }
            return obj;
        }

        public FPAttLog SoftDeleteObject(FPAttLog fpAttLog)
        {
            return (fpAttLog = _validator.ValidDeleteObject(fpAttLog) ?
                    _repository.SoftDeleteObject(fpAttLog) : fpAttLog);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsUserTimeDuplicated(FPAttLog fpAttLog)
        {
            IQueryable<FPAttLog> objs = _repository.FindAll(x => x.FPUserId == fpAttLog.FPUserId && x.Time_second == fpAttLog.Time_second && !x.IsDeleted && x.Id != fpAttLog.Id);
            return (objs.Count() > 0 ? true : false);
        }
        

    }
}