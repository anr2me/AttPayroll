using Core.DomainModel;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Interface.Repository;

namespace Core.Interface.Service
{
    public interface ISlipGajiMiniService
    {
        ISlipGajiMiniValidator GetValidator();
        ISlipGajiMiniRepository GetRepository();
        IQueryable<SlipGajiMini> GetQueryable();
        IList<SlipGajiMini> GetAll();
        SlipGajiMini GetObjectById(int Id);
        SlipGajiMini GetObjectByEmployeeMonth(int EmployeeId, DateTime YearMonth);
        SlipGajiMini FindOrCreateObject(SlipGajiMini slipGajiMini, IEmployeeService _employeeService);
        SlipGajiMini CreateObject(SlipGajiMini slipGajiMini, IEmployeeService _employeeService);
        SlipGajiMini UpdateObject(SlipGajiMini slipGajiMini, IEmployeeService _employeeService);
        bool DeleteObject(int Id);
    }

}