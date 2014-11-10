using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeeProfessionLicense
    {
        public int Id { get; set; }
        public int EmployeePersonalId { get; set; }

        public string NamaPerijinan { get; set; }
        public string NomorPerijinan { get; set; }
        public string DibuatDi { get; set; } // nama tempat
        public DateTime DariTanggal { get; set; }
        public DateTime SampaiTanggal { get; set; }
        public string Catatan { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual EmployeePersonal EmployeePersonal { get; set; }

    }
}
