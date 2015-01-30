using Core.DomainModel;
using Core.Interface.Repository;
using Core.Interface.Service;
using Core.Interface.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Dynamic;
using System.Data.Entity;
using Core.Constants;
using System.Globalization;
using System.Data.SqlTypes;
using System.Data.Objects;
using Data.Context;
//using WebView.ObjectCopier;

namespace Service.Service
{
    public class SalaryProcessService : ISalaryProcessService
    {
        //struct NameSign
        //{
        //    public string name;
        //    public int sign;
        //};

        public ISalaryProcessValidator _validator;

        public SalaryProcessService(ISalaryProcessValidator _salaryProcessValidator)
        {
            _validator = _salaryProcessValidator;
        }

        public decimal GetSalaryItemValue(Enum SalaryItemCode, IDictionary<string, decimal> salaryItemsValue, Dictionary<string, int> salaryItemsSign)
        {
            string code = SalaryItemCode.ToString();
            return salaryItemsValue[code] * salaryItemsSign[code];
        }

        // TODO: Split each part into separate functions
        public string ProcessEmployee(Nullable<int> EmployeeId, DateTime monthYear, int NoSlip = 1, string Disiapkan_oleh = null, string Disetujui_oleh = null, string Dikoreksi_oleh = null)
        {
            string error = null; //_validator.ValidProcessEmployee(EmployeeId, monthYear);
            if (error == null)
            {
                DateTime startDate = new DateTime(monthYear.Year, monthYear.Month, 1);
                DateTime endDate = startDate.AddMonths(1).AddDays(-1); // new DateTime(monthYear.Year, (monthYear.Month % 12) + 1, 1);
                //endDate = endDate.AddDays(-1);

                //var alltables = new AttPayrollEntities().Set<Database>()
                //                    .Include("Tables.Fields")
                //                    .Include("Tables.ForeingKeys");

                var employees = _validator._employeeService.GetQueryable().Include("EmployeeEducation").Include("LastEmployment").Include("TitleInfo").Include("Division").Include("EmployeeWorkingTimes")
                    //.Include("Department").Include("BranchOffice").Include("WorkingTime").Include("WorkingDays")
                                        .Where(x => (EmployeeId == null || x.Id == EmployeeId.Value)).OrderByDescending(x => x.CreatedAt);

                //Employee employee = employees.FirstOrDefault();
                foreach (var employee in employees)
                {

                    var spls = _validator._spklService.GetQueryable().Include("Employee").Where(x => x.EmployeeId == employee.Id).OrderBy(x => x.StartTime);
                    var empls = _validator._employeeLeaveService.GetQueryable().Include("Employee").Where(x => x.EmployeeId == employee.Id).OrderBy(x => x.StartDate);

                    var wts = _validator._workingTimeService.GetQueryable().Include("EmployeeWorkingTimes").Include("WorkingDays");

                    var wds = _validator._workingDayService.GetQueryable().Include("WorkingTime");
                    //.Include("EmployeeWorkingTimes").Include("Employees")

                    var ewts = _validator._employeeWorkingTimeService.GetQueryable().Include("Employee").Include("WorkingTime").OrderBy(x => x.StartDate);

                    var atts = _validator._employeeAttendanceService.GetQueryable().Include("Employee")//.Include("EmployeeAttendanceDetails")
                                    .Where(x => x.EmployeeId == employee.Id &&
                                                x.CheckIn.Year == monthYear.Year &&
                                                x.CheckIn.Month == monthYear.Month)
                                    .OrderBy(x => x.AttendanceDate);

                    //var attds = _validator._employeeAttendanceDetailService.GetQueryable().Include("EmployeeAttendance").Include("Employee")
                    //                .Include("SalaryItem").Include("Formula").Include("FirstSalaryItem").Include("SecondSalaryItem")
                    //                .Where(x => x.EmployeeAttendance.EmployeeId == employee.Id &&
                    //                            x.EmployeeAttendance.AttendanceDate.Year == yearMonth.Year &&
                    //                            x.EmployeeAttendance.AttendanceDate.Month == yearMonth.Month);

                    var sals = _validator._salaryEmployeeDetailService.GetQueryable().Include("SalaryEmployee").Include("SalaryItem")
                        //.Include("Employee").Include("Formula").Include("FirstSalaryItem").Include("SecondSalaryItem")
                                    .Where(x => x.SalaryEmployee.EmployeeId == employee.Id &&
                                        //x.SalaryEmployee.EffectiveDate >= startDate && // TODO : Only include the last effective salary before (or same day with) startDate instead of from beginning
                                                x.SalaryEmployee.EffectiveDate <= endDate)
                                    .OrderByDescending(x => x.SalaryEmployee.EffectiveDate);
                    var sallist = sals.ToList();

                    var genls = _validator._generalLeaveService.GetQueryable().OrderBy(x => x.StartDate);

                    var thrds = _validator._thrDetailService.GetQueryable().Where(x => x.EmployeeId == employee.Id).Include("Employee").Include("THR").OrderBy(x => x.THR.EffectiveDate);
                    var oids = _validator._otherIncomeDetailService.GetQueryable().Where(x => x.EmployeeId == employee.Id).Include("Employee").Include("OtherIncome").OrderBy(x => x.EffectiveDate);
                    var oeds = _validator._otherExpenseDetailService.GetQueryable().Where(x => x.EmployeeId == employee.Id).Include("Employee").Include("OtherExpense").OrderBy(x => x.EffectiveDate);

                    var slips = _validator._salarySlipService.GetQueryable().Include("SalarySlipDetails").Include("SalaryItem")
                        //.Include("Formula").Include("FirstSalaryItem").Include("SecondSalaryItem")
                                                .OrderBy(x => x.Index); // Code

                    var slipdets = _validator._salarySlipDetailService.GetQueryable().Include("SalarySlip").Include("Formula")
                        //.Include("FirstSalaryItem").Include("SecondSalaryItem").Include("SalaryItem")
                                                    .OrderBy(x => x.SalarySlip.Index).ThenBy(x => x.Index); // Code

                    var salits = _validator._salaryItemService.GetQueryable();
                    //.Include("Formula").Include("FirstSalaryItem").Include("SecondSalaryItem"); 
                    //.OrderBy(x => x.Code);

                    var forms = _validator._formulaService.GetQueryable().Include("SalarySlipDetail").Include("FirstSalaryItem").Include("SecondSalaryItem");

                    var pph21s = _validator._pph21sptService.GetQueryable().OrderBy(x => x.Code); // x.Index

                    decimal ptkp = _validator._ptkpService.CalcPTKP(employee.MaritalStatus != (int)Constant.MaritalStatus.Married, employee.Children);

                    // Gunakan Culture Indonesia
                    CultureInfo ci = new CultureInfo("id-ID"); // CultureInfo.GetCultures(CultureTypes.NeutralCultures).Where(x => x.EnglishName == "Indonesian (Indonesia)").FirstOrDefault(); // "en-US"

                    // Create temporary salary items sign
                    Dictionary<string, int> salaryItemsSign = new Dictionary<string, int>();
                    // Create temporary salary item value
                    Dictionary<DateTime, Dictionary<string, decimal>> salaryItemsValue = new Dictionary<DateTime, Dictionary<string, decimal>>();
                    //List<string> names = new List<string>();
                    //List<decimal> signs = new List<decimal>();
                    
                    //List<NameSign> namesigns = new List<NameSign>();
                    //names.AddRange(Enum.GetNames(typeof(Constant.LegacyAttendanceItem)));
                    //names.AddRange(Enum.GetNames(typeof(Constant.LegacySalaryItem)));
                    //names.AddRange(Enum.GetNames(typeof(Constant.LegacyMonthlyItem)));
                    //NameSign ns;
                    foreach (var item in salits)
                    {
                        //names.Add(item.Code);
                        //signs.Add(item.SalarySign);
                        //ns.name = item.Code;
                        //ns.sign = item.SalarySign;
                        //namesigns.Add(ns);
                        salaryItemsSign.Add(item.Code, item.SalarySign);
                    };

                    WorkingDay wd = new WorkingDay();
                    //OtherExpenseDetail oed = new OtherExpenseDetail();
                    //OtherIncomeDetail oid = new OtherIncomeDetail();
                    //DateTime curDay = endDate;
                    DateTime prevDay = endDate;
                    for (DateTime curDay = endDate; curDay >= startDate; curDay = curDay.AddDays(-1))
                    {
                        // Init Legacy items
                        salaryItemsValue[curDay] = new Dictionary<string, decimal>();
                        foreach (var item in salaryItemsSign)
                        {
                            salaryItemsValue[curDay].Add(item.Key, 0);
                        }

                        // Take monthly items values from previous date as a base
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.ALPHA.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PRESN.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.LATE.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.LATE.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT15.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.TOT15.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT20.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.TOT20.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT30.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.TOT30.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT40.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.TOT40.ToString()];
                        //salaryItemsValue[curDay][Constant.LegacyMonthlyItem.THR.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.THR.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH05D.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH05D.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH15D.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH15D.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH25D.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH25D.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH30D.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH30D.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH05P.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH05P.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH15P.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH15P.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH25P.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH25P.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH30P.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH30P.ToString()];

                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PTKP.ToString()] = ptkp;
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.GJPOK.ToString()] = salaryItemsValue[prevDay][Constant.LegacySalaryItem.GJPOK.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.OVTRT.ToString()] = salaryItemsValue[prevDay][Constant.LegacySalaryItem.OVTRT.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.TJLAP.ToString()] = salaryItemsValue[prevDay][Constant.LegacySalaryItem.TJLAP.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.TJMKN.ToString()] = salaryItemsValue[prevDay][Constant.LegacySalaryItem.TJMKN.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.TJTRN.ToString()] = salaryItemsValue[prevDay][Constant.LegacySalaryItem.TJTRN.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.TJLNA.ToString()] = salaryItemsValue[prevDay][Constant.LegacySalaryItem.TJLNA.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.INHDR.ToString()] = salaryItemsValue[prevDay][Constant.LegacySalaryItem.INHDR.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.LTTRT.ToString()] = salaryItemsValue[prevDay][Constant.LegacySalaryItem.LTTRT.ToString()];

                        salaryItemsValue[curDay][Constant.LegacyAttendanceItem.WKAGE.ToString()] = (decimal)(curDay - employee.StartWorkingDate).TotalDays;
                        if (employee.AppointmentDate != null)
                        {
                            salaryItemsValue[curDay][Constant.LegacyAttendanceItem.PMAGE.ToString()] = (decimal)(curDay - employee.AppointmentDate.GetValueOrDefault()).TotalDays;
                        }
                        else
                        {
                            salaryItemsValue[curDay][Constant.LegacyAttendanceItem.PMAGE.ToString()] = 0;
                        }
                        
                        // Cari gaji aktif yang sesuai hari
                        var gajiaktif = sallist.FirstOrDefault();
                        while (gajiaktif != null && gajiaktif.SalaryEmployee.EffectiveDate > curDay)
                        {
                            // Pop from list/stack
                            sallist.RemoveAt(0);
                            gajiaktif = sallist.FirstOrDefault();
                        }
                        // Skip when no active salary found (working without being paid)
                        if (gajiaktif == null)
                        {
                            prevDay = curDay;
                            continue;
                        }

                        // Cari hari kerja, tidak termasuk hari libur
                        var ewt = ewts.Where(x => x.EmployeeId == employee.Id && x.StartDate <= curDay && x.EndDate >= curDay).FirstOrDefault();
                        string kodehari = "";
                        WorkingDay oriwd = null;
                        if (ewt != null)
                        {
                            kodehari = ewt.WorkingTime.Code + ((int)curDay.DayOfWeek).ToString(); //ci.DateTimeFormat.GetDayName(curDay.DayOfWeek);
                            oriwd = wds.Where(x => x.WorkingTime.Id == ewt.WorkingTimeId && x.IsEnabled && x.Code == kodehari).FirstOrDefault();
                        }

                        // Cari surat lembur
                        var spls2 = spls.Where(x => EntityFunctions.TruncateTime(x.StartTime) <= curDay && EntityFunctions.TruncateTime(x.EndTime) >= curDay);

                        // Not working day (off day)
                        if (oriwd == null)
                        {
                            // Tidak ada lembur, lanjut ke hari lain
                            if (!spls2.Any())
                            {
                                prevDay = curDay;
                                continue;
                            }
                            else
                            {
                                // Jika Tidak ada jadwal kerja, lanjut ke hari lain
                                if (ewt == null)
                                {
                                    prevDay = curDay;
                                    continue;
                                }
                                // Jika ada lembur, Cari ulang working day, termasuk hari libur/off
                                oriwd = wds.Where(x => x.WorkingTime.Id == ewt.WorkingTimeId && x.Code == kodehari).FirstOrDefault();
                            }
                        }
                        //wd = oriwd.Clone(); // clone object using GeneralFunction at WebView to prevent new object referencing the same/original object

                        // Hitung ulang jam kerja sesuai hari, offsetkan hari supaya tanggal CheckIn = tanggal curDay
                        int ofset = 0;
                        if (oriwd.MinCheckIn.Date < oriwd.CheckIn.Date) ofset = -1; // 
                        wd.MinCheckIn = oriwd.MinCheckIn.Add(curDay.Date.Subtract((DateTime)SqlDateTime.MinValue)).AddDays(ofset);
                        wd.CheckIn = oriwd.CheckIn.Add(curDay.Date.Subtract((DateTime)SqlDateTime.MinValue)).AddDays(ofset);
                        wd.MaxCheckIn = oriwd.MaxCheckIn.Add(curDay.Date.Subtract((DateTime)SqlDateTime.MinValue)).AddDays(ofset);
                        wd.MinCheckOut = oriwd.MinCheckOut.Add(curDay.Date.Subtract((DateTime)SqlDateTime.MinValue)).AddDays(ofset);
                        wd.CheckOut = oriwd.CheckOut.Add(curDay.Date.Subtract((DateTime)SqlDateTime.MinValue)).AddDays(ofset);
                        wd.MaxCheckOut = oriwd.MaxCheckOut.Add(curDay.Date.Subtract((DateTime)SqlDateTime.MinValue)).AddDays(ofset);
                        wd.BreakOut = oriwd.BreakOut.Add(curDay.Date.Subtract((DateTime)SqlDateTime.MinValue)).AddDays(ofset);
                        wd.BreakIn = oriwd.BreakIn.Add(curDay.Date.Subtract((DateTime)SqlDateTime.MinValue)).AddDays(ofset);
                        wd.WorkInterval = oriwd.WorkInterval;
                        wd.BreakInterval = oriwd.BreakInterval;
                        wd.CheckInTolerance = oriwd.CheckInTolerance;
                        wd.CheckOutTolerance = oriwd.CheckOutTolerance;
                        wd.IsEnabled = oriwd.IsEnabled;
                        wd.Id = oriwd.Id;

                        // Cari absensi yg sesuai
                        var att = atts.Where(x => EntityFunctions.TruncateTime(x.CheckIn) == curDay).FirstOrDefault();

                        // Absensi tidak ada
                        if (att == null)
                        {
                            var cutibersama = genls.Where(x => x.StartDate <= curDay && x.EndDate >= curDay).FirstOrDefault();
                            // Bukan cuti bersama
                            if (cutibersama == null)
                            {
                                var izin = empls.Where(x => x.StartDate <= curDay && x.EndDate >= curDay).FirstOrDefault();
                                // Bukan cuti/izin/sakit dgn surat
                                if (izin == null)
                                {
                                    // Bolos, hitung jumlah hari alpa dlm satu periode dan lanjut ke hari lain
                                    salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] += 1;
                                    prevDay = curDay;
                                    continue;
                                }
                            }
                        }
                        // Absensi ada, hitung Attendance items
                        else
                        {
                            DateTime attCheckOut = att.CheckOut.GetValueOrDefault();
                            // Jika tidak checkout, gunakan maxcheckout
                            if (att.CheckOut == null)
                            {
                                attCheckOut = wd.MaxCheckOut;
                            }
                            // Hitung CheckIn Telat/Awal hanya dihari kerja
                            if (wd.IsEnabled)
                            {
                                // Telat checkin
                                TimeSpan telat = att.CheckIn.Subtract(wd.CheckIn);
                                if (att.CheckIn > wd.CheckIn.AddMinutes((double)wd.CheckInTolerance))
                                {
                                    // Tidak ada ijin/alasan, hitung menit potongan telat dan jumlah hari telat dlm satu periode
                                    if (att.Status == (int)Constant.AttendanceStatus.Present)
                                    {
                                        salaryItemsValue[curDay][Constant.LegacyAttendanceItem.CILTM.ToString()] = (decimal)telat.TotalMinutes / 60;
                                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.LATE.ToString()] += 1;
                                    }
                                }
                                // Checkout lebih awal
                                TimeSpan awal = wd.CheckOut.Subtract(attCheckOut);
                                if (attCheckOut < wd.CheckOut.AddMinutes((double)-wd.CheckOutTolerance))
                                {
                                    // Tidak ada ijin/alasan, hitung menit potongan
                                    if (att.Status == (int)Constant.AttendanceStatus.Present)
                                    {
                                        salaryItemsValue[curDay][Constant.LegacyAttendanceItem.COETM.ToString()] = (decimal)awal.TotalMinutes / 60;
                                    }
                                }
                            }
                            // Telat checkout/overtime
                            if (attCheckOut > wd.CheckOut)
                            {
                                // Ada surat lembur, hitung menit lembur
                                var spl = spls2.Where(x => x.StartTime < attCheckOut && x.StartTime >= att.CheckIn).FirstOrDefault();
                                if (spl != null)
                                {
                                    if (att.Status == (int)Constant.AttendanceStatus.Present)
                                    {
                                        DateTime maxlembur = spl.EndTime;
                                        if (maxlembur > attCheckOut)
                                        {
                                            maxlembur = attCheckOut;
                                        }
                                        DateTime minlembur = spl.StartTime;
                                        if (minlembur < att.CheckIn)
                                        {
                                            minlembur = att.CheckIn;
                                        }
                                        // Waktu Lembur tidak boleh negatif
                                        decimal menitlembur = Math.Max(0, (decimal)(maxlembur - minlembur).TotalMinutes); //(decimal)-awal.TotalMinutes;
                                        salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OVRTM.ToString()] = menitlembur / 60;
                                        // Lembur di Hari kerja (1jam pertama = 1.5x, selebihnya = 2x)
                                        if (wd.IsEnabled)
                                        {
                                            // 1jam pertama
                                            if (menitlembur >= 60)
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH15.ToString()] += 1;
                                                menitlembur -= 60;
                                            }
                                            // jam ke2 dst
                                            if (menitlembur >= 60)
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH20.ToString()] += (menitlembur / 60);
                                                //menitlembur = 0;
                                            }
                                        }
                                        // Lembur di Hari Libur
                                        else 
                                        {
                                            decimal duakali = 0;
                                            // 5hari kerja seminggu, 8jam pertama = 2x, jam ke9 = 3x, ke10-11 = 4x
                                            if (_validator._workingDayService.CountEnabledDays(wd) <= 5) 
                                            {
                                                duakali = 8;
                                            } 
                                            // 6hari kerja seminggu
                                            else if (_validator._workingDayService.CountEnabledDays(wd) >= 6)
                                            {
                                                // Libur pd hari kerja terpendek (6hari kerja, 5jam pertama = 2x, jam ke6 = 3x, ke7-8 = 4x)
                                                if (_validator._workingDayService.IsShortestWorkingDay(wd)) 
                                                {
                                                    duakali = 5;
                                                } 
                                                // 6hari kerja, 7jam pertama = 2x, jam ke8 = 3x, ke9-10 = 4x
                                                else
                                                {
                                                    duakali = 7;
                                                }
                                            }

                                            // upto (x)jam pertama
                                            if (menitlembur >= 60 * duakali)
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH20.ToString()] += duakali;
                                                menitlembur -= 60 * duakali;
                                            }
                                            else
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH20.ToString()] += (menitlembur / 60);
                                                menitlembur = 0;
                                            }
                                            // jam ke(x+1)
                                            if (menitlembur >= 60)
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH30.ToString()] += 1;
                                                menitlembur -= 60;
                                            }
                                            // jam ke(x+2 to x+3) (dst?)
                                            if (menitlembur >= 60)
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH40.ToString()] += (menitlembur / 60);
                                            }
                                        }
                                        
                                        // Update Monthly Items of Total OverTime Hours
                                        //salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT15.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH15.ToString()];
                                        //salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT20.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH20.ToString()];
                                        //salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT30.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH30.ToString()];
                                        //salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT40.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH40.ToString()];
                                    }
                                }
                            }
                        }
                        // Hadir, hitung jumlah hari hadir dlm satu periode
                        if (att.Status != (int)Constant.AttendanceStatus.Alpha &&
                            att.Status != (int)Constant.AttendanceStatus.Izin)
                        {
                            salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] += 1;
                        }
                        // Regular working time
                        salaryItemsValue[curDay][Constant.LegacyAttendanceItem.REGWT.ToString()] = (wd.WorkInterval - wd.BreakInterval) / 60;
                        // Actual working time
                        salaryItemsValue[curDay][Constant.LegacyAttendanceItem.WRKTM.ToString()] = ((wd.WorkInterval - wd.BreakInterval) / 60 -
                                               (salaryItemsValue[curDay][Constant.LegacyAttendanceItem.CILTM.ToString()] + salaryItemsValue[curDay][Constant.LegacyAttendanceItem.COETM.ToString()]));

                        // Update Legacy Salary items & Legacy Attendance items
                        //salaryItemsValue[curDay][Constant.LegacySalaryItem.PTKP.ToString()] = ptkp;

                        foreach (var detail in gajiaktif.SalaryEmployee.SalaryEmployeeDetails)
                        {
                            salaryItemsValue[curDay][detail.SalaryItem.Code] = detail.Amount;
                        }

                        foreach (var pph21spt in pph21s)
                        {
                            salaryItemsValue[curDay][pph21spt.Code + "D"] = Math.Max(0, pph21spt.MaxAmount - pph21spt.MinAmount);
                            salaryItemsValue[curDay][pph21spt.Code + "P"] = Math.Max(0, pph21spt.Percent/100.0m);
                        }

                        //foreach (var detail in att.EmployeeAttendanceDetails)
                        //{
                        //    salaryItemsValue[curDay][detail.SalaryItem.Code] = detail.Amount;
                        //}

                        prevDay = curDay;
                    }

                    // Cari THR yg berlaku sesuai hari
                    var thrd = thrds.Where(x => x.THR.EffectiveDate >= startDate && x.THR.EffectiveDate <= endDate).FirstOrDefault();
                    if (thrd != null)
                    {
                        salaryItemsValue[startDate][Constant.LegacyMonthlyItem.THR.ToString()] = salaryItemsValue[startDate][Constant.LegacySalaryItem.GJPOK.ToString()];
                        // Jika blm setahun bekerja, hitung persentasenya
                        if (salaryItemsValue[startDate][Constant.LegacyAttendanceItem.PMAGE.ToString()] < 365)
                        {
                            salaryItemsValue[startDate][Constant.LegacyMonthlyItem.THR.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.THR.ToString()] * salaryItemsValue[startDate][Constant.LegacyAttendanceItem.PMAGE.ToString()] / 365;
                        }
                    }

                    // Cari OtherIncome yg berlaku sesuai hari
                    var oids2 = oids.Where(x => x.EffectiveDate <= endDate && x.EndDate >= startDate && x.Recurring > 0);
                    foreach (var oid in oids2)
                    {
                        for (DateTime curDay = oid.EffectiveDate; curDay <= oid.EndDate; curDay = curDay.AddDays(1))
                        {
                            if (oid.Recurring > 0)
                            {
                                if (curDay >= startDate && curDay <= endDate)
                                {
                                    salaryItemsValue[oid.EffectiveDate][oid.OtherIncome.SalaryItem.Code] += oid.Amount;
                                    salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOTTJ.ToString()] += oid.Amount;
                                }
                                oid.Recurring--;
                                switch ((Constant.SalaryItemStatus)oid.OtherIncome.SalaryStatus)
                                {
                                    case Constant.SalaryItemStatus.Daily: oid.EffectiveDate = curDay.AddDays(1); break;
                                    case Constant.SalaryItemStatus.Weekly: oid.EffectiveDate = curDay.AddDays(7); break;
                                    case Constant.SalaryItemStatus.Monthly: oid.EffectiveDate = curDay.AddMonths(1); break;
                                    case Constant.SalaryItemStatus.Yearly: oid.EffectiveDate = curDay.AddYears(1); break;
                                }
                            }
                            else break;
                        }
                    }

                    // Cari OtherExpense yg berlaku sesuai hari
                    var oeds2 = oeds.Where(x => x.EffectiveDate <= endDate && x.EndDate >= startDate && x.Recurring > 0);
                    foreach (var oed in oeds2)
                    {
                        for (DateTime curDay = oed.EffectiveDate; curDay <= oed.EndDate; curDay = curDay.AddDays(1))
                        {
                            if (oed.Recurring > 0)
                            {
                                if (curDay >= startDate && curDay <= endDate)
                                {
                                    salaryItemsValue[oed.EffectiveDate][oed.OtherExpense.SalaryItem.Code] += oed.Amount;
                                    salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOTPT.ToString()] += oed.Amount;
                                }
                                oed.Recurring--;
                                switch ((Constant.SalaryItemStatus)oed.OtherExpense.SalaryStatus)
                                {
                                    case Constant.SalaryItemStatus.Daily: oed.EffectiveDate = curDay.AddDays(1); break;
                                    case Constant.SalaryItemStatus.Weekly: oed.EffectiveDate = curDay.AddDays(7); break;
                                    case Constant.SalaryItemStatus.Monthly: oed.EffectiveDate = curDay.AddMonths(1); break;
                                    case Constant.SalaryItemStatus.Yearly: oed.EffectiveDate = curDay.AddYears(1); break;
                                }
                            }
                            else break;
                        }
                    }

                    // Calculate Total/Monthy Items
                    for (DateTime curDay = startDate.AddDays(1); curDay <= endDate; curDay = curDay.AddDays(1))
                    {
                        salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT15.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH15.ToString()];
                        salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT20.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH20.ToString()];
                        salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT30.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH30.ToString()];
                        salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT40.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH40.ToString()];

                        //salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOTPT.ToString()] += salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOTPT.ToString()];
                        //salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOTTJ.ToString()] += salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOTTJ.ToString()];
                    }

                    // Update Monthly Attendance items, which might be used on custom salary items calculation (to make sure All Legacy Items are ready to be used on formula)
                    for (DateTime curDay = startDate.AddDays(1); curDay <= endDate; curDay = curDay.AddDays(1))
                    {
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.ALPHA.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.PRESN.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.LATE.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.LATE.ToString()];

                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT15.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT15.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT20.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT20.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT30.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT30.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT40.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT40.ToString()];

                        salaryItemsValue[curDay][Constant.LegacyAttendanceItem.WKAGE.ToString()] = salaryItemsValue[startDate][Constant.LegacyAttendanceItem.WKAGE.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyAttendanceItem.PMAGE.ToString()] = salaryItemsValue[startDate][Constant.LegacyAttendanceItem.PMAGE.ToString()];

                        salaryItemsValue[curDay][Constant.LegacySalaryItem.GJPOK.ToString()] = salaryItemsValue[startDate][Constant.LegacySalaryItem.GJPOK.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.OVTRT.ToString()] = salaryItemsValue[startDate][Constant.LegacySalaryItem.OVTRT.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.LTTRT.ToString()] = salaryItemsValue[startDate][Constant.LegacySalaryItem.LTTRT.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.INHDR.ToString()] = salaryItemsValue[startDate][Constant.LegacySalaryItem.INHDR.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.TJMKN.ToString()] = salaryItemsValue[startDate][Constant.LegacySalaryItem.TJMKN.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.TJTRN.ToString()] = salaryItemsValue[startDate][Constant.LegacySalaryItem.TJTRN.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.TJLAP.ToString()] = salaryItemsValue[startDate][Constant.LegacySalaryItem.TJLAP.ToString()];
                    }

                    //curDay = startDate;
                    for (DateTime curDay = startDate; curDay <= endDate; curDay = curDay.AddDays(1))
                    {
                        // Hitung custom Salary items
                        //foreach (var item in salits)
                        //{
                        //    item.CurrentValue = _validator._salaryItemService.CalcSalaryItem(item, salaryItemsValue[curDay], _validator._formulaService);
                        //    salaryItemsValue[curDay][item.Code] = item.CurrentValue;                           
                        //}

                        // Calculate Slip items (with formula), sorted by index as a slip value can be used by the next slip formula
                        SalarySlip curSlip = null;
                        SalarySlip prvSlip = null;
                        foreach (var slipdet in slipdets)
                        {
                            curSlip = slipdet.SalarySlip;
                            if (prvSlip != curSlip)
                            {
                                if (prvSlip != null)
                                {
                                    salaryItemsValue[curDay][prvSlip.SalaryItem.Code] = Math.Abs(salaryItemsValue[curDay][prvSlip.SalaryItem.Code]);
                                    slipdet.SalarySlip.TotalAmount = salaryItemsValue[curDay][prvSlip.SalaryItem.Code];
                                }
                                prvSlip = curSlip;
                            }

                            //slipdet.Amount = salaryItemsValue[curDay][slipdet.SalaryItem.Code]; // slipdet.SalaryItem.CurrentValue;
                            //slipdet.Value = _validator._formulaService.CalcFormula(slipdet.Formula, salaryItemsValue[curDay], salits);
                            slipdet.Value = _validator._salarySlipDetailService.CalcSalarySlipDetail(slipdet, salaryItemsValue[curDay], salits, _validator._formulaService);
                            salaryItemsValue[curDay][curSlip.SalaryItem.Code] += slipdet.Value * slipdet.SalarySign;
                            slipdet.SalarySlip.TotalAmount = salaryItemsValue[curDay][curSlip.SalaryItem.Code];
                        }
                        foreach (var slip in slips)
                        {
                            salaryItemsValue[curDay][slip.SalaryItem.Code] = Math.Abs(salaryItemsValue[curDay][slip.SalaryItem.Code]);
                            slip.TotalAmount = salaryItemsValue[curDay][slip.SalaryItem.Code];
                        }
                    }

                    // Hitung PTKP & PPH21
                    
                    //salaryItemsValue[startDate][Constant.LegacySalaryItem.PTKP.ToString()] = _validator._ptkpService.CalcPTKP(employee.MaritalStatus != (int)Constant.Marital.Married, employee.Children);
                    List<decimal> pphlist = new List<decimal>();
                    decimal pph21 = _validator._pph21sptService.CalcPPH21(employee.NPWP!="", salaryItemsValue[startDate][Constant.UserMonthlyItem.TDKPT.ToString()], pphlist) / 12;
                    salaryItemsValue[startDate][Constant.UserMonthlyItem.GJBSP.ToString()] = salaryItemsValue[startDate][Constant.UserMonthlyItem.GJKOT.ToString()] -
                                                                                             (salaryItemsValue[startDate][Constant.UserMonthlyItem.PTPJM.ToString()] +
                                                                                             salaryItemsValue[startDate][Constant.UserMonthlyItem.JMSTK.ToString()]);
                    //salaryItemsValue[startDate][Constant.UserMonthlyItem.GJBSP.ToString()] += salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH21.ToString()];
                    salaryItemsValue[startDate][Constant.UserMonthlyItem.GJBSP.ToString()] -= pph21;
                    salaryItemsValue[startDate][Constant.UserMonthlyItem.GJRND.ToString()] = 1000 - (salaryItemsValue[startDate][Constant.UserMonthlyItem.GJBSP.ToString()] % 1000);
                    salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH05.ToString()] = pphlist[0];
                    salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH15.ToString()] = pphlist[1];
                    salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH25.ToString()] = pphlist[2];
                    salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH30.ToString()] = pphlist[3];
                    salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH21.ToString()] = pph21;
                    salaryItemsValue[startDate][Constant.UserMonthlyItem.GJBSH.ToString()] = salaryItemsValue[startDate][Constant.UserMonthlyItem.GJBSP.ToString()] + salaryItemsValue[startDate][Constant.UserMonthlyItem.GJRND.ToString()];

                    // SlipGaji (dari Monthly Items)
                    // Buat ViewModel SlipGaji Mini (for each SalarySlip ?)
                    SlipGajiMini slipGajiMini = _validator._slipGajiMiniService.GetOrNewObjectByEmployeeMonth(employee.Id, monthYear);
                    slipGajiMini.MONTHyear = monthYear;
                    slipGajiMini.EmployeeId = employee.Id;
                    slipGajiMini.month = monthYear.ToString("MMMM", ci);
                    slipGajiMini.employee_code = employee.NIK;
                    slipGajiMini.employee_name = employee.Name;
                    slipGajiMini.title_code = employee.TitleInfo.Code;
                    slipGajiMini.title_name = employee.TitleInfo.Name;
                    slipGajiMini.start_working = employee.StartWorkingDate; // employee.AppointmentDate.GetValueOrDefault();
                    slipGajiMini.no_rekening = employee.BankAccount;
                    slipGajiMini.Disiapkan_oleh = Disiapkan_oleh;
                    slipGajiMini.Disetujui_oleh = Disetujui_oleh;
                    slipGajiMini.Dikoreksi_oleh = Dikoreksi_oleh;
                    slipGajiMini.company_code = employee.Division.Department.BranchOffice.Code;

                    slipGajiMini.salary_basic = salaryItemsValue[startDate][Constant.LegacySalaryItem.GJPOK.ToString()];
                    slipGajiMini.rate_hour = salaryItemsValue[startDate][Constant.LegacySalaryItem.OVTRT.ToString()];
                    slipGajiMini.allowance_rate = salaryItemsValue[startDate][Constant.LegacySalaryItem.TJLAP.ToString()];
                    slipGajiMini.uang_makan = salaryItemsValue[startDate][Constant.UserMonthlyItem.TTJMK.ToString()];
                    slipGajiMini.jml_jam_lembur = salaryItemsValue[startDate][Constant.LegacyAttendanceItem.OVRTM.ToString()];
                    slipGajiMini.jml_lembur = salaryItemsValue[startDate][Constant.UserMonthlyItem.TOVTM.ToString()];
                    slipGajiMini.jml_hari_absen = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.ALPHA.ToString()];
                    slipGajiMini.tunj_lap = salaryItemsValue[startDate][Constant.UserMonthlyItem.TTJLP.ToString()];
                    slipGajiMini.insentive_hadir = salaryItemsValue[startDate][Constant.UserMonthlyItem.TINHD.ToString()];
                    slipGajiMini.krg_bln_lalu = salaryItemsValue[startDate][Constant.UserMonthlyItem.KBLLU.ToString()];
                    slipGajiMini.thr = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.THR.ToString()];
                    slipGajiMini.pot_absensi = salaryItemsValue[startDate][Constant.UserMonthlyItem.PTABS.ToString()];
                    slipGajiMini.pot_others = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOTPT.ToString()];
                    slipGajiMini.gaji_kotor = salaryItemsValue[startDate][Constant.UserMonthlyItem.GJKOT.ToString()];
                    slipGajiMini.pot_pinjaman = salaryItemsValue[startDate][Constant.UserMonthlyItem.PTPJM.ToString()];
                    slipGajiMini.pot_jamsostek = salaryItemsValue[startDate][Constant.UserMonthlyItem.JMSTK.ToString()];
                    slipGajiMini.pjk_jkk_jkm_204 = salaryItemsValue[startDate][Constant.UserMonthlyItem.PJ204.ToString()];
                    slipGajiMini.tot_dpt_kotor = salaryItemsValue[startDate][Constant.UserMonthlyItem.TDKOT.ToString()];
                    slipGajiMini.pjk_tunj_jabatan = salaryItemsValue[startDate][Constant.UserMonthlyItem.PJTJJB.ToString()];
                    slipGajiMini.pjk_ptkp = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPTKP.ToString()];
                    slipGajiMini.tot_pengurang_pajak = salaryItemsValue[startDate][Constant.UserMonthlyItem.TPPJK.ToString()];
                    slipGajiMini.tot_dpt_kena_pajak = salaryItemsValue[startDate][Constant.UserMonthlyItem.TDKPJ.ToString()];
                    slipGajiMini.tot_dpt_kena_pajak_tahun = salaryItemsValue[startDate][Constant.UserMonthlyItem.TDKPT.ToString()];
                    slipGajiMini.pph_5_persen = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH05.ToString()];
                    slipGajiMini.pph_15_persen = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH15.ToString()];
                    slipGajiMini.pph_25_persen = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH25.ToString()];
                    slipGajiMini.pph_30_persen = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH30.ToString()];
                    slipGajiMini.pph21 = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH21.ToString()];
                    slipGajiMini.round = salaryItemsValue[startDate][Constant.UserMonthlyItem.GJRND.ToString()];
                    slipGajiMini.gaji_bersih = salaryItemsValue[startDate][Constant.UserMonthlyItem.GJBSH.ToString()];    
                    
                    _validator._slipGajiMiniService.CreateOrUpdateObject(slipGajiMini, _validator._employeeService);

                    // Buat ViewModel SlipGaji Detail
                    SlipGajiDetail slipGajiDetail = _validator._slipGajiDetailService.GetOrNewObjectByEmployeeMonth(employee.Id, monthYear);
                    if (slipGajiDetail.Id > 0)
                    {
                        // Delete the parent to cascade delete the details
                        _validator._slipGajiDetailService.DeleteObject(slipGajiDetail.Id);
                        slipGajiDetail.Id = 0;
                        //slipGajiDetail = new SlipGajiDetail();
                    }
                    slipGajiDetail.MONTH = monthYear;
                    slipGajiDetail.EmployeeId = employee.Id;
                    slipGajiDetail.NoBadge = employee.NIK;
                    slipGajiDetail.Name = employee.Name;
                    slipGajiDetail.Jabatan = employee.TitleInfo.Name;
                    slipGajiDetail.TanggalPenerimaan = (employee.AppointmentDate != null? employee.AppointmentDate.GetValueOrDefault(): employee.StartWorkingDate);
                    slipGajiDetail.PeriodeAwal = startDate;
                    slipGajiDetail.PeriodeAkhir = endDate;
                    slipGajiDetail.StatusMarriage = ((Constant.MaritalStatus)employee.MaritalStatus == Constant.MaritalStatus.Married ? "M/":"S/") + employee.Children.ToString(); // ((Constant.MaritalStatus)employee.MaritalStatus).ToString();
                    slipGajiDetail.NoSlip = NoSlip;
                    slipGajiDetail.Disiapkan_oleh = Disiapkan_oleh;
                    slipGajiDetail.Disetujui_oleh = Disetujui_oleh;
                    slipGajiDetail.Dikoreksi_oleh = Dikoreksi_oleh;
                    slipGajiDetail.company_code = employee.Division.Department.BranchOffice.Code;

                    slipGajiDetail.GajiBasis = GetSalaryItemValue(Constant.LegacySalaryItem.GJPOK, salaryItemsValue[startDate], salaryItemsSign); // salaryItemsValue[startDate][Constant.LegacySalaryItem.GJPOK.ToString()];
                    slipGajiDetail.Rate = GetSalaryItemValue(Constant.LegacySalaryItem.OVTRT, salaryItemsValue[startDate], salaryItemsSign); // salaryItemsValue[startDate][Constant.LegacySalaryItem.OVTRT.ToString()];
                    slipGajiDetail.Lembur15 = GetSalaryItemValue(Constant.LegacyMonthlyItem.TOT15, salaryItemsValue[startDate], salaryItemsSign); // salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT15.ToString()];
                    slipGajiDetail.Lembur20 = GetSalaryItemValue(Constant.LegacyMonthlyItem.TOT20, salaryItemsValue[startDate], salaryItemsSign); // salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT20.ToString()];
                    slipGajiDetail.Lembur30 = GetSalaryItemValue(Constant.LegacyMonthlyItem.TOT30, salaryItemsValue[startDate], salaryItemsSign); // salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT30.ToString()];
                    slipGajiDetail.Lembur40 = GetSalaryItemValue(Constant.LegacyMonthlyItem.TOT40, salaryItemsValue[startDate], salaryItemsSign); // salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT40.ToString()];
                    
                    _validator._slipGajiDetailService.CreateOrUpdateObject(slipGajiDetail, _validator._employeeService, _validator._slipGajiDetail1Service, _validator._slipGajiDetail2AService);

                    // Buat ViewModel SlipGajiDetail2A (for each SalarySlip ?)
                    SlipGajiDetail2A slipGajiDetail2A = new SlipGajiDetail2A
                    {
                        SlipGajiDetailId = slipGajiDetail.Id,
                        month = monthYear.ToString("MMMM", ci),
                        employee_code = employee.NIK,

                        other_allow = GetSalaryItemValue(Constant.LegacyMonthlyItem.TOTTJ, salaryItemsValue[startDate], salaryItemsSign),
                        salary_basic = GetSalaryItemValue(Constant.LegacySalaryItem.GJPOK, salaryItemsValue[startDate], salaryItemsSign),
                        rate_hour = GetSalaryItemValue(Constant.LegacySalaryItem.OVTRT, salaryItemsValue[startDate], salaryItemsSign),
                        allowance_rate = GetSalaryItemValue(Constant.LegacySalaryItem.TJLAP, salaryItemsValue[startDate], salaryItemsSign),
                        uang_makan = GetSalaryItemValue(Constant.UserMonthlyItem.TTJMK, salaryItemsValue[startDate], salaryItemsSign),
                        jml_jam_lembur = GetSalaryItemValue(Constant.LegacyAttendanceItem.OVRTM, salaryItemsValue[startDate], salaryItemsSign),
                        jml_lembur = GetSalaryItemValue(Constant.UserMonthlyItem.TOVTM, salaryItemsValue[startDate], salaryItemsSign),
                        jml_hari_absen = GetSalaryItemValue(Constant.LegacyMonthlyItem.ALPHA, salaryItemsValue[startDate], salaryItemsSign),
                        tunj_lap = GetSalaryItemValue(Constant.UserMonthlyItem.TTJLP, salaryItemsValue[startDate], salaryItemsSign),
                        insentive_hadir = GetSalaryItemValue(Constant.UserMonthlyItem.TINHD, salaryItemsValue[startDate], salaryItemsSign),
                        krg_bln_lalu = GetSalaryItemValue(Constant.UserMonthlyItem.KBLLU, salaryItemsValue[startDate], salaryItemsSign),
                        thr = GetSalaryItemValue(Constant.LegacyMonthlyItem.THR, salaryItemsValue[startDate], salaryItemsSign),
                        pot_absensi = GetSalaryItemValue(Constant.UserMonthlyItem.PTABS, salaryItemsValue[startDate], salaryItemsSign),
                        pot_others = GetSalaryItemValue(Constant.LegacyMonthlyItem.TOTPT, salaryItemsValue[startDate], salaryItemsSign),
                        gaji_kotor = GetSalaryItemValue(Constant.UserMonthlyItem.GJKOT, salaryItemsValue[startDate], salaryItemsSign),
                        pot_pinjaman = GetSalaryItemValue(Constant.UserMonthlyItem.PTPJM, salaryItemsValue[startDate], salaryItemsSign),
                        pot_jamsostek = GetSalaryItemValue(Constant.UserMonthlyItem.JMSTK, salaryItemsValue[startDate], salaryItemsSign),
                        pjk_jkk_jkm_204 = GetSalaryItemValue(Constant.UserMonthlyItem.PJ204, salaryItemsValue[startDate], salaryItemsSign),
                        tot_dpt_kotor = GetSalaryItemValue(Constant.UserMonthlyItem.TDKOT, salaryItemsValue[startDate], salaryItemsSign),
                        pjk_tunj_jabatan = GetSalaryItemValue(Constant.UserMonthlyItem.PJTJJB, salaryItemsValue[startDate], salaryItemsSign),
                        pjk_ptkp = GetSalaryItemValue(Constant.UserMonthlyItem.PPTKP, salaryItemsValue[startDate], salaryItemsSign),
                        tot_pengurang_pajak = GetSalaryItemValue(Constant.UserMonthlyItem.TPPJK, salaryItemsValue[startDate], salaryItemsSign),
                        tot_dpt_kena_pajak = GetSalaryItemValue(Constant.UserMonthlyItem.TDKPJ, salaryItemsValue[startDate], salaryItemsSign),
                        tot_dpt_kena_pajak_tahun = GetSalaryItemValue(Constant.UserMonthlyItem.TDKPT, salaryItemsValue[startDate], salaryItemsSign),
                        pph_5_persen = GetSalaryItemValue(Constant.UserMonthlyItem.PPH05, salaryItemsValue[startDate], salaryItemsSign),
                        pph_15_persen = GetSalaryItemValue(Constant.UserMonthlyItem.PPH15, salaryItemsValue[startDate], salaryItemsSign),
                        pph_25_persen = GetSalaryItemValue(Constant.UserMonthlyItem.PPH25, salaryItemsValue[startDate], salaryItemsSign),
                        pph_30_persen = GetSalaryItemValue(Constant.UserMonthlyItem.PPH30, salaryItemsValue[startDate], salaryItemsSign),
                        pph21 = GetSalaryItemValue(Constant.UserMonthlyItem.PPH21, salaryItemsValue[startDate], salaryItemsSign),
                        round = GetSalaryItemValue(Constant.UserMonthlyItem.GJRND, salaryItemsValue[startDate], salaryItemsSign),
                        gaji_bersih = GetSalaryItemValue(Constant.UserMonthlyItem.GJBSH, salaryItemsValue[startDate], salaryItemsSign),
                    };
                    _validator._slipGajiDetail2AService.CreateObject(slipGajiDetail2A, _validator._slipGajiDetailService);
                    // Update SlipGaji Detail
                    //slipGajiDetail.SlipGajiDetail2AId = slipGajiDetail2A.Id;
                    //_validator._slipGajiDetailService.UpdateObject(slipGajiDetail, _validator._employeeService, _validator._slipGajiDetail1Service, _validator._slipGajiDetail2AService);

                    // last loop
                    for (DateTime curDay = startDate; curDay <= endDate; curDay = curDay.AddDays(1))
                    {
                        // Cari employee workingtime
                        var ewt = ewts.Where(x => x.EmployeeId == employee.Id && x.StartDate <= curDay && x.EndDate >= curDay).FirstOrDefault();
                        // Cari hari kerja, termasuk hari libur
                        string kodehari = "";
                        WorkingDay oriwd = null;
                        if (ewt != null)
                        {
                            kodehari = ewt.WorkingTime.Code + ((int)curDay.DayOfWeek).ToString(); //ci.DateTimeFormat.GetDayName(curDay.DayOfWeek);
                            oriwd = wds.Where(x => x.WorkingTime.Id == ewt.WorkingTimeId && x.Code == kodehari).FirstOrDefault();
                        }

                        string stat = "OF";
                        // Cari attendance yg sesuai hari
                        var att = atts.Where(x => EntityFunctions.TruncateTime(x.CheckIn) == curDay).FirstOrDefault();
                        if (att != null)
                        {
                            switch ((Constant.AttendanceStatus)att.Status)
                            {
                                case Constant.AttendanceStatus.Alpha: stat = "A"; break;
                                case Constant.AttendanceStatus.Present:
                                    {
                                        if (oriwd == null) break;
                                        if (oriwd.IsEnabled || _validator._workingDayService.CountEnabledDays(oriwd) <= 5) {
                                            if (ewt == null) break;
                                            stat = ewt.WorkingTime.Code; // "N"
                                        }
                                        else if (_validator._workingDayService.IsShortestWorkingDay(oriwd)) { stat = "HIS"; }
                                        else { stat = "HS"; }
                                        break;
                                    }
                                case Constant.AttendanceStatus.Duty: stat = "DT"; break;
                                case Constant.AttendanceStatus.Cuti: stat = "PR"; break;
                                case Constant.AttendanceStatus.Sakit: stat = "S"; break;
                                case Constant.AttendanceStatus.Izin: stat = "P"; break;
                            };
                        }
                        
                        // Buat ViewModel SlipGajiDetail1
                        SlipGajiDetail1 slipGajiDetail1 = new SlipGajiDetail1
                        {
                            SlipGajiDetailId = slipGajiDetail.Id,
                            NoBadge = employee.NIK,
                            Tanggal = curDay,
                            Shift = stat, //ewt.WorkingTime.Code, // Status ? N/HS/OF/PR/ST/P/A/S
                            //Status = stat,
                            nmhari = ci.DateTimeFormat.GetDayName(curDay.DayOfWeek),

                            jamkerjaActual = GetSalaryItemValue(Constant.LegacyAttendanceItem.WRKTM, salaryItemsValue[curDay], salaryItemsSign),
                            jamReg = GetSalaryItemValue(Constant.LegacyAttendanceItem.REGWT, salaryItemsValue[curDay], salaryItemsSign),
                            Lembur15 = GetSalaryItemValue(Constant.LegacyAttendanceItem.OTH15, salaryItemsValue[curDay], salaryItemsSign),
                            Lembur20 = GetSalaryItemValue(Constant.LegacyAttendanceItem.OTH20, salaryItemsValue[curDay], salaryItemsSign),
                            Lembur30 = GetSalaryItemValue(Constant.LegacyAttendanceItem.OTH30, salaryItemsValue[curDay], salaryItemsSign),
                            Lembur40 = GetSalaryItemValue(Constant.LegacyAttendanceItem.OTH40, salaryItemsValue[curDay], salaryItemsSign),
                        };
                        slipGajiDetail1.jamkerjaActual = slipGajiDetail1.jamReg + slipGajiDetail1.Lembur15 + slipGajiDetail1.Lembur20 + slipGajiDetail1.Lembur30 + slipGajiDetail1.Lembur40;
                        _validator._slipGajiDetail1Service.CreateObject(slipGajiDetail1, _validator._slipGajiDetailService);

                        // free temporary memory
                        salaryItemsValue[curDay].Clear();
                    }
                    salaryItemsValue.Clear();
                    salaryItemsSign.Clear();
                }
            }
            return error;
        }

        
    }
}