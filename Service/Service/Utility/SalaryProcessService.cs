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

                    var spls = _validator._spklService.GetQueryable().Include("Employee").Where(x => x.EmployeeId == employee.Id);
                    var empls = _validator._employeeLeaveService.GetQueryable().Include("Employee").Where(x => x.EmployeeId == employee.Id);

                    var wts = _validator._workingTimeService.GetQueryable().Include("EmployeeWorkingTimes").Include("WorkingDays");

                    var wds = _validator._workingDayService.GetQueryable().Include("WorkingTime");
                    //.Include("EmployeeWorkingTimes").Include("Employees")

                    var ewts = _validator._employeeWorkingTimeService.GetQueryable().Include("Employee").Include("WorkingTime").OrderBy(x => x.StartDate);

                    var atts = _validator._employeeAttendanceService.GetQueryable().Include("Employee")//.Include("EmployeeAttendanceDetails")
                                    .Where(x => x.EmployeeId == employee.Id &&
                                                x.AttendanceDate.Year == monthYear.Year &&
                                                x.AttendanceDate.Month == monthYear.Month);

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

                    var genls = _validator._generalLeaveService.GetQueryable();

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
                    Dictionary<string, decimal> salaryItemSign = new Dictionary<string, decimal>();
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
                        salaryItemSign.Add(item.Code, item.SalarySign);
                    };

                    WorkingDay wd = new WorkingDay();
                    //DateTime curDay = endDate;
                    DateTime prevDay = endDate;
                    for (DateTime curDay = endDate; curDay >= startDate; curDay = curDay.AddDays(-1))
                    {
                        // Init Legacy items
                        salaryItemsValue[curDay] = new Dictionary<string, decimal>();
                        foreach (var item in salaryItemSign)
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
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.THR.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.THR.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH05D.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH05D.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH15D.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH15D.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH25D.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH25D.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH30D.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PPH30D.ToString()];

                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PTKP.ToString()] = ptkp;
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.GJPOK.ToString()] = salaryItemsValue[prevDay][Constant.LegacySalaryItem.GJPOK.ToString()];
                        salaryItemsValue[curDay][Constant.LegacySalaryItem.OVTRT.ToString()] = salaryItemsValue[prevDay][Constant.LegacySalaryItem.OVTRT.ToString()];


                        // Cari gaji aktif yang sesuai
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
                                // Jika ada lembur, Cari ulang working day, termasuk hari libur/off
                                oriwd = wds.Where(x => x.WorkingTime.Id == ewt.WorkingTimeId && x.Code == kodehari).FirstOrDefault();
                            }
                        }
                        // Hitung ulang jam kerja sesuai hari
                        int ofset = 0;
                        if (oriwd.MinCheckIn.Date < oriwd.CheckIn.Date) ofset = -1; // offsetkan hari supaya tanggal curDay = tanggal CheckIn
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

                        // Cari absensi yg sesuai
                        var att = atts.Where(x => x.AttendanceDate == curDay).FirstOrDefault();

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
                                    salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] + 1;
                                    prevDay = curDay;
                                    continue;
                                }
                            }
                        }
                        // Absensi ada, hitung Attendance items
                        else
                        {
                            // Telat checkin
                            TimeSpan telat = att.CheckIn.GetValueOrDefault().Subtract(wd.CheckIn);
                            if (att.CheckIn.GetValueOrDefault() > wd.CheckIn.AddMinutes((double)wd.CheckInTolerance))
                            {
                                // Tidak ada ijin/alasan, hitung menit potongan telat dan jumlah hari telat dlm satu periode
                                if (att.Status == (int)Constant.AttendanceStatus.Present)
                                {
                                    salaryItemsValue[curDay][Constant.LegacyAttendanceItem.CILTM.ToString()] = (decimal)telat.TotalMinutes / 60;
                                    salaryItemsValue[curDay][Constant.LegacyMonthlyItem.LATE.ToString()] = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.LATE.ToString()] + 1;
                                }
                            }
                            // Checkout lebih awal
                            TimeSpan awal = wd.CheckOut.Subtract(att.CheckOut.GetValueOrDefault());
                            if (att.CheckOut.GetValueOrDefault() < wd.CheckOut.AddMinutes((double)-wd.CheckOutTolerance))
                            {
                                // Tidak ada ijin/alasan, hitung menit potongan
                                if (att.Status == (int)Constant.AttendanceStatus.Present)
                                {
                                    salaryItemsValue[curDay][Constant.LegacyAttendanceItem.COETM.ToString()] = (decimal)awal.TotalMinutes / 60;
                                }
                            }
                            // Telat checkout/overtime
                            if (att.CheckOut.GetValueOrDefault() > wd.CheckOut)
                            {
                                // Ada surat lembur, hitung menit lembur
                                var spl = spls2.Where(x => x.StartTime < att.CheckOut.GetValueOrDefault()).FirstOrDefault();
                                if (spl != null)
                                {
                                    if (att.Status == (int)Constant.AttendanceStatus.Present)
                                    {
                                        DateTime maxlembur = spl.EndTime;
                                        if (maxlembur > att.CheckOut.GetValueOrDefault())
                                        {
                                            maxlembur = att.CheckOut.GetValueOrDefault();
                                        }
                                        decimal menitlembur = Math.Max(0, (decimal)(maxlembur - spl.StartTime).TotalMinutes); //(decimal)-awal.TotalMinutes;
                                        salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OVRTM.ToString()] = menitlembur / 60;
                                        // Lembur di Hari kerja
                                        if (wd.IsEnabled)
                                        {
                                            if (menitlembur >= 60)
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH15.ToString()] += 1;
                                                menitlembur -= 60;
                                            }
                                            if (menitlembur >= 60)
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH20.ToString()] += (menitlembur / 60); //1;
                                                //menitlembur -= 60;
                                            }
                                            //if (menitlembur >= 60)
                                            //{
                                            //    salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH30.ToString()] += (menitlembur/60);
                                            //}
                                        }
                                        else
                                        // Lembur di Hari Libur
                                        {
                                            if (menitlembur >= 60)
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH20.ToString()] += 1;
                                                menitlembur -= 60;
                                            }
                                            if (menitlembur >= 60)
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH30.ToString()] += 1;
                                                menitlembur -= 60;
                                            }
                                            if (menitlembur >= 60)
                                            {
                                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH40.ToString()] += (menitlembur / 60);
                                            }
                                        }
                                        // Update Monthly Items of Total OverTime Hours
                                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT15.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH15.ToString()];
                                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT20.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH20.ToString()];
                                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT30.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH30.ToString()];
                                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT40.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH40.ToString()];
                                    }
                                }
                            }
                        }
                        // Hadir, hitung jumlah hari hadir dlm satu periode
                        if (att.Status != (int)Constant.AttendanceStatus.Alpha &&
                            att.Status != (int)Constant.AttendanceStatus.Izin)
                        {
                            salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] + 1;
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
                            salaryItemsValue[curDay][pph21spt.Code] = Math.Max(0, pph21spt.MaxAmount - pph21spt.MinAmount);
                        }

                        //foreach (var detail in att.EmployeeAttendanceDetails)
                        //{
                        //    salaryItemsValue[curDay][detail.SalaryItem.Code] = detail.Amount;
                        //}

                        prevDay = curDay;
                    }

                    // Calculate Total/Monthy Items
                    for (DateTime curDay = startDate.AddDays(1); curDay <= endDate; curDay = curDay.AddDays(1))
                    {
                        salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT15.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH15.ToString()];
                        salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT20.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH20.ToString()];
                        salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT30.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH30.ToString()];
                        salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT40.ToString()] += salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH40.ToString()];

                    }

                    // Update Monthly Attendance items, which might be used on custom salary items calculation (make sure All Legacy Items are ready to be used on formula)
                    for (DateTime curDay = startDate.AddDays(1); curDay <= endDate; curDay = curDay.AddDays(1))
                    {
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.ALPHA.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.PRESN.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.LATE.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.LATE.ToString()];

                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT15.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT15.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT20.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT20.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT30.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT30.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOT40.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT40.ToString()];

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
                    //List<decimal> pphlist = new List<decimal>();
                    //salaryItemsValue[startDate][Constant.LegacySalaryItem.PTKP.ToString()] = _validator._ptkpService.CalcPTKP(employee.MaritalStatus != (int)Constant.Marital.Married, employee.Children);
                    //decimal pph21 = _validator._pph21sptService.CalcPPH21(salaryItemsValue[startDate][Constant.UserMonthlyItem.TDKPT.ToString()], pphlist);
                    //salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH05.ToString()] = pphlist[0];
                    //salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH15.ToString()] = pphlist[1];
                    //salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH25.ToString()] = pphlist[2];
                    //salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH30.ToString()] = pphlist[3];
                    //salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH21.ToString()] = pph21;

                    // SlipGaji (dari Monthly Items)
                    // Buat ViewModel SlipGaji Mini (for each SalarySlip ?)
                    SlipGajiMini slipGajiMini = _validator._slipGajiMiniService.GetOrNewObjectByEmployeeMonth(employee.Id, monthYear);
                    slipGajiMini.MONTH = monthYear;
                    slipGajiMini.EmployeeId = employee.Id;
                    slipGajiMini.start_working = employee.AppointmentDate.GetValueOrDefault();
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
                    slipGajiDetail.TanggalPenerimaan = employee.AppointmentDate.GetValueOrDefault();
                    slipGajiDetail.PeriodeAwal = startDate;
                    slipGajiDetail.PeriodeAkhir = endDate;
                    slipGajiDetail.GajiBasis = salaryItemsValue[startDate][Constant.LegacySalaryItem.GJPOK.ToString()];
                    slipGajiDetail.StatusMarriage = ((Constant.MaritalStatus)employee.MaritalStatus).ToString();
                    slipGajiDetail.NoSlip = NoSlip;
                    slipGajiDetail.Rate = salaryItemsValue[startDate][Constant.LegacySalaryItem.OVTRT.ToString()];
                    slipGajiDetail.Lembur15 = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT15.ToString()];
                    slipGajiDetail.Lembur20 = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT20.ToString()];
                    slipGajiDetail.Lembur30 = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT30.ToString()];
                    slipGajiDetail.Lembur40 = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.TOT40.ToString()];
                    slipGajiDetail.Disiapkan_oleh = Disiapkan_oleh;
                    slipGajiDetail.Disetujui_oleh = Disetujui_oleh;
                    slipGajiDetail.Dikoreksi_oleh = Dikoreksi_oleh;
                    slipGajiDetail.company_code = employee.Division.Department.BranchOffice.Code;
                    _validator._slipGajiDetailService.CreateOrUpdateObject(slipGajiDetail, _validator._employeeService, _validator._slipGajiDetail1Service, _validator._slipGajiDetail2AService);

                    // Buat ViewModel SlipGajiDetail2A (for each SalarySlip ?)
                    SlipGajiDetail2A slipGajiDetail2A = new SlipGajiDetail2A
                    {
                        SlipGajiDetailId = slipGajiDetail.Id,
                        month = monthYear.ToString("MMMM", ci),
                        employee_code = employee.NIK,
                        salary_basic = salaryItemsValue[startDate][Constant.LegacySalaryItem.GJPOK.ToString()],
                        rate_hour = salaryItemsValue[startDate][Constant.LegacySalaryItem.OVTRT.ToString()],
                        allowance_rate = salaryItemsValue[startDate][Constant.LegacySalaryItem.TJLAP.ToString()],
                        uang_makan = salaryItemsValue[startDate][Constant.UserMonthlyItem.TTJMK.ToString()],
                        jml_jam_lembur = salaryItemsValue[startDate][Constant.LegacyAttendanceItem.OVRTM.ToString()],
                        jml_lembur = salaryItemsValue[startDate][Constant.UserMonthlyItem.TOVTM.ToString()],
                        jml_hari_absen = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.ALPHA.ToString()],
                        tunj_lap = salaryItemsValue[startDate][Constant.UserMonthlyItem.TTJLP.ToString()],
                        insentive_hadir = salaryItemsValue[startDate][Constant.UserMonthlyItem.TINHD.ToString()],
                        other_allow = salaryItemsValue[startDate][Constant.UserMonthlyItem.TOTTJ.ToString()],
                        krg_bln_lalu = salaryItemsValue[startDate][Constant.UserMonthlyItem.KBLLU.ToString()],
                        thr = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.THR.ToString()],
                        pot_absensi = salaryItemsValue[startDate][Constant.UserMonthlyItem.PTABS.ToString()],
                        pot_others = salaryItemsValue[startDate][Constant.UserMonthlyItem.TOTPT.ToString()],
                        gaji_kotor = salaryItemsValue[startDate][Constant.UserMonthlyItem.GJKOT.ToString()],
                        pot_pinjaman = salaryItemsValue[startDate][Constant.UserMonthlyItem.PTPJM.ToString()],
                        pot_jamsostek = salaryItemsValue[startDate][Constant.UserMonthlyItem.JMSTK.ToString()],
                        pjk_jkk_jkm_204 = salaryItemsValue[startDate][Constant.UserMonthlyItem.PJ204.ToString()],
                        tot_dpt_kotor = salaryItemsValue[startDate][Constant.UserMonthlyItem.TDKOT.ToString()],
                        pjk_tunj_jabatan = salaryItemsValue[startDate][Constant.UserMonthlyItem.PJTJJB.ToString()],
                        pjk_ptkp = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPTKP.ToString()],
                        tot_pengurang_pajak = salaryItemsValue[startDate][Constant.UserMonthlyItem.TPPJK.ToString()],
                        tot_dpt_kena_pajak = salaryItemsValue[startDate][Constant.UserMonthlyItem.TDKPJ.ToString()],
                        tot_dpt_kena_pajak_tahun = salaryItemsValue[startDate][Constant.UserMonthlyItem.TDKPT.ToString()],
                        pph_5_persen = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH05.ToString()],
                        pph_15_persen = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH15.ToString()],
                        pph_25_persen = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH25.ToString()],
                        pph_30_persen = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH30.ToString()],
                        pph21 = salaryItemsValue[startDate][Constant.UserMonthlyItem.PPH21.ToString()],
                        round = salaryItemsValue[startDate][Constant.UserMonthlyItem.GJRND.ToString()],
                        gaji_bersih = salaryItemsValue[startDate][Constant.UserMonthlyItem.GJBSH.ToString()],
                    };
                    _validator._slipGajiDetail2AService.CreateObject(slipGajiDetail2A, _validator._slipGajiDetailService);
                    // Update SlipGaji Detail
                    //slipGajiDetail.SlipGajiDetail2AId = slipGajiDetail2A.Id;
                    //_validator._slipGajiDetailService.UpdateObject(slipGajiDetail, _validator._employeeService, _validator._slipGajiDetail1Service, _validator._slipGajiDetail2AService);

                    // last loop
                    for (DateTime curDay = startDate; curDay <= endDate; curDay = curDay.AddDays(1))
                    {
                        string stat = "OF";
                        // Cari attendance yg sesuai hari
                        var att = atts.Where(x => x.AttendanceDate == curDay).FirstOrDefault();
                        if (att != null)
                        {
                            switch ((Constant.AttendanceStatus)att.Status)
                            {
                                case Constant.AttendanceStatus.Alpha: stat = "A"; break;
                                case Constant.AttendanceStatus.Present:
                                    {
                                        stat = "N"; // "P";
                                        switch (curDay.DayOfWeek)
                                        {
                                            case DayOfWeek.Saturday: stat = "ST"; break;
                                            case DayOfWeek.Sunday: stat = "HS"; break;
                                        };
                                        break;
                                    }
                                case Constant.AttendanceStatus.Duty: stat = "DT"; break;
                                case Constant.AttendanceStatus.Cuti: stat = "PR"; break;
                                case Constant.AttendanceStatus.Sakit: stat = "S"; break;
                                case Constant.AttendanceStatus.Izin: stat = "P"; break;
                            };
                        }

                        // Cari employee workingtime
                        var ewt = ewts.Where(x => x.EmployeeId == employee.Id && x.StartDate <= curDay && x.EndDate >= curDay).FirstOrDefault();
                        // Buat ViewModel SlipGajiDetail1
                        SlipGajiDetail1 slipGajiDetail1 = new SlipGajiDetail1
                        {
                            SlipGajiDetailId = slipGajiDetail.Id,
                            NoBadge = employee.NIK,
                            Tanggal = curDay,
                            Shift = stat, //ewt.WorkingTime.Code, // Status ? N/HS/OF/PR/ST/P/A/S
                            //Status = stat,
                            jamkerjaActual = salaryItemsValue[curDay][Constant.LegacyAttendanceItem.WRKTM.ToString()],
                            jamReg = salaryItemsValue[curDay][Constant.LegacyAttendanceItem.REGWT.ToString()],
                            Lembur15 = salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH15.ToString()],
                            Lembur20 = salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH20.ToString()],
                            Lembur30 = salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH30.ToString()],
                            Lembur40 = salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OTH40.ToString()],
                            nmhari = ci.DateTimeFormat.GetDayName(curDay.DayOfWeek),
                        };
                        _validator._slipGajiDetail1Service.CreateObject(slipGajiDetail1, _validator._slipGajiDetailService);

                        // free temporary memory
                        salaryItemsValue[curDay].Clear();
                    }
                    salaryItemsValue.Clear();
                    salaryItemSign.Clear();
                }
            }
            return error;
        }

        
    }
}