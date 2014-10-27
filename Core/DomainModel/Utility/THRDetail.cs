using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class THRDetail
    {
        public int Id { get; set; }
        public int THRId { get; set; }
        public int EmployeeId { get; set; }
        //public decimal Amount { get; set; } // 1x gaji untuk minimal 1 tahun kerja, 0.5x untuk di bwh 1tahun (misal 6bln)
        
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual THR THR { get; set; }
    }
}
