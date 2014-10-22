using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class TitleInfo
    {
        public int Id { get; set; }

        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool IsShiftable { get; set; }
        //public int Grade { get; set; } // Senior, Junior
        public int AccessLevel { get; set; } // Managerial Access (can see underlings)
        public bool IsSalaryAllIn { get; set; } // Managerial Level

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual ICollection<Employee> Employees { get; set; }
    }
}
