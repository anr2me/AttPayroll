using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IOtherExpenseDetailRepository : IRepository<OtherExpenseDetail>
    {
        IQueryable<OtherExpenseDetail> GetQueryable();
        IList<OtherExpenseDetail> GetAll();
        OtherExpenseDetail GetObjectById(int Id);
        OtherExpenseDetail CreateObject(OtherExpenseDetail otherExpenseDetail);
        OtherExpenseDetail UpdateObject(OtherExpenseDetail otherExpenseDetail);
        OtherExpenseDetail SoftDeleteObject(OtherExpenseDetail otherExpenseDetail);
        bool DeleteObject(int Id);
    }
}