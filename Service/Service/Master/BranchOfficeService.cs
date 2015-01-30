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
    public class BranchOfficeService : IBranchOfficeService
    {
        private IBranchOfficeRepository _repository;
        private IBranchOfficeValidator _validator;
        public BranchOfficeService(IBranchOfficeRepository _branchOfficeRepository, IBranchOfficeValidator _branchOfficeValidator)
        {
            _repository = _branchOfficeRepository;
            _validator = _branchOfficeValidator;
        }

        public IBranchOfficeValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<BranchOffice> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<BranchOffice> GetAll()
        {
            return _repository.GetAll();
        }

        public BranchOffice GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public BranchOffice GetObjectByCode(string Code)
        {
            return _repository.FindAll(x => x.Code == Code && !x.IsDeleted).FirstOrDefault();
        }

        public BranchOffice GetObjectByName(string name)
        {
            return _repository.FindAll(x => x.Name == name && !x.IsDeleted).FirstOrDefault();
        }

        public BranchOffice CreateObject(string Code, string Name, string Address, string City, string PostalCode, string PhoneNumber, string FaxNumber, string Email, ICompanyInfoService _companyInfoService)
        {
            BranchOffice branchOffice = new BranchOffice
            {
                Code = Code,
                Name = Name,
                Address = Address,
                PostalCode = PostalCode,
                PhoneNumber = PhoneNumber,
                FaxNumber = FaxNumber,
                Email = Email,
            };
            return this.CreateObject(branchOffice, _companyInfoService);
        }

        public BranchOffice FindOrCreateObject(string Code, string Name, string Address, string City, string PostalCode, string PhoneNumber, string FaxNumber, string Email, ICompanyInfoService _companyInfoService)
        {
            BranchOffice branchOffice = GetObjectByCode(Code);
            if (branchOffice != null)
            {
                branchOffice.Errors = new Dictionary<String, String>();
                return branchOffice;
            }
            branchOffice = new BranchOffice
            {
                Code = Code,
                Name = Name,
                Address = Address,
                PostalCode = PostalCode,
                PhoneNumber = PhoneNumber,
                FaxNumber = FaxNumber,
                Email = Email,
            };
            return this.CreateObject(branchOffice, _companyInfoService);
        }

        public BranchOffice CreateObject(BranchOffice branchOffice, ICompanyInfoService _companyInfoService)
        {
            branchOffice.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(branchOffice, this, _companyInfoService) ? _repository.CreateObject(branchOffice) : branchOffice);
        }

        public BranchOffice FindOrCreateObject(BranchOffice branchOffice, ICompanyInfoService _companyInfoService)
        {
            branchOffice.Errors = new Dictionary<String, String>();
            BranchOffice obj = GetObjectByCode(branchOffice.Code);
            if (obj != null)
            {
                obj.Errors = new Dictionary<String, String>();
                return obj;
            }
            if (_validator.ValidCreateObject(branchOffice, this, _companyInfoService))
            {
                _repository.CreateObject(branchOffice);
            }
            return branchOffice;
        }

        public BranchOffice UpdateObject(BranchOffice branchOffice, ICompanyInfoService _companyInfoService)
        {
            //branchOffice.Errors.Clear();
            return (branchOffice = _validator.ValidUpdateObject(branchOffice, this, _companyInfoService) ? _repository.UpdateObject(branchOffice) : branchOffice);
        }

        public BranchOffice SoftDeleteObject(BranchOffice branchOffice, IDepartmentService _departmentService)
        {
            return (branchOffice = _validator.ValidDeleteObject(branchOffice, _departmentService) ?
                    _repository.SoftDeleteObject(branchOffice) : branchOffice);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(BranchOffice branchOffice)
        {
            IQueryable<BranchOffice> branchOffices = _repository.FindAll(x => x.Code == branchOffice.Code && !x.IsDeleted && x.Id != branchOffice.Id);
            return (branchOffices.Count() > 0 ? true : false);
        }

        public bool IsNameDuplicated(BranchOffice branchOffice)
        {
            IQueryable<BranchOffice> branchOffices = _repository.FindAll(x => x.Name == branchOffice.Name && !x.IsDeleted && x.Id != branchOffice.Id);
            return (branchOffices.Count() > 0 ? true : false);
        }
    }
}