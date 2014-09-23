using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface IFormulaService
    {
        IFormulaValidator GetValidator();
        IQueryable<Formula> GetQueryable();
        IList<Formula> GetAll();
        Formula GetObjectById(int Id);
        Formula CreateObject(Formula formula);
        Formula UpdateObject(Formula formula);
        Formula SoftDeleteObject(Formula formula);
        bool DeleteObject(int Id);
    }
}