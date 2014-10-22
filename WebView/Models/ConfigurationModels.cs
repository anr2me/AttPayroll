using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebView
{
    public class ConfigurationModels
    {
        public const int LogisAsSuperAdmin = 1;
        public const int LoginAsAdminCompany = 2;
        public const int LoginAsUser = 3;

        public const int JobIdSeaExport = 1;
        public const int JobIdSeaImport = 2;
        public const int JobIdAirExport = 3;
        public const int JobIdAirImport = 4;
        public const int JobIdEMKLSeaExport = 5;
        public const int JobIdEMKLSeaImport = 6;
        public const int JobIdEMKLAirExport = 7;
        public const int JobIdEMKLAirImport = 8;
        public const int JobIdEMKLDomestic = 9;

        public const string CompanyStatusPT = "PT";
        public const string CompanyStatusCV = "CV";
        public const string CompanyStatusOT = "OT";

        public const string PrintTypeFix = "f";
        public const string PrintTypeFDraft = "d";

        public const string VesselFeeder = "FEEDER";
        public const string VesselConnecting = "CONNECTING";
        public const string VesselMother = "MOTHER";

        public const string InvoiceToAgent = "AG";
        public const string InvoiceToShipper = "SM";
        public const string InvoiceToConsignee = "CM";

        public const int CustomerTypeIdAgent = 1;
        public const int CustomerTypeIdShipper = 2;
        public const int CustomerTypeIdConsignee = 3;
        public const int CustomerTypeIdSSLine = 4;
        public const int CustomerTypeIdIATA = 5;
        public const int CustomerTypeIdEMKL = 6;
        public const int CustomerTypeIdDepo = 7;
        public const int CustomerTypeIdRebateShipper = 8;
        public const int CustomerTypeIdRebateConsignee = 9;

        public const int MasterFileMappingLogo = 1;
        public const int MasterFileMappingBillOfLading = 2;

        public const string LinkToCodeConsignee = "10";
        public const string LinkToCodeSSLine = "1";
        public const string LinkToCodeIATA = "1";
        public const string LinkToCodeEMKL = "2";
        public const string LinkToCodeRebateShipper = "3";
        public const string LinkToCodeDepo = "7";

        // Menu Operational
        public const int MENU_MASTER_CONTINENTAL = 1;
        public const int MENU_MASTER_COUNTRY = 2;
        public const int MENU_MASTER_CITY = 3;
        public const int MENU_MASTER_PORT = 4;
        public const int MENU_MASTER_AIRPORT = 5;
        public const int MENU_MASTER_AIRLINE = 6;
        public const int MENU_MASTER_VESSEL = 7;
        public const int MENU_MASTER_GROUP = 8;
        public const int MENU_MASTER_CONTACT = 9;
        public const int MENU_MASTER_BILL_OF_LADING = 10;
        public const int MENU_MASTER_COMPANY = 11;
        public const int MENU_MASTER_COST_SALES_AIR_FREIGHT = 12;
        public const int MENU_MASTER_COST_SALES_SEA_FREIGHT = 13;
        public const int MENU_MASTER_USER = 14;
        public const int MENU_MASTER_PRINCIPLE = 15;
        public const int MENU_FILE_SHIPMENT_ORDER = 16;
        public const int MENU_FILE_PRE_BOOKING_ADVICE = 17;
        public const int MENU_FILE_CONTAINER_STATUS = 18;
        public const int MENU_FILE_ESTIMATE_PROFIT_AND_LOSS = 19;
        public const int MENU_FILE_PAYMENT_REQUEST = 20;
        public const int MENU_FILE_INVOICE = 21;
        public const int MENU_FILE_FAKTUR_PAJAK = 22;
        public const int MENU_PROSES_APPROVE_PAYMENT_REQUEST = 23;
        public const int MENU_PROSES_OPEN_SHIPMENT_ORDER = 24;
        public const int MENU_PROSES_CLOSE_SHIPMENT_ORDER = 25;
        public const int MENU_REPORT_PEB = 26;
        public const int MENU_REPORT_CARGO_MANIFEST = 27;
        public const int MENU_REPORT_DO_WITHOUT_ORIGINAL_BL = 28;
        public const int MENU_REPORT_PEMINJAMAN_CONTAINER = 29;
        public const int MENU_REPORT_NOTICE_OF_ARRIVAL = 30;
        public const int MENU_REPORT_SURAT_PENGANTAR = 31;
        public const int MENU_REPORT_PECAH_POS = 32;
        public const int MENU_REPORT_PERFORMANCE = 33;
        public const int MENU_REPORT_FINAL_LOADING = 34;
        public const int MENU_REPORT_RECAPITULATION_FAKTUR = 35;
        public const int MENU_REPORT_MASTER = 36;
        public const int MENU_REPORT_ACKNOWLEDGEMENT_OF_DOCUMENT = 37;
        public const int MENU_REPORT_TANDA_TERIMA_DOKUMEN = 38;
        public const int MENU_REPORT_SHIPMENT_PER_BL = 39;
        public const int MENU_REPORT_ACTIVITY = 40;
        public const int MENU_REPORT_INVOICE_UN_PRINTED = 41;
        public const int MENU_REPORT_PAYABLE_COST_LIST = 42;
        public const int MENU_REPORT_OUTSTANDING_PAYABLE = 43;
        public const int MENU_PROSES_APPROVE_REPRINT_INVOICE = 116;
        // END Menu Operational

        // Menu Accounting
        public const int MENU_MASTER_BANK = 44;
        public const int MENU_MASTER_EXCHANGE_RATE = 45;
        public const int MENU_MASTER_CHART_OF_ACCOUNT = 46;
        public const int MENU_MASTER_EMPLOYEE = 47;
        public const int MENU_FILE_GENERAL_INVOICE = 48;
        public const int MENU_FILE_TEMPORARY_PAYMENT = 49;
        public const int MENU_FILE_TEMPORARY_RECEIPT = 50;
        public const int MENU_FILE_RECEIPT_VOUCHER = 51;
        public const int MENU_FILE_PAYMENT_VOUCHER = 52;
        public const int MENU_FILE_OFFICIAL_RECEIPT = 53;
        public const int MENU_FILE_CASH_ADVANCE = 54;
        public const int MENU_FILE_CASH_SETTLEMENT = 55;
        public const int MENU_FILE_MEMORIAL = 56;
        public const int MENU_FILE_OUTSTANDING_INCOME_EPL = 57;
        public const int MENU_FILE_INCOME_NEXT_PERIOD = 58;
        public const int MENU_FILE_COST_EPL_NEXT_PERIOD = 59;
        public const int MENU_FILE_OUTSTANDING_COST_AGENT = 60;
        public const int MENU_FILE_ADJUSTMENT_AP = 61;
        public const int MENU_PROSES_POSTING = 62;
        public const int MENU_PROSES_CLOSING_MONTHLY = 63;
        public const int MENU_PROSES_RESTORE_CLOSING_MONTHLY = 64;
        public const int MENU_PROSES_DAILY_CLOSING = 65;
        public const int MENU_PROSES_RESTORE_DAILY_CLOSING = 66;
        public const int MENU_PROSES_DUE_DATE = 67;
        public const int MENU_PROSES_VERIFY_MEMORIAL = 68;
        public const int MENU_PROSES_VERIFY_TP = 69;
        public const int MENU_PROSES_VERIFY_TR = 70;
        public const int MENU_PROSES_VERIFY_RV = 71;
        public const int MENU_PROSES_VERIFY_PV = 72;
        public const int MENU_PROSES_VERIFY_OR = 73;
        public const int MENU_PROSES_INVOICE_JOURNAL = 74;
        public const int MENU_PROSES_PAYMENT_REQUEST_JOURNAL = 75;
        // END Menu Accounting

        #region MODE TRIAL
        // MODE TESTING
        public static bool MODE_TRIAL()
        {
            bool modeTrial = false;
            string strModeTrial = System.Configuration.ConfigurationManager.AppSettings["InfossTrial"];
            if (!String.IsNullOrEmpty(strModeTrial))
            {
                if (!bool.TryParse(strModeTrial, out modeTrial))
                {
                    modeTrial = false;
                }
            }
            return modeTrial;
        }
        #endregion

        #region MODE TESTING
        // MODE TESTING
        public static bool MODE_TESTING()
        {
            bool modeTesting = false;
            string strModeTesting = System.Configuration.ConfigurationManager.AppSettings["MODE.TESTING"];
            if (!String.IsNullOrEmpty(strModeTesting))
            {
                if (!bool.TryParse(strModeTesting, out modeTesting))
                {
                    modeTesting = false;
                }
            }
            return modeTesting;
        }

        // USER ID
        public static int GET_MODE_TESTING_USERID()
        {
            int userId = 0;
            string strModeTesting = System.Configuration.ConfigurationManager.AppSettings["MODE.TESTING.USERID"];
            if (!String.IsNullOrEmpty(strModeTesting))
            {
                if (!int.TryParse(strModeTesting, out userId))
                {
                    userId = 0;
                }
            }
            return userId;
        }

        // USER NAME
        public static string GET_MODE_TESTING_USERNAME()
        {
            string userName = "";
            string strModeTesting = System.Configuration.ConfigurationManager.AppSettings["MODE.TESTING.USERNAME"];
            if (!String.IsNullOrEmpty(strModeTesting))
            {
                userName = strModeTesting;
            }
            return userName;
        }

        // USER TYPE ID
        public static int GET_MODE_TESTING_USERTYPEID()
        {
            int userTypeId = 0;
            string strModeTesting = System.Configuration.ConfigurationManager.AppSettings["MODE.TESTING.USERTYPEID"];
            if (!String.IsNullOrEmpty(strModeTesting))
            {
                if (!int.TryParse(strModeTesting, out userTypeId))
                {
                    userTypeId = 0;
                }
            }
            return userTypeId;
        }

        // COMPANY ID
        public static int GET_MODE_TESTING_COMPANYID()
        {
            int companyId = 0;
            string strModeTesting = System.Configuration.ConfigurationManager.AppSettings["MODE.TESTING.COMPANYID"];
            if (!String.IsNullOrEmpty(strModeTesting))
            {
                if (!int.TryParse(strModeTesting, out companyId))
                {
                    companyId = 0;
                }
            }
            return companyId;
        }

        // END MODE TESTING
        #endregion

    }
}