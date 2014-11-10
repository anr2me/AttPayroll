using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DomainModel
{
    public partial class Education
    {
        public int Id { get; set; }

        public int Jenjang { get; set; }
        public string Jurusan { get; set; }
        //public string Lulusan { get; set; }

        //public bool IsDeleted { get; set; }
        //public DateTime CreatedAt { get; set; }
        //public Nullable<DateTime> UpdatedAt { get; set; }
        //public Nullable<DateTime> DeletedAt { get; set; }

        public Dictionary<string, string> Errors { get; set; }
    }
}
