using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.Constants;
using Core.DomainModel;
using NSpec;
using Service.Service;
using Core.Interface.Service;
using Data.Context;
using System.Data.Entity;
using Data.Repository;
using Validation.Validation;
using System.Data.SqlTypes;

namespace TestValidation
{
    public class DataBuilder
    {
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

        public CompanyInfo comp1;
        public BranchOffice branch1;
        public Department dept1, dept2;
        public Division div1, div2;
        public TitleInfo tit1, tit2;
        
        public WorkingTime wt1, wt2;
        public Employee emp1, emp2;
        public EmployeeEducation empEdu1, empEdu2;
        public EmployeeWorkingTime ewt1, ewt2;
        public EmployeeAttendance empatt;
        public SalaryStandard ss1, ss2;
        public SalaryStandardDetail ssd1, ssd2;
        public SalaryEmployee se1, se2;
        public SalaryEmployeeDetail sed1, sed2;
        public OtherIncome incomelain;
        public OtherIncomeDetail incomelaindet;
        public OtherExpense expenselain;
        public OtherExpenseDetail expenselaindet;
        public THR thr;
        public THRDetail thrdet;
        public PTKP ptkp;
        public PPH21SPT pph21spt;
        public SalaryItem salaryitem;
        public SalarySlip slip;
        public SalarySlipDetail slipdetail;

        public DataBuilder()
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

        public void PopulateData()
        {
            PopulateCompanyData();
            PopulateWorkingTimeData();
            PopulateLegacyData();
            PopulateSlipData();
            PopulateEmployeeData();
            PopulateAttendanceData();
            PopulateSalaryData();
            PopulateOtherData();
        }

        public void PopulateCompanyData()
        {
            //comp1 = _companyInfoService.CreateObject("PT. SINAR RODA UTAMA", "Jl. Panjang Banget No.1", "Jakarta", "11560", "021-5556666", "021-5557777", "admin@gmail.com", "www.sru.com", "01-345-789-1-345-789", DateTime.Now);
            comp1 = _companyInfoService.CreateObject("PT. KTC COAL MINING & ENERGY", "Jl. Panjang Banget No.1", "Jakarta", "11560", "021-5556666", "021-5557777", "admin@gmail.com", "www.ktc.com", "01-345-789-1-345-789", DateTime.Now);
            branch1 = _branchOfficeService.CreateObject("HQ", "HEADQUARTER", "Jl. Panjang Banget No.1", "Jakarta", "11560", "021-5556666", "021-5557777", "hq@gmail.com");
            dept1 = _departmentService.CreateObject(branch1.Id, "HRD", "HRD", "Human Resource Development", _branchOfficeService);
            dept2 = _departmentService.CreateObject(branch1.Id, "IT", "IT", "Information Technology", _branchOfficeService);
            div1 = _divisionService.CreateObject(dept1.Id, "RCT", "RECRUITMENT", "Recruitment", _departmentService);
            div2 = _divisionService.CreateObject(dept2.Id, "SPT", "SUPPORT", "Hardware System", _departmentService);
            tit1 = _titleInfoService.CreateObject("HRM", "HRD MANAGER", "HRD Manager", false);
            tit2 = _titleInfoService.CreateObject("HRM", "IT SUPPORT", "IT Support", true);
        }

        public void PopulateWorkingTimeData()
        {
            wt1 = new WorkingTime
            {
                Code = "N",
                Name = "Normal",
                MinCheckIn = (DateTime)SqlDateTime.MinValue + new TimeSpan(05, 00, 00),
                CheckIn = (DateTime)SqlDateTime.MinValue + new TimeSpan(09, 00, 00),
                MaxCheckIn = (DateTime)SqlDateTime.MinValue + new TimeSpan(10, 00, 00),
                MinCheckOut = (DateTime)SqlDateTime.MinValue + new TimeSpan(14, 00, 00),
                CheckOut = (DateTime)SqlDateTime.MinValue + new TimeSpan(17, 00, 00),
                MaxCheckOut = (DateTime)SqlDateTime.MinValue + new TimeSpan(04, 59, 00),
                BreakOut = (DateTime)SqlDateTime.MinValue + new TimeSpan(11, 30, 00),
                BreakIn = (DateTime)SqlDateTime.MinValue + new TimeSpan(12, 30, 00),
            };
            _workingTimeService.CreateObject(wt1, _workingDayService);

            wt2 = new WorkingTime
            {
                Code = "NS",
                Name = "Malam",
                MinCheckIn = (DateTime)SqlDateTime.MinValue + new TimeSpan(17, 00, 00),
                CheckIn = (DateTime)SqlDateTime.MinValue + new TimeSpan(19, 00, 00),
                MaxCheckIn = (DateTime)SqlDateTime.MinValue + new TimeSpan(22, 00, 00),
                MinCheckOut = (DateTime)SqlDateTime.MinValue + new TimeSpan(02, 00, 00),
                CheckOut = (DateTime)SqlDateTime.MinValue + new TimeSpan(03, 00, 00),
                MaxCheckOut = (DateTime)SqlDateTime.MinValue + new TimeSpan(16, 59, 00),
                BreakOut = (DateTime)SqlDateTime.MinValue + new TimeSpan(22, 30, 00),
                BreakIn = (DateTime)SqlDateTime.MinValue + new TimeSpan(23, 30, 00),
            };
            _workingTimeService.CreateObject(wt2, _workingDayService);
        }

