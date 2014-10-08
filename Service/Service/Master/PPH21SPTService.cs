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
    public class PPH21SPTService : IPPH21SPTService
    {
        private IPPH21SPTRepository _repository;
        private IPPH21SPTValidator _validator;
        public PPH21SPTService(IPPH21SPTRepository _pph21sptRepository, IPPH21SPTValidator _pph21sptValidator)
        {
            _repository = _pph21sptRepository;
            _validator = _pph21sptValidator;
        }

        public IPPH21SPTValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PPH21SPT> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PPH21SPT> GetAll()
        {
            return _repository.GetAll();
        }

        public PPH21SPT GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PPH21SPT CreateObject(PPH21SPT pph21spt)
        {
            pph21spt.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(pph21spt, this) ? _repository.CreateObject(pph21spt) : pph21spt);
        }

        public PPH21SPT UpdateObject(PPH21SPT pph21spt)
        {
            return (pph21spt = _validator.ValidUpdateObject(pph21spt, this) ? _repository.UpdateObject(pph21spt) : pph21spt);
        }

        public PPH21SPT SoftDeleteObject(PPH21SPT pph21spt)
        {
            return (pph21spt = _validator.ValidDeleteObject(pph21spt) ?
                    _repository.SoftDeleteObject(pph21spt) : pph21spt);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(PPH21SPT pph21spt)
        {
            IQueryable<PPH21SPT> pph21spts = _repository.FindAll(x => x.Code == pph21spt.Code && !x.IsDeleted && x.Id != pph21spt.Id);
            return (pph21spts.Count() > 0 ? true : false);
        }

    }
}