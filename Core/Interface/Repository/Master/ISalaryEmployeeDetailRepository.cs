using Core.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Repository
{
    public interface ISalaryEmployeeDetailRepository : IRepository<SalaryEmployeeDetail>
    {
        IQueryable<SalaryEmployeeDetail> GetQueryable();
        IList<SalaryEmployeeDetail> GetAll();
        SalaryEmployeeDetail GetObjectById(int Id);
        SalaryEmployeeDetail CreateObject(SalaryEmployeeDetail salaryEmployeeDetail);
        SalaryEmployeeDetail UpdateObject(SalaryEmployeeDetail salaryEmployeeDetail);
        SalaryEmployeeDetail SoftDeleteObject(SalaryEmployeeDetail salaryEmployeeDetail);
        bool DeleteObject(int Id);
    }
}