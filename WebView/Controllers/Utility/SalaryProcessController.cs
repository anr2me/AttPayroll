using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service.Service;
using Core.Interface.Service;
using Core.DomainModel;
using Data.Repository;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;

namespace WebView.Controllers
{
    
    public class SalaryProcessController : Controller
    {
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("SalaryProcessController");
        public ICompanyInfoService _companyInfoService;
        public IBranchOfficeService _branchOfficeService;
        public IDepartmentService _departmentService;
        public IDivisionService _divisionService;
        public ITitleInfoService _titleInfoService;
        public IEmployeeService _employeeService;
        public IEmployeeEducationService _employeeEducationService;
        public ISalaryItemService _salaryItemService;
        public IFormulaService _formulaService;
        public IWorkingTimeService _workingTimeService;
        public IWorkingDayService _workingDayService;
        public IEmployeeWorkingTimeService _employeeWorkingTimeService;
        public ISalaryStandardService _salaryStandardService;
        public ISalaryStandardDetailService _salaryStandardDetailService;
        public ISalaryEmployeeService _salaryEmployeeService;
        public ISalaryEmployeeDetailService _salaryEmployeeDetailService;
        public IEmployeeAttendanceService _employeeAttendanceService;
        //public IEmployeeAttendanceDetailService _employeeAttendanceDetailService;
        public ISalarySlipService _salarySlipService;
        public ISalarySlipDetailService _salarySlipDetailService;
        public IEmployeeLeaveService _employeeLeaveService;
        public IGeneralLeaveService _generalLeaveService;
        public ISPKLService _spklService;
        public IPTKPService _ptkpService;
        public IPPH21SPTService _pph21sptService;
        public IOtherExpenseService _otherExpenseService;
        public IOtherExpenseDetailService _otherExpenseDetailService;
        public IOtherIncomeService _otherIncomeService;
        public IOtherIncomeDetailService _otherIncomeDetailService;
        public ITHRService _thrService;
        public ITHRDetailService _thrDetailService;
        public ISlipGajiMiniService _slipGajiMiniService;
        public ISlipGajiDetailService _slipGajiDetailService;
        public ISlipGajiDetail1Service _slipGajiDetail1Service;
        public ISlipGajiDetail2AService _slipGajiDetail2AService;

        public IUserAccountService _userAccountService;
        //public IUserMenuService _userMenuService;
        //public IUserAccessService _userAccessService;
        public ISalaryProcessService _salaryProcessService;

