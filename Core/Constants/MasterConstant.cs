using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using Core.DomainModel;

namespace Core.Constants
{
    public partial class Constant
    {
        public class ErrorPage
        {
            public static string PageViewNotAllowed = "You are not allowed to View this Page. <br/> <a href='/Authentication/Logout'>[Logout]</a>";
            public static string PagePrintNotAllowed = "You are not allowed to Print this Page. <br/> <a href='/Authentication/Logout'>[Logout]</a>";
            public static string ClosingNotFound = "Closing Not Found.";
            public static string RecordNotFound = "Record Not Found.";
            public static string RecordDetailNotFound = "Record Detail Not Found.";
        }

        public string CompanyName = "PT. SINAR RODA UTAMA";

        public class MenuGroupName
        {
            public static string Master = "Master";
            public static string Utility = "Utility";
            public static string Report = "Report";
            public static string Setting = "Setting";
        }

        public class MenuName
        {
            public static string BranchOffice = "Branch Office";
            public static string Department = "Department";
            public static string Division = "Division";

            public static string Applicant = "Applicant";
            public static string Education = "Education";
            public static string JobDesc = "Job Description";

            public static string TitleInfo = "Title";
            public static string PersonalInfo = "Personal Info";
            public static string EmployeeFamilyInfo = "Family Info";
            public static string EmployeeStatusInfo = "Status Info";
            public static string EmployeeEducationInfo = "Education Info";
            public static string EmployeeProfessionInfo = "Profession Info";
            public static string EmployeeExperienceInfo = "Experience Info";

            public static string WorkingTime = "Working Time";
            public static string WorkingDay = "Working Day";

            public static string SalaryStandard = "Salary Standard";
            public static string SalarySlip = "Salary Slip";
            public static string PTKP = "PTKP";
            public static string PPH21SPT = "PPH21 SPT";
            public static string SalaryItems = "Salary Items";

            public static string GeneralLeave = "General Leave";
            public static string EmployeeLeave = "Employee Leave";
            public static string SPKL = "SPKL";
            public static string EmployeeWorkingTime = "Employee Working Time";
            public static string ManualAttendance = "Manual Attendance";

            public static string EmployeeSalary = "Employee Salary";
            public static string EmployeeLoan = "Employee Loan";
            public static string OtherIncome = "Other Income";
            public static string OtherExpense = "Other Expense";
            public static string THR = "THR";
            public static string PensionCompensation = "Pension Compensation";
            public static string SalaryProcess = "Salary Process";

            public static string SlipGajiDetail = "Slip Gaji Detail";
            public static string SlipGajiStandard = "Slip Gaji Standard";
            public static string SalarySummary = "Salary Summary";
            public static string PPH21 = "PPH21";

            public static string EmployeeAttendance = "Employee Attendance";
            public static string EmployeeInstallment = "Employee Installment";

            public static string Account = "Account";
            public static string Closing = "Closing";
            public static string GeneralLedger = "GeneralLedger";
            public static string ValidComb = "ValidComb";
            public static string BalanceSheet = "BalanceSheet";
            public static string IncomeStatement = "IncomeStatement";

            public static string User = "User";
            public static string UserAccessRight = "User Access Right";
            public static string CompanyInfo = "Company Info";
        }

        public class SheetName
        {
            public const string BranchOffice = "Cabang";
            public const string Department = "Dept";
            public const string Division = "Div";
            public const string TitleInfo = "Jabatan";
            public const string EmployeeStatus = "Status Karyawan";
            public const string Employee = "Karyawan";
        }

        public class UserType
        {
            public const string Admin = "Admin";
            public const string Super = "Super";
            public const string User = "User";
        }

        public enum CurrencyType
        {
            IDR,
            USD,
            AUD,
            SGD,
            EUR,
            GBP,
            JPY,
        }

        public enum GradeType
        {
            TidakBisa,
            Kurang,
            Cukup,
            Baik,
            SangatBaik,
        }

        public enum IDType
        {
            KTP,
            SIM,
            Passport,
        }

