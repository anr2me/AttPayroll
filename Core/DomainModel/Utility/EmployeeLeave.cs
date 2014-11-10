using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeeLeave
    {
        public int Id { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LeaveInterval { get; set; } // used internally
        public int LeaveType { get; set; } // cuti, izin, sakit dgn surat, dinas, pengganti tugas
        public string Remark { get; set; }
        public int EmployeeId { get; set; }
        public bool IsApproved { get; set; }
        public bool IsRealized { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
