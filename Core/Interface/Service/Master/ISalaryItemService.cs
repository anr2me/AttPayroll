using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ISalaryItemService
    {
        ISalaryItemValidator GetValidator();
        IQueryable<SalaryItem> GetQueryable();
        IList<SalaryItem> GetAll();
        SalaryItem GetObjectById(int Id);
        SalaryItem CreateObject(SalaryItem salaryItem);
        SalaryItem UpdateObject(SalaryItem salaryItem);
        SalaryItem SoftDeleteObject(SalaryItem salaryItem);
        bool DeleteObject(int Id);
    }
}