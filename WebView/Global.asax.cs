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

    }
}