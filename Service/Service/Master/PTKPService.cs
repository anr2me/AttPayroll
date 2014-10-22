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
    public class PTKPService : IPTKPService
    {
        private IPTKPRepository _repository;
        private IPTKPValidator _validator;
        public PTKPService(IPTKPRepository _ptkpRepository, IPTKPValidator _ptkpValidator)
        {
            _repository = _ptkpRepository;
            _validator = _ptkpValidator;
        }

        public IPTKPValidator GetValidator()
        {
            return _validator;
        }

        public IQueryable<PTKP> GetQueryable()
        {
            return _repository.GetQueryable();
        }

        public IList<PTKP> GetAll()
        {
            return _repository.GetAll();
        }

        public PTKP GetObjectById(int Id)
        {
            return _repository.GetObjectById(Id);
        }

        public PTKP GetObjectByCode(string code)
        {
            return _repository.GetObjectByCode(code);
        }

        public PTKP CreateObject(string Code, decimal Amount, string Desc)
        {
            PTKP ptkp = new PTKP
            {
                Code = Code,
                Amount = Amount,
                Description = Desc,
            };
            return CreateObject(ptkp);
        }

        public PTKP CreateObject(PTKP ptkp)
        {
            ptkp.Errors = new Dictionary<String, String>();
            return (_validator.ValidCreateObject(ptkp, this) ? _repository.CreateObject(ptkp) : ptkp);
        }

        public PTKP UpdateObject(PTKP ptkp)
        {
            return (ptkp = _validator.ValidUpdateObject(ptkp, this) ? _repository.UpdateObject(ptkp) : ptkp);
        }

        public PTKP SoftDeleteObject(PTKP ptkp)
        {
            return (ptkp = _validator.ValidDeleteObject(ptkp) ?
                    _repository.SoftDeleteObject(ptkp) : ptkp);
        }

        public bool DeleteObject(int Id)
        {
            return _repository.DeleteObject(Id);
        }

        public bool IsCodeDuplicated(PTKP ptkp)
        {
            IQueryable<PTKP> ptkps = _repository.FindAll(x => x.Code == ptkp.Code && !x.IsDeleted && x.Id != ptkp.Id);
            return (ptkps.Count() > 0 ? true : false);
        }

        //public string GetPTKPCode(bool Single, int NumberOfChildren)
        //{
        //    string code = null;
        //    if (Single)
        //    {
        //        code = "TK";
        //        if (NumberOfChildren > 0)
        //        {
        //            code += NumberOfChildren.ToString();
        //        }
        //    }
        //    else
        //    {
        //        code = "KW" + NumberOfChildren.ToString();
        //    }
        //    return code;
        //}

        public decimal CalcPTKP(bool Single, int NumberOfChildren)
        {
            decimal ptkp = 24300000;
            if (!Single)
            {
                ptkp += 2025000;
            }
            ptkp += (NumberOfChildren * 2025000);
            return ptkp;
        }

    }
}