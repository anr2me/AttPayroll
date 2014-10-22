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
    public class TitleInfoService : ITitleInfoService
    {
        private ITitleInfoRepository _repository;
        private ITitleInfoValidator _validator;
        public TitleInfoService(ITitleInfoRepository _titleInfoRepository, ITitleInfoValidator _titleInfoValidator)
        {
            _repository = _titleInfoRepository;
            _validator = _titleInfoValidator;
        }

        public ITitleInfoValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<TitleInfo> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<TitleInfo> GetAll()
        {
            return _repository.GetAll();
        }

        public TitleInfo GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public TitleInfo GetObjectByCode(string Code)
        {
            return _repository.FindAll(x => x.Code == Code && !x.IsDeleted).FirstOrDefault();
        }

        public TitleInfo CreateObject(string Code, string Name, string Description, bool IsShiftable)
        {
            TitleInfo titleInfo = new TitleInfo
            {
                Code = Code,
                Name = Name,
                Description = Description,
                IsShiftable = IsShiftable,
            };
            return this.CreateObject(titleInfo);
        }

        public TitleInfo CreateObject(TitleInfo titleInfo)
        {
            titleInfo.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(titleInfo, this) ? _repository.CreateObject(titleInfo) : titleInfo);
        }

        public TitleInfo UpdateObject(TitleInfo titleInfo)
        {
            return (titleInfo = _validator.ValidUpdateObject(titleInfo, this) ? _repository.UpdateObject(titleInfo) : titleInfo);
        }

        public TitleInfo SoftDeleteObject(TitleInfo titleInfo, IEmployeeService _employeeService)
        {
            return (titleInfo = _validator.ValidDeleteObject(titleInfo, _employeeService) ?
                    _repository.SoftDeleteObject(titleInfo) : titleInfo);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(TitleInfo titleInfo)
        {
            IQueryable<TitleInfo> titleInfos = _repository.FindAll(x => x.Code == titleInfo.Code && !x.IsDeleted && x.Id != titleInfo.Id);
            return (titleInfos.Count() > 0 ? true : false);
        }
    }
}