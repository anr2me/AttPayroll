using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class ManualAttendance
    {
        public int Id { get; set; }

        public DateTime AttendanceDate { get; set; }
        public DateTime EntryDate { get; set; }
        public string UserName { get; set; }
        public int Shift { get; set; } // Normal, Malam, HariLibur, ALL
        public int EmployeeId { get; set; }
        public int Status { get; set; } // Present, Alpha, Duty, Off, Ijin/SakitTanpaSurat, Cuti, SakitDenganSurat
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public int BreakHour { get; set; }
        public string Description { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
