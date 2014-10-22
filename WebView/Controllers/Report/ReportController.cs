using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Interface.Service;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Data.Repository;
using Service.Service;
using Validation.Validation;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.DomainModel;
using System.Collections;
using System.Globalization;

namespace WebView.Controllers
{
    public class ReportController : Controller
    {
        //
        // GET: /Report/
        private readonly static log4net.ILog LOG = log4net.LogManager.GetLogger("ReportController");
        public ICompanyInfoService _companyInfoService;
        public ISlipGajiDetailService _slipGajiDetailService;
        public ISlipGajiDetail1Service _slipGajiDetail1Service;
        public ISlipGajiDetail2AService _slipGajiDetail2AService;

        public ReportController()
        {
            _companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
            _slipGajiDetailService = new SlipGajiDetailService(new SlipGajiDetailRepository(), new SlipGajiDetailValidator());
            _slipGajiDetail1Service = new SlipGajiDetail1Service(new SlipGajiDetail1Repository(), new SlipGajiDetail1Validator());
            _slipGajiDetail2AService = new SlipGajiDetail2AService(new SlipGajiDetail2ARepository(), new SlipGajiDetail2AValidator());
            
        }

        public ActionResult SlipGajiDetail()
        {
            return View();
        }

        public ActionResult ReportSlipGajiDetail(Nullable<int> EmployeeId, DateTime monthYear)
        {
            var company = _companyInfoService.GetQueryable().FirstOrDefault();
            var q = _slipGajiDetailService.GetQueryable().Include("SlipGajiDetail1s").Include("SlipGajiDetail2As").Where(x => (EmployeeId == null || EmployeeId.Value == 0 || x.EmployeeId == EmployeeId.Value) && x.MONTH.Month == monthYear.Month && x.MONTH.Year == monthYear.Year);
						//.ToList();
            var q2 = _slipGajiDetail1Service.GetQueryable().Include("SlipGajiDetail").Where(x => (EmployeeId == null || EmployeeId.Value == 0 || x.SlipGajiDetail.EmployeeId == EmployeeId.Value) && x.SlipGajiDetail.MONTH.Month == monthYear.Month && x.SlipGajiDetail.MONTH.Year == monthYear.Year).ToList();
            var q3 = _slipGajiDetail2AService.GetQueryable().Include("SlipGajiDetail").Where(x => (EmployeeId == null || EmployeeId.Value == 0 || x.SlipGajiDetail.EmployeeId == EmployeeId.Value) && x.SlipGajiDetail.MONTH.Month == monthYear.Month && x.SlipGajiDetail.MONTH.Year == monthYear.Year).ToList();
            
            string user = AuthenticationModel.GetUserName();

            CultureInfo ci = new CultureInfo("id-ID");

			var query = (from model in q
                         select new
                         {
                             model.MONTH,
                             model.NoBadge,
							 model.Name,
							 model.Jabatan,
							 model.TanggalPenerimaan,
							 model.PeriodeAwal,
							 model.PeriodeAkhir,
							 model.GajiBasis,
							 model.StatusMarriage,
							 model.NoSlip,
							 model.Rate,
							 model.Lembur15,
							 model.Lembur20,
							 model.Lembur30,
							 model.Lembur40,
							 Disiapkan_oleh = "",
							 Disetujui_oleh = "",
							 Dikoreksi_oleh = "",
                             company_code = company.Name,
                             //CompanyAddress = company.Address,
                             //User = user,
							 model.SlipGajiDetail1s,
							 model.SlipGajiDetail2As,
                         }).ToList();

            if (!query.Any()) return Content(Core.Constants.Constant.ErrorPage.RecordDetailNotFound);

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/rpt_slip_gajiA.rpt");

            // Setting report data source
            rd.SetDataSource(query);
            
            rd.Subreports["rpt_SlipGaji2.rpt"].SetDataSource(q2);
            rd.Subreports["rpt_SlipGaji3.rpt"].SetDataSource(q3);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue(0, "Periode " + monthYear.ToString("MMMM yyyy", ci).ToUpper());

            // Set printer paper size
            //System.Drawing.Printing.PrintDocument doctoprint = new System.Drawing.Printing.PrintDocument();
            //doctoprint.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";
            //int i = 0;
            //for (i = 0; i < doctoprint.PrinterSettings.PaperSizes.Count; i++)
            //{
            //    int rawKind = 0;
            //    if (doctoprint.PrinterSettings.PaperSizes[i].PaperName.ToLower() == "struk")
            //    {
            //        rawKind = Convert.ToInt32(doctoprint.PrinterSettings.PaperSizes[i].GetType().GetField("kind", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).GetValue(doctoprint.PrinterSettings.PaperSizes[i]));
            //        if (Enum.IsDefined(typeof(CrystalDecisions.Shared.PaperSize), rawKind))
            //        {
            //            rd.PrintOptions.PaperSize = (CrystalDecisions.Shared.PaperSize)rawKind;
            //            //rd.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
            //        }
            //        break;
            //    }
            //}

            var stream = rd.ExportToStream(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat);
            return File(stream, "application/pdf");
        }

        

    }
}
