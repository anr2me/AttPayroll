using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class PensionCompensation
    {
        public int Id { get; set; }

        public DateTime EntryDate { get; set; } // Posting date
        public string UserName { get; set; } // Poster
        public DateTime PensionDate { get; set; }
        public int EmployeeId { get; set; }
        public decimal PensionValue { get; set; }
        public decimal PensionMultiply { get; set; }
        public decimal MedAndHousing { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Employee Employee { get; set; }
    }
}
