using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Core.DomainModel;

namespace Core.Constants
{
    public partial class Constant
    {
        public enum Sex
        {
            Male,
            Female
        }

        public enum Marital
        {
            Single,
            Married,
        }

        public enum Religion
        {
            Budha,
            Hindu,
            Islam,
            Katolik,
            Kristen,
            Other
        }

        public enum WorkingStatus
        {
            Intern,
            Contract,
            Probation,
            Jobholder,
        }

        public class UserType
        {
            public const string Admin = "Admin";
            public const string Super = "Super";
            public const string User = "User";
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

        public enum LegacyAttendanceItem
        {
            WRKTM, // = "WKTM", // working minutes on a day
            OVRTM, // = "OVTM", // overtime minutes on a day
            CILTM, // = "CLTM", // checkin latetime minutes on a day
            COETM, // = "CETM", // checkout earlytime minutes on a day
            BILTM, // = "BLTM", // breakin latetime minutes on a day
            BOETM, // = "BETM", // breakout earlytime minutes on a day
           
        }

        public enum LegacyMonthlyItem
        {
            // Absensi
            PRESN, // = "PRSN", // present days on a month
            ALPHA, // = "ALPA", // alpha days on a month

            // Total uang lembur
            TOVTM,

            // Tunjangan tidak tetap
            TTJLP, // total tunjangan lapangan (dlm sebulan)
            TINHD, // total insentif hadir
            TOTTJ, // total tunjangan lainnya
            KBLLU, // kekurangan bulan lalu

            THR, // THR

            // Potongan penerimaan
            PTABS, // potongan absensi
            TOTPT, // potongan lainnya

            GJKOT, // gaji kotor

            // Potingan loan/jamsostek/pajak
            PTPJM, // potongan pinjaman
            JMSTK, // jamsostek
            PPH21, // pph21

            PJ204, // pajak jkk jkm 204
            TDKOT, // total pendapatan kotor
            PJTJJB, // pajak tunjangan jabatan
            PPTKP, // pajak ptkp
            TPPJK, // total pengurangan pajak
            TDKPJ, // total pendapatan kena pajak
            TDKPT, // total pendapatan kena pajak setahun

            PPH05, // pph 5%
            PPH15, // pph 15%
            PPH25, // pph 25%
            PPH30, // pph 30%

            GJBSH, // gaji bersih
        }

        public enum LegacySalaryItem
        {
            GJPOK, // = "GJPK", // gaji pokok
            OTR10, // = "OTR0", // rate lembur 1x
            OTR15, // = "OTR1", // rate lembur 1.5x
            OTR20, // = "OTR2", // rate lembur 2x
            OTR30, // = "OTR3", // rate lembur 3x
            OTR40, // = "OTR4", // rate lembur 4x
            TJMKN, // = "TJMK", // tunjangan makan
            TJTRN, // = "TJTR", // tunjangan transport
        }
    }
}
