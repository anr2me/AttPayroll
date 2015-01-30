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
    public class FPUserService : IFPUserService
    {
        private IFPUserRepository _repository;
        private IFPUserValidator _validator;
        public FPUserService(IFPUserRepository _fpUserRepository, IFPUserValidator _fpUserValidator)
        {
            _repository = _fpUserRepository;
            _validator = _fpUserValidator;
        }

        public IFPUserValidator GetValidator()
        {
            return _validator;
        }

        public IFPUserRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<FPUser> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<FPUser> GetAll()
        {
            return _repository.GetAll();
        }

        public FPUser GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public FPUser GetObjectByPIN(int PIN)
        {
            var obj = GetQueryable().Where(x => !x.IsDeleted && x.PIN == PIN).Include(x => x.Employee).FirstOrDefault();
            if (obj != null) obj.Errors = new Dictionary<string, string>();
            return obj;
        }

        public FPUser CreateObject(FPUser fpUser, IEmployeeService _employeeService)
        {
            fpUser.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(fpUser, this, _employeeService))
            {
                //fpUser.IsInSync = false;
                _repository.CreateObject(fpUser);
            }
            return fpUser;
        }

        public FPUser UpdateObject(FPUser fpUser, IEmployeeService _employeeService)
        {
            fpUser.Errors = new Dictionary<String, String>();
            if (_validator.ValidUpdateObject(fpUser, this, _employeeService))
            {
                //fpUser.IsInSync = false;
                _repository.UpdateObject(fpUser);
            }
            return fpUser;
        }

        public FPUser UpdateOrCreateObject(FPUser fpUser, IEmployeeService _employeeService)
        {
            //fpUser.Errors = new Dictionary<String, String>();
            FPUser obj = GetObjectById(fpUser.Id);
            if (obj == null)
            {
                CreateObject(fpUser, _employeeService);
                fpUser = GetObjectById(fpUser.Id); // so virtual can be accessed/included
            }
            else
            {
                UpdateObject(fpUser, _employeeService);
            }
            return fpUser;
        }

        public FPUser SoftDeleteObject(FPUser fpUser)
        {
            if (_validator.ValidDeleteObject(fpUser))
            {
                //fpUser.IsInSync = false;
                _repository.SoftDeleteObject(fpUser);
            }
            return fpUser;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsPINDuplicated(FPUser fpUser)
        {
            IQueryable<FPUser> objs = _repository.FindAll(x => x.PIN == fpUser.PIN && !x.IsDeleted && x.Id != fpUser.Id);
            return (objs.Count() > 0 ? true : false);
        }

        

    }
}