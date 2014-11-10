using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeeEducationMini
    {
        public int Id { get; set; }
        public int EmployeePersonalId { get; set; }
        public string EmployeePersonalNIK { get; set; }
        public string EmployeePersonalName { get; set; }

        public string NamaSekolah { get; set; }
        public string Alamat { get; set; }
        public string Tahun { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual EmployeePersonal EmployeePersonal { get; set; }

    }
}
