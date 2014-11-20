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
using Core.Constants;
using System.Data.Objects.SqlClient;
using System.Data.Objects;

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
        public ISlipGajiMiniService _slipGajiMiniService;
        public IEmployeeAttendanceService _employeeAttendanceService;

        public ReportController()
        {
            _companyInfoService = new CompanyInfoService(new CompanyInfoRepository(), new CompanyInfoValidator());
            _slipGajiDetailService = new SlipGajiDetailService(new SlipGajiDetailRepository(), new SlipGajiDetailValidator());
            _slipGajiDetail1Service = new SlipGajiDetail1Service(new SlipGajiDetail1Repository(), new SlipGajiDetail1Validator());
            _slipGajiDetail2AService = new SlipGajiDetail2AService(new SlipGajiDetail2ARepository(), new SlipGajiDetail2AValidator());
            _slipGajiMiniService = new SlipGajiMiniService(new SlipGajiMiniRepository(), new SlipGajiMiniValidator());
            _employeeAttendanceService = new EmployeeAttendanceService(new EmployeeAttendanceRepository(), new EmployeeAttendanceValidator());

        }

        public ActionResult SlipGajiDetail()
        {
            return View();
        }

        public ActionResult ReportSlipGajiDetail(Nullable<int> Id, DateTime monthYear)
        {
            var company = _companyInfoService.GetQueryable().FirstOrDefault();
            var q = _slipGajiDetailService.GetQueryable().Include("SlipGajiDetail1s").Include("SlipGajiDetail2As").Where(x => (Id == null || Id.Value == 0 || x.EmployeeId == Id.Value) && x.MONTH.Month == monthYear.Month && x.MONTH.Year == monthYear.Year);
						//.ToList();
            var q2 = _slipGajiDetail1Service.GetQueryable().Include("SlipGajiDetail").Where(x => (Id == null || Id.Value == 0 || x.SlipGajiDetail.EmployeeId == Id.Value) && x.SlipGajiDetail.MONTH.Month == monthYear.Month && x.SlipGajiDetail.MONTH.Year == monthYear.Year).ToList();
            var q3 = _slipGajiDetail2AService.GetQueryable().Include("SlipGajiDetail").Where(x => (Id == null || Id.Value == 0 || x.SlipGajiDetail.EmployeeId == Id.Value) && x.SlipGajiDetail.MONTH.Month == monthYear.Month && x.SlipGajiDetail.MONTH.Year == monthYear.Year).ToList();
            
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
            //    if (doctoprint.PrinterSettings.PaperSizes[i].PaperName.ToLower() == "Letter")
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

        public ActionResult SlipGajiMini()
        {
            return View();
        }

        public ActionResult ReportSlipGajiMini(Nullable<int> Id, DateTime monthYear)
        {
            var company = _companyInfoService.GetQueryable().FirstOrDefault();
            var q = _slipGajiMiniService.GetQueryable().Include("Employee").Where(x => (Id == null || Id.Value == 0 || x.EmployeeId == Id.Value) && x.MONTHyear.Month == monthYear.Month && x.MONTHyear.Year == monthYear.Year);
            //.ToList();
            
            string user = AuthenticationModel.GetUserName();

            CultureInfo ci = new CultureInfo("id-ID");

            var query = (from model in q
                         select new
                         {
                             model.month,
                             model.employee_code,
                             model.employee_name,
                             model.title_code,
                             model.title_name,
                             model.start_working,
                             model.no_rekening,
                             model.salary_basic,
                             model.rate_hour,
                             model.allowance_rate,
                             model.uang_makan,
                             model.jml_jam_lembur,
                             model.jml_lembur,
                             model.jml_hari_absen,
                             model.tunj_lap,
                             model.insentive_hadir,
                             model.krg_bln_lalu,
                             model.thr,
                             model.pot_absensi,
                             model.pot_others,
                             model.gaji_kotor,
                             model.pot_pinjaman,
                             model.pot_jamsostek,
                             model.pjk_jkk_jkm_204,
                             model.tot_dpt_kotor,
                             model.pjk_tunj_jabatan,
                             model.pjk_ptkp,
                             model.tot_pengurang_pajak,
                             model.tot_dpt_kena_pajak,
                             model.tot_dpt_kena_pajak_tahun,
                             model.pph_5_persen,
                             model.pph_15_persen,
                             model.pph_25_persen,
                             model.pph_30_persen,
                             model.pph21,
                             model.round,
                             model.gaji_bersih,
                             Disiapkan_oleh = "",
                             Disetujui_oleh = "",
                             Dikoreksi_oleh = "",
                             company_code = company.Name,
                             //CompanyAddress = company.Address,
                             //User = user,
                         }).ToList();

            if (!query.Any()) return Content(Core.Constants.Constant.ErrorPage.RecordDetailNotFound);

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/rpt_slip_mini.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue(0, "Periode " + monthYear.ToString("MMMM yyyy", ci).ToUpper());

            // Set printer paper size
            //System.Drawing.Printing.PrintDocument doctoprint = new System.Drawing.Printing.PrintDocument();
            //doctoprint.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";
            //int i = 0;
            //for (i = 0; i < doctoprint.PrinterSettings.PaperSizes.Count; i++)
            //{
            //    int rawKind = 0;
            //    if (doctoprint.PrinterSettings.PaperSizes[i].PaperName.ToLower() == "Letter")
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

        public ActionResult PPH21()
        {
            return View();
        }

        public ActionResult ReportPPH21(Nullable<int> Id, DateTime year)
        {
            var company = _companyInfoService.GetQueryable().FirstOrDefault();
            //var q = _slipGajiDetailService.GetQueryable().Include("SlipGajiDetail1s").Include("SlipGajiDetail2As").Where(x => (Id == null || Id.Value == 0 || x.EmployeeId == Id.Value) && x.MONTH.Year == year.Year);
            //.ToList();
            //var q2 = _slipGajiDetail1Service.GetQueryable().Include("SlipGajiDetail").Where(x => (Id == null || Id.Value == 0 || x.SlipGajiDetail.EmployeeId == Id.Value) && x.SlipGajiDetail.MONTH.Year == year.Year).ToList();
            var q = _slipGajiDetail2AService.GetQueryable().Include("SlipGajiDetail").Where(x => (Id == null || Id.Value == 0 || x.SlipGajiDetail.EmployeeId == Id.Value) && x.SlipGajiDetail.MONTH.Year == year.Year).OrderBy(x => x.SlipGajiDetail.MONTH).ToList();

            string user = AuthenticationModel.GetUserName();

            CultureInfo ci = new CultureInfo("id-ID");

            //var query = (from model in q
            //             group model by new {
            //                 model.employee_code, 
            //                 year = year,
            //                 company_npwp = company.NPWP,
            //                 company_city = company.City,
            //                 company_name = company.Name,
            //                 employee_name = model.SlipGajiDetail.Employee.Name,
            //                 title_name = model.SlipGajiDetail.Employee.TitleInfo.Name,
            //                 employee_npwp = model.SlipGajiDetail.Employee.NPWP,
            //                 address = model.SlipGajiDetail.Employee.Address,
            //                 sex = ((Constant.Sex)model.SlipGajiDetail.Employee.Sex).ToString(),
            //                 marital_status	= ((Constant.Sex)model.SlipGajiDetail.Employee.MaritalStatus).ToString(),
            //                 number_child = model.SlipGajiDetail.Employee.Children,
            //                 start_date	= "",
            //                 end_date = "",
            //             } into g
            //             select new
            //             {
            //                 salary_basic = g.Sum(x => x.salary_basic),
            //                 model.rate_hour,
            //                 model.allowance_rate,
            //                 model.uang_makan,
            //                 model.jml_jam_lembur,
            //                 model.jml_lembur,
            //                 model.jml_hari_absen,
            //                 model.tunj_lap,
            //                 model.insentive_hadir,
            //                 model.krg_bln_lalu,
            //                 model.thr,
            //                 model.pot_absensi,
            //                 model.pot_others,
            //                 model.gaji_kotor,
            //                 model.pot_pinjaman,
            //                 model.pot_jamsostek,
            //                 model.pjk_jkk_jkm_204,
            //                 model.tot_dpt_kotor,
            //                 model.pjk_tunj_jabatan,
            //                 model.pjk_ptkp,
            //                 model.tot_pengurang_pajak,
            //                 model.tot_dpt_kena_pajak,
            //                 model.tot_dpt_kena_pajak_tahun,
            //                 model.pph_5_persen,
            //                 model.pph_15_persen,
            //                 model.pph_25_persen,
            //                 model.pph_30_persen,
            //                 model.pph21,
            //                 model.round,
            //                 model.gaji_bersih,
            //             }).ToList();

            var query = q//.Select(x => x)
            .GroupBy(m => new
            {
                m.employee_code,
                employee_name = m.SlipGajiDetail.Employee.Name,
                employee_npwp = m.SlipGajiDetail.Employee.NPWP,
                year = year.ToString("yyyy"),
                company_npwp = company.NPWP,
                company_city = company.City,
                company_name = company.Name,
                title_name = m.SlipGajiDetail.Employee.TitleInfo.Name,
                address = m.SlipGajiDetail.Employee.Address,
                sex = (m.SlipGajiDetail.Employee.Sex == (int)Constant.Sex.Male)? "LAKI-LAKI":"PEREMPUAN", // ((Constant.Sex)m.SlipGajiDetail.Employee.Sex).ToString(),
                marital_status = (m.SlipGajiDetail.Employee.MaritalStatus == (int)Constant.MaritalStatus.Married) ? "KAWIN":"TIDAK KAWIN", //((Constant.MaritalStatus)m.SlipGajiDetail.Employee.MaritalStatus).ToString(),
                number_child = m.SlipGajiDetail.Employee.PTKPCode,
            })
            .Select(g => new
            {
                employee_code = g.Key.employee_code,
                employee_name = g.Key.employee_name,
                employee_npwp = g.Key.employee_npwp,
                year = g.Key.year,
                company_name = g.Key.company_name,
                company_npwp = g.Key.company_npwp,
                company_city = g.Key.company_city,
                title_name = g.Key.title_name,
                address = g.Key.address,
                sex = g.Key.sex,
                marital_status = g.Key.marital_status,
                number_child = g.Key.number_child,
                start_date = g.FirstOrDefault().SlipGajiDetail.MONTH.ToString("MM"),
                end_date = g.LastOrDefault().SlipGajiDetail.MONTH.ToString("MM"),

                jml_bln_kerja = g.Count(),
                basic_salary1 = g.Sum(m => m.salary_basic),
                tunj_pph2 = g.Sum(m => Math.Abs(m.pph21)),
                lembur3 = g.Sum(m => m.jml_lembur),
                honor4 = 0, // honor ?
                premi5 = g.Sum(m => Math.Abs(m.pot_jamsostek)),
                uangmakan6 = g.Sum(m => m.uang_makan),
                thr7 = g.Sum(m => m.thr),
                jht12 = 0, // pension ?
                jml_ptkp17 = g.Sum(m => Math.Abs(m.pjk_ptkp)),
                pph21 = g.Sum(m => Math.Abs(m.pph21)),
                neto15 = g.Sum(m => m.gaji_bersih),
                tunj_lap = g.Sum(m => m.tunj_lap),
                insentive_hadir = g.Sum(m => m.insentive_hadir),
                jkk_jkm_204 = g.Sum(m => Math.Abs(m.pjk_jkk_jkm_204)),
                tunj_lain = g.Sum(m => m.other_allow),
                kurang_bulan_lalu = g.Sum(m => m.krg_bln_lalu),
                potongan_absen = g.Sum(m => Math.Abs(m.pot_absensi)),
                potongan_lain = g.Sum(m => Math.Abs(m.pot_others)),
                pot_tunj_jabatan = g.Sum(m => Math.Abs(m.pjk_tunj_jabatan)),
                nol = 0,
            }).ToList();

            if (!query.Any()) return Content(Core.Constants.Constant.ErrorPage.RecordDetailNotFound);

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/rpt_spt_pph21_ebiz.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            rd.SetParameterValue(0, "Periode " + year.ToString("yyyy", ci));

            // Set printer paper size
            //System.Drawing.Printing.PrintDocument doctoprint = new System.Drawing.Printing.PrintDocument();
            //doctoprint.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";
            //int i = 0;
            //for (i = 0; i < doctoprint.PrinterSettings.PaperSizes.Count; i++)
            //{
            //    int rawKind = 0;
            //    if (doctoprint.PrinterSettings.PaperSizes[i].PaperName.ToLower() == "Letter")
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

        public ActionResult AbsensiKaryawan()
        {
            return View();
        }

        public ActionResult ReportAbsensiKaryawan(DateTime startDate, DateTime endDate, int Id, int ChartType = 0)
        {
            DateTime endDay = endDate.Date.AddDays(1);
            var company = _companyInfoService.GetQueryable().FirstOrDefault();
            var q = _employeeAttendanceService.GetQueryable().Include("Employee").Where(x => x.EmployeeId == Id && EntityFunctions.TruncateTime(x.AttendanceDate) >= startDate.Date && EntityFunctions.TruncateTime(x.AttendanceDate) < endDay);

            string user = AuthenticationModel.GetUserName();

            CultureInfo ci = new CultureInfo("id-ID");

            var query = (from model in q
                         select new
                         {
                             StartDate = EntityFunctions.TruncateTime(startDate),
                             EndDate = EntityFunctions.TruncateTime(endDate),
                             AttDate = model.AttendanceDate,
                             model.Employee.NIK,
                             model.Employee.Name,
                             title = model.Employee.TitleInfo.Name,
                             division = model.Employee.Division.Name,
                             department = model.Employee.Division.Department.Name,
                             branch = model.Employee.Division.Department.BranchOffice.Name,
                             companyname = company.Name,
                             mode = ChartType,
                             work = (model.CheckOut == null) ? 0 : EntityFunctions.DiffMinutes(model.CheckIn, model.CheckOut.Value) - model.BreakMinutes,
                             early = model.CheckEarlyMinutes,
                             late = model.CheckLateMinutes,
                             //User = user,
                         }).ToList();

            if (!query.Any()) return Content(Core.Constants.Constant.ErrorPage.RecordNotFound);

            var rd = new ReportDocument();

            //Loading Report
            rd.Load(Server.MapPath("~/") + "Reports/General/rpt_absensi_karyawan.rpt");

            // Setting report data source
            rd.SetDataSource(query);

            // Set parameters, need to be done after all data sources are set (to prevent reseting parameters)
            //rd.SetParameterValue(0, "Periode " + monthYear.ToString("MMMM yyyy", ci).ToUpper());

            // Set printer paper size
            //System.Drawing.Printing.PrintDocument doctoprint = new System.Drawing.Printing.PrintDocument();
            //doctoprint.PrinterSettings.PrinterName = "Microsoft XPS Document Writer";
            //int i = 0;
            //for (i = 0; i < doctoprint.PrinterSettings.PaperSizes.Count; i++)
            //{
            //    int rawKind = 0;
            //    if (doctoprint.PrinterSettings.PaperSizes[i].PaperName.ToLower() == "Letter")
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
