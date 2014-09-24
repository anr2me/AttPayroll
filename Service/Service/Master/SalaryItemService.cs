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
    public class SalaryItemService : ISalaryItemService
    {
        private ISalaryItemRepository _repository;
        private ISalaryItemValidator _validator;
        public SalaryItemService(ISalaryItemRepository _salaryItemRepository, ISalaryItemValidator _salaryItemValidator)
        {
            _repository = _salaryItemRepository;
            _validator = _salaryItemValidator;
        }

        public ISalaryItemValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<SalaryItem> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<SalaryItem> GetAll()
        {
            return _repository.GetAll();
        }

        public SalaryItem GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public SalaryItem GetObjectByCode(string Code)
        {
            return _repository.FindAll(x => x.Code == Code && !x.IsDeleted).FirstOrDefault();
        }

        public SalaryItem CreateObject(string Code, string Name)
        {
            SalaryItem salaryItem = new SalaryItem
            {
                Code = Code,
                Name = Name,
            };
            return this.CreateObject(salaryItem);
        }

        public SalaryItem CreateObject(SalaryItem salaryItem)
        {
            salaryItem.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(salaryItem, this) ? _repository.CreateObject(salaryItem) : salaryItem);
        }

        public SalaryItem UpdateObject(SalaryItem salaryItem)
        {
            return (salaryItem = _validator.ValidUpdateObject(salaryItem, this) ? _repository.UpdateObject(salaryItem) : salaryItem);
        }

        public SalaryItem SoftDeleteObject(SalaryItem salaryItem)
        {
            return (salaryItem = _validator.ValidDeleteObject(salaryItem) ?
                    _repository.SoftDeleteObject(salaryItem) : salaryItem);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(SalaryItem salaryItem)
        {
            IQueryable<SalaryItem> salaryItems = _repository.FindAll(x => x.Code == salaryItem.Code && !x.IsDeleted && x.Id != salaryItem.Id);
            return (salaryItems.Count() > 0 ? true : false);
        }
    }
}