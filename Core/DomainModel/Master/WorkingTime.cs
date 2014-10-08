using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class WorkingTime
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public decimal Tolerance { get; set; } // toleransi checkin (minutes)
        public DateTime CheckIn { get; set; }
        public DateTime CheckOut { get; set; }
        public DateTime MinCheckIn { get; set; }
        public DateTime MaxCheckIn { get; set; }
        public DateTime MinCheckOut { get; set; }
        public DateTime MaxCheckOut { get; set; }
        public DateTime BreakIn { get; set; }
        public DateTime BreakOut { get; set; }
        //public DateTime MinBreakIn { get; set; }
        //public DateTime MaxBreakIn { get; set; }
        //public DateTime MinBreakOut { get; set; }
        //public DateTime MaxBreakOut { get; set; }
        public decimal WorkInterval { get; set; } // waktu kerja minus waktu break (minutes)
        public decimal BreakInterval { get; set; } // waktu istirahat (minutes)

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }
        public ICollection<EmployeeWorkingTime> EmployeeWorkingTimes { get; set; }
        public ICollection<WorkingDay> WorkingDays { get; set; }
    }
}
