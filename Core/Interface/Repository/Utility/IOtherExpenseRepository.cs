using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IOtherExpenseRepository : IRepository<OtherExpense>
    {
        IQueryable<OtherExpense> GetQueryable();
        IList<OtherExpense> GetAll();
        OtherExpense GetObjectById(int Id);
        OtherExpense CreateObject(OtherExpense otherExpense);
        OtherExpense UpdateObject(OtherExpense otherExpense);
        OtherExpense SoftDeleteObject(OtherExpense otherExpense);
        bool DeleteObject(int Id);
    }
}