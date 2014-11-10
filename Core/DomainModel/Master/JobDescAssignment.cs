using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class JobDescAssignment
    {
        public int Id { get; set; }
        
        public int JobDescId { get; set; }
        public string JobDescName { get; set; }
        public int ApplicantId { get; set; }
        public string ApplicantName { get; set; }
        public int EmployeePersonalId { get; set; }
        //public int CompanyInfoId { get; set; }
        //public int BranchOfficeId { get; set; }
        //public int DepartmentId { get; set; }
        public int DivisionId { get; set; }
        public string DivisionName { get; set; }
        public int SuperiorId { get; set; }
        public string SuperiorName { get; set; }
        public int SupervisorId { get; set; }
        public string SupervisorName { get; set; }
        public string Location { get; set; }
        public DateTime EffectiveDate { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }

        public virtual Applicant Applicant { get; set; }
        public virtual EmployeePersonal EmployeePersonal { get; set; }
        public virtual EmployeePersonal Superior { get; set; }
        public virtual EmployeePersonal Supervisor { get; set; }
        public virtual Division Division { get; set; }
        public virtual JobDesc JobDesc { get; set; }
    }
}
