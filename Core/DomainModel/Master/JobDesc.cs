using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class JobDesc
    {
        public int Id { get; set; }
        
        public int TitleInfoId { get; set; }
        public string TitleInfoName { get; set; }
        
        public string FungsiJabatan { get; set; }

        public string TugasTambahan { get; set; }
        public string LingkupAktivitas { get; set; }

        public string Kewenangan { get; set; }

        public int MinEducationId { get; set; }
        public string MinEducationName { get; set; }
        public string TechnicalCompetency { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Education MinEducation { get; set; }
        public virtual TitleInfo TitleInfo { get; set; }
        public virtual ICollection<KRA> KRAs { get; set; }
    }
}
