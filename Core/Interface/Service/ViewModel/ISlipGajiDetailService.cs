using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Repository;

namespace Core.Interface.Service
{
    public interface ISlipGajiDetailService
    {
        ISlipGajiDetailValidator GetValidator();
        ISlipGajiDetailRepository GetRepository();
        IQueryable<SlipGajiDetail> GetQueryable();
        IList<SlipGajiDetail> GetAll();
        SlipGajiDetail GetObjectById(int Id);
        SlipGajiDetail GetOrNewObjectByEmployeeMonth(int EmployeeId, DateTime YearMonth);
        SlipGajiDetail CreateOrUpdateObject(SlipGajiDetail slipGajiDetail, IEmployeeService _employeeService, 
                                ISlipGajiDetail1Service _slipGajiDetail1Service, ISlipGajiDetail2AService _slipGajiDetail2AService);
        SlipGajiDetail CreateObject(SlipGajiDetail slipGajiDetail, IEmployeeService _employeeService, 
                                ISlipGajiDetail1Service _slipGajiDetail1Service, ISlipGajiDetail2AService _slipGajiDetail2AService);
        SlipGajiDetail UpdateObject(SlipGajiDetail slipGajiDetail, IEmployeeService _employeeService, 
                                ISlipGajiDetail1Service _slipGajiDetail1Service, ISlipGajiDetail2AService _slipGajiDetail2AService);
        bool DeleteObject(int Id);
    }
    
}