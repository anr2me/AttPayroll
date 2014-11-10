using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class Applicant
    {
        public int Id { get; set; }

        //public int Index { get; set; }
        public int Status { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string PhoneNumber { get; set; }
        public int EducationId { get; set; }
        public string EducationName { get; set; }
        public string Email { get; set; }
        //public int Sex { get; set; }
        //public int MaritalStatus { get; set; } //Single(Widow)/Married
        public string Position { get; set; }
        public int Experience { get; set; } // in month
        public string LastPosition { get; set; }
        public string Source { get; set; }

        public int TotalScore { get; set; }
        public int IQ { get; set; }

        public int KecepatanKerja { get; set; }
        public int KetelitianKerja { get; set; }
        public int StabilitasKerja { get; set; }
        public int KetahananKerja { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime CreatedAt { get; set; }
        public Nullable<DateTime> UpdatedAt { get; set; }
        public Nullable<DateTime> DeletedAt { get; set; }

        public virtual Education Education { get; set; }
        public Dictionary<string, string> Errors { get; set; }
    }
}
