using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Service
{
    public interface ITHRDetailService
    {
        ITHRDetailValidator GetValidator();
        IQueryable<THRDetail> GetQueryable();
        IList<THRDetail> GetAll();
        THRDetail GetObjectById(int Id);
        THRDetail GetObjectByEmployeeIdAndSalaryItemId(int employeeId, int salaryItemId, DateTime date);
        THRDetail GetObjectByEmployeeIdAndTHRId(int employeeId, int THRId);
        THRDetail CreateObject(THRDetail thrDetail, ITHRService _thrService, IEmployeeService _employeeService);
        THRDetail UpdateObject(THRDetail thrDetail, ITHRService _thrService, IEmployeeService _employeeService);
        THRDetail SoftDeleteObject(THRDetail thrDetail);
        bool DeleteObject(int Id);
    }
}