        public void PopulateLegacyData()
        {
            // Legacy User Account
            UserAccount userAccount = _userAccountService.GetObjectByUsername(Constant.UserType.Admin);
            if (userAccount == null)
            {
                userAccount = _userAccountService.CreateObject(Constant.UserType.Admin, "sysadmin", "Administrator", "Administrator", true);
            }

            // Legacy Attendance Items
            _salaryItemService.FindOrCreateObject("REGWT", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.REGWT), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("WRKTM", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.WRKTM), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("OVRTM", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.OVRTM), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("CILTM", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.CILTM), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("COETM", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.COETM), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            //_salaryItemService.FindOrCreateObject("BILTM", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.BILTM), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            //_salaryItemService.FindOrCreateObject("BOETM", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.BOETM), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("OTH15", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.OTH15), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("OTH20", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.OTH20), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("OTH30", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.OTH30), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("OTH40", Constant.GetEnumDesc(Constant.LegacyAttendanceItem.OTH40), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Daily, true, true, true);

            // Legacy Salary Items
            _salaryItemService.FindOrCreateObject("GJPOK", Constant.GetEnumDesc(Constant.LegacySalaryItem.GJPOK), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("OVTRT", Constant.GetEnumDesc(Constant.LegacySalaryItem.OVTRT), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Evently, true, true, true);
            _salaryItemService.FindOrCreateObject("LTTRT", Constant.GetEnumDesc(Constant.LegacySalaryItem.LTTRT), (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Evently, true, true, true);
            _salaryItemService.FindOrCreateObject("INHDR", Constant.GetEnumDesc(Constant.LegacySalaryItem.INHDR), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Evently, true, true, true);
            _salaryItemService.FindOrCreateObject("TJMKN", Constant.GetEnumDesc(Constant.LegacySalaryItem.TJMKN), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("TJTRN", Constant.GetEnumDesc(Constant.LegacySalaryItem.TJTRN), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("TJLAP", Constant.GetEnumDesc(Constant.LegacySalaryItem.TJLAP), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Daily, true, true, true);
            _salaryItemService.FindOrCreateObject("TJLNA", Constant.GetEnumDesc(Constant.LegacySalaryItem.TJLNA), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Daily, true, true, true);

            // Legacy Monthly Items
            _salaryItemService.FindOrCreateObject("PRESN", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.PRESN), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("ALPHA", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.ALPHA), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("LATE", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.LATE), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Attendance, (int)Constant.SalaryItemStatus.Monthly, true, true, true);

            _salaryItemService.FindOrCreateObject("TOT15", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.TOT15), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("TOT20", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.TOT20), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("TOT30", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.TOT30), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("TOT40", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.TOT40), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            
            _salaryItemService.FindOrCreateObject("PTKP", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.PTKP), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.Salary/*PTKP*/, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            
            _salaryItemService.FindOrCreateObject("THR", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.THR), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.THR, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("TOTTJ", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.TOTTJ), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.OtherIncome, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("TOTPT", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.TOTPT), (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemType.OtherExpense, (int)Constant.SalaryItemStatus.Monthly, true, true, true);

            _salaryItemService.FindOrCreateObject("PPH05D", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.PPH05D), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.PPH21, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("PPH15D", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.PPH15D), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.PPH21, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("PPH25D", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.PPH25D), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.PPH21, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("PPH30D", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.PPH30D), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.PPH21, (int)Constant.SalaryItemStatus.Monthly, true, true, true);

            _salaryItemService.FindOrCreateObject("PPH05P", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.PPH05P), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.PPH21, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("PPH15P", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.PPH15P), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.PPH21, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("PPH25P", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.PPH25P), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.PPH21, (int)Constant.SalaryItemStatus.Monthly, true, true, true);
            _salaryItemService.FindOrCreateObject("PPH30P", Constant.GetEnumDesc(Constant.LegacyMonthlyItem.PPH30P), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemType.PPH21, (int)Constant.SalaryItemStatus.Monthly, true, true, true);

        }

        public void PopulateSlipData()
        {
            // Custom Salary Slip/Items (details may have formula)
            slip = _salarySlipService.CreateObject("TOT10", Constant.GetEnumDesc(Constant.UserMonthlyItem.TOT10), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TOT15", "*", "", 1.5m, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TOT20", "*", "", 2, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TOT30", "*", "", 3, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TOT40", "*", "", 4, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TOVTM", Constant.GetEnumDesc(Constant.UserMonthlyItem.TOVTM), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "OVTRT", "*", "TOT10", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TTJLP", Constant.GetEnumDesc(Constant.UserMonthlyItem.TTJLP), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TJLAP", "*", "PRESN", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TINHD", Constant.GetEnumDesc(Constant.UserMonthlyItem.TINHD), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "INHDR", "*", "PRESN", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TTJMK", Constant.GetEnumDesc(Constant.UserMonthlyItem.TTJMK), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TJMKN", "*", "PRESN", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TTJTR", Constant.GetEnumDesc(Constant.UserMonthlyItem.TTJTR), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TJTRN", "*", "PRESN", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            //slip = _salarySlipService.CreateObject("TOTTJ", Constant.GetEnumDesc(Constant.UserMonthlyItem.TOTTJ), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);

            slip = _salarySlipService.CreateObject("KBLLU", Constant.GetEnumDesc(Constant.UserMonthlyItem.KBLLU), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);

            //slip = _salarySlipService.CreateObject("THR", Constant.GetEnumDesc(Constant.UserMonthlyItem.THR), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Yearly, true, true, true, false, false, _salaryItemService);
            //_salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJPOK", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            //_salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TOTTJ", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("PTABS", Constant.GetEnumDesc(Constant.UserMonthlyItem.PTABS), (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "LTTRT", "*", "ALPHA", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            //slip = _salarySlipService.CreateObject("TOTPT", Constant.GetEnumDesc(Constant.UserMonthlyItem.TOTPT), (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);

            slip = _salarySlipService.CreateObject("GJKOT", Constant.GetEnumDesc(Constant.UserMonthlyItem.GJKOT), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJPOK", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TOVTM", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TTJLP", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TTJMK", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TINHD", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TOTTJ", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "KBLLU", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "THR", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "PTABS", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "TOTPT", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("PTPJM", Constant.GetEnumDesc(Constant.UserMonthlyItem.PTPJM), (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);

            slip = _salarySlipService.CreateObject("JMSTK", Constant.GetEnumDesc(Constant.UserMonthlyItem.JMSTK), (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJPOK", "*", "", 0.02m, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("PJ204", Constant.GetEnumDesc(Constant.UserMonthlyItem.PJ204), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJPOK", "*", "", 0.02040m, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TDKOT", Constant.GetEnumDesc(Constant.UserMonthlyItem.TDKOT), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJKOT", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "PJ204", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("PJTJJB", Constant.GetEnumDesc(Constant.UserMonthlyItem.PJTJJB), (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "TDKOT", "*", "", 0.05m, false, 0, true, 500000, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("PPTKP", Constant.GetEnumDesc(Constant.UserMonthlyItem.PPTKP), (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "PTKP", "/", "", 12, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TPPJK", Constant.GetEnumDesc(Constant.UserMonthlyItem.TPPJK), (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "PJTJJB", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "JMSTK", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "PPTKP", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TDKPJ", Constant.GetEnumDesc(Constant.UserMonthlyItem.TDKPJ), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKOT", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "TPPJK", "+", "", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TDKPT", Constant.GetEnumDesc(Constant.UserMonthlyItem.TDKPT), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPJ", "*", "", 12, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TDKPT05", Constant.GetEnumDesc(Constant.UserMonthlyItem.TDKPT05), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPT", "%", "PPH05D", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("PPH05", Constant.GetEnumDesc(Constant.UserMonthlyItem.PPH05), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPT05", "*", "PPH05P", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TDKPT2", Constant.GetEnumDesc(Constant.UserMonthlyItem.TDKPT2), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPT", "-", "PPH05D", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TDKPT15", Constant.GetEnumDesc(Constant.UserMonthlyItem.TDKPT15), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPT2", "%", "PPH15D", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("PPH15", Constant.GetEnumDesc(Constant.UserMonthlyItem.PPH15), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPT15", "*", "PPH15P", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TDKPT3", Constant.GetEnumDesc(Constant.UserMonthlyItem.TDKPT3), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPT2", "-", "PPH15D", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TDKPT25", Constant.GetEnumDesc(Constant.UserMonthlyItem.TDKPT25), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPT3", "%", "PPH25D", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("PPH25", Constant.GetEnumDesc(Constant.UserMonthlyItem.PPH25), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPT25", "*", "PPH25P", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("TDKPT4", Constant.GetEnumDesc(Constant.UserMonthlyItem.TDKPT4), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPT3", "-", "PPH25D", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("PPH30", Constant.GetEnumDesc(Constant.UserMonthlyItem.PPH30), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "TDKPT4", "*", "PPH30P", 0, true, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("PPH21", Constant.GetEnumDesc(Constant.UserMonthlyItem.PPH21), (int)Constant.SalarySign.Expense, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "PPH05", "/", "", 12, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "PPH15", "/", "", 12, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "PPH25", "/", "", 12, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "PPH30", "/", "", 12, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("GJBSP", Constant.GetEnumDesc(Constant.UserMonthlyItem.GJBSP), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJKOT", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "PTPJM", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "JMSTK", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Expense, "PPH21", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("GJBDV", Constant.GetEnumDesc(Constant.UserMonthlyItem.GJBDV), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJBSP", "\\", "", 1000, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("GJBRM", Constant.GetEnumDesc(Constant.UserMonthlyItem.GJBRM), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJBSP", "%", "", 1000, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("GJRND", Constant.GetEnumDesc(Constant.UserMonthlyItem.GJRND), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJBRM", "-", "", 1000, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

            slip = _salarySlipService.CreateObject("GJBSH", Constant.GetEnumDesc(Constant.UserMonthlyItem.GJBSH), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJBSP", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);
            _salarySlipDetailService.CreateObject(slip.Id, (int)Constant.SalarySign.Income, "GJRND", "+", "", 0, false, 0, false, 0, _salarySlipService, _formulaService, _salaryItemService);

        }

        public void PopulateEmployeeData()
        {
            emp1 = new Employee
            {
                NIK = "08051000089",
                Name = "SYAMSUL HADI",
                NickName = "SYAMSUL HADI",
                PlaceOfBirth = "DEMAK",
                BirthDate = new DateTime(1983, 4, 11),
                Sex = (int)Constant.Sex.Male,
                Religion = (int)Constant.Religion.Islam,
                MaritalStatus = (int)Constant.MaritalStatus.Married,
                Children = 1,
                Address = "JL YOS SUDARSO RT 08 RW II KEL TELUK LINGGA KEC SA",
                Email = "nyoels@yahoo.co.id",
                NPWP = "67.977.026.3-728.000",
                PhoneNumber = "081334137250",
                BankAccount = "1480005786739",
                Bank = "BCA",
                BankAccountName = "SYAMSUL HADI",
                WorkingStatus = (int)Constant.WorkingStatus.Tetap,
                StartWorkingDate = new DateTime(2008, 5, 5),
                AppointmentDate = new DateTime(2008, 5, 5),
                ActiveStatus = (int)Constant.ActiveStatus.Active,
                TitleInfoId = tit1.Id,
                DivisionId = div1.Id,
            };
            if (!_employeeService.CreateObject(emp1, _divisionService, _titleInfoService).Errors.Any())
            {
                empEdu1 = new EmployeeEducation
                {
                    EmployeeId = emp1.Id,
                    Level = "S1",
                    Major = "Ekonomi",
                    Institute = "Universitas Indonesia",
                    EnrollmentDate = new DateTime(2003, 1, 1),
                    GraduationDate = new DateTime(2007, 1, 1),
                };
                _employeeEducationService.CreateObject(empEdu1, _employeeService);
                ewt1 = new EmployeeWorkingTime
                {
                    EmployeeId = emp1.Id,
                    WorkingTimeId = _workingTimeService.GetObjectByCode("N").Id,
                    //IsShiftable = false,
                    //IsEnabled = true,
                    StartDate = new DateTime(2012, 1, 1),
                    EndDate = new DateTime(2012, 12, 31),
                };
                _employeeWorkingTimeService.CreateObject(ewt1, _workingTimeService, _employeeService);
                //emp1.EmployeeWorkingTimeId = ewt1.Id;
                emp1.EmployeeEducationId = empEdu1.Id;
                _employeeService.UpdateObject(emp1, _divisionService, _titleInfoService);
            }

        }

        public void PopulateAttendanceData()
        {
            Random rnd = new Random();

            DateTime startDate = new DateTime(2012, 1, 1);
            DateTime endDate = new DateTime(2012, 12, 31);

            var wt = _workingTimeService.GetObjectByCode("N");

            for (DateTime curDay = startDate; curDay <= endDate; curDay = curDay.AddDays(1))
            {
                var ewt = _employeeWorkingTimeService.GetQueryable().Where(x => x.EmployeeId == emp1.Id && x.StartDate <= curDay && x.EndDate >= curDay).Include("Employee").Include("WorkingTime").OrderBy(x => x.StartDate).FirstOrDefault();
                string kodehari = ewt.WorkingTime.Code + ((int)curDay.DayOfWeek).ToString(); //ci.DateTimeFormat.GetDayName(curDay.DayOfWeek);
                var oriwd = _workingDayService.GetQueryable().Include("WorkingTime").Where(x => x.WorkingTime.Id == ewt.WorkingTimeId && x.IsEnabled && x.Code == kodehari).FirstOrDefault();
                empatt = new EmployeeAttendance
                {
                    //EntryDate = DateTime.Today,
                    EmployeeId = emp1.Id,
                    AttendanceDate = curDay,
                    CheckIn = curDay + wt.CheckIn.Subtract((DateTime)SqlDateTime.MinValue),
                    CheckOut = curDay + wt.CheckOut.Subtract((DateTime)SqlDateTime.MinValue), //.AddMinutes(rnd.Next(300)),
                    BreakOut = curDay + wt.BreakOut.Subtract((DateTime)SqlDateTime.MinValue),
                    BreakIn = curDay + wt.BreakIn.Subtract((DateTime)SqlDateTime.MinValue),
                    Status = (int)Constant.AttendanceStatus.Present,
                };
                if (oriwd == null || !oriwd.IsEnabled)
                {
                    empatt.Status = (int)Constant.AttendanceStatus.Off;
                }
                _employeeAttendanceService.CreateObject(empatt, _employeeService);
                
            }
        }

        public void PopulateSalaryData()
        {
            // Create master/standard salary (per title)
            ss1 = new SalaryStandard
            {
                TitleInfoId = tit1.Id,
                EffectiveDate = (DateTime)SqlDateTime.MinValue,
            };
            _salaryStandardService.CreateObject(ss1, _titleInfoService, _salaryStandardDetailService, _salaryItemService);

            ssd1 =  _salaryStandardDetailService.GetObjectBySalaryItemId(_salaryItemService.GetObjectByCode(Constant.LegacySalaryItem.GJPOK).Id, ss1.Id);
            ssd1.Amount = 3600000;
            _salaryStandardDetailService.UpdateObject(ssd1, _salaryStandardService, _salaryItemService);

            ssd1 = _salaryStandardDetailService.GetObjectBySalaryItemId(_salaryItemService.GetObjectByCode(Constant.LegacySalaryItem.OVTRT).Id, ss1.Id);
            ssd1.Amount = 20809;
            _salaryStandardDetailService.UpdateObject(ssd1, _salaryStandardService, _salaryItemService);

            ssd1 = _salaryStandardDetailService.GetObjectBySalaryItemId(_salaryItemService.GetObjectByCode(Constant.LegacySalaryItem.TJLAP).Id, ss1.Id);
            ssd1.Amount = 54000;
            _salaryStandardDetailService.UpdateObject(ssd1, _salaryStandardService, _salaryItemService);

            ssd1 = _salaryStandardDetailService.GetObjectBySalaryItemId(_salaryItemService.GetObjectByCode(Constant.LegacySalaryItem.TJMKN).Id, ss1.Id);
            ssd1.Amount = 21000;
            _salaryStandardDetailService.UpdateObject(ssd1, _salaryStandardService, _salaryItemService);

            ssd1 = _salaryStandardDetailService.GetObjectBySalaryItemId(_salaryItemService.GetObjectByCode(Constant.LegacySalaryItem.TJLNA).Id, ss1.Id);
            ssd1.Amount = 0;
            _salaryStandardDetailService.UpdateObject(ssd1, _salaryStandardService, _salaryItemService);
            //ssd1 = new SalaryStandardDetail
            //{
            //    SalaryStandardId = ss1.Id,
            //    SalaryItemId = _salaryItemService.GetObjectByCode(Constant.LegacySalaryItem.OVTRT).Id, // overtime rate (1x)
            //    Amount = 20809, // OTRate = (GJPOKOK / 173)
            //};
            //_salaryStandardDetailService.CreateObject(ssd1, _salaryStandardService, _salaryItemService);
            //ssd1 = new SalaryStandardDetail
            //{
            //    SalaryStandardId = ss1.Id,
            //    SalaryItemId = _salaryItemService.GetObjectByCode(Constant.LegacySalaryItem.TJLAP).Id, // tunjangan lapangan
            //    Amount = 54000,
            //};
            //_salaryStandardDetailService.CreateObject(ssd1, _salaryStandardService, _salaryItemService);
            //ssd1 = new SalaryStandardDetail
            //{
            //    SalaryStandardId = ss1.Id,
            //    SalaryItemId = _salaryItemService.GetObjectByCode(Constant.LegacySalaryItem.TJMKN).Id, // tunjangan makan
            //    Amount = 21000,
            //};
            //_salaryStandardDetailService.CreateObject(ssd1, _salaryStandardService, _salaryItemService);
            //ssd1 = new SalaryStandardDetail
            //{
            //    SalaryStandardId = ss1.Id,
            //    SalaryItemId = _salaryItemService.GetObjectByCode(Constant.LegacySalaryItem.TJLNA).Id, // tunjangan lainnya/transport?
            //    Amount = 0,
            //};
            //_salaryStandardDetailService.CreateObject(ssd1, _salaryStandardService, _salaryItemService);

            // Create customizable employee salary (per employee)
            se1 = new SalaryEmployee
            {
                EmployeeId = emp1.Id,
                EffectiveDate = (DateTime)SqlDateTime.MinValue,
                IsActive = true,
            };
            _salaryEmployeeService.CreateObject(se1, _employeeService, _salaryEmployeeDetailService, _salaryItemService, _salaryStandardDetailService);
            //IList<SalaryStandardDetail> details = _salaryStandardDetailService.GetObjectsByTitleInfoId(emp1.TitleInfoId);
            //foreach (var detail in details)
            //{
            //    sed1 = new SalaryEmployeeDetail
            //    {
            //        SalaryEmployeeId = se1.Id,
            //        SalaryItemId = detail.SalaryItemId,
            //        Amount = detail.Amount,
            //    };
            //    _salaryEmployeeDetailService.CreateObject(sed1, _salaryEmployeeService, _salaryItemService);
            //};
        }

        public void PopulateOtherData()
        {
            _ptkpService.CreateObject("TK", 24300000, "TIDAK KAWIN");
            _ptkpService.CreateObject("KW_00", 26325000, "KAWIN TANPA ANAK");
            _ptkpService.CreateObject("KW_01", 28350000, "KAWIN DENGAN 1 ANAK");
            _ptkpService.CreateObject("KW_02", 30375000, "KAWIN DENGAN 2 ANAK");
            _ptkpService.CreateObject("KW_03", 32400000, "KAWIN DENGAN 3 ANAK");
            _ptkpService.CreateObject("TK_01", 26325000, "TIDAK KAWIN DENGAN 1 ANAK");
            _ptkpService.CreateObject("TK_02", 28350000, "TIDAK KAWIN DENGAN 2 ANAK");
            _ptkpService.CreateObject("TK_03", 30375000, "TIDAK KAWIN DENGAN 3 ANAK");

            _pph21sptService.CreateObject("PPH05", 0, false, 50000000, 5, "");
            _pph21sptService.CreateObject("PPH15", 50000001, false, 250000000, 15, "");
            _pph21sptService.CreateObject("PPH25", 250000001, false, 500000000, 25, "");
            _pph21sptService.CreateObject("PPH30", 500000001, true, 0, 30, "");
        }

    }
}
