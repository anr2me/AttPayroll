using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeeFamilyInfo
    {
        public int Id { get; set; }
        public int EmployeePersonalId { get; set; }
        public string EmployeePersonalNIK { get; set; }
        public string EmployeePersonalName { get; set; }

        public string Name { get; set; }
        public int Relationship { get; set; }
        public DateTime BirthDate { get; set; }
        public string Job { get; set; }

        public int Sex { get; set; }
        public int MaritalStatus { get; set; }

        public string LastEducation { get; set; }
        public bool IsDeceased { get; set; }
        public string Remark { get; set; }

        public bool IsRelative { get; set; } // Family/Relative

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual EmployeePersonal EmployeePersonal { get; set; }
    }
}
