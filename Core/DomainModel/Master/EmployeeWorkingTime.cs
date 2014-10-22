using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeeWorkingTime
    {
        public int Id { get; set; }
        public int WorkingTimeId { get; set; }
        public int EmployeeId { get; set; }
        //public bool IsShiftable { get; set; }
        //public bool IsEnabled { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual WorkingTime WorkingTime { get; set; }
        public virtual Employee Employee { get; set; }

    }
}
