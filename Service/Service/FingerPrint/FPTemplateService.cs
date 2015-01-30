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
    public class FPTemplateService : IFPTemplateService
    {
        private IFPTemplateRepository _repository;
        private IFPTemplateValidator _validator;
        public FPTemplateService(IFPTemplateRepository _fpTemplateRepository, IFPTemplateValidator _fpTemplateValidator)
        {
            _repository = _fpTemplateRepository;
            _validator = _fpTemplateValidator;
        }

        public IFPTemplateValidator GetValidator()
        {
            return _validator;
        }

        public IFPTemplateRepository GetRepository()
        {
            return _repository;
        }

        public IQueryable<FPTemplate> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<FPTemplate> GetAll()
        {
            return _repository.GetAll();
        }

        public FPTemplate GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public FPTemplate GetObjectByUserFingerID(int FPUserID, int FingerID)
        {
            var obj = GetQueryable().Where(x => !x.IsDeleted && x.FPUserId == FPUserID && x.FingerID == FingerID).Include(x => x.FPUser).FirstOrDefault();
            if (obj != null) obj.Errors = new Dictionary<string, string>();
            return obj;
        }

        public IList<FPTemplate> GetObjectsByFPUserID(int FPUserID)
        {
            return GetQueryable().Where(x => !x.IsDeleted && x.FPUserId == FPUserID).Include(x => x.FPUser).ToList();
        }

        public FPTemplate CreateObject(FPTemplate fpTemplate, IFPUserService _fpUserService)
        {
            fpTemplate.Errors = new Dictionary<String, String>();
            if (_validator.ValidCreateObject(fpTemplate, this, _fpUserService))
            {
                //fpTemplate.IsInSync = false;
                _repository.CreateObject(fpTemplate);
                //FPUser fpUser = _fpUserService.GetObjectById(fpTemplate.FPUserId);
                //if (fpUser != null && fpUser.IsInSync)
                //{
                //    fpUser.IsInSync = false;
                //    _fpUserService.GetRepository().Update(fpUser);
                //}
            }
            return fpTemplate;
        }

        public FPTemplate UpdateObject(FPTemplate fpTemplate, IFPUserService _fpUserService)
        {
            fpTemplate.Errors = new Dictionary<String, String>();
            if (_validator.ValidUpdateObject(fpTemplate, this, _fpUserService))
            {
                //fpTemplate.IsInSync = false;
                _repository.UpdateObject(fpTemplate);
                //FPUser fpUser = _fpUserService.GetObjectById(fpTemplate.FPUserId);
                //if (fpUser != null && fpUser.IsInSync)
                //{
                //    fpUser.IsInSync = false;
                //    _fpUserService.GetRepository().Update(fpUser);
                //}
            }
            return fpTemplate;
        }

        public FPTemplate UpdateOrCreateObject(FPTemplate fpTemplate, IFPUserService _fpUserService)
        {
            //fpTemplate.Errors = new Dictionary<String, String>();
            FPTemplate obj = GetObjectById(fpTemplate.Id);
            if (obj == null)
            {
                CreateObject(fpTemplate, _fpUserService);
                fpTemplate = GetObjectById(fpTemplate.Id); // so virtual can be accessed/included
            }
            else
            {
                UpdateObject(fpTemplate, _fpUserService);
            }
            return fpTemplate;
        }

        public FPTemplate SoftDeleteObject(FPTemplate fpTemplate, IFPUserService _fpUserService)
        {
            if (_validator.ValidDeleteObject(fpTemplate))
            {
                //fpTemplate.IsInSync = false;
                _repository.SoftDeleteObject(fpTemplate);
                //FPUser fpUser = _fpUserService.GetObjectById(fpTemplate.FPUserId);
                //if (fpUser != null && fpUser.IsInSync)
                //{
                //    fpUser.IsInSync = false;
                //    _fpUserService.GetRepository().Update(fpUser);
                //}
            }
            return fpTemplate;
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsFingerIDDuplicated(FPTemplate fpTemplate)
        {
            IQueryable<FPTemplate> objs = _repository.FindAll(x => x.FingerID == fpTemplate.FingerID && x.FPUserId == fpTemplate.FPUserId && !x.IsDeleted && x.Id != fpTemplate.Id);
            return (objs.Count() > 0 ? true : false);
        }

        public bool IsUserInSync(int FPUserId)
        {
            var list = GetQueryable().Where(x => x.FPUserId == FPUserId && !x.IsInSync);
            return (list.Count() == 0);
        }

    }
}