        public SalaryProcessController()
        {
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            _companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
            _branchOfficeService = new BranchOfficeService(new BranchOfficeRepository(), new BranchOfficeValidator());
            _departmentService = new DepartmentService(new DepartmentRepository(), new DepartmentValidator());
            _divisionService = new DivisionService(new DivisionRepository(), new DivisionValidator());
            _titleInfoService = new TitleInfoService(new TitleInfoRepository(), new TitleInfoValidator());
            _employeeService = new EmployeeService(new EmployeeRepository(), new EmployeeValidator());
            _employeeEducationService = new EmployeeEducationService(new EmployeeEducationRepository(), new EmployeeEducationValidator());
            _salaryItemService = new SalaryItemService(new SalaryItemRepository(), new SalaryItemValidator());
            _formulaService = new FormulaService(new FormulaRepository(), new FormulaValidator());
            _workingTimeService = new WorkingTimeService(new WorkingTimeRepository(), new WorkingTimeValidator());
            _workingDayService = new WorkingDayService(new WorkingDayRepository(), new WorkingDayValidator());
            _employeeWorkingTimeService = new EmployeeWorkingTimeService(new EmployeeWorkingTimeRepository(), new EmployeeWorkingTimeValidator());
            _salaryStandardService = new SalaryStandardService(new SalaryStandardRepository(), new SalaryStandardValidator());
            _salaryStandardDetailService = new SalaryStandardDetailService(new SalaryStandardDetailRepository(), new SalaryStandardDetailValidator());
            _salaryEmployeeService = new SalaryEmployeeService(new SalaryEmployeeRepository(), new SalaryEmployeeValidator());
            _salaryEmployeeDetailService = new SalaryEmployeeDetailService(new SalaryEmployeeDetailRepository(), new SalaryEmployeeDetailValidator());
            _employeeAttendanceService = new EmployeeAttendanceService(new EmployeeAttendanceRepository(), new EmployeeAttendanceValidator());
            //_employeeAttendanceDetailService = new EmployeeAttendanceDetailService(new EmployeeAttendanceDetailRepository(), new EmployeeAttendanceDetailValidator());
            _salarySlipService = new SalarySlipService(new SalarySlipRepository(), new SalarySlipValidator());
            _salarySlipDetailService = new SalarySlipDetailService(new SalarySlipDetailRepository(), new SalarySlipDetailValidator());
            _employeeLeaveService = new EmployeeLeaveService(new EmployeeLeaveRepository(), new EmployeeLeaveValidator());
            _generalLeaveService = new GeneralLeaveService(new GeneralLeaveRepository(), new GeneralLeaveValidator());
            _spklService = new SPKLService(new SPKLRepository(), new SPKLValidator());
            _ptkpService = new PTKPService(new PTKPRepository(), new PTKPValidator());
            _pph21sptService = new PPH21SPTService(new PPH21SPTRepository(), new PPH21SPTValidator());
            _otherExpenseService = new OtherExpenseService(new OtherExpenseRepository(), new OtherExpenseValidator());
            _otherExpenseDetailService = new OtherExpenseDetailService(new OtherExpenseDetailRepository(), new OtherExpenseDetailValidator());
            _otherIncomeService = new OtherIncomeService(new OtherIncomeRepository(), new OtherIncomeValidator());
            _otherIncomeDetailService = new OtherIncomeDetailService(new OtherIncomeDetailRepository(), new OtherIncomeDetailValidator());
            _thrService = new THRService(new THRRepository(), new THRValidator());
            _thrDetailService = new THRDetailService(new THRDetailRepository(), new THRDetailValidator());
            _slipGajiMiniService = new SlipGajiMiniService(new SlipGajiMiniRepository(), new SlipGajiMiniValidator());
            _slipGajiDetailService = new SlipGajiDetailService(new SlipGajiDetailRepository(), new SlipGajiDetailValidator());
            _slipGajiDetail1Service = new SlipGajiDetail1Service(new SlipGajiDetail1Repository(), new SlipGajiDetail1Validator());
            _slipGajiDetail2AService = new SlipGajiDetail2AService(new SlipGajiDetail2ARepository(), new SlipGajiDetail2AValidator());
            _salaryProcessService = new SalaryProcessService(new SalaryProcessValidator()
            {
                _employeeAttendanceService = _employeeAttendanceService,
                _employeeLeaveService = _employeeLeaveService,
                _employeeService = _employeeService,
                _employeeWorkingTimeService = _employeeWorkingTimeService,
                _formulaService = _formulaService,
                _generalLeaveService = _generalLeaveService,
                _spklService = _spklService,
                _ptkpService = _ptkpService,
                _pph21sptService = _pph21sptService,
                _otherExpenseService = _otherExpenseService,
                _otherExpenseDetailService = _otherExpenseDetailService,
                _otherIncomeService = _otherIncomeService,
                _otherIncomeDetailService = _otherIncomeDetailService,
                _thrService = _thrService,
                _thrDetailService = _thrDetailService,
                _workingDayService = _workingDayService,
                _workingTimeService = _workingTimeService,
                _salaryEmployeeDetailService = _salaryEmployeeDetailService,
                _salaryEmployeeService = _salaryEmployeeService,
                _salaryItemService = _salaryItemService,
                _salarySlipDetailService = _salarySlipDetailService,
                _salarySlipService = _salarySlipService,
                _slipGajiDetail1Service = _slipGajiDetail1Service,
                _slipGajiDetail2AService = _slipGajiDetail2AService,
                _slipGajiDetailService = _slipGajiDetailService,
                _slipGajiMiniService = _slipGajiMiniService,
            });
        }

        public ActionResult Index()
        {
            if (!AuthenticationModel.IsAllowed("View", Core.Constants.Constant.MenuName.SalaryProcess, Core.Constants.Constant.MenuGroupName.Setting))
            {
                return Content(Core.Constants.Constant.ErrorPage.PageViewNotAllowed);
            }

            return View(this);
        }

        public dynamic DoProcess(Nullable<int> EmployeeId, bool IsAllEmployee, DateTime StartPeriod, DateTime EndPeriod)
        {
            
            Dictionary<string, string> Errors = new Dictionary<string, string>();
            string error = "";
            //try
            {
                for (var monthYear = StartPeriod; monthYear <= EndPeriod; monthYear = monthYear.AddMonths(1))
                {
                    if (!IsAllEmployee && EmployeeId.GetValueOrDefault() > 0)
                    {
                        error = _salaryProcessService.ProcessEmployee(EmployeeId.GetValueOrDefault(), monthYear);
                    }
                    else
                    {
                        IList<Employee> employees = _employeeService.GetAll();
                        int i = 1;
                        foreach (var employee in employees)
                        {
                            error = _salaryProcessService.ProcessEmployee(employee.Id, monthYear, i++);
                        }
                    }
                }  
            }
            //catch (Exception ex)
            //{
            //    LOG.Error("Process Failed", ex);
            //    Errors.Add("Generic", "Error " + ex);
            //}

            if (error != null && !Errors.Any())
            {
                Errors.Add("Generic", error);
            }

            return Json(new
            {
                Errors
            }, JsonRequestBehavior.AllowGet);
        }



    }
}