        public enum Sex
        {
            Male,
            Female
        }

        public enum MaritalStatus
        {
            Single, //Widowed, // or Divorced
            Married,
            Divorced,
            //Widowed // Widow or Widower
        }

        public enum Relationship
        {
            Istri,
            Suami,
            Anak,
            Ayah,
            Ibu,
            Adik,
            Kakak,
            Sepupu,
            Lainnya,
        }

        public enum Religion
        {
            Other,
            Kristen,
            Katolik,
            Islam,
            Hindu,
            Budha,
        }

        public enum BloodType
        {
            A,
            AB,
            B,
            O,
        }

        public enum PolarType
        {
            Negative,
            Positive,
        }

        public enum JobType
        {
            PartTime,
            FullTime,
        }

        public enum WorkingStatus
        {
            Magang,
            Kontrak,
            Percobaan,
            Tetap,
        }

        public enum ApplicantStatus
        {
            Unprocessed,
            Stock,
            Hired,
            Rejected,
        }

        public enum JenjangType
        {
            SD,
            SLTP,
            SLTA,
            D1,
            D3,
            S1,
            S2,
            S3,
        }

        public enum ApplicantSourceType
        {
            Internal,
            Kurir,
            Jobstreet,
        }

        public enum WorkingShift
        {
            Normal,
            Malam,
            Sabtu,
            SabtuMalam,
            MingguLibur,
        }

        public enum ActiveStatus
        {
            Active,
            NonActive,
            //MedicalCheck,
        }

        public enum LeaveType
        {
            Biasa,
            PenggantiTugas,
            DinasPerusahaan,
        }

        public enum AttendanceStatus
        {
            Present,
            Alpha, // bolos tanpa alasan
            Duty, // Tugas luar
            Off, // Libur
            Cuti,
            Izin, // izin atau sakit tanpa surat dokter (sama seperti alpha tp dengan alasan)
            Sakit, // Sakit dengan surat dokter
        }

        public enum SalarySign
        {
            Expense = -1, // Credit
            Income = 1, // Debit
        }

        public enum OtherIncomeType
        {
            DanaLainnya,
            Materai,
            Parkir,
            JagaGedung,
            KostKaryawan,
            Pulsa,
            UangMakan,
            Transport,
            Kerajinan,
            Magang,
        }

        public enum SalaryItemType
        {
            Salary,
            Attendance,
            OtherIncome,
            OtherExpense,
            SalarySlip,
            PPH21,
            THR,
            //PTKP,
        }

        public enum SalaryItemStatus
        {
            Evently,
            Daily,
            Weekly,
            Monthly,
            Yearly,
        }

        public enum LegacyAttendanceItem // untuk perhitungan lembur dan potongan telat
        {
            [Description("Masa Kerja")]
            WKAGE, // = "WKAGE", // masa kerja semenjak awal kerja (dlm hari)
            [Description("Masa Kerja Permanen")]
            PMAGE, // = "PMAGE", // masa kerja semenjak jadi pegawai tetap (dlm hari)

            [Description("Jam Kerja Regular")]
            REGWT, // = "WKTM", // regular working hours on a day
            [Description("Jam Kerja")]
            WRKTM, // = "WKTM", // working hours on a day
            [Description("Jam Lembur")]
            OVRTM, // = "OVTM", // overtime hours on a day

            [Description("Jam CheckIn Telat")]
            CILTM, // = "CLTM", // checkin latetime hours on a day
            [Description("Jam CheckOut Awal")]
            COETM, // = "CETM", // checkout earlytime hours on a day
            //[Description("Jam BreakIn Telat")]
            //BILTM, // = "BLTM", // breakin latetime hours on a day
            //[Description("Jam BreakOut Awal")]
            //BOETM, // = "BETM", // breakout earlytime hours on a day

            [Description("Jam Lembur 1.5x")]
            OTH15, // = "OTR1", // rate lembur 1.5x
            [Description("Jam Lembur 2x")]
            OTH20, // = "OTR2", // rate lembur 2x
            [Description("Jam Lembur 3x")]
            OTH30, // = "OTR3", // rate lembur 3x
            [Description("Jam Lembur 4x")]
            OTH40, // = "OTR4", // rate lembur 4x
        }

