using Core.DomainModel;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Interface.Validation
{
    public interface ISalaryProcessValidator
    {
        IEmployeeService _employeeService { get; set; }
        ISalaryItemService _salaryItemService { get; set; }
        IFormulaService _formulaService { get; set; }
        IWorkingTimeService _workingTimeService { get; set; }
        IWorkingDayService _workingDayService { get; set; }
        IEmployeeWorkingTimeService _employeeWorkingTimeService { get; set; }
        ISalaryEmployeeService _salaryEmployeeService { get; set; }
        ISalaryEmployeeDetailService _salaryEmployeeDetailService { get; set; }
        IEmployeeAttendanceService _employeeAttendanceService { get; set; }
        //IEmployeeAttendanceDetailService _employeeAttendanceDetailService { get; set; }
        ISalarySlipService _salarySlipService { get; set; }
        ISalarySlipDetailService _salarySlipDetailService { get; set; }
        IEmployeeLeaveService _employeeLeaveService { get; set; }
        IGeneralLeaveService _generalLeaveService { get; set; }
        ISPKLService _spklService { get; set; }
        IPTKPService _ptkpService { get; set; }
        IPPH21SPTService _pph21sptService { get; set; }
        IOtherExpenseService _otherExpenseService { get; set; }
        IOtherIncomeService _otherIncomeService { get; set; }
        ISlipGajiMiniService _slipGajiMiniService { get; set; }
        ISlipGajiDetailService _slipGajiDetailService { get; set; }
        ISlipGajiDetail1Service _slipGajiDetail1Service { get; set; }
        ISlipGajiDetail2AService _slipGajiDetail2AService { get; set; }

        string VHasEmployee(int EmployeeId);
        string VHasDate(DateTime date);

        string ValidProcessEmployee(Nullable<int> EmployeeId, DateTime yearMonth);
    }
}