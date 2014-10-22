using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IPPH21SPTService
    {
        IPPH21SPTValidator GetValidator();
        IQueryable<PPH21SPT> GetQueryable();
        IList<PPH21SPT> GetAll();
        PPH21SPT GetObjectById(int Id);
        PPH21SPT GetObjectBySalary(decimal Amount);
        PPH21SPT CreateObject(string Code, decimal MinAmount, bool IsInfiniteMax, decimal MaxAmount, decimal Percent, string Desc);
        PPH21SPT CreateObject(PPH21SPT pph21spt);
        PPH21SPT UpdateObject(PPH21SPT pph21spt);
        PPH21SPT SoftDeleteObject(PPH21SPT pph21spt);
        bool DeleteObject(int Id);
        bool IsCodeDuplicated(PPH21SPT pph21spt);
        decimal CalcPPH21(bool HasNPWP, decimal TaxableIncome, IList<decimal> pphlist);
    }
}