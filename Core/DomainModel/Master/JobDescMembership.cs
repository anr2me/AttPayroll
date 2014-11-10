using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class JobDescMembership
    {
        public int Id { get; set; }
        
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public int JobDescId { get; set; }
        public string JobDescName { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Applicant Applicant { get; set; }
        public virtual JobDesc JobDesc { get; set; }
    }
}
