using Core.DomainModel;
using Core.Interface.Validation;
using Core.Interface.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Validation.Validation
{
    public class SalaryProcessValidator : ISalaryProcessValidator
    {
        public IEmployeeService _employeeService { get; set; }
        public ISalaryItemService _salaryItemService { get; set; }
        public IFormulaService _formulaService { get; set; }
        public IWorkingTimeService _workingTimeService { get; set; }
        public IWorkingDayService _workingDayService { get; set; }
        public IEmployeeWorkingTimeService _employeeWorkingTimeService { get; set; }
        public ISalaryEmployeeService _salaryEmployeeService { get; set; }
        public ISalaryEmployeeDetailService _salaryEmployeeDetailService { get; set; }
        public IEmployeeAttendanceService _employeeAttendanceService { get; set; }
        //public IEmployeeAttendanceDetailService _employeeAttendanceDetailService { get; set; }
        public ISalarySlipService _salarySlipService { get; set; }
        public ISalarySlipDetailService _salarySlipDetailService { get; set; }
        public IEmployeeLeaveService _employeeLeaveService { get; set; }
        public IGeneralLeaveService _generalLeaveService { get; set; }
        public ISPKLService _spklService { get; set; }
        public IPTKPService _ptkpService { get; set; }
        public IPPH21SPTService _pph21sptService { get; set; }
        public IOtherExpenseService _otherExpenseService { get; set; }
        public IOtherIncomeService _otherIncomeService { get; set; }
        public ISlipGajiMiniService _slipGajiMiniService { get; set; }
        public ISlipGajiDetailService _slipGajiDetailService { get; set; }
        public ISlipGajiDetail1Service _slipGajiDetail1Service { get; set; }
        public ISlipGajiDetail2AService _slipGajiDetail2AService { get; set; }

        public string VHasDate(DateTime date)
        {
            if (date == null || date.Equals(DateTime.FromBinary(0)))
            {
                return ("Date Tidak valid");
            }
            return null;
        }

        public string VHasEmployee(int EmployeeId)
        {
            Employee employee = _employeeService.GetObjectById(EmployeeId);
            if (employee == null)
            {
                return ("Employee Tidak valid");
            }
            return null;
        }

        public string ValidProcessEmployee(Nullable<int> EmployeeId, DateTime yearMonth)
        {
            string error = VHasDate(yearMonth);
            if (error != null) return error;
            error = VHasEmployee(EmployeeId.GetValueOrDefault());
            return error;
        }


    }
}
