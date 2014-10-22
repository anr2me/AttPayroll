using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface IOtherIncomeDetailRepository : IRepository<OtherIncomeDetail>
    {
        IQueryable<OtherIncomeDetail> GetQueryable();
        IList<OtherIncomeDetail> GetAll();
        OtherIncomeDetail GetObjectById(int Id);
        OtherIncomeDetail CreateObject(OtherIncomeDetail otherIncomeDetail);
        OtherIncomeDetail UpdateObject(OtherIncomeDetail otherIncomeDetail);
        OtherIncomeDetail SoftDeleteObject(OtherIncomeDetail otherIncomeDetail);
        bool DeleteObject(int Id);
    }
}