        public enum LegacySalaryItem // Used for salary standard details ()
        {
            // Base/Standard employee's salary items
            [Description("Gaji Pokok")]
            GJPOK, // = "GJPK", // gaji pokok
            [Description("Rate Lembur")]
            OVTRT, // = "OTR0", // rate lembur (1x) = (gajipokok / 173)
            [Description("Rate Potongan Telat")]
            LTTRT, // = "OTR0", // rate potongan telat
            [Description("Incentive Hadir")]
            INHDR, // insentif hadir
            //[Description("Tunjangan Jabatan")]
            //TJJAB, // = "TJJB", // tunjangan jabatan
            [Description("Tunjangan Makan")]
            TJMKN, // = "TJMK", // tunjangan makan
            [Description("Tunjangan Transport")]
            TJTRN, // = "TJTR", // tunjangan transport
            [Description("Tunjangan Lapangan")]
            TJLAP, // tunjangan lapangan
            [Description("Tunjangan Lainnya")]
            TJLNA, // tunjangan lainnya
        }

        public enum LegacyMonthlyItem // Used for Salary Slip and it's calculation
        {
            // Absensi
            [Description("Hadir")]
            PRESN, // = "PRSN", // present days on a month
            [Description("Alpha")]
            ALPHA, // = "ALPA", // alpha days on a month
            [Description("Telat")]
            LATE, // = "LATE", // telat days on a month

            [Description("Total Jam Lembur 1.5x")]
            TOT15, // = "OTR1", // rate lembur 1.5x on a month
            [Description("Total Jam Lembur 2x")]
            TOT20, // = "OTR2", // rate lembur 2x on a month
            [Description("Total Jam Lembur 3x")]
            TOT30, // = "OTR3", // rate lembur 3x on a month
            [Description("Total Jam Lembur 4x")]
            TOT40, // = "OTR4", // rate lembur 4x on a month

            [Description("Tunjangan Hari Raya")]
            THR, // take home pay amount?

            [Description("Total Tunjangan Lainnya")]
            TOTTJ, // total tunjangan lainnya
            [Description("Potongan Lainnya")]
            TOTPT, // potongan lainnya

            [Description("PTKP")]
            PTKP, // = "PTKP", // pendapatan tidak kena pajak

            [Description("PPH21 5% Delta")]
            PPH05D, // pph 5%
            [Description("PPH21 15% Delta")]
            PPH15D, // pph 15%
            [Description("PPH21 25% Delta")]
            PPH25D, // pph 25%
            [Description("PPH21 30% Delta")]
            PPH30D, // pph 30%

            [Description("PPH21 5% Percent")]
            PPH05P, // pph 5% = 0.05m
            [Description("PPH21 15% Percent")]
            PPH15P, // pph 15% = 0.15m
            [Description("PPH21 25% Percent")]
            PPH25P, // pph 25% = 0.25m
            [Description("PPH21 30% Percent")]
            PPH30P, // pph 30% = 0.30m

            //[Description("PPH21 5%")]
            //PPH05, // pph 5%
            //[Description("PPH21 15%")]
            //PPH15, // pph 15%
            //[Description("PPH21 25%")]
            //PPH25, // pph 25%
            //[Description("PPH21 30%")]
            //PPH30, // pph 30%
            //[Description("PPH21")]
            //PPH21, // pph21
            //[Description("Gaji Bersih")]
            //GJBSH, // gaji bersih
            
        }

        public enum UserMonthlyItem // Used for Salary Slip and it's calculation
        {
            // User Defined Slip items
            [Description("Total Jam Lembur dlm 1x")]
            TOT10, // = "OTR1", // rate lembur setelah di conversi ke 1x dlm sebulan
            // Total uang lembur
            [Description("Total Uang Lembur")]
            TOVTM,

