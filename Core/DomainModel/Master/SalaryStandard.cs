using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class SalaryStandard
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime EffectiveDate { get; set; }
        public int TitleInfoId { get; set; }
        
        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual TitleInfo TitleInfo { get; set; }
        public virtual ICollection<SalaryStandardDetail> SalaryStandardDetails { get; set; }

    }
}
