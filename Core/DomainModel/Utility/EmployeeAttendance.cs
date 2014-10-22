using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeeAttendance
    {
        public int Id { get; set; }

        //public DateTime EntryDate { get; set; } // Posting date = created at
        //public string UserName { get; set; } // Poster
        public DateTime AttendanceDate { get; set; }
        public int Shift { get; set; } // Normal, Malam, Sabtu, Sabtu Malam, Minggu/HariLibur, Sabtu Tgl Merah, AllShift
        //public bool AllEmployees { get; set; }
        public int EmployeeId { get; set; }
        public int Status { get; set; } // Present, Alpha, Duty, Off, Ijin/SakitTanpaSurat, Cuti, SakitDenganSurat
        //public DateTime FromDate { get; set; } // used for multiple entry in a single post (creating multiple records per day/attendance date)
        //public DateTime ToDate { get; set; }
        
        public Nullable<DateTime> CheckIn { get; set; } // CheckIn Time on a day
        public Nullable<DateTime> CheckOut { get; set; } // CheckOut Time on a day
        public Nullable<DateTime> BreakOut { get; set; } // BreakOut Time on a day
        public Nullable<DateTime> BreakIn { get; set; } // BreakIn Time on a day
        public string Remark { get; set; }

        //public int BreakMinutes { get; set; }
        //public int BreakEarlyMinutes { get; set; } // breakout early
        //public int BreakLateMinutes { get; set; } // breakin late
        //public int CheckEarlyMinutes { get; set; } // checkout early
        //public int CheckLateMinutes { get; set; } // checkin late
        //public int OverTimeMinutes { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Employee Employee { get; set; }
        //public virtual ICollection<EmployeeAttendanceDetail> EmployeeAttendanceDetails { get; set; }
    }
}
