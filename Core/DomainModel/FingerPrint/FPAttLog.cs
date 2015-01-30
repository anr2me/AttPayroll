using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class FPAttLog
    {
        public int Id { get; set; }
        public int FPUserId { get; set; }

        public int PIN { get; set; } // EnrollNumber
        public Int64 PIN2 { get; set; } // byte[]/string/uint //U8 PIN2[24];
        public DateTime Time_second { get; set; }
        public int DeviceID { get; set; } // string //MachineID?
        public int VerifyMode { get; set; } // string
        public int InOutMode { get; set; } // string // AttState
        public int WorkCode { get; set; } // string
        public Int32 Reserved { get; set; } //BYTE reserved[4];

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual FPUser FPUser { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}
