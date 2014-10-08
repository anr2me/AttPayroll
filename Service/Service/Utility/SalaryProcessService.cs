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

namespace Service.Service
{
    public class SalaryProcessService : ISalaryProcessService
    {
        public ISalaryProcessValidator _validator;

        public SalaryProcessService(ISalaryProcessValidator _salaryProcessValidator)
        {
            _validator = _salaryProcessValidator;
        }

        public string ProcessEmployee(int EmployeeId, DateTime yearMonth, int NoSlip = 1, string Disiapkan_oleh = null, string Disetujui_oleh = null, string Dikoreksi_oleh = null)
        {
            string error = _validator.ValidProcessEmployee(EmployeeId, yearMonth);
            if (error == null)
            {
                DateTime startDate = new DateTime(yearMonth.Year, yearMonth.Month, 1);
                DateTime endDate = new DateTime(yearMonth.Year, yearMonth.Month + 1, 1);
                endDate = endDate.AddDays(-1);

                Employee employee = _validator._employeeService.GetQueryable().Include("EmployeeWorkingTime").Include("WorkingTime").Include("WorkingDays").Include("LastEducation").Include("LastEmployement").Include("TitleInfo").Include("Division").Include("Department").Include("CompanyInfo").Where(x => x.Id == EmployeeId).OrderByDescending(x => x.CreatedAt).FirstOrDefault();
                var spls = _validator._spklService.GetQueryable().Include("Employee").Where(x => x.EmployeeId == EmployeeId);
                var empls = _validator._employeeLeaveService.GetQueryable().Include("Employee").Where(x => x.EmployeeId == EmployeeId);

                var wts = _validator._workingTimeService.GetQueryable().Include("EmployeeWorkingTimes").Include("WorkingDays");

                var wds = _validator._workingDayService.GetQueryable().Include("WorkingTime").Include("EmployeeWorkingTimes").Include("Employees");

                var emwts = _validator._employeeWorkingTimeService.GetQueryable().Include("Employees").Include("WorkingTime").Include("WorkingDays");

                var atts = _validator._employeeAttendanceDetailService.GetQueryable().Include("EmployeeAttendance").Include("Employee")
                                .Include("SalaryItem").Include("Formula").Include("FirstSalaryItem").Include("SecondSalaryItem")
                                .Where(x => x.EmployeeAttendance.EmployeeId == EmployeeId && 
                                            x.EmployeeAttendance.AttendanceDate.Year == yearMonth.Year && 
                                            x.EmployeeAttendance.AttendanceDate.Month == yearMonth.Month);

                var sals = _validator._salaryEmployeeDetailService.GetQueryable().Include("SalaryEmployee").Include("Employee")
                                .Include("SalaryItem").Include("Formula").Include("FirstSalaryItem").Include("SecondSalaryItem")
                                .Where(x => x.SalaryEmployee.EmployeeId == EmployeeId &&
                                            x.SalaryEmployee.EffectiveDate >= startDate && // TODO : Only include the last effective salary before (or same day with) startDate instead of from beginning
                                            x.SalaryEmployee.EffectiveDate <= endDate) 
                                .OrderByDescending(x => x.SalaryEmployee.EffectiveDate);
                var sallist = sals.ToList();

                var genls = _validator._generalLeaveService.GetQueryable();

                var slips = _validator._salarySlipService.GetQueryable().Include("SalarySlipDetails").Include("SalaryItem").Include("Formula").OrderBy(x => x.Code);
                
                var slipdets = _validator._salarySlipDetailService.GetQueryable().Include("SalarySlip").Include("SalaryItem").Include("Formula");

                var salits = _validator._salaryItemService.GetQueryable().Include("Formula").Include("FirstSalaryItem").Include("SecondSalaryItem"); //.OrderBy(x => x.Code);

                var forms = _validator._formulaService.GetQueryable().Include("SalaryItem").Include("FirstSalaryItem").Include("SecondSalaryItem");

                // Gunakan Culture Indonesia
                CultureInfo ci = CultureInfo.GetCultures(CultureTypes.NeutralCultures).Where(x => x.EnglishName == "Indonesia").FirstOrDefault();

                // Create temporary salary item value
                Dictionary<DateTime, Dictionary<string, decimal>> salaryItemsValue = new Dictionary<DateTime, Dictionary<string, decimal>>(); 
                var names = Enum.GetNames(typeof(Constant.LegacyAttendanceItem));
                names.Concat(Enum.GetNames(typeof(Constant.LegacySalaryItem)));

                //DateTime curDay = endDate;
                DateTime prevDay = endDate;
                for (DateTime curDay = endDate; curDay >= startDate; curDay = curDay.AddDays(-1))
                {
                    // Init Legacy items
                    foreach (var name in names)
                    {
                        salaryItemsValue[curDay] = new Dictionary<string, decimal>();
                        salaryItemsValue[curDay].Add(name, 0);
                    }

                    // Cari gaji aktif yang sesuai
                    var gajiaktif = sallist.FirstOrDefault();
                    while (gajiaktif != null && gajiaktif.SalaryEmployee.EffectiveDate > curDay)
                    {
                        // Pop from list/stack
                        sallist.RemoveAt(0);
                        gajiaktif = sallist.FirstOrDefault();
                    }
                    // Skip when no active salary found (working without being paid)
                    if (gajiaktif == null) {
                        // Need to set this (monthly items) using previous value to prevent it from being 0
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.ALPHA.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PRESN.ToString()]; 
                        prevDay = curDay;
                        continue;
                    }

                    // Cari hari kerja
                    string hari = curDay.DayOfWeek.ToString();
                    var wd = wds.Where(x => x.WorkingTime.Id == employee.EmployeeWorkingTime.WorkingTimeId && x.IsEnabled && x.Code == hari).FirstOrDefault();

                    // Skip if not working day
                    if (wd == null)
                    {
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.ALPHA.ToString()];
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PRESN.ToString()];
                        prevDay = curDay;
                        continue;
                    }

