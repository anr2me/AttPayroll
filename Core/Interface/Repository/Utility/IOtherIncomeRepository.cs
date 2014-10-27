using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IOtherIncomeRepository : IRepository<OtherIncome>
    {
        IQueryable<OtherIncome> GetQueryable();
        IList<OtherIncome> GetAll();
        OtherIncome GetObjectById(int Id);
        OtherIncome CreateObject(OtherIncome otherIncome);
        OtherIncome UpdateObject(OtherIncome otherIncome);
        OtherIncome SoftDeleteObject(OtherIncome otherIncome);
        bool DeleteObject(int Id);
    }
}