            // Tunjangan tidak tetap
            [Description("Total Tunjangan Lapangan")]
            TTJLP, // total tunjangan lapangan (dlm sebulan)
            [Description("Total Tunjangan Makan")]
            TTJMK, // total tunjangan makan (dlm sebulan)
            [Description("Total Tunjangan Transport")]
            TTJTR, // total tunjangan transport (dlm sebulan)
            [Description("Total Insentif Hadir")]
            TINHD, // total insentif hadir
            //[Description("Total Tunjangan Lainnya")]
            //TOTTJ, // total tunjangan lainnya
            [Description("Kekurangan Bulan Lalu")]
            KBLLU, // kekurangan bulan lalu

            //[Description("Tunjangan Hari Raya")]
            //THR, // take home pay amount?

            // Potongan penerimaan
            [Description("Potongan Absensi")]
            PTABS, // potongan absensi
            //[Description("Potongan Lainnya")]
            //TOTPT, // potongan lainnya

            [Description("Gaji Kotor")]
            GJKOT, // gaji kotor

            // Potingan loan/jamsostek/pajak
            [Description("Potongan Pinjaman")]
            PTPJM, // potongan pinjaman
            [Description("Jamsostek")] // PBJS
            JMSTK, // jamsostek

            [Description("Pajak JKK JKM 204")]
            PJ204, // pajak jkk jkm 204
            [Description("Total Pendapatan Kotor")]
            TDKOT, // total pendapatan kotor
            [Description("Pajak Tunjangan Jabatan")]
            PJTJJB, // pajak tunjangan jabatan
            [Description("Pajak PTKP")]
            PPTKP, // pajak ptkp
            [Description("Total Pengurangan Pajak")]
            TPPJK, // total pengurangan pajak
            [Description("Total Pendapatan Kena Pajak")]
            TDKPJ, // total pendapatan kena pajak
            [Description("Total Pendapatan Kena Pajak Setahun")]
            TDKPT, // total pendapatan kena pajak setahun

            [Description("Total Pendapatan Kena Pajak Setahun setelah PPH21 5%")]
            TDKPT2, // total pendapatan kena pajak setahun
            [Description("Total Pendapatan Kena Pajak Setahun setelah PPH21 15%")]
            TDKPT3, // total pendapatan kena pajak setahun
            [Description("Total Pendapatan Kena Pajak Setahun setelah PPH21 25%")]
            TDKPT4, // total pendapatan kena pajak setahun
            [Description("Total Pendapatan Kena Pajak Setahun untuk PPH21 5%")]
            TDKPT05, // total pendapatan kena pajak setahun
            [Description("Total Pendapatan Kena Pajak Setahun untuk PPH21 15%")]
            TDKPT15, // total pendapatan kena pajak setahun
            [Description("Total Pendapatan Kena Pajak Setahun untuk PPH21 25%")]
            TDKPT25, // total pendapatan kena pajak setahun

            [Description("PPH21 5%")]
            PPH05, // pph 5%
            [Description("PPH21 15%")]
            PPH15, // pph 15%
            [Description("PPH21 25%")]
            PPH25, // pph 25%
            [Description("PPH21 30%")]
            PPH30, // pph 30%
            [Description("PPH21")]
            PPH21, // pph21

            [Description("Gaji Bersih sebelum Pembulatan")]
            GJBSP, // gaji bersih (sebelum pembulatan)
            [Description("Gaji Bersih (ribuan)")]
            GJBDV, // gaji bersih per 1000
            [Description("Receh Gaji")]
            GJBRM, // sisa gaji bersih per 1000
            [Description("Rounding/Pembulatan")]
            GJRND, // pembulatan (ke atas) gaji bersih
            [Description("Gaji Bersih")]
            GJBSH, // gaji bersih
        }

        public static string GetEnumDesc(Enum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes =
                (DescriptionAttribute[])fi.GetCustomAttributes(typeof(DescriptionAttribute), false);

            if (attributes != null && attributes.Length > 0)
                return attributes[0].Description;
            else
                return value.ToString();
        }

        
    }


}
