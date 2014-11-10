using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeeProfessionMembership
    {
        public int Id { get; set; }
        public int EmployeePersonalId { get; set; }

        public string NamaOrganisasi { get; set; }
        public string NomorKeanggotaan { get; set; }
        public string DiterbitkanOleh { get; set; } // nama tempat
        public DateTime DariTanggal { get; set; }
        public DateTime SampaiTanggal { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual EmployeePersonal EmployeePersonal { get; set; }

    }
}