                    // Cari absensi yg sesuai
                    var att = atts.Where(x => x.EmployeeAttendance.AttendanceDate == curDay).FirstOrDefault();

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
                                // Bolos, lanjut ke hari lain
                                salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] + 1;
                                salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.PRESN.ToString()]; // Need to set this (monthly item) using previous value to prevent it from being 0
                                prevDay = curDay;
                                continue;
                            }
                        }
                    }
                    // Absensi ada, hitung Attendance items
                    else
                    {
                        // Telat checkin
                        TimeSpan telat = att.EmployeeAttendance.CheckIn.GetValueOrDefault().Subtract(employee.EmployeeWorkingTime.WorkingTime.CheckIn);
                        if (att.EmployeeAttendance.CheckIn.GetValueOrDefault().AddMinutes((double)-employee.EmployeeWorkingTime.WorkingTime.Tolerance) >= employee.EmployeeWorkingTime.WorkingTime.CheckIn)
                        {
                            // Tidak ada ijin/alasan, hitung menit potongan telat
                            if (att.EmployeeAttendance.Status == (int)Constant.AttendanceStatus.Present) 
                            {
                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.CILTM.ToString()] = (decimal)telat.TotalMinutes;
                            }
                        }
                        // Checkout lebih awal
                        TimeSpan awal = employee.EmployeeWorkingTime.WorkingTime.CheckOut.Subtract(att.EmployeeAttendance.CheckOut.GetValueOrDefault());
                        if (att.EmployeeAttendance.CheckOut.GetValueOrDefault() < employee.EmployeeWorkingTime.WorkingTime.CheckOut)
                        {
                            // Tidak ada ijin/alasan, hitung menit potongan
                            if (att.EmployeeAttendance.Status == (int)Constant.AttendanceStatus.Present)
                            {
                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.COETM.ToString()] = (decimal)awal.TotalMinutes;
                            }
                        }
                        // Telat checkout/overtime
                        if (att.EmployeeAttendance.CheckOut.GetValueOrDefault() > employee.EmployeeWorkingTime.WorkingTime.CheckOut)
                        {
                            // Ada surat lembur, hitung menit lembur
                            if (att.EmployeeAttendance.Status == (int)Constant.AttendanceStatus.Present)
                            {
                                salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OVRTM.ToString()] = (decimal)-awal.TotalMinutes;
                            }
                        }
                    }
                    // Hadir
                    if (att.EmployeeAttendance.Status != (int)Constant.AttendanceStatus.Alpha &&
                        att.EmployeeAttendance.Status != (int)Constant.AttendanceStatus.Izin)
                    {
                        salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] = salaryItemsValue[prevDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] + 1;
                    }
                    // Actual working time
                    salaryItemsValue[curDay][Constant.LegacyAttendanceItem.WRKTM.ToString()] = employee.EmployeeWorkingTime.WorkingTime.WorkInterval - 
                                           (salaryItemsValue[curDay][Constant.LegacyAttendanceItem.CILTM.ToString()] + salaryItemsValue[curDay][Constant.LegacyAttendanceItem.COETM.ToString()]);

                    // Update Legacy Salary items & Legacy Attendance items
                    foreach (var detail in gajiaktif.SalaryEmployee.SalaryEmployeeDetails)
                    {
                        salaryItemsValue[curDay][detail.SalaryItem.Code] = detail.Amount;
                    }

                    foreach (var detail in att.EmployeeAttendance.EmployeeAttendanceDetails)
                    {
                        salaryItemsValue[curDay][detail.SalaryItem.Code] = detail.Amount;
                    }

                    prevDay = curDay;
                }

                // Update Monthly Attendance items, which might be used on custom salary items calculation
                for (DateTime curDay = startDate; curDay <= endDate; curDay = curDay.AddDays(1))
                {
                    salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.ALPHA.ToString()];
                    salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PRESN.ToString()] = salaryItemsValue[startDate][Constant.LegacyMonthlyItem.PRESN.ToString()];
                }

                //curDay = startDate;
                for (DateTime curDay = startDate; curDay <= endDate; curDay = curDay.AddDays(1))
                {
                    // Hitung custom Salary items
                    foreach (var item in salits)
                    {
                        item.CurrentValue = _validator._salaryItemService.CalcSalaryItem(item, salaryItemsValue[curDay], _validator._formulaService);
                                                              
                    }

                    // Hitung Slip items
                    foreach (var slip in slips)
                    {
                        foreach (var slipdet in slipdets)
                        {
                            slipdet.Amount = salaryItemsValue[curDay][slipdet.SalaryItem.Code]; // slipdet.SalaryItem.CurrentValue;
                            
                        }
                        slip.TotalAmount = salaryItemsValue[curDay][slip.SalaryItem.Code]; //slip.SalaryItem.CurrentValue;
                        
                    }

                    // Buat ViewModel SlipGaji Detail
                    SlipGajiDetail slipGajiDetail = new SlipGajiDetail
                    {
                        MONTH = yearMonth,
                        EmployeeId = EmployeeId,
                        NoBadge = employee.NIK,
                        Name = employee.Name,
                        Jabatan = employee.TitleInfo.Name,
                        TanggalPenerimaan = employee.Appointment.GetValueOrDefault(),
                        PeriodeAwal = startDate,
                        PeriodeAkhir = endDate,
                        GajiBasis = salaryItemsValue[curDay][Constant.LegacySalaryItem.GJPOK.ToString()],
                        StatusMarriage = ((Constant.Marital)employee.MaritalStatus).ToString(),
                        NoSlip = NoSlip,
                        Rate = salaryItemsValue[curDay][Constant.LegacySalaryItem.OTR10.ToString()],
                        Lembur15 = salaryItemsValue[curDay][Constant.LegacySalaryItem.OTR15.ToString()],
                        Lembur20 = salaryItemsValue[curDay][Constant.LegacySalaryItem.OTR20.ToString()],
                        Lembur30 = salaryItemsValue[curDay][Constant.LegacySalaryItem.OTR30.ToString()],
                        Lembur40 = salaryItemsValue[curDay][Constant.LegacySalaryItem.OTR40.ToString()],
                        Disiapkan_oleh = Disiapkan_oleh,
                        Disetujui_oleh = Disetujui_oleh,
                        Dikoreksi_oleh = Dikoreksi_oleh,
                        company_code = employee.Division.Department.CompanyInfo.Code,
                    };
                    _validator._slipGajiDetailService.CreateObject(slipGajiDetail, _validator._employeeService, _validator._slipGajiDetail1Service, _validator._slipGajiDetail2AService);

                    // Buat ViewModel SlipGajiDetail1
                    SlipGajiDetail1 slipGajiDetail1 = new SlipGajiDetail1
                    {
                        SlipGajiDetailId = slipGajiDetail.Id,
                        NoBadge = employee.NIK,
                        Tanggal = curDay,
                        Shift = employee.EmployeeWorkingTime.WorkingTime.Name,
                        jamkerjaActual = salaryItemsValue[curDay][Constant.LegacyAttendanceItem.WRKTM.ToString()]/60,
                        jamReg = employee.EmployeeWorkingTime.WorkingTime.WorkInterval/60,
                        Lembur15 = salaryItemsValue[curDay][Constant.LegacySalaryItem.OTR15.ToString()],
                        Lembur20 = salaryItemsValue[curDay][Constant.LegacySalaryItem.OTR20.ToString()],
                        Lembur30 = salaryItemsValue[curDay][Constant.LegacySalaryItem.OTR30.ToString()],
                        Lembur40 = salaryItemsValue[curDay][Constant.LegacySalaryItem.OTR40.ToString()],
                        nmhari = curDay.ToString("DDDD", ci),
                    };
                    _validator._slipGajiDetail1Service.CreateObject(slipGajiDetail1, _validator._slipGajiDetailService);

                    // Buat ViewModel SlipGajiDetail2A
                    SlipGajiDetail2A slipGajiDetail2A = new SlipGajiDetail2A
                    {
                        SlipGajiDetailId = slipGajiDetail.Id,
                        month = yearMonth.ToString("MMMM", ci),
                        employee_code = employee.NIK,
                        salary_basic = salaryItemsValue[curDay][Constant.LegacySalaryItem.GJPOK.ToString()],
                        rate_hour = salaryItemsValue[curDay][Constant.LegacySalaryItem.OTR10.ToString()],
                        allowance_rate = salaryItemsValue[curDay][Constant.LegacySalaryItem.TJTRN.ToString()],
                        uang_makan = salaryItemsValue[curDay][Constant.LegacySalaryItem.TJMKN.ToString()],
                        jml_jam_lembur = salaryItemsValue[curDay][Constant.LegacyAttendanceItem.OVRTM.ToString()]/60,
                        jml_lembur = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOVTM.ToString()],
                        jml_hari_absen = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.ALPHA.ToString()],
                        tunj_lap = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TTJLP.ToString()],
                        insentive_hadir = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TINHD.ToString()],
                        other_allow = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOTTJ.ToString()],
                        krg_bln_lalu = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.KBLLU.ToString()],
                        thr = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.THR.ToString()],
                        pot_absensi = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PTABS.ToString()],
                        pot_others = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TOTPT.ToString()],
                        gaji_kotor = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.GJKOT.ToString()],
                        pot_pinjaman = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PTPJM.ToString()],
                        pot_jamsostek = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.JMSTK.ToString()],
                        pjk_jkk_jkm_204 = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PJ204.ToString()],
                        tot_dpt_kotor = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TDKOT.ToString()],
                        pjk_tunj_jabatan = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PJTJJB.ToString()],
                        pjk_ptkp = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPTKP.ToString()],
                        tot_pengurang_pajak = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TPPJK.ToString()],
                        tot_dpt_kena_pajak = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TDKPJ.ToString()],
                        tot_dpt_kena_pajak_tahun = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.TDKPT.ToString()],
                        pph_5_persen = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH05.ToString()],
                        pph_15_persen = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH15.ToString()],
                        pph_25_persen = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH25.ToString()],
                        pph_30_persen = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH30.ToString()],
                        pph21 = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH21.ToString()],
                        round = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.PPH21.ToString()] % 1000,
                        gaji_bersih = salaryItemsValue[curDay][Constant.LegacyMonthlyItem.GJBSH.ToString()],
                    };
                    _validator._slipGajiDetail2AService.CreateObject(slipGajiDetail2A, _validator._slipGajiDetailService);

                    slipGajiDetail.SlipGajiDetail1Id = slipGajiDetail1.Id;
                    slipGajiDetail.SlipGajiDetail2AId = slipGajiDetail2A.Id;
                    _validator._slipGajiDetailService.UpdateObject(slipGajiDetail, _validator._employeeService, _validator._slipGajiDetail1Service, _validator._slipGajiDetail2AService);

                    // Buat ViewModel SlipGaji Mini
                    SlipGajiMini slipGajiMini = new SlipGajiMini
                    {
                        MONTH = yearMonth,
                        EmployeeId = EmployeeId,

                    };
                    _validator._slipGajiMiniService.CreateObject(slipGajiMini, _validator._employeeService);

                    // free temporary memory
                    salaryItemsValue[curDay].Clear();
                }
                salaryItemsValue.Clear();
                
            }
            return error;
        }

        
    }
}