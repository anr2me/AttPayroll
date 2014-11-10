using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class EmployeeStatusInfo
    {
        public int Id { get; set; }
        public int EmployeePersonalId { get; set; }

        public string NIK { get; set; }
        public int WorkingStatus { get; set; }
        public DateTime JoinedDate { get; set; }
        public int TitleInfoId { get; set; }
        public int DivisionId { get; set; }
        public int SupervisorId { get; set; }
        public int BossId { get; set; }
        public DateTime StartingDate { get; set; }
        public DateTime EndingDate { get; set; }
        public string Remark { get; set; }
        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual EmployeePersonal EmployeePersonal { get; set; }
        public virtual EmployeePersonal Supervisor { get; set; }
        public virtual EmployeePersonal Boss { get; set; }
        public virtual TitleInfo TitleInfo { get; set; }
        public virtual Division Division { get; set; }
    }
}
