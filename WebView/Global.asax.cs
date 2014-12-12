using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Core.DomainModel;
using Core.Interface.Service;
using Data.Repository;
using Service.Service;
using Validation.Validation;
using Core.Constants;

namespace WebView
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        //private IAccountService _accountService;
        //private IContactGroupService _contactGroupService;
        //private IContactService _contactService;
        //private IUserMenuService _userMenuService;
        private IUserAccountService _userAccountService;
        //private IUserAccessService _userAccessService;
        //private ICompanyService _companyService;
        //private ContactGroup baseContactGroup;
        //private Contact baseContact;
        //private Company baseCompany;
        //private Account Asset, CashBank, AccountReceivable, GBCHReceivable, Inventory;
        //private Account Expense, CashBankAdjustmentExpense, COGS, Discount, SalesAllowance, StockAdjustmentExpense;
        //private Account Liability, AccountPayable, GBCHPayable, GoodsPendingClearance;
        //private Account Equity, OwnersEquity, EquityAdjustment;
        //private Account Revenue;
        private IFormulaService _formulaService;
        private ISalaryItemService _salaryItemService;
        private ISalarySlipService _salarySlipService;
        private ISalarySlipDetailService _salarySlipDetailService;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            //AuthConfig.RegisterAuth();

            PopulateData();
        }

        public void PopulateData()
        {
            //_accountService = new AccountService(new AccountRepository(), new AccountValidator());
            //_contactGroupService = new ContactGroupService(new ContactGroupRepository(), new ContactGroupValidator());
            //_contactService = new ContactService(new ContactRepository(), new ContactValidator());
            //_userMenuService = new UserMenuService(new UserMenuRepository(), new UserMenuValidator());
            _userAccountService = new UserAccountService(new UserAccountRepository(), new UserAccountValidator());
            //_userAccessService = new UserAccessService(new UserAccessRepository(), new UserAccessValidator());
            //_companyService = new CompanyService(new CompanyRepository(), new CompanyValidator());
            _formulaService = new FormulaService(new FormulaRepository(), new FormulaValidator());
            _salaryItemService = new SalaryItemService(new SalaryItemRepository(), new SalaryItemValidator());
            _salarySlipService = new SalarySlipService(new SalarySlipRepository(), new SalarySlipValidator());
            _salarySlipDetailService = new SalarySlipDetailService(new SalarySlipDetailRepository(), new SalarySlipDetailValidator());

            //baseContactGroup = _contactGroupService.FindOrCreateBaseContactGroup(); // .CreateObject(Core.Constants.Constant.GroupType.Base, "Base Group", true);
            //baseContact = _contactService.FindOrCreateBaseContact(_contactGroupService); // _contactService.CreateObject(Core.Constants.Constant.BaseContact, "BaseAddr", "123456", "PIC", "123", "Base@email.com", _contactGroupService);
            //baseCompany = _companyService.GetQueryable().FirstOrDefault();
            //if (baseCompany == null)
            //{
            //    baseCompany = _companyService.CreateObject("Jakarta Andalan Bike", "Jl. Hos Cokroaminoto No.12A Mencong Ciledug, Tangerang", "021-7316575", "", "jakartaandalanbike@gmail.com");
            //}
            
            //if (!_accountService.GetLegacyObjects().Any())
            //{
            //    Asset = _accountService.CreateLegacyObject(new Account() { Name = "Asset", Code = Constant.AccountCode.Asset, LegacyCode = Constant.AccountLegacyCode.Asset, Level = 1, Group = Constant.AccountGroup.Asset, IsLegacy = true }, _accountService);
            //    CashBank = _accountService.CreateLegacyObject(new Account() { Name = "CashBank", Code = Constant.AccountCode.CashBank, LegacyCode = Constant.AccountLegacyCode.CashBank, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
            //    AccountReceivable = _accountService.CreateLegacyObject(new Account() { Name = "Account Receivable", IsLeaf = true, Code = Constant.AccountCode.AccountReceivable, LegacyCode = Constant.AccountLegacyCode.AccountReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
            //    GBCHReceivable = _accountService.CreateLegacyObject(new Account() { Name = "GBCH Receivable", IsLeaf = true, Code = Constant.AccountCode.GBCHReceivable, LegacyCode = Constant.AccountLegacyCode.GBCHReceivable, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);
            //    Inventory = _accountService.CreateLegacyObject(new Account() { Name = "Inventory", IsLeaf = true, Code = Constant.AccountCode.Inventory, LegacyCode = Constant.AccountLegacyCode.Inventory, Level = 2, Group = Constant.AccountGroup.Asset, ParentId = Asset.Id, IsLegacy = true }, _accountService);

            //    Expense = _accountService.CreateLegacyObject(new Account() { Name = "Expense", Code = Constant.AccountCode.Expense, LegacyCode = Constant.AccountLegacyCode.Expense, Level = 1, Group = Constant.AccountGroup.Expense, IsLegacy = true }, _accountService);
            //    CashBankAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Name = "CashBank Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.CashBankAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.CashBankAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
            //    COGS = _accountService.CreateLegacyObject(new Account() { Name = "Cost Of Goods Sold", IsLeaf = true, Code = Constant.AccountCode.COGS, LegacyCode = Constant.AccountLegacyCode.COGS, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
            //    Discount = _accountService.CreateLegacyObject(new Account() { Name = "Discount", IsLeaf = true, Code = Constant.AccountCode.Discount, LegacyCode = Constant.AccountLegacyCode.Discount, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
            //    SalesAllowance = _accountService.CreateLegacyObject(new Account() { Name = "Sales Allowance", IsLeaf = true, Code = Constant.AccountCode.SalesAllowance, LegacyCode = Constant.AccountLegacyCode.SalesAllowance, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);
            //    StockAdjustmentExpense = _accountService.CreateLegacyObject(new Account() { Name = "Stock Adjustment Expense", IsLeaf = true, Code = Constant.AccountCode.StockAdjustmentExpense, LegacyCode = Constant.AccountLegacyCode.StockAdjustmentExpense, Level = 2, Group = Constant.AccountGroup.Expense, ParentId = Expense.Id, IsLegacy = true }, _accountService);

            //    Liability = _accountService.CreateLegacyObject(new Account() { Name = "Liability", Code = Constant.AccountCode.Liability, LegacyCode = Constant.AccountLegacyCode.Liability, Level = 1, Group = Constant.AccountGroup.Liability, IsLegacy = true }, _accountService);
            //    AccountPayable = _accountService.CreateLegacyObject(new Account() { Name = "Account Payable", IsLeaf = true, Code = Constant.AccountCode.AccountPayable, LegacyCode = Constant.AccountLegacyCode.AccountPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
            //    GBCHPayable = _accountService.CreateLegacyObject(new Account() { Name = "GBCH Payable", IsLeaf = true, Code = Constant.AccountCode.GBCHPayable, LegacyCode = Constant.AccountLegacyCode.GBCHPayable, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);
            //    GoodsPendingClearance = _accountService.CreateLegacyObject(new Account() { Name = "Goods Pending Clearance", IsLeaf = true, Code = Constant.AccountCode.GoodsPendingClearance, LegacyCode = Constant.AccountLegacyCode.GoodsPendingClearance, Level = 2, Group = Constant.AccountGroup.Liability, ParentId = Liability.Id, IsLegacy = true }, _accountService);

            //    Equity = _accountService.CreateLegacyObject(new Account() { Name = "Equity", Code = Constant.AccountCode.Equity, LegacyCode = Constant.AccountLegacyCode.Equity, Level = 1, Group = Constant.AccountGroup.Equity, IsLegacy = true }, _accountService);
            //    OwnersEquity = _accountService.CreateLegacyObject(new Account() { Name = "Owners Equity", Code = Constant.AccountCode.OwnersEquity, LegacyCode = Constant.AccountLegacyCode.OwnersEquity, Level = 2, Group = Constant.AccountGroup.Equity, ParentId = Equity.Id, IsLegacy = true }, _accountService);
            //    EquityAdjustment = _accountService.CreateLegacyObject(new Account() { Name = "Equity Adjustment", IsLeaf = true, Code = Constant.AccountCode.EquityAdjustment, LegacyCode = Constant.AccountLegacyCode.EquityAdjustment, Level = 3, Group = Constant.AccountGroup.Equity, ParentId = OwnersEquity.Id, IsLegacy = true }, _accountService);

            //    Revenue = _accountService.CreateLegacyObject(new Account() { Name = "Revenue", IsLeaf = true, Code = Constant.AccountCode.Revenue, LegacyCode = Constant.AccountLegacyCode.Revenue, Level = 1, Group = Constant.AccountGroup.Revenue, IsLegacy = true }, _accountService);
            //}
            
            CreateUserMenus();
            CreateSysAdmin();
            PopulateLegacyData();
            PopulateSlipData();
        }

        public void CreateUserMenus()
        {
            //_userMenuService.CreateObject(Constant.MenuName.Contact, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.ItemType, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.UoM, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.QuantityPricing, Constant.MenuGroupName.Master);

            //_userMenuService.CreateObject(Constant.MenuName.CashBank, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.CashBankAdjustment, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.CashBankMutation, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.CashMutation, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.PaymentRequest, Constant.MenuGroupName.Master);

            //_userMenuService.CreateObject(Constant.MenuName.Item, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.StockAdjustment, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.StockMutation, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.Warehouse, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.WarehouseItem, Constant.MenuGroupName.Master);
            //_userMenuService.CreateObject(Constant.MenuName.WarehouseMutation, Constant.MenuGroupName.Master);

            ////_userMenuService.CreateObject(Constant.MenuName.PurchaseOrder, Constant.MenuGroupName.Transaction);
            ////_userMenuService.CreateObject(Constant.MenuName.PurchaseReceival, Constant.MenuGroupName.Transaction);
            ////_userMenuService.CreateObject(Constant.MenuName.PurchaseInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.CustomPurchaseInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.PaymentVoucher, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.Payable, Constant.MenuGroupName.Transaction);

            ////_userMenuService.CreateObject(Constant.MenuName.SalesOrder, Constant.MenuGroupName.Transaction);
            ////_userMenuService.CreateObject(Constant.MenuName.DeliveryOrder, Constant.MenuGroupName.Transaction);
            ////_userMenuService.CreateObject(Constant.MenuName.SalesInvoice, Constant.MenuGroupName.Transaction);
            ////_userMenuService.CreateObject(Constant.MenuName.RetailSalesInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.CashSalesInvoice, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.CashSalesReturn, Constant.MenuGroupName.Transaction);
            
            //_userMenuService.CreateObject(Constant.MenuName.ReceiptVoucher, Constant.MenuGroupName.Transaction);
            //_userMenuService.CreateObject(Constant.MenuName.Receivable, Constant.MenuGroupName.Transaction);

            //_userMenuService.CreateObject(Constant.MenuName.Item, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.Sales, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.TopSales, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.ProfitLoss, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.Account, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.Closing, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.GeneralLedger, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.ValidComb, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.BalanceSheet, Constant.MenuGroupName.Report);
            //_userMenuService.CreateObject(Constant.MenuName.IncomeStatement, Constant.MenuGroupName.Report);
            
            //_userMenuService.CreateObject(Constant.MenuName.User, Constant.MenuGroupName.Setting);
            //_userMenuService.CreateObject(Constant.MenuName.UserAccessRight, Constant.MenuGroupName.Setting);
            //_userMenuService.CreateObject(Constant.MenuName.CompanyInfo, Constant.MenuGroupName.Setting);
        }

        public void CreateSysAdmin()
        {
            UserAccount userAccount = _userAccountService.GetObjectByUsername(Constant.UserType.Admin);
            if (userAccount == null)
            {
                userAccount = _userAccountService.CreateObject(Constant.UserType.Admin, "sysadmin", "Administrator", "Administrator", true);
            }
            //_userAccessService.CreateDefaultAccess(userAccount.Id, _userMenuService, _userAccountService);

        }

        public void PopulateLegacyData()
        {
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
            SalarySlip slip = _salarySlipService.CreateObject("TOT10", Constant.GetEnumDesc(Constant.UserMonthlyItem.TOT10), (int)Constant.SalarySign.Income, (int)Constant.SalaryItemStatus.Monthly, true, true, true, false, false, _salaryItemService);
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

    }
}