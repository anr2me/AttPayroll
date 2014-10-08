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
    public class CompanyInfoService : ICompanyInfoService
    {
        private ICompanyInfoRepository _repository;
        private ICompanyInfoValidator _validator;
        public CompanyInfoService(ICompanyInfoRepository _companyInfoRepository, ICompanyInfoValidator _companyInfoValidator)
        {
            _repository = _companyInfoRepository;
            _validator = _companyInfoValidator;
        }

        public ICompanyInfoValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<CompanyInfo> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<CompanyInfo> GetAll()
        {
            return _repository.GetAll();
        }

        public CompanyInfo GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public CompanyInfo GetObjectByName(string name)
        {
            return _repository.FindAll(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
        }

        public CompanyInfo CreateObject(string Name, string Address, string PostalCode, string PhoneNumber, string FaxNumber, string Website, string Email)
        {
            CompanyInfo companyInfo = new CompanyInfo
            {
                Name = Name,
                Address = Address,
                PostalCode = PostalCode,
                PhoneNumber = PhoneNumber,
                FaxNumber = FaxNumber,
                Website = Website,
                Email = Email,
            };
            return this.CreateObject(companyInfo);
        }

        public CompanyInfo CreateObject(CompanyInfo companyInfo)
        {
            companyInfo.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(companyInfo, this) ? _repository.CreateObject(companyInfo) : companyInfo);
        }

        public CompanyInfo UpdateObject(CompanyInfo companyInfo)
        {
            //companyInfo.Errors.Clear();
            return (companyInfo = _validator.ValidUpdateObject(companyInfo, this) ? _repository.UpdateObject(companyInfo) : companyInfo);
        }

        public CompanyInfo SoftDeleteObject(CompanyInfo companyInfo)
        {
            return (companyInfo = _validator.ValidDeleteObject(companyInfo) ?
                    _repository.SoftDeleteObject(companyInfo) : companyInfo);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsNameDuplicated(CompanyInfo companyInfo)
        {
            IQueryable<CompanyInfo> companyInfos = _repository.FindAll(x => x.Name == companyInfo.Name && !x.IsDeleted && x.Id != companyInfo.Id);
            return (companyInfos.Count() > 0 ? true : false);
        }
    }
}